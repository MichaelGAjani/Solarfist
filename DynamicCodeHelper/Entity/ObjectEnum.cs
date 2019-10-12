using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DynamicCodeHelper.Entity
{
    public static class ObjectEnum
    {
        public enum ColumnEditType
        {
            Text,
            Memo,
            Combo,
            LookUp,
            CheckedCombo,
            Spin,
            Calc,
            Date,
            Time,
            Check,
            Toggle,
            Process,
            Rating,
            Range,
            Grid,
            Tree
        }           
        public enum ClassType
        {
            Table,
            View,
            ColumnReference,
            Report
        }
        public enum LanguageCode
        {
            US_ENGLISH=1033,
            PORTUGUESE=2070,
            JAPANESE=1041,
            SIMPLE_CHINESE=2052,
            TRAD_CHINESE=1028,
            ITALIAN=1040,
            FRENCH=1036,
            GERMAN=1031,
            SPANISH=3082
        }
        public enum ExternalProgramType
        {
            XtraUserControl,
            XtraForm,
            FluentDesignForm,
            ToolBarForm,
            NavbarForm,
            ModernToolBarForm
        }
        public enum DatabaseType
        {
            Oracle,
            SqlServer,
            MySql,
            Sqlite
        }
        public enum SaveDataType
        {
            Insert,
            Update,
            Delete
        }
    }
}
