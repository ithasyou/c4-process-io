using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AproIO.Model
{
    public abstract class IProcess
    {
        public IProcess()
        {
            Tables = new List<ITable>();
        }

        public string Path { get; protected set; }
        public string Name { get; protected set; }
        public abstract bool IsValid { get; }
        public List<ITable> Tables { get; protected set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("\t" + (IsValid ? "" : "INVALID: ") + Name);
            Tables.ForEach(t => sb.AppendLine(t.ToString()));
            return sb.ToString();
        }

        public void GetCsv(string header, string utils, StringBuilder sb)
        {
            Tables.ForEach(t => sb.AppendLine($"{header},{utils},{t.GetCsv(Name)}"));
        }
    }
}