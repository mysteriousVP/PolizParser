using System.Collections.Generic;
using System.Linq;

namespace LexicalAnalyzer
{
    public class TokenTable
    {
        public static readonly List<uint> LogicOpCodes = new List<uint>()
        {
            (uint)Code.Equal,
            (uint)Code.NotEqual,
            (uint)Code.Less,
            (uint)Code.Greater
        };

        public static readonly Dictionary<uint, string> CodesTable = new Dictionary<uint, string>
        {
            { (uint)Code.If, "if"},
            { (uint)Code.End, "end"},
            { (uint)Code.Print, "writeline"},
            { (uint)Code.Int, "int"},
            { (uint)Code.Program, "program"},
            { (uint)Code.Quote, "\""},
            { (uint)Code.Plus, "+"},
            { (uint)Code.Minus, "-"},
            { (uint)Code.Mul, "*"},
            { (uint)Code.Div, "/"},
            { (uint)Code.Pow, "^"},
            { (uint)Code.Equal, "=="},
            { (uint)Code.Assign, "="},
            { (uint)Code.NotEqual, "!="},
            { (uint)Code.Less, "<"},
            { (uint)Code.Greater, ">"},
            { (uint)Code.For, "for"},
            { (uint)Code.Semicolon, ";"},
            { (uint)Code.OpenBrackets, "("},
            { (uint)Code.CloseBrackets, ")"},
            { (uint)Code.OpenSep, "{"},
            { (uint)Code.CloseSep, "}"}
        };
        public struct Token
        {
            /// <summary>
            /// line of source file where was current token
            /// </summary>
            public uint Line;
            /// <summary>
            /// Position on line where current token starts
            /// </summary>
            public uint Position;
            /// <summary>
            /// just lexeme
            /// </summary>
            public string Lexeme;
            /// <summary>
            /// token code in tokens table in specification
            /// </summary>
            public uint Code;
            /// <summary>
            /// id of lexeme in otherwise table
            /// </summary>
            public uint ForeignId;
        }

        public readonly LinkedList<Token> TokensList = new LinkedList<Token>();

        public readonly Dictionary<uint, string> LiteralsTable = new Dictionary<uint, string>();
        public readonly Dictionary<uint, string> IdentifiersTable = new Dictionary<uint, string>();

        public enum Code : uint
        {
            If = 1,
            End,
            Print,
            Int,
            Program,
            Quote,
            Plus,
            Minus,
            Mul,
            Div,
            Pow,
            Equal,
            Assign,
            NotEqual,
            Less,
            Greater,
            For,
            Semicolon,
            OpenBrackets,
            CloseBrackets,
            OpenSep,
            CloseSep,
            ConstantNumber,
            Id,
            Ws,
            String = 101
        }

        public void AddToken(uint line, uint position, string lexeme, int stateId)
        {
            var token = new Token { Line = line, Position = position, Lexeme = lexeme, ForeignId = 0 };
            
            // checking if lexeme is language specific
            if (CodesTable.ContainsValue(lexeme))
            {
                token.Code = CodesTable.First(pair => pair.Value == lexeme).Key;
            }
            else
            {
                // identifiers
                if (stateId == 3)
                {
                    token.Code = (uint)Code.Id;
                    if (!IdentifiersTable.ContainsValue(lexeme))
                    {
                        IdentifiersTable.Add((uint)(IdentifiersTable.Count+1), lexeme);
                    }
                    token.ForeignId = IdentifiersTable.First(pair => pair.Value == lexeme).Key;
                }
                // string
                else if (stateId == 23)
                {
                    token.Code = (uint)Code.String;
                    if (!IdentifiersTable.ContainsValue(lexeme))
                    {
                        IdentifiersTable.Add((uint)(IdentifiersTable.Count + 1), lexeme);
                    }
                    token.ForeignId = IdentifiersTable.First(pair => pair.Value == lexeme).Key;
                }
                // (int | float) literals
                else
                {
                    // 2 - int 3 - float
                    token.Code = (uint) (stateId == 7 ? (uint)Code.ConstantNumber : 99);

                    if (!LiteralsTable.ContainsValue(lexeme))
                    {
                        LiteralsTable.Add((uint)(LiteralsTable.Count+1), lexeme);
                    }
                    token.ForeignId = LiteralsTable.First(pair => pair.Value == lexeme).Key;
                }
            }

            TokensList.AddLast(token);
        }

        public static string GetLexemeName(uint code)
        {
            if (code == (uint)Code.Id) return "id";
            if (code == (uint)Code.ConstantNumber) return "c_int";
            if (code == 101) return "string";

            return CodesTable[code];
        }

        public static uint GetLexemeId(string lexeme)
        {
            if (lexeme == "id") return (uint)Code.Id;
            if (lexeme == "c_int") return (uint)Code.ConstantNumber;
            if (lexeme == "c_float") return 99;
            if (lexeme == "string") return 101;
            return CodesTable.First(p => p.Value == lexeme).Key;
        }
    }
}