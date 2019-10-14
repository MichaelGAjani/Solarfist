// FileInfo
// File:"CSVHelper.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:CSV File Operate
// 1.ReadCSVFile(string file, bool includeColumnTitle)
//
// File Lines:57

using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Jund.NETHelper.FileHelper
{
    /// <summary>
    /// CSV 文件操作类
    /// </summary>
    public class CSVHelper
    {
        public DataTable ReadCSVFile(string file, bool includeColumnTitle)
        {
            DataTable table = new DataTable();

            StreamReader reader = new StreamReader(file);

            List<string> columnList = reader.ReadLine().Split(',').ToList();

            foreach (string col in columnList)
            {
                if (includeColumnTitle)
                    table.Columns.Add(col);
                else
                    table.Columns.Add();
            }

            if (!includeColumnTitle) table.NewRow().ItemArray = columnList.ToArray();

            while (!reader.EndOfStream)
            {
                List<string> row = reader.ReadLine().Split(',').ToList();
                table.NewRow().ItemArray = columnList.ToArray();
            }

            return table;
        }
    }
}
