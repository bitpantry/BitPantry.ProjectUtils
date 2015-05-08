namespace BitPantry.ProjectUtils.Output
{
    public interface IProcessLogger
    {
        void WriteMessage(string str);
        void WriteError(string str);
    }
}
