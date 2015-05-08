using BitPantry.StringWriters;

namespace BitPantry.ProjectUtils.Output
{
    class NullConsolidatedStringWriter : ConsolidatedStringWriter
    {
        protected override void OnWrite(string str)
        {
            // do nothing - null writer
        }
    }
}
