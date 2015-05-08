using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitPantry.ProjectUtils.Output;

namespace BitPantry.ProjectUtils
{
    public class NugetPushConfiguration
    {
        public NugetSourceConfiguration PackageConfiguration { get; set; }
        public NugetSourceConfiguration SymbolsConfiguration { get; set; }
    }
}
