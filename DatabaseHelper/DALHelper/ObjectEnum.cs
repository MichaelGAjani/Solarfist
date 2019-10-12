using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DatabaseHelper.DALHelper
{
    public static class ObjectEnum
    {
        public enum DatabaseType
        {
            SQLSERVER2008R2,
            ORACLE9i,
            MYSQL,
            SQLITE
        }
    }
}
