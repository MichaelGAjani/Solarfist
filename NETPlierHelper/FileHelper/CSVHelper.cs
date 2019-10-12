using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.NETHelper.FileHelper
{
    public class CSVHelper
    {
        public DataTable ReadCSVFile(string file, bool includeColumnTitle)
        {
            DataTable table = new DataTable();

            StreamReader reader = new StreamReader(file);


            List<string> col_list = reader.ReadLine().Split(',').ToList();

            foreach (string col in col_list)
            {
                if (includeColumnTitle)
                    table.Columns.Add(col);
                else
                    table.Columns.Add();
            }

            if (!includeColumnTitle) table.NewRow().ItemArray = col_list.ToArray();

            while (!reader.EndOfStream)
            {
                List<string> row = reader.ReadLine().Split(',').ToList();
                table.NewRow().ItemArray = col_list.ToArray();
            }

            return table;
        }
    }
}
