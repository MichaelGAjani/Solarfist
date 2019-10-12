using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Jund.NETHelper.WebHelper
{
    public class FTPHelper
    {
        public static async Task FtpDownloadAsync(Uri ftpSite, string targetPath, string loginID, string password)
        {
            try
            {
                FtpWebRequest request =
                (FtpWebRequest)WebRequest.Create(
                ftpSite);
                request.Credentials = new NetworkCredential(loginID,
                password);
                using (FtpWebResponse response =
                (FtpWebResponse)await request.GetResponseAsync())
                {
                    Stream data = response.GetResponseStream();
                    File.Delete(targetPath);
                    Console.WriteLine(
                    $"Downloading {ftpSite.AbsoluteUri} to {targetPath}...");
                    byte[] byteBuffer = new byte[4096];
                    using (FileStream output = new FileStream(targetPath, FileMode.CreateNew,
                    FileAccess.ReadWrite, FileShare.ReadWrite, 4096, useAsync: true))
                    {
                        int bytesRead = 0;
                        do
                        {
                            bytesRead = await data.ReadAsync(byteBuffer, 0,
                            byteBuffer.Length);
                            if (bytesRead > 0)
                                await output.WriteAsync(byteBuffer, 0, bytesRead);
                        }
                        while (bytesRead > 0);
                    }
                    Console.WriteLine($"Downloaded {ftpSite.AbsoluteUri} to {targetPath}");
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(
                $"Failed to download {ftpSite.AbsoluteUri} to {targetPath}");
                Console.WriteLine(e);
            }
        }
    }
}
