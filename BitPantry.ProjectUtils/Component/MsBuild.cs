using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;

namespace BitPantry.ProjectUtils.Component
{
    public class MsBuild
    {
        public string ProjectOrSolutionFilePath { get; private set; }

        public Dictionary<string, string> Properties { get; set; }
        public string ToolsVersion { get; set; }
        public List<string> BuildTargets { get; set; }

        public MsBuild(string projectOrSolutionFilePath)
        {
            // process project or solution file path

            if (string.IsNullOrEmpty(projectOrSolutionFilePath))
                throw new ArgumentException("projectOrSolutionFilePath cannot be a null or empty string");

            if (!File.Exists(projectOrSolutionFilePath))
                throw new ArgumentException(string.Format("projectOrSolutionFilePath does not exist, \"{0}\"", projectOrSolutionFilePath));

            ProjectOrSolutionFilePath = projectOrSolutionFilePath;

            // initialize property defaults

            Properties = new Dictionary<string, string>()
            {
                {"Configuration", "Debug"},
                {"Platform", "Any CPU"},
            };

            ToolsVersion = "4.0";

            BuildTargets = new List<string>(new[] { "Build" });
        }

        public BuildResult Build(List<ILogger> loggers = null)
        {
            // validate target project or solution file exists

            if(string.IsNullOrEmpty(ProjectOrSolutionFilePath))
                throw new ArgumentException("The ProjectOrSolutionPath cannot be null or empty");

            if (!File.Exists(ProjectOrSolutionFilePath))
                throw new FileNotFoundException(string.Format("ProjectOrSolutionFilePath could not be found, \"{0}\"", ProjectOrSolutionFilePath));

            ValidateProperties();

            // add output path to properties if needed and doesn't already exist


            if (!Properties.Any(p => p.Key.Equals("OutputPath", StringComparison.Ordinal))
                && Path.GetExtension(ProjectOrSolutionFilePath).Equals(".csproj", StringComparison.OrdinalIgnoreCase))
            {
                var projectOrSolutionDirectoryPath = Path.GetDirectoryName(ProjectOrSolutionFilePath);
                if(string.IsNullOrEmpty(projectOrSolutionDirectoryPath))
                    throw new InvalidOperationException(string.Format("A directory path cannot be found for the ProjectOrSolutionFilePath, \"{0}\"", ProjectOrSolutionFilePath));
                
                Properties.Add("OutputPath",
                    Path.Combine(projectOrSolutionDirectoryPath,
                        string.Format(@"bin\{0}", Properties["Configuration"])));
            }

            // setup build parameters and data

            var buildParams = new BuildParameters(new ProjectCollection());
            
            if(loggers != null && loggers.Any())
                buildParams.Loggers = loggers;          
            
            var buildRequestData = new BuildRequestData(
                ProjectOrSolutionFilePath, 
                Properties, 
                ToolsVersion, 
                BuildTargets.ToArray(), 
                null);

            // build

            return BuildManager.DefaultBuildManager.Build(buildParams, buildRequestData); 
        }

        void ValidateProperties()
        {
            // validate properties

            if (Properties == null)
                throw new InvalidOperationException("Cannot build with null properties");

            if (!Properties.Any(p => p.Key.Equals("Configuration", StringComparison.Ordinal)))
                throw new InvalidOperationException("Missing 'Configuration' property");

            if (string.IsNullOrEmpty(ToolsVersion))
                throw new InvalidOperationException("ToolsVersion cannot be a null or empty string");

            if (BuildTargets == null || !BuildTargets.Any())
                throw new InvalidOperationException("At least one build target must be specified");
        }

    }
}
