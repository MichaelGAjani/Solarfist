using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jund.NETHelper.ThreadHelper
{
    public class AcionTask:BaseTask
    {
        Action<object> _action;
        object _parameter;

        public AcionTask()
        {
        }

        public AcionTask(Action<object> action, object parameter)
        {
            _action = action;
            _parameter = parameter;
        }

        public AcionTask(CancellationToken token, bool long_run) : base(token, long_run)
        {
        }

        public Action<object> ActionFunc { get => _action; set => _action = value; }
        public object Parameter { get => _parameter; set => _parameter = value; }

    }
}
