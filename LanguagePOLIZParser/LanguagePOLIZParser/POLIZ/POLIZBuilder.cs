using System.Collections.Generic;
using LexicalAnalyzer;

namespace PolizParser
{
    class POLIZBuilder
    {
        private List<uint> operators;
        private List<uint> standart_operators =
            new List<uint>(new []
            {
                (uint)TokenTable.Code.OpenBrackets,
                (uint)TokenTable.Code.Assign,
                (uint)TokenTable.Code.CloseBrackets, 
                (uint)TokenTable.Code.Plus,
                (uint)TokenTable.Code.Minus,
                (uint)TokenTable.Code.Mul,
                (uint)TokenTable.Code.Div,
                (uint)TokenTable.Code.Pow
            });

        private readonly List<TokenTable.Token> outputData;
        private readonly Stack<TokenTable.Token> stack;

        public POLIZBuilder()
        {
            outputData = new List<TokenTable.Token>();
            stack = new Stack<TokenTable.Token>();
            operators = new List<uint>(standart_operators);
        }

        private byte GetPriority(uint code)
        {
            switch (code)
            {
                case 19:
                case 20:
                    return 0;
                case 7:
                case 8:
                    return 1;
                case 9:
                case 10:
                    return 2;
                case 11: 
                    return 3;
                default:
                    return 4;
            }
        }

        public List<TokenTable.Token> End()
        {
            if (stack.Count > 0)
            {
                foreach (var c in stack)
                {
                    outputData.Add(c);
                }
            }

            return outputData;
        }
        public void Append(TokenTable.Token token)
        {
            if (operators.Contains(token.Code))
            {
                if (stack.Count > 0 && !token.Code.Equals((uint)TokenTable.Code.OpenBrackets))
                {
                    if (token.Code.Equals((uint)TokenTable.Code.CloseBrackets))
                    {
                        var s = stack.Pop();
                        while (s.Code != (uint)TokenTable.Code.OpenBrackets)
                        {
                            outputData.Add(s);
                            s = stack.Pop();
                        }
                    }
                    else if (GetPriority(token.Code) > GetPriority(stack.Peek().Code))
                        stack.Push(token);
                    else
                    {
                        while (stack.Count > 0 && GetPriority(token.Code) <= GetPriority(stack.Peek().Code))
                            outputData.Add(stack.Pop());
                        stack.Push(token);
                    }
                }
                else
                    stack.Push(token);
            }
            else
            {
                outputData.Add(token);
            }
        }

    }
}
