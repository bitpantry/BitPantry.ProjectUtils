using System;
using BitPantry.ProjectUtils;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            const string solutionDirPath = @"C:\VS Projects\BitPantry\BitPantry.StringWriters\";
            const string targetProjectFile = @"C:\VS Projects\BitPantry\BitPantry.StringWriters\BitPantry.StringWriters\BitPantry.StringWriters.csproj";
            const string versionPattern = "{#}.{#}.{#}.{increment}";

            var proj = new Project(solutionDirPath, targetProjectFile);

            proj.AssemblyInfo.Patch(versionPattern);
            proj.NuGet.Pack();

            Console.ReadLine();
        }
    }
}
