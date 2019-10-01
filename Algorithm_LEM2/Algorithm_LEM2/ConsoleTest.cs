using System;
using System.Collections.Generic;

namespace Algorithm_LEM2
{
    /// <summary>
    /// Class to test the library
    /// </summary>
    class ConsoleTest
    {
        static void Main(string[] args)
        {
            DataSet x = new DataSet();
            x.Attributes = new List<string> { "Wiek", "Wada_wzroku", "Astygmatyzm", "Lzawienie", "SOCZEWKI" };
            Dictionary<string, string> row1 = new Dictionary<string, string>();
            row1.Add("Wiek", "mlody");
            row1.Add("Wada_wzroku", "krotkowidz");
            row1.Add("Astygmatyzm", "nie");
            row1.Add("Lzawienie", "normalne");
            row1.Add("SOCZEWKI", "miekkie");
            Dictionary<string, string> row2 = new Dictionary<string, string>();
            row2.Add("Wiek", "mlody");
            row2.Add("Wada_wzroku", "dalekowidz");
            row2.Add("Astygmatyzm", "tak");
            row2.Add("Lzawienie", "normalne");
            row2.Add("SOCZEWKI", "twarde");
            Dictionary<string, string> row3 = new Dictionary<string, string>();
            row3.Add("Wiek", "mlody");
            row3.Add("Wada_wzroku", "dalekowidz");
            row3.Add("Astygmatyzm", "nie");
            row3.Add("Lzawienie", "zmniejszone");
            row3.Add("SOCZEWKI", "brak");
            Dictionary<string, string> row4 = new Dictionary<string, string>();
            row4.Add("Wiek", "mlody");
            row4.Add("Wada_wzroku", "krotkowidz");
            row4.Add("Astygmatyzm", "tak");
            row4.Add("Lzawienie", "zmniejszone");
            row4.Add("SOCZEWKI", "brak");
            Dictionary<string, string> row5 = new Dictionary<string, string>();
            row5.Add("Wiek", "prestarczy");
            row5.Add("Wada_wzroku", "krotkowidz");
            row5.Add("Astygmatyzm", "tak");
            row5.Add("Lzawienie", "zmniejszone");
            row5.Add("SOCZEWKI", "brak");
            Dictionary<string, string> row6 = new Dictionary<string, string>();
            row6.Add("Wiek", "prestarczy");
            row6.Add("Wada_wzroku", "krotkowidz");
            row6.Add("Astygmatyzm", "nie");
            row6.Add("Lzawienie", "normalne");
            row6.Add("SOCZEWKI", "miekkie");
            Dictionary<string, string> row7 = new Dictionary<string, string>();
            row7.Add("Wiek", "mlody");
            row7.Add("Wada_wzroku", "krotkowidz");
            row7.Add("Astygmatyzm", "tak");
            row7.Add("Lzawienie", "normalne");
            row7.Add("SOCZEWKI", "twarde");
            Dictionary<string, string> row8 = new Dictionary<string, string>();
            row8.Add("Wiek", "starczy");
            row8.Add("Wada_wzroku", "dalekowidz");
            row8.Add("Astygmatyzm", "tak");
            row8.Add("Lzawienie", "zmniejszone");
            row8.Add("SOCZEWKI", "brak");
            Dictionary<string, string> row9 = new Dictionary<string, string>();
            row9.Add("Wiek", "prestarczy");
            row9.Add("Wada_wzroku", "dalekowidz");
            row9.Add("Astygmatyzm", "nie");
            row9.Add("Lzawienie", "zmniejszone");
            row9.Add("SOCZEWKI", "brak");
            Dictionary<string, string> row10 = new Dictionary<string, string>();
            row10.Add("Wiek", "prestarczy");
            row10.Add("Wada_wzroku", "dalekowidz");
            row10.Add("Astygmatyzm", "tak");
            row10.Add("Lzawienie", "zmniejszone");
            row10.Add("SOCZEWKI", "brak");
            Dictionary<string, string> row11 = new Dictionary<string, string>();
            row11.Add("Wiek", "prestarczy");
            row11.Add("Wada_wzroku", "krotkowidz");
            row11.Add("Astygmatyzm", "nie");
            row11.Add("Lzawienie", "zmniejszone");
            row11.Add("SOCZEWKI", "brak");
            Dictionary<string, string> row12 = new Dictionary<string, string>();
            row12.Add("Wiek", "mlody");
            row12.Add("Wada_wzroku", "dalekowidz");
            row12.Add("Astygmatyzm", "tak");
            row12.Add("Lzawienie", "zmniejszone");
            row12.Add("SOCZEWKI", "brak");
            Dictionary<string, string> row13 = new Dictionary<string, string>();
            row13.Add("Wiek", "starczy");
            row13.Add("Wada_wzroku", "krotkowidz");
            row13.Add("Astygmatyzm", "tak");
            row13.Add("Lzawienie", "normalne");
            row13.Add("SOCZEWKI", "twarde");
            Dictionary<string, string> row14 = new Dictionary<string, string>();
            row14.Add("Wiek", "prestarczy");
            row14.Add("Wada_wzroku", "dalekowidz");
            row14.Add("Astygmatyzm", "nie");
            row14.Add("Lzawienie", "normalne");
            row14.Add("SOCZEWKI", "miekkie");
            Dictionary<string, string> row15 = new Dictionary<string, string>();
            row15.Add("Wiek", "starczy");
            row15.Add("Wada_wzroku", "krotkowidz");
            row15.Add("Astygmatyzm", "nie");
            row15.Add("Lzawienie", "normalne");
            row15.Add("SOCZEWKI", "brak");
            Dictionary<string, string> row16 = new Dictionary<string, string>();
            row16.Add("Wiek", "prestarczy");
            row16.Add("Wada_wzroku", "krotkowidz");
            row16.Add("Astygmatyzm", "tak");
            row16.Add("Lzawienie", "normalne");
            row16.Add("SOCZEWKI", "twarde");
            Dictionary<string, string> row17 = new Dictionary<string, string>();
            row17.Add("Wiek", "mlody");
            row17.Add("Wada_wzroku", "krotkowidz");
            row17.Add("Astygmatyzm", "nie");
            row17.Add("Lzawienie", "zmniejszone");
            row17.Add("SOCZEWKI", "brak");
            Dictionary<string, string> row18 = new Dictionary<string, string>();
            row18.Add("Wiek", "starczy");
            row18.Add("Wada_wzroku", "krotkowidz");
            row18.Add("Astygmatyzm", "nie");
            row18.Add("Lzawienie", "zmniejszone");
            row18.Add("SOCZEWKI", "brak");
            Dictionary<string, string> row19 = new Dictionary<string, string>();
            row19.Add("Wiek", "starczy");
            row19.Add("Wada_wzroku", "dalekowidz");
            row19.Add("Astygmatyzm", "nie");
            row19.Add("Lzawienie", "zmniejszone");
            row19.Add("SOCZEWKI", "brak");
            Dictionary<string, string> row20 = new Dictionary<string, string>();
            row20.Add("Wiek", "mlody");
            row20.Add("Wada_wzroku", "dalekowidz");
            row20.Add("Astygmatyzm", "nie");
            row20.Add("Lzawienie", "normalne");
            row20.Add("SOCZEWKI", "miekkie");
            Dictionary<string, string> row21 = new Dictionary<string, string>();
            row21.Add("Wiek", "starczy");
            row21.Add("Wada_wzroku", "dalekowidz");
            row21.Add("Astygmatyzm", "nie");
            row21.Add("Lzawienie", "normalne");
            row21.Add("SOCZEWKI", "miekkie");
            Dictionary<string, string> row22 = new Dictionary<string, string>();
            row22.Add("Wiek", "prestarczy");
            row22.Add("Wada_wzroku", "dalekowidz");
            row22.Add("Astygmatyzm", "tak");
            row22.Add("Lzawienie", "normalne");
            row22.Add("SOCZEWKI", "brak");
            x.Rows.Add(row1);
            x.Rows.Add(row2);
            x.Rows.Add(row3);
            x.Rows.Add(row4);
            x.Rows.Add(row5);
            x.Rows.Add(row6);
            x.Rows.Add(row7);
            x.Rows.Add(row8);
            x.Rows.Add(row9);
            x.Rows.Add(row10);
            x.Rows.Add(row11);
            x.Rows.Add(row12);
            x.Rows.Add(row13);
            x.Rows.Add(row14);
            x.Rows.Add(row15);
            x.Rows.Add(row16);
            x.Rows.Add(row17);
            x.Rows.Add(row18);
            x.Rows.Add(row19);
            x.Rows.Add(row20);
            x.Rows.Add(row21);
            x.Rows.Add(row22);

            Console.WriteLine("Begin algorithm \n");

            x.StartAlgorithmLEM2();
            Console.WriteLine(x.GetRulesAsString());

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            foreach (var item in x.tmpDeletedRules)
            {
                Console.WriteLine(item.ToString());
            }
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("End");

            Console.ReadKey();
        }
    }
}
