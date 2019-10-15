// FileInfo
// File:"EmailHelper.cs" 
// Solution:"Solarfist"
// Project:"DotNET Framework Helper" 
// Create:"2019-10-10"
// Author:"Michael G"
// https://github.com/MichaelGAjani/Solarfist
//
// License:GNU General Public License v3.0
// 
// Version:"1.0"
// Function:Email
// 1.SendEmail(MailInfo mailInfo)
//
// File Lines:58
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;

namespace Jund.NETHelper.WebHelper
{
    public class EmailHelper
    {
        public static bool SendEmail(MailInfo mailInfo)
        {
            MailMessage email = new MailMessage();
            foreach (string addr in mailInfo.ReceiveAddressList)
                email.To.Add(addr);

            email.Subject = mailInfo.MsgSubject;
            email.Body = mailInfo.MsgBody;
            email.IsBodyHtml = true;
            email.From = mailInfo.SendAddress;

            foreach (string file in mailInfo.File_list)
            {
                Attachment attachment = new Attachment(file, MediaTypeNames.Application.Octet);
                email.Attachments.Add(attachment);
            }

            SmtpClient smtp = new SmtpClient();
            smtp.EnableSsl = mailInfo.IsEnableSSL;

            try
            {
                smtp.Send(email);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
