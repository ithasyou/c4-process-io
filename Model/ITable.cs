using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AproIO.Model.Implementation;

namespace AproIO.Model
{
    public enum TableType
    {
        KUDU,
        HIVE,
        UNKNOWN
    }

    public abstract class ITable
    {
        public TableType Tipo { get; protected set; }
        public bool IsRead { get; protected set; }
        public bool IsWrite { get; protected set; }
        public bool IsReadWrite { get { return IsRead && IsWrite; } }
        public bool IsValid { get { return IsRead || IsWrite; } }
        public string Schema { get; protected set; }
        public string TableName { get; protected set; }
        public int SortOrder { get { return IsRead ? 9999 : IsReadWrite ? 999 : IsWrite ? 99 : 9; } }


        public override string ToString()
        {
            var io = IsReadWrite ? "R / W" : IsRead ? "READ " : IsWrite ? "WRITE" : "?????";
            return $"\t\t [{Tipo} {io}] - {Schema}.{TableName}";
        }

        public string GetCsv(string header)
        {
            var io = IsReadWrite ? "READ & WRITE" : IsRead ? "READ" : IsWrite ? "WRITE" : "?????";
            var tipo = this is PythonTable ? "Python" : "Scala";
            return $"{header},{Tipo},{io},{Schema},{TableName},{Schema}.{TableName},{tipo}";
        }
    }
}