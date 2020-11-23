using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LongestPath
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = File.ReadAllText(args[0]);
            //string file = File.ReadAllText("graph.txt");
            string pattern = "\n(\\w+);";
            var matches = Regex.Matches(file, pattern).ToList();
            if (matches.Count == 0)
            {
                pattern = "\"(\\w+)\" ;";
                matches = Regex.Matches(file, pattern).ToList();
            }
            int[,] m = new int[matches.Count, matches.Count];

            var allNodes = new HashSet<string>();
            foreach (var match in matches)
            {
                allNodes.Add(match.Groups[1].Value);
            }

            pattern = "(\\w+) -> (\\w+);";
            matches = Regex.Matches(file, pattern).ToList();

            foreach (var match in matches)
            {
                
                m[int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)] = 1;
            }

            
            var data = DFS(m, 0);
            
            
            var wyniki = new List<int>();           
            
            int prev = data[m.GetLength(0)-1].Previous;
            do
            {                
                wyniki.Add(prev);
                prev = data[prev].Previous;
            } while (prev != 0);

            wyniki.Add(0);
            var sb = new StringBuilder();
            for (int i = wyniki.Count-1; i >=0 ; i--)
            {                
                sb.Append(wyniki[i]);
                sb.Append(" -> ");
            }
            sb.Append(data.Length - 1);
            Console.WriteLine(sb.ToString());
        }
        struct Data
        {
            public bool Visited { get; set; }
            public int Previous { get; set; }
            public int Dist { get; set; }
        }

        static Data[] DFS(int[,] matrix, int start)
        {
            Data[] data = new Data[matrix.GetLength(0)];
            for (int i = 0; i < data.Length; i++)
            {
                data[i].Visited = false;
                data[i].Previous = -1;
            }
            Visit(matrix, ref data, start, 0);
            for (int i = 0; i < data.Length; i++)
            {
                if (!data[i].Visited)
                    Visit(matrix, ref data, i, 0);
            }
            return data;
        }
        static void Visit(int[,] matrix, ref Data[] tab, int u, int dist)
        {
            tab[u].Visited = true;
            tab[u].Dist = dist;
            if (u == 5) return;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (matrix[u, i] > 0 && !tab[i].Visited)
                {
                    tab[i].Previous = u;
                    Visit(matrix, ref tab, i, dist + 1);
                }
            }
        }
    }
}
