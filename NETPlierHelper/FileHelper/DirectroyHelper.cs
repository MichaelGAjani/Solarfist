// FileInfo
// File:"DirectroyHelper.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:Directroy Func
// 1.IsExist(string directoryPath)
// 2.GetDirectories(string directoryPath,string searchPattern,bool searchChild)
// 3.GetFiles(string directoryPath, string searchPattern, bool searchChild) 
// 4.IsEmpty(string directoryPath, string searchPattern)
// 5.FileContains(string directoryPath, string searchPattern, bool searchChild)
// 6.CreateDirectory(string dir)
// 7.DeleteDirectory(string dir)
// 8.CopyFolder(string sourceDirectory, string destDirectory)
// 9.GetAllDirectoriesAndFiles(string dir)
// 10.DisplayAllDriveInfo
//
// File Lines:87

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.NETHelper.FileHelper
{
    /// <summary>
    /// 目录操作类
    /// </summary>
    public class DirectroyHelper
    {
        public static bool IsExist(string directoryPath)=> Directory.Exists(directoryPath);
        public static List<string> GetDirectories(string directoryPath,string searchPattern,bool searchChild) => Directory.GetDirectories(directoryPath,searchPattern, searchChild?SearchOption.AllDirectories: SearchOption.TopDirectoryOnly).ToList();
        public static List<string> GetFiles(string directoryPath, string searchPattern, bool searchChild) => Directory.GetFiles(directoryPath, searchPattern, searchChild ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).ToList();
        public static bool IsEmpty(string directoryPath, string searchPattern) => GetFiles(directoryPath, searchPattern, true).Count == 0;
        public static bool FileContains(string directoryPath, string searchPattern, bool searchChild) => GetFiles(directoryPath, searchPattern, searchChild).Count > 0;
        public static void CreateDirectory(string dir) => Directory.CreateDirectory(dir);
        public static void DeleteDirectory(string dir) => Directory.Delete(dir, true);
        public static void CopyFolder(string sourceDirectory, string destDirectory)
        {
            Task t1 = new Task(() => ZipHelper.CreateZip(sourceDirectory, sourceDirectory + @"\tmp.zip"));
            t1.Start();
            Task.WaitAll();

            Task t2 = new Task(() => ZipHelper.ExtractZip(sourceDirectory + @"\tmp.zip", sourceDirectory, Encoding.Unicode));
            t2.Start();
            Task.WaitAll();

            File.Delete(sourceDirectory + @"\tmp.zip");
        }
        public static IEnumerable<FileSystemInfo> GetAllDirectoriesAndFiles(string dir)
        {
            if (string.IsNullOrWhiteSpace(dir))
                throw new ArgumentNullException(nameof(dir));
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            Stack<FileSystemInfo> stack = new Stack<FileSystemInfo>();
            stack.Push(dirInfo);
            while (dirInfo != null || stack.Count > 0)
            {
                FileSystemInfo fileSystemInfo = stack.Pop();
                DirectoryInfo subDirectoryInfo = fileSystemInfo as DirectoryInfo;
                if (subDirectoryInfo != null)
                {
                    yield return subDirectoryInfo;
                    foreach (FileSystemInfo fsi in subDirectoryInfo.GetFileSystemInfos())
                        stack.Push(fsi);
                    dirInfo = subDirectoryInfo;
                }
                else
                {
                    yield return fileSystemInfo;
                    dirInfo = null;
                }
            }
        }
        public static List<DriveInfo> DisplayAllDriveInfo() => DriveInfo.GetDrives().ToList();
    }
}
