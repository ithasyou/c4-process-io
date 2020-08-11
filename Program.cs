using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using AproIO.Model;
using AproIO.Model.Implementation;

namespace AproIO
{
    class Program
    {
        static string GetHeader()
        {
            return $"Repository,Utils,Process,Tipo,io,Schema,TableName,FullTableName,Tipo";
        }

        static void AnalyzeIO(params string[] allpaths)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(GetHeader());

            foreach (var path in allpaths)
            {
                Console.Out.WriteLine("Running AnalyzeIO @ " + path);
                var repositories = new DirectoryInfo(path).GetDirectories()
                    .Select(t =>
                    {
                        if (t.GetFiles("pom.xml").Any())
                            return (IRepository)new ScalaRepository(t);
                        else if (t.GetFiles("gradle.properties").Any())
                            return (IRepository)new PythonRepository(t);
                        else
                            return null;
                    })
                    .Where(t => t != null)
                    .OrderBy(t => t.Name);

                foreach (var repo in repositories)
                {
                    repo.GetCsv(sb);
                    Console.Out.WriteLine(repo);
                }
            }

            File.WriteAllText("/home/malobato/proyectos/io.csv", sb.ToString());
        }

        static void Main(string[] args)
        {
            var BASE_PATH = "/home/malobato/proyectos/";
            AnalyzeIO(new List<string>() { "Aprovisionamiento", "Datascience" }.Select(t => BASE_PATH + t).ToArray());
        }

    }
}
