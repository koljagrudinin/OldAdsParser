using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace _074_Parser.Enigne.Parser
{
    public class Data
    {
        private static ObservableCollection<DataRow> data = new ObservableCollection<DataRow>();

        public static ObservableCollection<DataRow> DATA { get { return data; } set { data = value; } }
    }
}
