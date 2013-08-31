using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeScriptParser.Parser;

namespace TypeScriptParser
{
    public class TsParser
    {
        public TsParser()
        {
            TokenTypes = TsTokenTypes.TypeScript;
        }
        public List<TsNode> Parse(string code)
        {
            var tokens = TsTokenizer.Tokenize(code);
            var parsed = Parse(tokens);
            return parsed;
        }

        List<TsNode> Parse(List<Token> tokens)
        {
            var list = new List<TsNode>();
            var tp = new TokenPointer(tokens, 0);
            while (tp.Value != null)
            {
                var node = Parse(ref tp);
                if (node != null)
                    list.Add(node);
            }
            return list;
        }

        TsNode Parse(ref TokenPointer tp)
        {
            if (tp.Value.Is(TokenTypes.Comments))
            {
                tp = tp.Next();
                return null;
            }
            else if (tp.Value.IsIdentifier("declare"))
            {
                return ParseDeclare(ref tp);
            }
            else if (tp.Value.IsIdentifier("interface"))
            {
                return ParseClassOrInterface(ref tp);
            }
            var pos = tp.Value.Selection.Start.Position;
            throw new Exception(pos + " : " + tp.Value.Selection.Start.GetLineText());
        }

        TsTypeDecl ParseClassOrInterface(ref TokenPointer tp)
        {
            var iface = new TsTypeDecl { Token = tp, };

            if (tp.Value.IsIdentifier("interface"))
                iface.Kind = TsTypeKind.Interface;
            else if (tp.Value.IsIdentifier("class"))
                iface.Kind = TsTypeKind.Class;
            else
                throw new Exception();
            tp = tp.Next().Verify(t => t.IsIdentifier());
            var typeName = tp;
            iface.Name = typeName.Value.Text;
            tp = tp.Next();
            if (tp.Value.IsIdentifier("extends"))
            {
                iface.Extends.AddRange(ParseExtends(ref tp));
            }
            if (tp.Value.IsIdentifier("implements"))
            {
                tp = tp.Next();
                iface.Implements.AddRange(ParseImplements(ref tp));
            }
            tp = ParseBracesAndMembers(tp, iface);
            return iface;
        }

        List<TsTypeRef> ParseExtends(ref TokenPointer tp)
        {
            var list = new List<TsTypeRef>();
            while (tp.Value.IsIdentifier("extends") || tp.Value.IsOperator(","))
            {
                tp = tp.Next();
                list.Add(ParseTypeRef(ref tp));
            }
            return list;
        }
        List<TsTypeRef> ParseImplements(ref TokenPointer tp)
        {
            var list = new List<TsTypeRef>();
            while (true)
            {
                list.Add(ParseTypeRef(ref tp));
                if (!tp.Value.IsOperator(","))
                    break;
                tp = tp.Next();
            }
            return list;
        }


        TsNode ParseDeclare(ref TokenPointer tp)
        {
            tp = tp.Verify(t => t.IsIdentifier("declare")).Next();
            if (tp.Value.IsIdentifier("var"))
            {
                return ParseVar(ref tp);
            }
            else if (tp.Value.IsIdentifier("function"))
            {
                tp = tp.Next();
                var func = ParseTsFunction(ref tp);
                return func;
            }
            else if (tp.Value.IsIdentifier("module"))
            {
                var module = new TsTypeDecl { Token = tp, Kind = TsTypeKind.Module };
                tp = tp.Next();
                if (tp.Value.Is(TokenTypes.StringLiteral))
                    module.Name = tp.Value.Text;
                else if (tp.Value.IsIdentifier())
                    module.Name = tp.Value.Text;
                else
                    throw new Exception();
                tp = tp.Next().Verify(t => t.IsOperator("{")).Next();
                while (!tp.Value.IsOperator("}"))
                {
                    tp = SkipComments(tp);
                    if (tp.Value.IsIdentifier("import"))
                    {
                        var import = new TsImportDecl { Token = tp };
                        tp = tp.Next().Verify(t => t.IsIdentifier());
                        import.Name = tp.Value.Text;
                        tp = tp.Next();
                        tp = tp.Verify(t => t.IsOperator("=")).Next();
                        tp = tp.Verify(t => t.IsIdentifier("module")).Next();
                        tp = tp.Verify(t => t.IsOperator("(")).Next();
                        tp = tp.Verify(t => t.Is(TokenTypes.StringLiteral));
                        module.Type = new TsModuleTypeRef { Name = tp.Value.Text };
                        tp = tp.Next().Verify(t => t.IsOperator(")")).Next();
                        tp = tp.Verify(t => t.IsOperator(";")).Next();
                        module.Members.Add(import);
                    }
                    else if (tp.Value.IsIdentifier("export"))
                    {
                        tp = tp.Next();
                        var me = ParseModuleMember(ref tp);
                        module.Members.Add(me);
                    }
                    else
                    {
                        var me = ParseModuleMember(ref tp);
                        module.Members.Add(me);
                    }
                }
                tp = tp.Verify(t => t.IsOperator("}")).Next();
                return module;
            }
            else
            {
                throw new Exception();
            }
        }

        TsMemberDecl ParseModuleMember(ref TokenPointer tp)
        {
            if (tp.Value.IsIdentifier("declare"))
            {
                tp = tp.Next(); //declare class Events {
            }
            if (tp.Value.IsIdentifier("function"))
            {
                tp = tp.Next();
                var func = ParseTsFunction(ref tp);
                return func;
            }
            else if (tp.Value.IsIdentifier("interface"))
            {
                var iface2 = ParseClassOrInterface(ref tp);
                return iface2;
            }
            else if (tp.Value.IsIdentifier("class"))
            {
                var iface2 = ParseClassOrInterface(ref tp);
                return iface2;
            }
            else if (tp.Value.IsIdentifier("var"))
            {
                var varx = ParseVar(ref tp);
                return varx;
            }
            else
                throw new Exception();
        }

        TsMemberDecl ParseVar(ref TokenPointer tp)
        {
            tp = tp.Next();
            if (tp.Next(2).Value.IsOperator("{"))
            {
                var iface = new TsTypeDecl { Token = tp, Name = tp.Value.Text, Kind = TsTypeKind.Anonymous };
                tp = tp.Next();
                tp = tp.Next();
                tp = ParseBracesAndMembers(tp, iface);
                if (tp.Value != null && tp.Value.IsOperator(";"))
                    tp = tp.Next();
                return iface;
            }
            else
            {
                var prm = ParseTsParameter(ref tp);
                tp = tp.Verify(t => t.IsOperator(";")).Next();
                var decl = new TsVarDecl { Token = prm.Token, Type = prm.Type, Name = prm.Name };
                return decl;
            }
        }
        TokenPointer ParseBracesAndMembers(TokenPointer tp, TsTypeDecl iface)
        {
            tp = tp.Verify(t => t.IsOperator("{")).Next();
            while (!tp.Value.IsOperator("}"))
            {
                tp = SkipComments(tp);
                var me = ParseTsMember(ref tp);
                iface.Members.Add(me);
            }
            tp = tp.Verify(t => t.IsOperator("}")).Next();
            return tp;
        }

        TokenPointer SkipComments(TokenPointer tp)
        {
            while (tp.Value.Is(TokenTypes.Comments))
            {
                tp = tp.Next();
                continue;
            }
            return tp;
        }

        TsMemberDecl ParseTsMember(ref TokenPointer tp)
        {
            if (tp.Value.IsOperator("["))
            {
                return ParseTsIndexer(ref tp);
            }
            else if (tp.Value.IsOperator("("))
            {
                var func = new TsFunctionDecl();
                tp = ParseBracketParametersAndReturnType(tp, func, "(", ")");
                return func;
            }
            tp = tp.Verify(t => t.IsIdentifier());
            if (tp.Next().Value.IsOperator(";"))    //export function listeners(event: string): { Function; }[];
            {
                return ParseTsField(ref tp);
            }
            else if (tp.Next().Value.IsOperator(":") || tp.Next(2).Value.IsOperator(":"))
            {
                return ParseTsField(ref tp);
            }
            else
            {
                return ParseTsFunction(ref tp);
            }

        }

        TsFunctionDecl ParseTsIndexer(ref TokenPointer tp)
        {
            var func = new TsFunctionDecl { IsIndexer = true };
            tp = ParseBracketParametersAndReturnType(tp, func, "[", "]");
            return func;

        }

        TsFieldDecl ParseTsField(ref TokenPointer tp)
        {
            var prm = ParseTsParameter(ref tp);
            tp = tp.Verify(t => t.IsOperator(";")).Next();
            return new TsFieldDecl { Token = prm.Token, Type = prm.Type, Name = prm.Name, IsOptional = prm.IsOptional };
        }

        TsFunctionDecl ParseTsFunction(ref TokenPointer tp)
        {
            var func = new TsFunctionDecl { Token = tp };
            if (tp.Value.IsIdentifier("static")) // static extend(properties: any, classProperties?: any): any;
            {
                func.IsStatic = true;
                tp = tp.Next();
            }
            if (tp.Value.IsIdentifier()) //doesn't happen at: export function (booleanValue: bool, message?: string);
            {
                func.Name = tp.Value.Text;
                tp = tp.Next();
            }

            TokenPointer optional = null;
            if (tp.Value.IsOperator("?"))
            {
                optional = tp;
                tp = tp.Next();
                func.IsOptional = true;
            }

            tp = ParseBracketParametersAndReturnType(tp, func, "(", ")");
            return func;
        }

        TokenPointer ParseBracketParametersAndReturnType(TokenPointer tp, TsFunctionDecl func, string openBracket, string closeBracket)
        {
            tp = ParseBracketParameters(tp, func, openBracket, closeBracket);

            if (tp.Value.IsOperator(":"))
            {
                tp = tp.Next();
                func.Type = ParseTypeRef(ref tp);
            }
            tp = tp.Verify(t => t.IsOperator(";")).Next();
            return tp;
        }

        TokenPointer ParseBracketParameters(TokenPointer tp, TsFunctionDecl func, string openBracket, string closeBracket)
        {
            tp = tp.Verify(t => t.IsOperator(openBracket)).Next();

            if (!tp.Value.IsOperator(closeBracket))
            {
                while (true)
                {
                    var prm = ParseTsParameter(ref tp);
                    func.Parameters.Add(prm);
                    if (!tp.Value.IsOperator(","))
                        break;
                    tp = tp.Next();
                }
            }
            tp = tp.Verify(t => t.IsOperator(closeBracket)).Next();
            return tp;
        }
        TsParameter ParseTsParameter(ref TokenPointer tp)
        {
            var isParams = false;
            if (tp.Value.Is(TokenTypes.ArgAny)) //...
            {
                isParams = true;
                tp = tp.Next();
            }
            tp = tp.Verify(t => t.IsIdentifier());

            var varName = tp;
            TokenPointer optional = null;
            tp = tp.Next();
            if (tp.Value.IsOperator("?"))
            {
                optional = tp;
                tp = tp.Next();
            }
            var node = new TsParameter { Token = varName, Name = varName.Value.Text, IsOptional = optional != null, IsParams = isParams };

            if (!tp.Value.IsOperator(":"))
                return node; //allow Echo(s);
            tp = tp.Verify(t => t.IsOperator(":")).Next();

            node.Type = ParseTypeRef(ref tp);
            //tp = tp.Next();
            return node;
        }

        TsTypeRef ParseTypeRef(ref TokenPointer tp)
        {
            TsTypeRef tr;
            if (tp.Value.IsOperator("("))
            {
                //delegate
                var func = new TsFunctionDecl { IsDelegate = true };
                tp = ParseBracketParameters(tp, func, "(", ")");
                tp = tp.Verify(t => t.Is(TokenTypes.LambdaOperator)).Next(); //=>
                func.Type = ParseTypeRef(ref tp);
                tr = new TsDelegateTypeRef { Decl = func };
            }
            else if (tp.Value.IsOperator("{"))
            {
                var tr2 = new TsInterfaceDeclRef { Token = tp, Decl = new TsTypeDecl() };
                tp = ParseBracesAndMembers(tp, tr2.Decl);
                tr = tr2;
            }
            else
            {
                tp = tp.Verify(t => t.IsIdentifier());
                var typeRef = new TsNamedTypeRef { Token = tp, Name = tp.Value.Text };
                tr = typeRef;
                tp = tp.Next();
                var previous = typeRef;
                while (tp.Value.IsOperator("."))
                {
                    tp = tp.Next().Verify(t => t.IsIdentifier());
                    var me = new TsMemberTypeRef { Name = tp.Value.Text, Previous = previous };
                    previous = me;
                    tr = me;
                    tp = tp.Next();
                }

            }
            if (tp.Value.IsOperator("[") && tp.Next().Value.IsOperator("]"))
            {
                tp = tp.Next();
                tp = tp.Next();
                tr.IsArray = true;
            }
            if (tp.Value.IsOperator("[") && tp.Next().Value.IsOperator("]"))
            {
                tp = tp.Next();
                tp = tp.Next();
                tr.IsDoubleArray = true;
            }
            return tr;
        }


        public TsTokenTypes TokenTypes { get; set; }

    }

}
