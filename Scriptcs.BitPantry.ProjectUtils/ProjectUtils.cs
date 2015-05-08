using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptCs.Contracts;

namespace Scriptcs.BitPantry.ProjectUtils
{
    public class ProjectUtils : IScriptPackContext
    {
        public ProjectController Create(
            string solutionDirPath,
            string targetProjectFile)
        {
            return new ProjectController(solutionDirPath, targetProjectFile);
        }
    }
}
