using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Reflection;
using System.IO;
using DevExpress.XtraBars;
using DevExpress.XtraBars.ToolbarForm;
using System.Diagnostics;
using System.Dynamic;
using DevExpress.XtraVerticalGrid.Rows;
using DevExpress.XtraEditors.Repository;

namespace TestDemo
{
    public partial class XtraForm1 : DevExpress.XtraEditors.XtraForm
    {
       public class aaa
        {
            public int aaaa { get; set; }
        }
        public XtraForm1()
        {
            InitializeComponent();
        }

        private void Test()
        {
            //StackTrace st = new StackTrace();
            //StackFrame[] f=st.GetFrames();
            dynamic expando = new ExpandoObject();
            expando.Name = "Brian";
            expando.Country = "USA";
            expando.Number = 1;
            expando.Decimal = 1.1;
            expando.Color = Color.Red;
            expando.DateTime = DateTime.Now;
            expando.Bollean = true;

            List<Car> list = new List<Car>();
            list.Add(new Car());
            list.Add(new Car());

            List<object> ob = new List<object>();

            Car car = new Car();
            PropertyInfo[] infoCar = car.GetType().GetProperties();
            PropertyInfo[] infoAAA = typeof(aaa).GetProperties();

            var dynamicResult = new ExpandoObject() as IDictionary<string, Object>;
            foreach (PropertyInfo info in infoCar)
            {
                Type type = info.PropertyType;
                dynamicResult.Add("["+info.ReflectedType.Name+"]." + info.Name, info.GetValue(car,null));
            }
            foreach (PropertyInfo info in infoAAA)
                dynamicResult.Add("AAA_" + info.Name, new object());

            ob.Add(dynamicResult);

            this.gridControl1.DataSource = ob;
            //Show For Focused Cell

            if (list.Count>0)
            {
                object itm = list[0];

                List<PropertyInfo> info_list = itm.GetType().GetProperties().ToList();

                //foreach(PropertyInfo info in info_list)
                //{
                //    EditorRow row = new EditorRow();
                //    row.Properties.FieldName = info.Name;
                //    row.Properties.Caption = info.Name;
                //    switch (info.PropertyType.Name.ToLower())
                //    {
                //        case "int32":
                //            row.Properties.RowEdit= new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
                //            (row.Properties.RowEdit as RepositoryItemSpinEdit).IsFloatValue = false;
                //            break;
                //        case "color": row.Properties.RowEdit = new DevExpress.XtraEditors.Repository.RepositoryItemColorEdit(); break;
                //            case "image": row.Properties.RowEdit = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit(); break;
                //    }

                //    vGridControl1.Rows.Add(row);
                //}
            }

            //vGridControl1.Rows.Clear();
        }

       

        private void XtraForm1_Load(object sender, EventArgs e)
        {
            Test();
            List<string> list=System.IO.Directory.GetFiles(Application.StartupPath + @"\\Demo").ToList();

            foreach(string s in list)
            {
                FileInfo info = new FileInfo(s);
                if (info.Extension.ToLower() == ".dll")
                {
                    DevExpress.XtraBars.BarButtonItem itm = new DevExpress.XtraBars.BarButtonItem(barManager1, info.Name.Replace(info.Extension, ""));
                    itm.ItemClick += Itm_ItemClick;
                    itm.Tag = s;
                    this.bar2.AddItem(itm);
                }
            }
        }

        private void Itm_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string path =e.Item.Tag.ToString();
            Assembly DllAssembly = Assembly.LoadFrom(path);
            AssemblyFileVersionAttribute VersionInfo = DllAssembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)[0] as AssemblyFileVersionAttribute;
            AssemblyCompanyAttribute CompanyInfo = DllAssembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false)[0] as AssemblyCompanyAttribute;
            AssemblyProductAttribute ProductInfo = DllAssembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false)[0] as AssemblyProductAttribute;
            AssemblyTitleAttribute TitleInfo = DllAssembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0] as AssemblyTitleAttribute;
            AssemblyCopyrightAttribute CopyrightInfo = DllAssembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0] as AssemblyCopyrightAttribute;
            AssemblyDescriptionAttribute DescriptionInfo = DllAssembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0] as AssemblyDescriptionAttribute;
            Type[] type_list = DllAssembly.GetExportedTypes();
            foreach (Type outerForm in type_list)
            {
                //Type outerForm = DllAssembly.GetTypes()[3];
                if(outerForm.BaseType.Name== "XtraUserControl")
                {
                    var obj = (Activator.CreateInstance(outerForm) as XtraUserControl);
                    obj.Dock = DockStyle.Fill;
                    this.panelControl1.Controls.Clear();
                    this.panelControl1.Controls.Add(obj);
                    break;
                }
                else if(outerForm.BaseType.Name == "ToolbarForm")
                {
                    var obj = (Activator.CreateInstance(outerForm) as ToolbarForm);
                    obj.ShowDialog();
                    break;
                }
            }
        }
    }
}