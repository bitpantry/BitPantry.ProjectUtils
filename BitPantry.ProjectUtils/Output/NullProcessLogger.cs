namespace BitPantry.ProjectUtils.Output
{
    class NullProcessLogger : IProcessLogger
    {
        public void WriteMessage(string str)
        {
            // do nothing - null logger
        }

        public void WriteError(string str)
        {
            // do nothing - null logger
        }
    }
}
