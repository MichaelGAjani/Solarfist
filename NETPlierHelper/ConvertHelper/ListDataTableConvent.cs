// FileInfo
// File:"ListDataTableConvent.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:ListDataTableConvent
// 1.ConvertListToDataTable<T>(IList<T> list)
// 2.ConvertDataTableToList(DataTable table)
//
// File Lines:92
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Jund.NETHelper.ConvertHelper
{
    /// <summary>
    /// DataTable与List转换帮助类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ListDataTableConvent<T> where T : new()
    {
        public static DataTable ConvertListToDataTable<T>(IList<T> list)
        {
            DataTable result = new DataTable();

            result.TableName = typeof(T).FullName;

            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }

            GC.Collect();
            return result;
        }

        public static List<T> ConvertDataTableToList(DataTable table)
        {
            List<T> list = new List<T>();

            PropertyInfo[] propertys = typeof(T).GetProperties();

            foreach (DataRow row in table.Rows)
            {
                T t = new T();

                foreach (PropertyInfo pi in propertys)
                {
                    if (table.Columns.Contains(pi.Name) && pi.CanWrite)
                    {
                        if (row[pi.Name] == System.DBNull.Value)
                            pi.SetValue(t, null, null);
                        else
                            pi.SetValue(t, row[pi.Name], null);
                    }
                }

                list.Add(t);
            }

            GC.Collect();

            return list;
        }
    }
}
