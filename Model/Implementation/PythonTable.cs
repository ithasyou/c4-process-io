using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AproIO.Model.Implementation
{
    public class PythonTable : ITable
    {
        public PythonTable(string token, string[] lines, Dictionary<string, string> entities)
        {
            Tipo = lines
                .Where(t => !t.Contains("logger"))
                .All(t => t.Contains("kudu", StringComparison.InvariantCultureIgnoreCase))
                    ? TableType.KUDU
                    : TableType.HIVE;
            IsRead = lines.Any(t => t.Contains("read") || t.Contains("spark.table"));
            IsWrite = lines.Any(t => t.Contains("write") || t.Contains("insert"));
            Schema = entities[token].Split(".")[0];
            TableName = entities[token].Split(".")[1];
        }
    }
}