using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DatabaseHelper.DBInfoHelper
{
    interface ISqlDDL
    {
        void CreateDatabase(string db_name);
    }
}
