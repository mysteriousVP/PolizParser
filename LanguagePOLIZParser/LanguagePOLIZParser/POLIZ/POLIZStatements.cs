using LexicalAnalyzer;
using System;

namespace PolizParser
{
    public partial class POLIZ
    {
        private CustomTree ExpressionStatement()
        {
            CustomTree node = Expression();
            ExpectedToken((uint)TokenTable.Code.Semicolon); // ';'
            return node;
        }

        private CustomTree ConditionStatement()
        {
            CustomTree node = new CustomTree("if");

            ExpectedToken((uint)TokenTable.Code.If); // 'if'
            ExpectedToken((uint)TokenTable.Code.OpenBrackets); // '('
            node.AddChild(LogicalExpression());
            TransitToNextToken();
            ExpectedToken((uint)TokenTable.Code.CloseBrackets); // ')'
            node.AddChild(Statement());

            return node;
        }

        private CustomTree LoopStatement()
        {
            CustomTree node = new CustomTree("for");

            ExpectedToken((uint)TokenTable.Code.For); // 'for'
            ExpectedToken((uint)TokenTable.Code.OpenBrackets); // '('
            node.AddChild(AssignmentExpression());
            ExpectedToken((uint)TokenTable.Code.Semicolon); // ';'
            node.AddChild(LogicalExpression());
            ExpectedToken((uint)TokenTable.Code.Semicolon); // ';'
            ExpectedToken((uint)TokenTable.Code.CloseBrackets); // ')'
            node.AddChild(Statement());
            return node;
        }

        private CustomTree DeclarationStatement()
        {
            CustomTree node = new CustomTree("declaration");

            //"int" | "float"
            var token = ExpectedOneOfTokens("TypeSpecifier", (uint)TokenTable.Code.Int, 100);
            node.AddChild(new CustomTree(TokenTable.GetLexemeName(token.Code)));
            // identifier
            token = ExpectedToken((uint)TokenTable.Code.Id);
            node.AddChild(new CustomTree("identifier", token.ForeignId));

            ExpectedToken((uint)TokenTable.Code.Semicolon); // ';'

            return node;
        }

        private CustomTree OutputStatement()
        {
            CustomTree node = new CustomTree("output");

            ExpectedToken((uint)TokenTable.Code.Print); // 'print'
            node.AddChild(Expression());
            ExpectedToken((uint)TokenTable.Code.Semicolon); // ';'
            
            return node;
        }

        private CustomTree AssignmentStatement()
        {
            CustomTree node = AssignmentExpression();
            ExpectedToken((uint)TokenTable.Code.Semicolon); // ';'
            return node;
        }

        private CustomTree ProgramStart()
        {
            ExpectedToken((uint)TokenTable.Code.Program); // 'program'

            return new CustomTree("start program");
        }

        private CustomTree ProgramEnd()
        {
            ExpectedToken((uint)TokenTable.Code.End); // 'end'

            return new CustomTree("end program");
        }
        private CustomTree Statement()
        {
            var node = GetTokenCode() switch
            {
                // 'program'
                (uint)TokenTable.Code.Program => ProgramStart(),
                // 'identifier'
                (uint)TokenTable.Code.Id => AssignmentStatement(),
                // 'print'
                (uint)TokenTable.Code.Print => OutputStatement(),
                // 'if'
                (uint)TokenTable.Code.If => ConditionStatement(),
                // 'for'
                (uint)TokenTable.Code.For => LoopStatement(),
                // 'int'
                (uint)TokenTable.Code.Int => DeclarationStatement(),
                // 'end'
                (uint)TokenTable.Code.End => ProgramEnd(),
                // default
                _ => ExpressionStatement()
            };

            return node;
        }
    }
}