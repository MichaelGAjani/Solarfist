using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.NETHelper.FileHelper
{
    public static class ZipHelper
    {
        public static void CreateZip(string startPath, string zipPath)
        {
            CreateZip(startPath, zipPath, CompressionLevel.Optimal, Encoding.Unicode);
        }
        public static void CreateZip(string startPath, string zipPath, CompressionLevel compressionLevel)
        {
            CreateZip(startPath, zipPath, compressionLevel, Encoding.Unicode);
        }
        public static void CreateZip(string startPath, string zipPath, CompressionLevel compressionLevel, Encoding encoding)
        {
            try
            {
                ZipFile.CreateFromDirectory(startPath, zipPath, CompressionLevel.Optimal, true, encoding);
            }
            catch (ArgumentException argumentException)
            {
                //sourceDirectoryName 或 destinationArchiveFileName 是 Empty，仅包含空格，或包含至少一个无效字符。
            }
            catch (PathTooLongException argumentException)
            {
                //在 sourceDirectoryName 或 destinationArchiveFileName 内，指定的路径、文件名或者两者都超出了系统定义的最大长度。sourceDirectoryName 或 destinationArchiveFileName 是 Empty，仅包含空格，或包含至少一个无效字符。
            }
            catch (DirectoryNotFoundException argumentException)
            {
                //sourceDirectoryName 无效或不存在（例如，在未映射的驱动器上）。
            }
            catch (UnauthorizedAccessException argumentException)
            {
                //destinationArchiveFileName 指定目录。                或 调用方不具有访问在 sourceDirectoryName 中指定的目录或在 destinationArchiveFileName 中指定的文件的所需权限。
            }
            catch (NotSupportedException argumentException)
            {
                //sourceDirectoryName 或 destinationArchiveFileName 包含的格式无效。
            }
        }
        public static void ExtractZip(string zipPath, string extractPath, Encoding encoding)
        {
            try
            {
                ZipFile.ExtractToDirectory(zipPath, extractPath, encoding);
            }
            catch (ArgumentException argumentException)
            {
                //sourceDirectoryName 或 destinationArchiveFileName 是 Empty，仅包含空格，或包含至少一个无效字符。
            }
            catch (PathTooLongException argumentException)
            {
                //在 sourceDirectoryName 或 destinationArchiveFileName 内，指定的路径、文件名或者两者都超出了系统定义的最大长度。sourceDirectoryName 或 destinationArchiveFileName 是 Empty，仅包含空格，或包含至少一个无效字符。
            }
            catch (DirectoryNotFoundException argumentException)
            {
                //sourceDirectoryName 无效或不存在（例如，在未映射的驱动器上）。
            }
            catch (UnauthorizedAccessException argumentException)
            {
                //destinationArchiveFileName 指定目录。                或 调用方不具有访问在 sourceDirectoryName 中指定的目录或在 destinationArchiveFileName 中指定的文件的所需权限。
            }
            catch (NotSupportedException argumentException)
            {
                //sourceDirectoryName 或 destinationArchiveFileName 包含的格式无效。
            }
            catch (FileNotFoundException ex)
            {

            }
            catch (InvalidDataException ex)
            {

            }
        }
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
