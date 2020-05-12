using System;
using System.Collections.Generic;
using System.Linq;
using LexicalAnalyzer;

namespace PolizParser
{
    public partial class POLIZ
    {
        private LinkedListNode<TokenTable.Token> tokenListNode;

        public POLIZ(TokenTable tokenTable)
        {
            tokenListNode = tokenTable.TokensList.First;
        }

        public CustomTree Analyze()
        {
            return _Program();
        }

        CustomTree _Program()
        {
            CustomTree node = new CustomTree("test");
            
            while (tokenListNode != null)
            {
                node.AddChild(Statement());
            }

            return node;
        }

        uint GetTokenCode()
        {
            return tokenListNode.Value.Code;
        }

        void CheckUnexpectedEnd()
        {
            if (tokenListNode == null)
                throw new Exception("Unexpected end of the program.");
        }

        void TransitToNextToken()
        {
            CheckUnexpectedEnd();

            tokenListNode = tokenListNode.Next;
        }

        TokenTable.Token ExpectedOneOfTokens(string expected, params uint[] tokenCodes)
        {
            CheckUnexpectedEnd();

            if (tokenCodes.All(tokenCode => GetTokenCode() != tokenCode))
                throw new Exception($"Expected {expected} but found \"{TokenTable.GetLexemeName(GetTokenCode())}\" in line {tokenListNode.Value.Line}.");

            var token = tokenListNode.Value;
            TransitToNextToken();
            return token;
        }

        TokenTable.Token GetCurrent()
        {
            return tokenListNode.Value;
        }

        TokenTable.Token ExpectedToken(uint tokenCode)
        {
            CheckUnexpectedEnd();
            
            if (GetTokenCode() != tokenCode)
                throw new Exception($"Expected \"{TokenTable.GetLexemeName(tokenCode)}\" but found \"{TokenTable.GetLexemeName(GetTokenCode())}\" in line {tokenListNode.Value.Line}.");

            var token = tokenListNode.Value;
            TransitToNextToken();
            return token;
        }

        TokenTable.Token ExpectLogicalOp()
        {
            CheckUnexpectedEnd();

            if (!TokenTable.LogicOpCodes.Contains(GetTokenCode()))
            {
                throw new Exception($"Expected \"{String.Join(", ", TokenTable.LogicOpCodes)}\" but found \"{TokenTable.GetLexemeName(GetTokenCode())}\" in line {tokenListNode.Value.Line}.");
            }

            var token = tokenListNode.Value;
            TransitToNextToken();
            return token;
        }
    }
}