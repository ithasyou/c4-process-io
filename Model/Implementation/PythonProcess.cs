using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AproIO.Model.Implementation
{
    public class PythonProcess : IProcess
    {
        public PythonProcess(FileInfo path, Dictionary<string, string> entities)
        {
            Name = path.Name.Replace(".py", "");
            Path = path.FullName;

            var allText = File.ReadAllLines(path.FullName);
            Tables = _readTokens(allText, entities)
                .GroupBy(t => t.Item1)
                .Select(t => (ITable)new PythonTable(t.Key, t.Select(x => x.Item2).ToArray(), entities))
                .ToList();
        }

        private IEnumerable<(string, string)> _readTokens(string[] lines, Dictionary<string, string> entities)
        {
            foreach (var line in lines)
            {
                var regex = "TABLENAME_[A-Z0-9_]*";
                var match = new Regex(regex).Match(line);

                if (match.Success)
                {
                    yield return (match.Value, line);
                }
                else
                {
                    continue;
                }
            }
        }

        public override bool IsValid { get { return Name != "__init__" && Tables.Any(); } }
    }
}