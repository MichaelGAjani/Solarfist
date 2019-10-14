// FileInfo
// File:"FileInfoHelper.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:Inherited from FileSystemInfo
//
// File Lines:75

using System.IO;
using System.Security.AccessControl;

namespace Jund.NETHelper.FileHelper
{
    public class FileInfoHelper: FileSystemInfo
    {
        FileInfo info;
        public DirectoryInfo Directory { get; set; }
        public string DirectoryName { get; set; }
        public long Length { get; set; }
        public bool IsReadOnly { get; set; }
        public string Extension { get; set; }
        public FileAttributes Attributes { get; set; }
        public string FileName { get; set; }
        public bool FileExists { get; set; }
        public override string Name { get; }
        public override bool Exists { get; }
        public FileInfoHelper(string file)
        {
            info= new FileInfo(file);

            this.Directory = info.Directory;
            this.DirectoryName = info.DirectoryName;
            this.Length = info.Length;
            this.FileName = info.Name;
            this.IsReadOnly = info.IsReadOnly;
            this.Extension = info.Extension;
            this.LastWriteTime = info.LastWriteTime;
            this.LastAccessTimeUtc = info.LastAccessTimeUtc;
            this.LastAccessTime = info.LastAccessTime;
            this.CreationTimeUtc = info.CreationTimeUtc;
            this.CreationTime = info.CreationTime;
            this.LastWriteTimeUtc = info.LastWriteTimeUtc;
            this.FileExists = info.Exists;
        }
       
        public StreamWriter AppendText() => info.AppendText();      
        public FileInfo CopyTo(string destFileName) => info.CopyTo(destFileName);
        public FileInfo CopyTo(string destFileName, bool overwrite) => info.CopyTo(destFileName, overwrite); 
        public FileStream Create() => info.Create();
        public StreamWriter CreateText() => info.CreateText();
        public void Decrypt() => info.Decrypt();      
        public override void Delete() => info.Delete();
        public void Encrypt() => info.Encrypt();
        public FileSecurity GetAccessControl() => info.GetAccessControl();
        public FileSecurity GetAccessControl(AccessControlSections includeSections) => info.GetAccessControl(includeSections);
        public void MoveTo(string destFileName) => info.MoveTo(destFileName);
        public FileStream Open(FileMode mode) => info.Open(mode);
        public FileStream Open(FileMode mode, FileAccess access, FileShare share) => info.Open(mode, access, share);    
        public FileStream Open(FileMode mode, FileAccess access) => info.Open(mode, access);
        public FileStream OpenRead() => info.OpenRead();
        public StreamReader OpenText() => info.OpenText();
        public FileStream OpenWrite() => info.OpenWrite();
        public FileInfo Replace(string destinationFileName, string destinationBackupFileName) => info.Replace(destinationFileName, destinationBackupFileName);
        public FileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors) => info.Replace(destinationFileName, destinationBackupFileName, ignoreMetadataErrors);
        public void SetAccessControl(FileSecurity fileSecurity) => info.SetAccessControl(fileSecurity);
    }
}
