using System;
using System.Diagnostics;
using BitPantry.StringWriters;

namespace BitPantry.ProjectUtils.Output
{
    /// <summary>
    /// Provides output helper functions
    /// </summary>
    static class ProcessOutput
    {

        public static int OutputExecution(
            ProcessStartInfo info,
            IProcessLogger logger)
        {
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;

            var proc = Process.Start(info);

            if(proc == null)
                throw new InvalidProgramException("Process.Start has generated a null process");

            logger = logger ?? new NullProcessLogger();

            proc.ErrorDataReceived += (sender, e) => { if (e.Data != null) { logger.WriteError(e.Data); } }; 
            proc.OutputDataReceived += (sender, e) => { if(e.Data != null) { logger.WriteMessage(e.Data); }};

            proc.BeginErrorReadLine();
            proc.BeginOutputReadLine();

            proc.WaitForExit();
            return proc.ExitCode;
        }

    }


}
