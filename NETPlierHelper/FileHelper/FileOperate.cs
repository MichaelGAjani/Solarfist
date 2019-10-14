// FileInfo
// File:"FileOperate.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:File Operate
// 1.WriteFile(string outputfile, string str, Encoding encoding, bool append)
// 2.WriteFile(string outputfile, List<string> list, Encoding encoding, bool append)
// 3.ReadFile(string filename, Encoding encoding)
// 4.ReadFileByLine(string filename, Encoding encoding)
// 5.CopyFile(string sourceFile, string destFile, bool overwriter = true)
// 6.DeletePath(string Path)
// 7.MoveFile(string sourceFile, string destFile)
// 8.DisplayPathParts(string path)
// 9.ComparePart(int ver1, int ver2)
// 10.CompareFileVersions(string file1, string file2)
//
// File Lines:154

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Jund.NETHelper.FileHelper
{
    /// <summary>
    /// 文件操作类
    /// </summary>
    public class FileOperate
    {
        public enum FileComparison
        {
            Error = 0,
            Newer = 1,
            Older = 2,
            Same = 3
        }
        #region 写文件
        public void WriteFile(string outputfile, string str, Encoding encoding, bool append)
        {
            StreamWriter writer = new StreamWriter(outputfile, append, encoding);
            writer.Write(str);
            writer.Flush();
            writer.Close();
        }
        public void WriteFile(string outputfile, List<string> list, Encoding encoding, bool append)
        {
            StreamWriter writer = new StreamWriter(outputfile, append, encoding);
            foreach (string str in list)
                writer.WriteLine(str);
            writer.Flush();
            writer.Close();
        }
        #endregion

        #region 读文件
        public string ReadFile(string filename, Encoding encoding)
        {
            if (File.Exists(filename))
            {
                StreamReader reader = new StreamReader(filename, encoding);
                string str = reader.ReadToEnd();
                reader.Close();
                return str;
            }
            else
                throw (new FileNotFoundException());
        }
        public List<string> ReadFileByLine(string filename, Encoding encoding)
        {
            List<string> lines = new List<string>();

            if (File.Exists(filename))
            {
                StreamReader reader = new StreamReader(filename, encoding);
                while (!reader.EndOfStream)
                    lines.Add(reader.ReadLine());
                reader.Close();
            }
            else
                throw (new FileNotFoundException());

            return lines;
        }
        #endregion

        public static void CopyFile(string sourceFile, string destFile, bool overwriter = true)=> File.Copy(sourceFile, destFile, overwriter);
        public static void DeletePath(string Path)=> File.Delete(Path);
        public static void MoveFile(string sourceFile, string destFile)=> File.Move(sourceFile, destFile);
        public static StringBuilder DisplayPathParts(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));
            string root = Path.GetPathRoot(path);
            string dirName = Path.GetDirectoryName(path);
            string fullFileName = Path.GetFileName(path);
            string fileExt = Path.GetExtension(path);
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(path);
            StringBuilder format = new StringBuilder();
            format.Append($"ParsePath of {path} breaks up into the following pieces:" +
            $"{Environment.NewLine}");
            format.Append($"\tRoot: {root}{Environment.NewLine}");
            format.Append($"\tDirectory Name: {dirName}{Environment.NewLine}");
            format.Append($"\tFull File Name: {fullFileName}{Environment.NewLine}");
            format.Append($"\tFile Extension: {fileExt}{Environment.NewLine}");
            format.Append($"\tFile Name Without Extension: {fileNameWithoutExt}" +
            $"{Environment.NewLine}");

            return format;
        }
        /// <summary>
        /// Compare Version
        /// </summary>
        /// <param name="ver1"></param>
        /// <param name="ver2"></param>
        /// <returns></returns>
        public static FileComparison ComparePart(int ver1, int ver2) =>ver1 > ver2 ? FileComparison.Newer :(ver1 < ver2 ? FileComparison.Older : FileComparison.Same);
        public static FileComparison CompareFileVersions(string file1, string file2)
        {
            if (string.IsNullOrWhiteSpace(file1))
                throw new ArgumentNullException(nameof(file1));
            if (string.IsNullOrWhiteSpace(file2))
                throw new ArgumentNullException(nameof(file2));
            FileComparison retValue = FileComparison.Error;
            // get the version information
            FileVersionInfo file1Version = FileVersionInfo.GetVersionInfo(file1);
            FileVersionInfo file2Version = FileVersionInfo.GetVersionInfo(file2);
            retValue = ComparePart(file1Version.FileMajorPart,
            file2Version.FileMajorPart);
            if (retValue != FileComparison.Same)
            {
                retValue = ComparePart(file1Version.FileMinorPart, file2Version.FileMinorPart);
                if (retValue != FileComparison.Same)
                {
                    retValue = ComparePart(file1Version.FileBuildPart,
                    file2Version.FileBuildPart);
                    if (retValue != FileComparison.Same)
                        retValue = ComparePart(file1Version.FilePrivatePart,
                        file2Version.FilePrivatePart);
                }
            }
            return retValue;
        }
    }
}
