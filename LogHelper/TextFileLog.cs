using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.LogHelper
{
    public static class TextFileLog
    {
        public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");
        public static void LogDebug(object message) => loginfo.Debug(StackHelper.StackInfoString + message);
        public static void LogDebug(object message, Exception ex) =>loginfo.Debug(StackHelper.StackInfoString + message,ex);
        public static void LogError(object message) => loginfo.Error(StackHelper.StackInfoString + message);
        public static void LogError(object message, Exception ex) => loginfo.Error(StackHelper.StackInfoString + message, ex);
        public static void LogInfo(object message) => loginfo.Info(StackHelper.StackInfoString + message);
        public static void LogInfo(object message, Exception ex) => loginfo.Info(StackHelper.StackInfoString + message, ex);
        public static void LogFatal(object message) => loginfo.Fatal(StackHelper.StackInfoString + message);
        public static void LogFatal(object message, Exception ex) => loginfo.Fatal(StackHelper.StackInfoString + message, ex);
        public static void LogWarn(object message) => loginfo.Warn(StackHelper.StackInfoString + message);
        public static void LogWarn(object message, Exception ex) => loginfo.Warn(StackHelper.StackInfoString + message, ex);        
    }
}
