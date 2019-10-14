// FileInfo
// File:"ZipHelper.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:Zip Operate
// 1.CreateZip(string startPath, string zipPath)
// 2.CreateZip(string startPath, string zipPath, CompressionLevel compressionLevel)
// 3.CreateZip(string startPath, string zipPath, CompressionLevel compressionLevel, Encoding encoding)
// 4.ExtractZip(string zipPath, string extractPath, Encoding encoding)
// 5.AddFiles(string zipPath, string newZip, List<string> files)
//
// File Lines:46

using System.Collections.Generic;
using System.IO.Compression;
using System.Text;

namespace Jund.NETHelper.FileHelper
{
    /// <summary>
    /// 压缩操作类
    /// </summary>
    public static class ZipHelper
    {
        public static void CreateZip(string startPath, string zipPath)=> CreateZip(startPath, zipPath, CompressionLevel.Optimal, Encoding.Unicode);
        public static void CreateZip(string startPath, string zipPath, CompressionLevel compressionLevel) => CreateZip(startPath, zipPath, compressionLevel, Encoding.Unicode);
        public static void CreateZip(string startPath, string zipPath, CompressionLevel compressionLevel, Encoding encoding)=> ZipFile.CreateFromDirectory(startPath, zipPath, CompressionLevel.Optimal, true, encoding);
        public static void ExtractZip(string zipPath, string extractPath, Encoding encoding)=> ZipFile.ExtractToDirectory(zipPath, extractPath, encoding);
        public static void AddFiles(string zipPath, string newZip, List<string> files)
        {
            using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
            {
                foreach (string file in files)
                    archive.CreateEntryFromFile(newZip, file);
            }
        }
    }
}
