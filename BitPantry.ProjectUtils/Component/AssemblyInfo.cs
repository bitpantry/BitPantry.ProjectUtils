using System;
using System.IO;
using BitPantry.AssemblyPatcher;

namespace BitPantry.ProjectUtils.Component
{
    public class AssemblyInfo
    {
        public string SolutionDirPath { get; private set; }
        public string TargetProjectFile { get; private set; }
        public Version CurrentAssemblyInfoVersion { get; set; }

        public AssemblyInfo(
            string solutionDirPath, 
            string targetProjectFile)
        {
            // process solution dir path

            if(string.IsNullOrEmpty(solutionDirPath))
                throw new ArgumentException("solutionDirPath cannot be a null or empty string");
            
            if(!Directory.Exists(solutionDirPath))
                throw new ArgumentException(string.Format("solutionDirPath does not exist, \"{0}\"", solutionDirPath));

            SolutionDirPath = solutionDirPath;
            
            // process target project file

            if (string.IsNullOrEmpty(targetProjectFile))
                throw new ArgumentException("targetProjectFile cannot be a null or empty string");

            if (!File.Exists(targetProjectFile))
                throw new ArgumentException(string.Format("targetProjectFile does not exist, \"{0}\"", targetProjectFile));

            TargetProjectFile = targetProjectFile;

            // load the current assembly version

            ReadCurrentAssemblyVersion();
        }

        public void Patch(string versionPattern)
        {
            if (string.IsNullOrEmpty(versionPattern))
                throw new InvalidOperationException("A version pattern has not been applied.");

            CurrentAssemblyInfoVersion = VersionPatcher.Patch(
                SolutionDirPath,
                TargetProjectFile,
                versionPattern).AssemblyVersion.Version;

        }

        void ReadCurrentAssemblyVersion()
        {
            var assemblyInfoFilePath = Path.Combine(Path.GetDirectoryName(TargetProjectFile), "properties", "AssemblyInfo.cs");
            CurrentAssemblyInfoVersion = new AssemblyInfoFile(assemblyInfoFilePath).AssemblyVersion.Version;
        }
    }
}
