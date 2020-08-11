using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AproIO.Model.Implementation
{
    public class ScalaRepository : IRepository
    {
        private List<string> _invalidRepos = new List<string>() { "Assembly", "Configuration", "logs", "metastore_db", "docs" };
        public ScalaRepository(DirectoryInfo path)
        {
            Name = path.Name;
            Path = path.FullName;

            if (path.GetFiles("pom.xml").Any())
            {
                Utils = _getUtils(path.GetFiles("pom.xml").First().FullName);
            }

            Processes = path.GetDirectories()
                .Where(t => !t.Name.StartsWith(".") && !_invalidRepos.Contains(t.Name))
                .Select(t => (IProcess)new ScalaProcess(t))
                .Where(t => t.IsValid)
                .ToList();
        }

        private string _getUtils(string path)
        {
            var regex = new Regex(@"<utils\.version>([^<]+)<\/utils\.version>");

            var content = File.ReadAllText(path);
            var match = regex.Match(content);
            if (match.Success && match.Groups.Count == 2)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return "?.?.?";
            }
        }
    }
}