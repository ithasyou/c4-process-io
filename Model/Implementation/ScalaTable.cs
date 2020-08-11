using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AproIO.Model.Implementation
{
    public class ScalaTable : ITable
    {
        public ScalaTable(string path, Dictionary<string, string> entities)
        {
            Path = path;
            FileName = new FileInfo(path).Name;

            var content = File.ReadAllText(path);
            var regex = new Regex(@"tableName:(\sString)?\s=\s([A-Za-z0-9_]+)\.([A-Za-z0-9_]+)(\.([A-Za-z0-9_]+))?");
            var match = regex.Match(content);
            if (match.Success && match.Groups.Count == 6)
            {
                if (string.IsNullOrEmpty(match.Groups[5].Value))
                {
                    Schema = match.Groups[2].Value.ToLower();
                    TableName = entities[match.Groups[3].Value];
                }
                else
                {
                    Schema = match.Groups[3].Value.ToLower();
                    TableName = entities[match.Groups[5].Value];
                }
            }

            if (content.Contains("FromToHiveTable"))
            {
                IsRead = true;
                IsWrite = true;
                Tipo = TableType.HIVE;
            }
            else if (content.Contains("FromHiveTable"))
            {
                IsRead = true;
                Tipo = TableType.HIVE;
            }
            else if (content.Contains("ToHiveTable"))
            {
                IsWrite = true;
                Tipo = TableType.HIVE;
            }
            else if (content.Contains("FromToKuduTable"))
            {
                IsRead = true;
                IsWrite = true;
                Tipo = TableType.KUDU;
            }
            else if (content.Contains("FromKuduTable"))
            {
                IsRead = true;
                Tipo = TableType.KUDU;
            }
            else if (content.Contains("ToKuduTable"))
            {
                IsWrite = true;
                Tipo = TableType.KUDU;
            }
            else
            {
                Tipo = TableType.UNKNOWN;
            }
        }

        public static List<ITable> LoadTables(string path)
        {
            var dict = LoadDictionary(new DirectoryInfo(path)
                .GetFiles("*.scala")
                .Where(t => t.Name == "entities.scala")
                .FirstOrDefault()?.FullName);

            return new DirectoryInfo(path)
                .GetFiles("*.scala")
                .Where(t => t.Name != "entities.scala")
                .Select(t => (ITable)new ScalaTable(t.FullName, dict))
                .OrderByDescending(t => t.SortOrder)
                .ToList();
        }

        private static Dictionary<string, string> LoadDictionary(string path)
        {
            var regex = new Regex(@"val ([A-Za-z0-9_]+)(: String)? = ""([A-Za-z0-9_]+)""");

            var content = File.ReadAllText(path);
            var matches = regex.Matches(content);

            var dict = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var match in matches.Where(t => t.Success && t.Groups.Count == 4))
            {
                dict.TryAdd(match.Groups[1].Value, match.Groups[3].Value);
            }

            return dict;
        }

        public string Path { get; set; }
        public string FileName { get; set; }
    }
}