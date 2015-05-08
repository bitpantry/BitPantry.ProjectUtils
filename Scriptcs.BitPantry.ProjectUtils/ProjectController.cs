using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using BitPantry.ProjectUtils;
using BitPantry.ProjectUtils.Output;
using BitPantry.StringWriters;
using BitPantry.StringWriters.Implementation.Console;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using ScriptCs.Contracts;

namespace Scriptcs.BitPantry.ProjectUtils
{
    public class ProjectController
    {
        private readonly Project _proj = null;
        private readonly IConsolidatedStringWriterCollection _output = null;

        public ProjectController(
            string solutionDirPath,
            string targetProjectFile)
        {
            _proj = new Project(solutionDirPath, targetProjectFile);
            _output = new StandardConsoleWriterCollection();
        }

        public bool Build()
        {
            ConsoleOutputUtil.WriteHeader("Building", _output);

            var result = _proj.MsBuild.Build(new List<ILogger>() { new ConsoleBuildLogger(_output) });

            if (result.OverallResult == BuildResultCode.Success)
            {
                _output.Standard.WriteLine("Build completed successfully");
                return true;
            }
            
            _output.Error.Write("Build failed");
            if (result.Exception != null)
            {
                var ex = result.Exception;
                while (ex != null)
                {
                    _output.Error.WriteLine("{0}\r\n{1}", ex.Message, ex.StackTrace);
                    ex = ex.InnerException;
                }
            }

            return false;
        }

        public void PatchAssemblyVersion(string versionPattern)
        {
            ConsoleOutputUtil.WriteHeader("Patching Assembly Version", _output);

            _output.Standard.WriteLine("Current Version:\t\t{0}", _proj.AssemblyInfo.CurrentAssemblyInfoVersion);
            _proj.AssemblyInfo.Patch(versionPattern);
            _output.Standard.WriteLine("Patched Version:\t\t{0}", _proj.AssemblyInfo.CurrentAssemblyInfoVersion);

            _output.Standard.WriteLine();
            _output.Standard.WriteLine("- Done -");
        }

        public bool Pack(bool doCreateSymbolsPackage = true)
        {
            ConsoleOutputUtil.WriteHeader("Creating NuGet Package", _output);
            if (_proj.NuGet.Pack(new ProcessLogger(_output.Standard, _output.Error), doCreateSymbolsPackage))
            {
                _output.Standard.WriteLine("- Done -");
                return true;
            }
            
            _output.Warning.WriteLine("- Done with Errors -");
            return false;
        }

        public bool Push() { return Push(null); }
        public bool Push(NugetPushConfiguration config)
        {
            ConsoleOutputUtil.WriteHeader("Pushing NuGet Package", _output);

            var rootPackageFilename = Path.GetFileNameWithoutExtension(_proj.TargetProjectFile);

            if(string.IsNullOrEmpty(rootPackageFilename))
                throw new Exception(string.Format("Cannot derive root package file name from project path, \"{0}\"", _proj.TargetProjectFile));

            var targetPackagePath = Path.Combine(_proj.TargetProjectDirectoryPath, string.Format("{0}.{1}.nupkg",
                rootPackageFilename,
                _proj.AssemblyInfo.CurrentAssemblyInfoVersion));

            var targetSymbolsPath = Path.Combine(_proj.TargetProjectDirectoryPath, string.Format("{0}.{1}.symbols.nupkg",
                rootPackageFilename,
                _proj.AssemblyInfo.CurrentAssemblyInfoVersion));

            if (!Push(targetPackagePath, config == null ? null : config.PackageConfiguration))
            {
                _output.Warning.WriteLine("- Done with Errors -");
                return false;
            }

            if (config != null && config.SymbolsConfiguration != null &&
                !string.IsNullOrEmpty(config.SymbolsConfiguration.Source))
            {
                if (!Push(targetSymbolsPath, config.SymbolsConfiguration))
                {
                    _output.Warning.WriteLine("- Done with Errors -");
                    return false;
                }
            }

            _output.Standard.WriteLine("- Done -");
            return true;
        }

        bool Push(
            string packageFilePath, 
            NugetSourceConfiguration config)
        {
            var logger = new ProcessLogger(_output.Standard, _output.Error);

            _output.Accent1.WriteLine("Pushing {0}", packageFilePath);
            _output.Standard.WriteLine();

            if (_proj.NuGet.Push(
                packageFilePath,
                logger,
                config == null ? null : config.Source,
                config == null ? null : config.ApiKey))
            {
                _output.Standard.WriteLine();
                _output.Accent1.WriteLine("Successfully pushed {0}", packageFilePath);
                return true;
            }
            else
            {
                _output.Standard.WriteLine();
                _output.Error.WriteLine("An error occured while pushing {0}", packageFilePath);
                return false;
            }
        }

        public bool PatchBuildPackAndPublish(
            string versionPattern, 
            bool doCreateSymbolsPackage = true,
            NugetPushConfiguration config = null)
        {
            PatchAssemblyVersion(versionPattern);
            if (Build())
            {
                if (Pack(doCreateSymbolsPackage))
                {
                    if (Push(config))
                        return true;
                }
            }

            _output.Standard.WriteLine();
            _output.Standard.WriteLine(("- PROCESS EXITED DUE TO ERRORS -"));
            _output.Standard.WriteLine();

            return false;
        }

    }
}
