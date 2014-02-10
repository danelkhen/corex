using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeScriptParser.Parser;

namespace TypeScriptParser
{
    enum TsTypeKind
    {
        Interface,
        Anonymous,
        Module,
        Class
    }

    class TsTypeDecl : TsMemberDecl
    {
        public TsTypeDecl()
        {
            Members = new List<TsMemberDecl>();
            Extends = new List<TsTypeRef>();
            Implements = new List<TsTypeRef>();
        }
        public List<TsMemberDecl> Members { get; set; }

        public TsTypeKind Kind { get; set; }

        public List<TsTypeRef> Extends { get; set; }
        public List<TsTypeRef> Implements { get; set; }

        public override string ToString()
        {
            return String.Format("{0} {1}", Kind, Name);
        }
    }

    class TsFunctionDecl : TsMemberDecl
    {
        public TsFunctionDecl()
        {
            Parameters = new List<TsParameter>();
        }
        public List<TsParameter> Parameters { get; set; }
        public bool IsIndexer { get; set; }

        public bool IsDelegate { get; set; }
        public bool IsOptional { get; set; }

        public override string ToString()
        {
            return String.Format("{0}({1})", Name, String.Join(", ", Parameters.Select(t => t.Type + " " + t.Name).ToArray()));
        }

        public bool IsStatic { get; set; }
    }

    class TsFieldDecl : TsMemberDecl
    {
        public bool IsOptional { get; set; }
    }
    class TsMemberDecl : TsTypedNamedDecl
    {

    }
    class TsImportDecl : TsMemberDecl
    {

    }

    class TsVarDecl : TsMemberDecl
    {
    }

    class TsTypedNamedDecl : TsNode
    {
        public string Name { get; set; }
        public TsTypeRef Type { get; set; }
        public override string ToString()
        {
            return String.Format("{0} {1}", Name, Type);
        }
    }

    class TsParameter : TsTypedNamedDecl
    {
        public bool IsOptional { get; set; }
        public bool IsParams { get; set; }
    }

    public class TsNode
    {
        public TokenPointer Token { get; set; }
    }


    class TsTypeRef : TsNode
    {
        public bool IsArray { get; set; }
        public bool IsDoubleArray { get; set; }
    }
    class TsNamedTypeRef : TsTypeRef
    {
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
    class TsMemberTypeRef : TsNamedTypeRef
    {
        public TsNamedTypeRef Previous { get; set; }

        public override string ToString()
        {
            return Name + "." + Previous;
        }
    }
    class TsDelegateTypeRef : TsTypeRef
    {
        public TsFunctionDecl Decl { get; set; }
        public override string ToString()
        {
            return "Anonymous Delegate: " + Decl;
        }
    }
    class TsInterfaceDeclRef : TsTypeRef
    {
        public TsTypeDecl Decl { get; set; }
        public override string ToString()
        {
            return "Anonymous Interface: " + Decl;
        }
    }

    class TsModuleTypeRef : TsNamedTypeRef
    {
    }


}
