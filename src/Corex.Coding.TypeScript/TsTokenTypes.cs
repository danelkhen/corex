using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeScriptParser.Parser;

namespace TypeScriptParser
{
    public class TsTokenTypes
    {
        public static TsTokenTypes TypeScript = new TsTokenTypes();
        public TokenType Whitespace { get; set; }
        public TokenType Identifier { get; set; }
        public TokenType Comment2 { get; set; }
        public TokenType Comment { get; set; }
        public TokenType LambdaOperator { get; set; }
        public TokenType ArgAny { get; set; }
        public TokenType Operator { get; set; }
        public TsTokenTypes()
        {
            Whitespace = new TokenType("Whitespace", TryParseWhitespace);
            Identifier = new TokenType("Identifier", TryParseIdentifier);
            Comment2 = new TokenType("Comment2", TryParseComment2);
            Comment = new TokenType("Comment", TryParseComment);
            LambdaOperator = new TokenType("LambdaOperator", TryParseLambdaOperator);
            ArgAny = new TokenType("ArgAny", TryParseArgAny);
            Operator = new TokenType("Operator", TryParseOperator);
            StringLiteral = new TokenType("StringLiteral", TryParseStringLiteral);
            All = new List<TokenType>
            {
                Whitespace,
                Identifier,
                Comment2,     
                Comment,    
                LambdaOperator,
                ArgAny,
                Operator,
                StringLiteral,
            };
            Comments = new[] { Comment, Comment2 };
            CommentsOrWhitespace = new[] { Comment, Comment2, Whitespace };
        }

        public StringSelection TryParseStringLiteral(StringLocation loc)
        {
            var s = loc.Select().ExtendIfAfterEqualsTo("\"");
            if (s.IsEmpty)
                return s;
            s = s.ExtendCharsUntil(ch => ch == '\"');
            s = s.ExtendBy(1);
            return s;
        }
        public StringSelection TryParseWhitespace(StringLocation loc)
        {
            return loc.Select().ExtendCharsAsLongAs(ch => "\r\n ".Contains(ch));
        }
        public StringSelection TryParseIdentifier(StringLocation loc)
        {
            return loc.Select().ExtendCharsAsLongAs(ch => "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_$".Contains(ch));
        }
        public StringSelection TryParseComment2(StringLocation loc)
        {
            var commentStart = loc.Select().ExtendIfAfterEqualsTo("//");
            if (commentStart.IsEmpty)
                return commentStart;
            var comment = commentStart.ExtendCharsUntil(ch => "\r\n".Contains(ch));
            return comment;
        }
        public StringSelection TryParseComment(StringLocation loc)
        {
            var commentStart = loc.Select().ExtendIfAfterEqualsTo("/*");
            if (commentStart.IsEmpty)
                return commentStart;
            var comment = commentStart.ExtendUntilIncluding("*/");
            return comment;
        }
        public StringSelection TryParseLambdaOperator(StringLocation loc)
        {
            return loc.Select().ExtendIfAfterEqualsTo("=>");
        }
        public StringSelection TryParseArgAny(StringLocation loc)
        {
            return loc.Select().ExtendIfAfterEqualsTo("...");
        }
        public StringSelection TryParseOperator(StringLocation loc)
        {
            var ch = loc.Select(1);
            if (!ch.IsValid)
                return loc.Select();
            if (":?;()[],{}=.><".Contains(ch.Text))
            {
                return ch;
            }
            return loc.Select();
        }


        public List<TokenType> All { get; set; }

        public TokenType[] Comments { get; set; }

        public TokenType[] CommentsOrWhitespace { get; set; }

        public TokenType StringLiteral { get; set; }
    }


}
