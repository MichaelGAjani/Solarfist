using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Jund.DynamicCodeHelper.Entity.ObjectEnum;

namespace Jund.DynamicCodeHelper
{
    public static class GlobeSetting
    {
        public static string ApplicationName="My Application";
        public static LanguageCode language = LanguageCode.US_ENGLISH;
        public static int CardViewWidth = 300;
        public static int TileViewColumns = 5;
        public static DatabaseType dbtype = DatabaseType.SqlServer;
    }
}
