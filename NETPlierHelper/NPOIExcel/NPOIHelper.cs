// FileInfo
// File:"NPOIHelper.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:Use Npoi Operate Excel
// 1.ExportHtml(string fileName, string htmlFile, bool outputColumnHeaders = false, bool outputHiddenColumns = false, bool outputHiddenRows = false, bool outputRowNumbers = false)
// 2.CreateExcel(string fileName, DataTable table)
// 3.CreateExcel(string fileName, string sheetName, List<object> list)
// 4.WriteCell(string fileName, int rowNum, int colNum, string value)
// 5.MergeCell(string fileName,int firstRow,int lastRow,int firstCol,int lastCol)
//
// File Lines:146
using NPOI.SS.Converter;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Jund.NETHelper.NPOIExcel
{
    public class NPOIHelper
    {
        public void ExportHtml(string fileName, string htmlFile, bool outputColumnHeaders = false, bool outputHiddenColumns = false, bool outputHiddenRows = false, bool outputRowNumbers = false)
        {
            ExcelToHtmlConverter excelToHtmlConverter = new ExcelToHtmlConverter();

            FileStream file = File.Open(fileName, FileMode.Open);
            IWorkbook workbook = new XSSFWorkbook(file);

            excelToHtmlConverter.OutputColumnHeaders = outputColumnHeaders;
            excelToHtmlConverter.OutputHiddenColumns = outputHiddenColumns;
            excelToHtmlConverter.OutputHiddenRows = outputHiddenRows;
            excelToHtmlConverter.OutputLeadingSpacesAsNonBreaking = true;
            excelToHtmlConverter.OutputRowNumbers = outputRowNumbers;
            excelToHtmlConverter.UseDivsToSpan = false;

            // 处理的Excel文件
            excelToHtmlConverter.ProcessWorkbook(workbook);
            //添加表格样式
            //excelToHtmlConverter.Document.InnerXml =
            //    excelToHtmlConverter.Document.InnerXml.Insert(
            //        excelToHtmlConverter.Document.InnerXml.IndexOf("<head>", 0) + 6,
            //        @"<style>table, td, th{border:1px solid green;}th{background-color:green;color:white;}</style>"
            //    );

            excelToHtmlConverter.Document.Save(htmlFile);
        }
        public void CreateExcel(string fileName, DataTable table)
        {
            FileStream file = File.Create(fileName);
            IWorkbook workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet(table.TableName);
            var iRow = sheet.CreateRow(0);

            foreach (DataColumn col in table.Columns)
            {
                iRow.CreateCell(iRow.Cells.Count).SetCellValue(col.Caption);
            }

            foreach (DataRow row in table.Rows)
            {
                iRow = sheet.CreateRow(sheet.LastRowNum + 1);

                object[] cell_list = row.ItemArray;

                foreach (object cell in cell_list)
                    iRow.CreateCell(iRow.Cells.Count).SetCellValue(cell.ToString());
            }

            workbook.Write(file);
            file.Close();
        }
        public void CreateExcel(string fileName, string sheetName, List<object> list)
        {
            FileStream file = File.Create(fileName);
            IWorkbook workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet(sheetName);
            var iRow = sheet.CreateRow(0);

            List<PropertyInfo> prop_list = list[0].GetType().GetProperties().ToList();

            foreach (PropertyInfo prop in prop_list)
            {
                string displayName = prop.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                string propName = prop.Name;

                iRow.CreateCell(iRow.Cells.Count).SetCellValue(displayName == null ? propName : displayName);
            }

            foreach (object itm in list)
            {
                iRow = sheet.CreateRow(sheet.LastRowNum + 1);

                List<PropertyInfo> itm_prop_list = itm.GetType().GetProperties().ToList();

                foreach (PropertyInfo cell in itm_prop_list)
                    iRow.CreateCell(iRow.Cells.Count).SetCellValue(cell.GetValue(cell).ToString());
            }

            workbook.Write(file);
            file.Close();
        }
        public void WriteCell(string fileName, int rowNum, int colNum, string value)
        {
            FileStream file = File.Open(fileName, FileMode.Open);
            IWorkbook workbook = new XSSFWorkbook(file);
            ISheet sheet = workbook.GetSheetAt(0);

            while (sheet.LastRowNum < rowNum)
                sheet.CreateRow(sheet.LastRowNum );

            IRow row = sheet.GetRow(rowNum - 1);

            while (row.LastCellNum < colNum)
                row.CreateCell(row.LastCellNum);

            row.GetCell(colNum - 1).SetCellValue(value);

            workbook.Write(file);
            file.Close();
        }
        public void MergeCell(string fileName,int firstRow,int lastRow,int firstCol,int lastCol)
        {
            FileStream file = File.Open(fileName, FileMode.Open);
            IWorkbook workbook = new XSSFWorkbook(file);
            ISheet sheet = workbook.GetSheetAt(0);

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(firstRow, lastRow, firstCol, lastCol));

            workbook.Write(file);
            file.Close();
        }
    }
}
