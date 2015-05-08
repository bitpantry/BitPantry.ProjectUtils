using BitPantry.StringWriters;

namespace BitPantry.ProjectUtils.Output
{
    class NullConsolidatedStringWriterCollection : IConsolidatedStringWriterCollection
    {
        public ConsolidatedStringWriter Standard { get; private set; }
        public ConsolidatedStringWriter Warning { get; private set; }
        public ConsolidatedStringWriter Error { get; private set; }
        public ConsolidatedStringWriter Debug { get; private set; }
        public ConsolidatedStringWriter Verbose { get; private set; }
        public ConsolidatedStringWriter Accent1 { get; private set; }
        public ConsolidatedStringWriter Accent2 { get; private set; }
        public ConsolidatedStringWriter Accent3 { get; private set; }

        public NullConsolidatedStringWriterCollection()
        {
            Standard = new NullConsolidatedStringWriter();
            Warning = new NullConsolidatedStringWriter();
            Error = new NullConsolidatedStringWriter();
            Debug = new NullConsolidatedStringWriter();
            Verbose = new NullConsolidatedStringWriter();
            Accent1 = new NullConsolidatedStringWriter();
            Accent2 = new NullConsolidatedStringWriter();
            Accent3 = new NullConsolidatedStringWriter();
        }
    }
}
