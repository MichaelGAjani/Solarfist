using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jund.LogHelper
{
    public class StackHelper
    {
        public static List<StackInfo> GetStackInfo()
        {
            StackTrace trace = new StackTrace();
            List<StackFrame> frame_list = trace.GetFrames().ToList();
            List<StackInfo> list = new List<StackInfo>();

            foreach (StackFrame frame in frame_list)
            {
                StackInfo info = new StackInfo();
                MethodBase method=frame.GetMethod();
                info.FullName = method.Name;
                info.MethodType = method.ReflectedType.Name;
                info.MoudleName = method.Module.Name;

                list.Add(info);
            }

            return list;
        }
        private static string GetStackInfoString()
        {
            StringBuilder builder = new StringBuilder();

            List<StackInfo> list = GetStackInfo();

            foreach (StackInfo info in list)
                builder.AppendLine(info.ToString());

            GC.Collect();

            builder.AppendLine();
            return builder.ToString();
        }

        public static string StackInfoString { get => GetStackInfoString(); }
    }
}
