using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DatabaseHelper.DBObject.MSSQL2008
{
    public class DBAllObjec
    {
        string _name;
        int _id;
        string _xtype;
        int _uid;
        int _info;
        int _status;
        int _base_schema_ver;
        int _replinfo;
        int _parent_obj;
        DateTime _crdate;
        int _ftcatid;
        int _schema_ver;
        int _stats_schema_ver;
        string _type;
        int _userstat;
        int _sysstat;
        int _indexdel;
        DateTime _refdate;
        int _version;
        int _deltrig;
        int _instrig;
        int _updtrig;
        int _seltrig;
        int _category;
        int _cache;

        /// <summary>
        /// 对象名称
        /// </summary>
        public string Name { get => _name; set => _name = value; }
        /// <summary>
        /// 对象标识号
        /// </summary>
        public int Id { get => _id; set => _id = value; }
        /// <summary>
        /// 对象类型。 可以是以下对象类型之一：
        ///AF = 聚合函数(CLR)
        ///C = CHECK 约束
        ///D = 默认值或 DEFAULT 约束
        ///F = FOREIGN KEY 约束
        ///L = 日志
        ///FN = 标量函数
        ///FS = 程序集(CLR) 标量函数
        ///FT = 程序集(CLR) 表值函数
        ///IF = 内联表函数
        ///IT = 内部表
        ///P = 存储过程
        ///PC = 程序集(CLR) 存储过程
        ///PK = PRIMARY KEY 约束（类型为 K）
        ///RF = 复制筛选存储过程
        ///S = 系统表
        ///SN = 同义词
        ///SQ = 服务队列
        ///TA = 程序集(CLR) DML 触发器
        ///TF = 表函数
        ///TR = SQL DML 触发器
        ///TT = 表类型
        ///U = 用户表
        ///UQ = UNIQUE 约束（类型为 K）
        ///V = 视图
        ///X = 扩展存储过程
        /// </summary>
        public string Xtype { get => _xtype; set => _xtype = value; }
        /// <summary>
        /// 对象所有者的架构 ID
        /// </summary>
        public int Uid { get => _uid; set => _uid = value; }
        public int Info { get => _info; set => _info = value; }
        public int Status { get => _status; set => _status = value; }
        public int Base_schema_ver { get => _base_schema_ver; set => _base_schema_ver = value; }
        public int Replinfo { get => _replinfo; set => _replinfo = value; }
        /// <summary>
        /// 父对象的对象标识号
        /// </summary>
        public int Parent_obj { get => _parent_obj; set => _parent_obj = value; }
        /// <summary>
        /// 对象的创建日期
        /// </summary>
        public DateTime Crdate { get => _crdate; set => _crdate = value; }
        /// <summary>
        /// 全文目录标识符
        /// </summary>
        public int Ftcatid { get => _ftcatid; set => _ftcatid = value; }
        /// <summary>
        /// 每次更改表的架构时都会增加的版本号
        /// </summary>
        public int Schema_ver { get => _schema_ver; set => _schema_ver = value; }
        public int Stats_schema_ver { get => _stats_schema_ver; set => _stats_schema_ver = value; }
        public string Type { get => _type; set => _type = value; }
        public int Userstat { get => _userstat; set => _userstat = value; }
        public int Sysstat { get => _sysstat; set => _sysstat = value; }
        public int Indexdel { get => _indexdel; set => _indexdel = value; }
        public DateTime Refdate { get => _refdate; set => _refdate = value; }
        public int Version { get => _version; set => _version = value; }
        public int Deltrig { get => _deltrig; set => _deltrig = value; }
        public int Instrig { get => _instrig; set => _instrig = value; }
        public int Updtrig { get => _updtrig; set => _updtrig = value; }
        public int Seltrig { get => _seltrig; set => _seltrig = value; }
        public int Category { get => _category; set => _category = value; }
        public int Cache { get => _cache; set => _cache = value; }
    }
}
