using System;
using System.Diagnostics;
using System.IO;
using BitPantry.ProjectUtils.Output;

namespace BitPantry.ProjectUtils.Component
{
    public class NuGet
    {
        public string TargetProjectFilePath { get; private set; }

        public NuGet(string targetProjectFilePath)
        {
            if (string.IsNullOrEmpty(targetProjectFilePath))
                throw new ArgumentException("targetProjectFilePath cannot be a null or empty string");

            if (!File.Exists(targetProjectFilePath))
                throw new ArgumentException(string.Format("targetProjectFilePath does not exist, \"{0}\"", targetProjectFilePath));


            TargetProjectFilePath = targetProjectFilePath;
        }

        public bool Pack(
            IProcessLogger logger = null,
            bool doOutputSymbols = true)
        {
            var nugetPackStartInfo = new ProcessStartInfo
            {
                FileName = "nuget.exe",
                Arguments =
                    string.Format("pack \"{0}\" {1} -build -outputDirectory \"{2}\" -properties Configuration=Release",
                        TargetProjectFilePath,
                        doOutputSymbols ? "-symbols" : string.Empty,
                        Path.GetDirectoryName(TargetProjectFilePath)),
                WindowStyle = ProcessWindowStyle.Hidden
            };

            return ProcessOutput.OutputExecution(nugetPackStartInfo, logger) == 0;
        }

        public bool Push(
            string packagePath,
            IProcessLogger logger = null,
            string source = null, 
            string apiKey = null)
        {
            var nugetPushStartInfo = new ProcessStartInfo
            {
                FileName = "nuget.exe",
                Arguments = string.Format("push \"{0}\" {1} {2}",
                    packagePath,
                    string.IsNullOrEmpty(apiKey) ? string.Empty : apiKey,
                    string.IsNullOrEmpty(source) ? string.Empty : string.Format("-s {0}", source)),
                WindowStyle = ProcessWindowStyle.Hidden
            };

            return ProcessOutput.OutputExecution(nugetPushStartInfo, logger) == 0;
        }
    }
}
