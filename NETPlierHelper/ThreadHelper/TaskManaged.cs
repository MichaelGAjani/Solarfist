using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jund.NETHelper.ThreadHelper
{
    public class TaskManaged
    {
        public Task CreateTask(Action<object> func,object para,bool is_long_run,CancellationToken token)
        {
            return Task.Factory.StartNew(func, para, token, is_long_run ? TaskCreationOptions.LongRunning : TaskCreationOptions.None,TaskScheduler.Default);
        }
        public Task<object> CreateTask(Func<object,object> func, object para, bool is_long_run, CancellationToken token)
        {
            return Task.Factory.StartNew<object>(func, para, token, is_long_run ? TaskCreationOptions.LongRunning : TaskCreationOptions.None, null);
        }

    }
}
