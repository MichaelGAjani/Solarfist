using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Jund.NETHelper.WebHelper
{
    public class EmailHelper
    {
        #region 发送电子邮件
        /// <summary>
        /// 发送电子邮件,所有SMTP配置信息均在config配置文件中system.net节设置.
        /// </summary>
        /// <param name="receiveEmail">接收电子邮件的地址</param>
        /// <param name="msgSubject">电子邮件的标题</param>
        /// <param name="msgBody">电子邮件的正文</param>
        /// <param name="IsEnableSSL">是否开启SSL</param>
        public static bool SendEmail(MailAddress sendAddress, List<string> receiveAddressList, string msgSubject, string msgBody, bool IsEnableSSL, List<string> file_list)
        {
            //创建电子邮件对象
            MailMessage email = new MailMessage();
            //设置接收人的电子邮件地址
            foreach (string addr in receiveAddressList)
                email.To.Add(addr);
            //设置邮件的标题
            email.Subject = msgSubject;
            //设置邮件的正文
            email.Body = msgBody;
            //设置邮件为HTML格式
            email.IsBodyHtml = true;
            email.From = sendAddress;

            foreach (string file in file_list)
            {
                Attachment attachment = new Attachment(file, MediaTypeNames.Application.Octet);
                email.Attachments.Add(attachment);
            }

            //创建SMTP客户端，将自动从配置文件中获取SMTP服务器信息
            SmtpClient smtp = new SmtpClient();
            //开启SSL
            smtp.EnableSsl = IsEnableSSL;

            try
            {
                //发送电子邮件
                smtp.Send(email);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
