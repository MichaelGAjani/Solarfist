// FileInfo
// File:"DownLoadFileHelper.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:Download
// 1.DownloadLargeFile(string fileName,string filePath)
// 2.DownloadFile(string fileName, string filePath)
// 3.DownloadFileByBlock(string fileName, string filePath)
// 4.DownloadFileByStream(string fileName, string filePath)
//
// File Lines:92
using System;
using System.IO;
using System.Web;

namespace Jund.NETHelper.WebHelper
{
    public class DownLoadFileHelper
    {
        public void DownloadLargeFile(string fileName,string filePath)
        {
            filePath = HttpContext.Current.Server.MapPath(filePath);          //目标文件路径
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            HttpContext.Current.Response.TransmitFile(filePath);
        }
        public void DownloadFile(string fileName, string filePath)
        {
            filePath = HttpContext.Current.Server.MapPath(filePath);          //目标文件路径
            FileHelper.FileInfoHelper file = new FileHelper.FileInfoHelper(filePath);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
            HttpContext.Current.Response.AddHeader("Content-Transfer-Encoding", "binary");
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            HttpContext.Current.Response.WriteFile(file.FullName);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        public void DownloadFileByBlock(string fileName, string filePath)
        {
            filePath = HttpContext.Current.Server.MapPath(filePath);          //目标文件路径
            FileHelper.FileInfoHelper file = new FileHelper.FileInfoHelper(filePath);
            if (file.Exists)
            {
                const long ChunkSize = 102400;                            //100K 每次读取文件，只读取100K，这样可以缓解服务器的压力 
                byte[] buffer = new byte[ChunkSize];
                HttpContext.Current.Response.Clear();
                System.IO.FileStream iStream = System.IO.File.OpenRead(filePath);
                long dataLengthToRead = iStream.Length;                      //获取下载的文件总大小 
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                while (dataLengthToRead > 0 && HttpContext.Current.Response.IsClientConnected)
                {
                    int lengthRead = iStream.Read(buffer, 0, Convert.ToInt32(ChunkSize));  //读取的大小 
                    HttpContext.Current.Response.OutputStream.Write(buffer, 0, lengthRead);
                    HttpContext.Current.Response.Flush();
                    dataLengthToRead = dataLengthToRead - lengthRead;
                }
                HttpContext.Current.Response.Close();
            }
        }
        public void DownloadFileByStream(string fileName, string filePath)
        {
            filePath = HttpContext.Current.Server.MapPath(filePath);          //目标文件路径
            FileHelper.FileInfoHelper file = new FileHelper.FileInfoHelper(filePath);
            FileStream fs = new FileStream(filePath, FileMode.Open);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            //通知浏览器下载文件而不是打开 
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            HttpContext.Current.Response.BinaryWrite(bytes);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

    }
}
