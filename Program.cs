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
            //string file = File.ReadAllText(args[0]);
            string file = File.ReadAllText("graf100.gv");
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
            Console.WriteLine(wyniki.Count);
            var sb = new StringBuilder();
            for (int i = wyniki.Count-1; i >=0 ; i--)
            {                
                sb.Append(wyniki[i]);
                sb.Append(" -> ");
            }
            sb.Append(data.Length - 1);
            Console.WriteLine(sb.ToString());
        }
        struct Info
        {
            public bool Visited { get; set; }
            public int Previous { get; set; }
            public int Dist { get; set; }
        }

        static Info[] DFS(int[,] matrix, int start)
        {
            Info[] tab = new Info[matrix.GetLength(0)];
            for (int i = 0; i < tab.Length; i++)
            {
                tab[i] = new Info();
                tab[i].Visited = false;
                tab[i].Previous = -1;
            }
            Visit(matrix, tab, start, 0);
            for (int i = 0; i < tab.Length; i++)
            {
                if (!tab[i].Visited)
                    Visit(matrix, tab, i, 0);
            }
            return tab;
        }
        static void Visit(int[,] matrix, Info[] tab, int u, int dist)
        {
            tab[u].Visited = true;
            tab[u].Dist = dist;
            
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (matrix[u, i] > 0 && !tab[i].Visited)
                {
                    tab[i].Previous = u;
                    Visit(matrix, tab, i, dist + 1);
                }
            }
        }
    }
}
