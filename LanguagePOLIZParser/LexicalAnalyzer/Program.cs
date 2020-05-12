using System;

namespace LexicalAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var tokensTable = new LexicalAnalyzer().Analyze("example.diamond");

                foreach (var token in tokensTable.TokensList)
                {
                    Console.Write($"{token.Line} {token.Lexeme} {token.Code}");

                    Console.WriteLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
