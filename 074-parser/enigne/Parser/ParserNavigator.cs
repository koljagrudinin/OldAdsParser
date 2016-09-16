using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Threading;

namespace _074_Parser.Enigne.Parser
{
    class ParserNavigator
    {

        public static void DoStart(string filter)
        {
            switch (ParserValuesList.SelectedSourceType)
            {
                case "Желтые страницы":
                    ParseYellowPages(filter);
                    break;
            }
        }

        private static void ParseYellowPages(string filter)
        {
            YPParser ypp=new YPParser();
            ypp.GetList();
        }

        public static List<string> GetURLs(string row)
        {
            List<string> temp = new List<string>();

            switch (ParserValuesList.SelectedSourceType)
            {
                case "Желтые страницы":
                    YPParser ypp = new YPParser();
                    temp = ypp.ParseURIs(row);
                    break;
            }


            return temp;
        }

        public static List<string> GetInnerURLs(string row)
        {
            List<string> temp = new List<string>();

            switch (ParserValuesList.SelectedSourceType)
            {
                case "Желтые страницы":
                    YPParser ypp = new YPParser();
                    temp = ypp.ParseIneerURL(row);
                    break;
            }


            return temp;
        }

        public static void ParseRow(string data)
        {
            switch (ParserValuesList.SelectedSourceType)
            {
                case "Желтые страницы":
                    YPParser ypp = new YPParser();
                    ypp.ParseRow(data);
                    break;
            }
        }
    }
}
