using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _074_Parser.Enigne.Parser
{
    public class ParserValuesList
    {
        public static string SelectedSourceType { get; set; }

        public static OneRow SelectedFilter { get; set; }

        public static List<OneRow> FilterOptions { get; set; }

        public class OneRow
        {
            public string Name { get; set; }

            public string URL { get; set; }
        }
    }
}
