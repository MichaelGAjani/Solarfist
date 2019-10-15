using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Jund.NETHelper.WebHelper
{
    public class MailInfo
    {
        MailAddress _sendAddress;
        List<string> _receiveAddressList;
        string _msgSubject;
        string _msgBody;
        bool _isEnableSSL;
        List<string> _file_list;

        public MailAddress SendAddress { get => _sendAddress; set => _sendAddress = value; }
        public List<string> ReceiveAddressList { get => _receiveAddressList; set => _receiveAddressList = value; }
        public string MsgSubject { get => _msgSubject; set => _msgSubject = value; }
        public string MsgBody { get => _msgBody; set => _msgBody = value; }
        public bool IsEnableSSL { get => _isEnableSSL; set => _isEnableSSL = value; }
        public List<string> File_list { get => _file_list; set => _file_list = value; }
    }
}
