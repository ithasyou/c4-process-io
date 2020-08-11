using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AproIO.Model
{
    public abstract class IRepository
    {
        public IRepository()
        {
            Processes = new List<IProcess>();
        }

        public string Path { get; set; }
        public string Name { get; set; }
        public string Utils { get; set; }
        public List<IProcess> Processes { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(Name);
            Processes.ForEach(t => sb.AppendLine(t.ToString()));
            return sb.ToString();
        }

        public void GetCsv(StringBuilder sb)
        {
            Processes.OrderBy(t => t.Name).ToList().ForEach(t => t.GetCsv(Name, Utils, sb));
        }
    }
}