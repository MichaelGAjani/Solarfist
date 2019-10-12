using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Jund.DynamicCodeHelper.Entity.ObjectEnum;

namespace Jund.DynamicCodeHelper.Entity
{
    public class ExternalProgram
    {
        int _id;
        string _name;
        string _path;
        string _entryName;
        ExternalProgramType _entryType;
        decimal _version;

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public string Path { get => _path; set => _path = value; }
        public string EntryName { get => _entryName; set => _entryName = value; }
        public ExternalProgramType EntryType { get => _entryType; set => _entryType = value; }
        public decimal Version { get => _version; set => _version = value; }
    }
}
