using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.NETHelper.WebHelper
{
    public enum HttpMethod
    {
        Post,
        Get
    }
    public enum PostDataType
    {
        String,//字符串
        Byte,//字符串和字节流
        FilePath//表示传入的是文件
    }
    public enum ResultType
    {
        String,//表示只返回字符串
        Byte//表示返回字符串和字节流
    }
}
