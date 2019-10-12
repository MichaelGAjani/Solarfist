using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DatabaseHelper.DBObject.MSSQL2008
{
    public class Database
    {
        string _name;
        int _dbid;
        byte[] _sid;
        int _mode;
        int _status;
        int _status2;
        DateTime _crdate;
        DateTime _reserved;
        int _category;
        int _cmptlevel;
        string _filename;
        int _version;

        public string Name { get => _name; set => _name = value; }
        public int Dbid { get => _dbid; set => _dbid = value; }
        public byte[] Sid { get => _sid; set => _sid = value; }
        public int Mode { get => _mode; set => _mode = value; }
        public int Status { get => _status; set => _status = value; }
        public int Status2 { get => _status2; set => _status2 = value; }
        public DateTime Crdate { get => _crdate; set => _crdate = value; }
        public DateTime Reserved { get => _reserved; set => _reserved = value; }
        public int Category { get => _category; set => _category = value; }
        public int Cmptlevel { get => _cmptlevel; set => _cmptlevel = value; }
        public string Filename { get => _filename; set => _filename = value; }
        public int Version { get => _version; set => _version = value; }
    }
}
