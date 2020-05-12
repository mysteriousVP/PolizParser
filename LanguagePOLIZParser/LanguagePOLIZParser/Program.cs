using System;
using System.IO;
using LexicalAnalyzer;

namespace PolizParser
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var tokens = new LexicalAnalyzer.LexicalAnalyzer()
                    .Analyze(@"C:\Users\User\Desktop\6 семестр\САПР-2\LanguagePOLIZParser\LanguagePOLIZParser\example.diamond");

                Console.WriteLine("Tokens table");
                using (StreamWriter sw = new StreamWriter("ex.txt"))
                {
                    foreach(var token in tokens.TokensList)
                    {
                        Console.WriteLine($"Line:{token.Line}; <{token.Lexeme}>; #{token.Code}; P:{token.Position}");
                        sw.WriteLine($"Line:{token.Line}; <{token.Lexeme}>; #{token.Code}; P:{token.Position}");
                    }
                }

                Console.WriteLine();
                Console.WriteLine(new String('#', 40));
                foreach (var token in tokens.IdentifiersTable)
                {
                    Console.WriteLine($"Key: {token.Key}; Name: <{token.Value}>;");
                }
                Console.WriteLine(new String('#', 40));
                Console.WriteLine();

                var tree = new POLIZ(tokens)
                        .Analyze();


                Console.WriteLine();
                Console.WriteLine(new String('#', 40));
                foreach (var token in tokens.IdentifiersTable)
                {
                    Console.WriteLine($"Key: {token.Key}; Name: <{token.Value}>;");
                }
                Console.WriteLine(new String('#', 40));
                Console.WriteLine();

                Console.WriteLine("Syntax tree");
                PrintSyntaxTree(tokens, tree);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
        }

        static void PrintSyntaxTree(TokenTable tokensTable, CustomTree node, int level = 0)
        {
            Console.Write($"{new string('\t', level)} {node.Name}");
            if (node.Id != 0)
            {
                Console.Write(node.Name == "identifier"
                    ? $"\"{tokensTable.IdentifiersTable[node.Id]}\""
                    : $"\"{tokensTable.LiteralsTable[node.Id]}\"");
                Console.Write($" ({node.Id})");
            }

            Console.WriteLine();

            if (node.GetChildren() == null) 
                return;

            foreach (var child in node.GetChildren())
            {
                PrintSyntaxTree(tokensTable, child, level+1);
            }
        }
    }
}
