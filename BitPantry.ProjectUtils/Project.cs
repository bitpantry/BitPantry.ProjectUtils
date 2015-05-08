using System;
using System.IO;
using BitPantry.ProjectUtils.Component;

namespace BitPantry.ProjectUtils
{
    public class Project
    {
        // project file / directory paths

        public string SolutionDirPath { get; private set; }
        public string TargetProjectFile { get; private set; }
        public string TargetProjectDirectoryPath { get; set; }

        // components 

        public AssemblyInfo AssemblyInfo { get; private set; }
        public MsBuild MsBuild { get; private set; }
        public NuGet NuGet { get; private set; }

        public Project(
            string solutionDirPath,
            string targetProjectFile)
        {
            // process solution dir path

            if (string.IsNullOrEmpty(solutionDirPath))
                throw new ArgumentException("solutionDirPath cannot be a null or empty string");

            if (!Directory.Exists(solutionDirPath))
                throw new ArgumentException(string.Format("solutionDirPath does not exist, \"{0}\"", solutionDirPath));

            SolutionDirPath = solutionDirPath;

            // process target project file

            if (string.IsNullOrEmpty(targetProjectFile))
                throw new ArgumentException("targetProjectFile cannot be a null or empty string");

            if (!File.Exists(targetProjectFile))
                throw new ArgumentException(string.Format("targetProjectFile does not exist, \"{0}\"", targetProjectFile));

            TargetProjectFile = targetProjectFile;

            // set project directory path

            TargetProjectDirectoryPath = Path.GetDirectoryName(TargetProjectFile);
            if (string.IsNullOrEmpty(TargetProjectDirectoryPath))
                throw new InvalidOperationException(string.Format("Could not get directory path from TargetProjectFile, \"{0}\"", TargetProjectFile));

            // instantiate components

            AssemblyInfo = new AssemblyInfo(SolutionDirPath, TargetProjectFile);
            MsBuild = new MsBuild(TargetProjectFile);
            NuGet = new NuGet(TargetProjectFile);
        }


    }
}
