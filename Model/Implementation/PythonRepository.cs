using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AproIO.Model.Implementation
{
    public class PythonRepository : IRepository
    {

        public PythonRepository(DirectoryInfo path)
        {
            Name = path.Name;
            Path = path.FullName;

            var entites = readEntities(path);

            Processes = new DirectoryInfo(path.FullName + "/lib").GetFiles("*.py")
                .Select(t => (IProcess)new PythonProcess(t, entites))
                .Where(t => t.IsValid)
                .ToList();
        }

        private Dictionary<string, string> readEntities(DirectoryInfo path)
        {
            if (File.Exists(path + "/share/tags.py"))
            {
                return readEntitiesLines(File.ReadAllLines(path + "/share/tags.py")).ToDictionary(t => t.Item1, t => t.Item2);
            }
            else if (File.Exists(path + "/share/constants.py"))
            {
                return readEntitiesLines(File.ReadAllLines(path + "/share/constants.py")).ToDictionary(t => t.Item1, t => t.Item2);
            }
            else
            {
                return new Dictionary<string, string>();
            }
        }

        private IEnumerable<(string, string)> readEntitiesLines(string[] lines)
        {
            foreach (var line in lines)
            {
                if (line.StartsWith("TABLENAME_"))
                {
                    var split = line.Replace("\"", "").Replace(" ", "").Replace("'", "").Split("=");
                    yield return (split[0], split[1]);

                }
                else
                {
                    continue;
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(Name);
            Processes.ForEach(t => sb.AppendLine(t.ToString()));
            return sb.ToString();
        }

        public void GetCsv(StringBuilder sb)
        {

        }
    }
}