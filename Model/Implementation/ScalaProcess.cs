using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AproIO.Model.Implementation
{
    public class ScalaProcess : IProcess
    {
        public ScalaProcess(DirectoryInfo path)
        {
            Name = path.Name;
            Path = path.FullName;

            EntitiesPath = _findEntitiesFolder(path);
            Tables = !string.IsNullOrEmpty(EntitiesPath) ? ScalaTable.LoadTables(EntitiesPath) : new List<ITable>()
                .ToList();
        }

        private string _findEntitiesFolder(DirectoryInfo path)
        {
            if (path.GetFiles("entities.scala").Any())
            {
                return path.FullName;
            }
            else if (path.GetDirectories().Any())
            {
                return path.GetDirectories()
                    .Select(t => _findEntitiesFolder(t))
                    .Where(t => t != null)
                    .FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public string EntitiesPath { get; set; }
        public override bool IsValid { get { return !string.IsNullOrEmpty(EntitiesPath) && Tables.Any(); } }

    }
}