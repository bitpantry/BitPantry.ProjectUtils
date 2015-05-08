using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptCs.Contracts;

namespace Scriptcs.BitPantry.ProjectUtils
{
    public class ScriptPack : IScriptPack
    {
        public void Initialize(IScriptPackSession session)
        {
            // do nothing
        }

        public IScriptPackContext GetContext()
        {
            return new ProjectUtils();
        }

        public void Terminate()
        {
            // do nothing
        }
    }
}
