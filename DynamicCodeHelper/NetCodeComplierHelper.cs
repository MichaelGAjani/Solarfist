using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jund.DynamicCodeHelper.Entity;
using System.Reflection;
using System.Dynamic;

namespace Jund.DynamicCodeHelper
{
    public class NetCodeComplierHelper
    {
        public static Assembly assembly;
        public void CreateCodeCompileUnit(string nameSpace)
        {
            CodeCompileUnit compile_unit = new CodeCompileUnit();
            CodeNamespace code_namespace = new CodeNamespace(nameSpace);

            code_namespace.Imports.Add(new CodeNamespaceImport("System.dll"));
            code_namespace.Imports.Add(new CodeNamespaceImport("System.Data.dll"));
            code_namespace.Imports.Add(new CodeNamespaceImport("System.Data.Linq.dll"));
            code_namespace.Imports.Add(new CodeNamespaceImport("System.Xml.dll"));
            code_namespace.Imports.Add(new CodeNamespaceImport("System.ComponentModel.DataAnnotations.dll"));

            compile_unit.Namespaces.Add(code_namespace);
        }
        public CompilerParameters CreateCompilerParameters(string mainClass, string assemblyName)
        {
            CompilerParameters paras = new CompilerParameters();
            paras.GenerateExecutable = false;
            paras.GenerateInMemory = true;
            paras.MainClass = mainClass;
            paras.OutputAssembly = assemblyName;

            paras.ReferencedAssemblies.Add("System.dll");
            paras.ReferencedAssemblies.Add("System.Data.dll");
            paras.ReferencedAssemblies.Add("System.Data.Linq.dll");
            paras.ReferencedAssemblies.Add("System.Xml.dll");
            paras.ReferencedAssemblies.Add("System.ComponentModel.DataAnnotations.dll");

            return paras;
        }
        public string CreateClassUsingString()
        {
            StringBuilder classSource = new StringBuilder();
            classSource.AppendLine("using System; ");
            classSource.AppendLine("using System.ComponentModel; ");
            classSource.AppendLine("using System.Data; ");
            classSource.AppendLine("using System.Data.Linq;");
            classSource.AppendLine("using System.Collections.Generic; ");
            classSource.AppendLine("using System.Reflection;");

            return classSource.ToString();
        }
        public string CreateClassBodyString(ClassDesc classDesc, List<TableColumnDesc> tableColumnList, List<TableColumnDisplay> columnDisplays)
        {
            StringBuilder classSource = new StringBuilder();
            classSource.AppendLine("public class " + classDesc.Class_name);
            classSource.AppendLine("{");

            foreach (TableColumnDesc column in tableColumnList)
            {
                string displayName = GlobeData.GetTableColumnDisplayName(column.Id);

                if (displayName != String.Empty)
                    classSource.AppendLine("[DisplayName(\"" + displayName.ToUpper() + "\")]");

                classSource.AppendLine("private " + column.Column_type + " _" + column.Column_name + ";");
                classSource.AppendLine("public " + column.Column_type + " " + column.Column_name + " {get=> return _"+column.Column_name + ";set _"+ column.Column_name+"=value;}");

            }

            return classSource.ToString();
        }
        public string CreateClassToString(ClassDesc classDesc)
        {
            StringBuilder classSource = new StringBuilder();

            classSource.AppendLine("public override string ToString()");
            classSource.AppendLine("{");
            classSource.AppendLine("return \"" + classDesc.Table_name + "\";");
            classSource.AppendLine("}");

            return classSource.ToString();
        }
        public string CreateClassGetListString(ClassDesc classDesc)
        {
            StringBuilder classSource = new StringBuilder();

            classSource.AppendLine("public List<" + classDesc.Class_name + "> GetList(DataTable dt)");
            classSource.AppendLine("{");
            classSource.AppendLine("List<" + classDesc.Class_name + "> list = new List<" + classDesc.Class_name + ">();");

            classSource.AppendLine("PropertyInfo[] propertys = typeof(" + classDesc.Class_name + ").GetProperties();");

            classSource.AppendLine("foreach (DataRow row in dt.Rows)");
            classSource.AppendLine("{");
            classSource.AppendLine("" + classDesc.Class_name + " t = new " + classDesc.Class_name + "();");

            classSource.AppendLine("foreach (PropertyInfo pi in propertys)");
            classSource.AppendLine("{");
            classSource.AppendLine("if (dt.Columns.Contains(pi.Name) && pi.CanWrite)");
            classSource.AppendLine("{");
            classSource.AppendLine("if (row[pi.Name] == System.DBNull.Value)");

            classSource.AppendLine("pi.SetValue(t, null, null);");
            classSource.AppendLine("else");


            classSource.AppendLine("pi.SetValue(t, row[pi.Name], null);");
            classSource.AppendLine("}");
            classSource.AppendLine(" }");


            classSource.AppendLine("list.Add(t);");
            classSource.AppendLine("}");

            classSource.AppendLine("GC.Collect();");

            classSource.AppendLine("return list;");
            classSource.AppendLine("}");

            classSource.AppendLine("}");

            return classSource.ToString();
        }     
        public void ComplierCode(string code, CompilerParameters paras)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();

            System.Diagnostics.Debug.WriteLine(code);
            //编译代码。
            CompilerResults result = provider.CompileAssemblyFromSource(paras, code);
            //获取编译后的程序集。

            assembly = result.CompiledAssembly;
        }

        public static object ClassObject(string className) => assembly.CreateInstance(className);
        public static object CombineObject(object classOne,object classTwo)
        {
            PropertyInfo[] info_class_base = classOne.GetType().GetProperties();
            PropertyInfo[] info_class_append = classTwo.GetType().GetProperties();

            var dynamicResult = new ExpandoObject() as IDictionary<string, Object>;
            foreach (PropertyInfo info in info_class_base)
            {
                dynamicResult.Add("[" + info.ReflectedType.Name + "]." + info.Name, info.GetValue(classOne, null));
            }
            foreach (PropertyInfo info in info_class_append)
            {
                dynamicResult.Add("[" + info.ReflectedType.Name + "]." + info.Name, info.GetValue(classTwo, null));
            }

            return dynamicResult;
        }
        public static object CombineMultiObject(object classOne, List<object> appendClassList)
        {
            PropertyInfo[] info_class_base = classOne.GetType().GetProperties();
            

            var dynamicResult = new ExpandoObject() as IDictionary<string, Object>;
            foreach (PropertyInfo info in info_class_base)
            {
                dynamicResult.Add("[" + info.ReflectedType.Name + "]." + info.Name, info.GetValue(classOne, null));
            }
            foreach (object appendClass in appendClassList)
            {
                PropertyInfo[] info_class_append = appendClass.GetType().GetProperties();

                foreach (PropertyInfo info in info_class_append)
                {
                    dynamicResult.Add("[" + info.ReflectedType.Name + "]." + info.Name, info.GetValue(appendClass, null));
                }
            }

            return dynamicResult;
        }
    }
}
