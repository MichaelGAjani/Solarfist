using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jund.DatabaseHelper.DBObject.MSSQL2008
{
    public class TableColumn
    {
        string _name;
        int _id;
        int _xtype;
        int _typestat;
        int _xusertype;
        int _length;
        int _xprec;
        int _xscale;
        int _colid;
        int _xoffset;
        int _bitpos;
        int _reserved;
        int _colstat;
        int _cdefault;
        int _domain;
        int _number;
        int _colorder;
        string _autoval;
        int _offset;
        int _collationid;
        int _status;
        int _type;
        int _usertype;
        string _printfmt;
        int _prec;
        int _scale;
        int _iscomputed;
        int _isoutparam;
        int _isnullable;
        string _collation;
        byte[] _tdscollation;

        public string Name { get => _name; set => _name = value; }
        public int Id { get => _id; set => _id = value; }
        public int Xtype { get => _xtype; set => _xtype = value; }
        public int Typestat { get => _typestat; set => _typestat = value; }
        public int Xusertype { get => _xusertype; set => _xusertype = value; }
        public int Length { get => _length; set => _length = value; }
        public int Xprec { get => _xprec; set => _xprec = value; }
        public int Xscale { get => _xscale; set => _xscale = value; }
        public int Colid { get => _colid; set => _colid = value; }
        public int Xoffset { get => _xoffset; set => _xoffset = value; }
        public int Bitpos { get => _bitpos; set => _bitpos = value; }
        public int Reserved { get => _reserved; set => _reserved = value; }
        public int Colstat { get => _colstat; set => _colstat = value; }
        public int Cdefault { get => _cdefault; set => _cdefault = value; }
        public int Domain { get => _domain; set => _domain = value; }
        public int Number { get => _number; set => _number = value; }
        public int Colorder { get => _colorder; set => _colorder = value; }
        public string Autoval { get => _autoval; set => _autoval = value; }
        public int Offset { get => _offset; set => _offset = value; }
        public int Collationid { get => _collationid; set => _collationid = value; }
        public int Status { get => _status; set => _status = value; }
        public int Type { get => _type; set => _type = value; }
        public int Usertype { get => _usertype; set => _usertype = value; }
        public string Printfmt { get => _printfmt; set => _printfmt = value; }
        public int Prec { get => _prec; set => _prec = value; }
        public int Scale { get => _scale; set => _scale = value; }
        public int Iscomputed { get => _iscomputed; set => _iscomputed = value; }
        public int Isoutparam { get => _isoutparam; set => _isoutparam = value; }
        public int Isnullable { get => _isnullable; set => _isnullable = value; }
        public string Collation { get => _collation; set => _collation = value; }
        public byte[] Tdscollation { get => _tdscollation; set => _tdscollation = value; }
    }
}
