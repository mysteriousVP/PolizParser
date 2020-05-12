using System;
using LexicalAnalyzer;

namespace PolizParser
{
    public partial class POLIZ
    {
        CustomTree LogicalExpression()
        {
            CustomTree node = new CustomTree("logic_expression");
            node.AddChild(Expression()); // Expression
            node.AddChild(new CustomTree(ExpectLogicalOp().Lexeme));
            node.AddChild(Expression());
            return node;
        }

        CustomTree AssignmentExpression()
        {
            CustomTree node = new CustomTree("assignment");
            var token = ExpectedToken((uint)TokenTable.Code.Id); // identifier
            node.AddChild(new CustomTree("identifier", token.ForeignId));
            ExpectedToken((uint)TokenTable.Code.Assign); // '='
            node.AddChild(Expression(token.Lexeme));

            return node;
        }

        CustomTree Expression(string idName = null, POLIZBuilder builder = null)
        {
            CustomTree node = new CustomTree("expression");
            builder = new POLIZBuilder();

            while (GetTokenCode() != (uint)TokenTable.Code.Semicolon && !TokenTable.LogicOpCodes.Contains(GetTokenCode()))
            {

                builder.Append(GetCurrent());
                TransitToNextToken();
            }
            var tokens = builder.End();

            if (idName != null)
            {
                CustomTree st = new CustomTree(idName);
                st.AddChild(new CustomTree(idName));
                foreach (var token in tokens)
                {
                    st.AddChild(token.Code == (uint)TokenTable.Code.Id
                        ? new CustomTree("identifier", token.ForeignId)
                        : new CustomTree(token.Lexeme));

                    node.AddChild(token.Code == (uint)TokenTable.Code.Id
                        ? new CustomTree("identifier", token.ForeignId)
                        : new CustomTree(token.Lexeme));
                }
                st.AddChild(new CustomTree("="));

                Console.Write("POLIZ: ");

                foreach (var child in st.GetChildren())
                {
                    Console.Write(child.Name + " ");
                }

                Console.WriteLine();
            }
            else
            {
                foreach (var token in tokens)
                {
                    node.AddChild(token.Code == (uint)TokenTable.Code.Id
                        ? new CustomTree("identifier", token.ForeignId)
                        : new CustomTree(token.Lexeme));
                }
            }
            return node;
        }
    }
}
