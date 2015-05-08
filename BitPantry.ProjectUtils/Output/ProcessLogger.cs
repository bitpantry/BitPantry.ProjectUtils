using System;
using BitPantry.StringWriters;

namespace BitPantry.ProjectUtils.Output
{
    public class ProcessLogger : IProcessLogger
    {
        private readonly ConsolidatedStringWriter _messageOutput;
        private readonly ConsolidatedStringWriter _errorOutput;

        public ProcessLogger(ConsolidatedStringWriter messageOutput, ConsolidatedStringWriter errorOutput)
        {
            _messageOutput = messageOutput;
            _errorOutput = errorOutput;
        }

        public void WriteMessage(string str)
        {
            _messageOutput.Write("{0}{1}", str, Environment.NewLine);
        }

        public void WriteError(string str)
        {
            _errorOutput.Write("{0}{1}", str, Environment.NewLine);
        }
    }
}
