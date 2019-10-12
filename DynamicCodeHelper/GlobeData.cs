using Jund.DynamicCodeHelper.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Jund.DynamicCodeHelper.Entity.ObjectEnum;

namespace Jund.DynamicCodeHelper
{
    public static class GlobeData
    {
        public static Assembly assembly;
        public static List<ClassDesc> classDescList = new List<ClassDesc>();
        public static List<ClassRelated> classRelatedList = new List<ClassRelated>();
        public static List<ExternalProgram> externalProgramList = new List<ExternalProgram>();
        public static List<MenuDesc> menuDescList = new List<MenuDesc>();
        public static List<TableColumnDesc> tableColumnDescList = new List<TableColumnDesc>();
        public static List<TableColumnDisplay> tableColumnDisplayList = new List<TableColumnDisplay>();
        public static List<TableColumnReference> tableColumnReferenceList = new List<TableColumnReference>();
        public static List<TableColumnRegular> tableColumnRegularList = new List<TableColumnRegular>();

        public static string GetTableColumnDisplayName(int id)
        {
            TableColumnDisplay tableColumnDisplay = tableColumnDisplayList.Find(obj => obj.Id == id&&obj.Language_id==GlobeSetting.language);

            if (tableColumnDisplay != null) return tableColumnDisplay.Column_display_name;
            else return String.Empty;
        }
        public static string GetColumnName(int id) => tableColumnDescList.Find(obj => obj.Id == id).Column_name;
    }
}
