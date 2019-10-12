using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jund.NETHelper.ThreadHelper
{
    public class BaseTask
    {
        CancellationToken _token;
        bool _long_run;

        public BaseTask()
        {
            
        }
        public BaseTask(CancellationToken token, bool long_run)
        {
            _token = token;
            _long_run = long_run;
        }

        public CancellationToken Token { get => _token; set => _token = value; }
        public bool Long_run { get => _long_run; set => _long_run = value; }
        public TaskCreationOptions TaskOption => this.Long_run ? TaskCreationOptions.LongRunning : TaskCreationOptions.None;
    }
}
