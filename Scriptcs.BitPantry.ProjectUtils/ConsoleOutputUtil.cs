using BitPantry.StringWriters;

namespace Scriptcs.BitPantry.ProjectUtils
{
    /// <summary>
    /// Provides output helper functions
    /// </summary>
    static class ConsoleOutputUtil
    {
        public static void WriteHeader(
            string headerText, 
            IConsolidatedStringWriterCollection output, 
            int totalWidth = 100, 
            int topBottomMargin = 1)
        {
            var margin = (totalWidth - headerText.Length) / 2;
            headerText = string.Format("{0}{1}{0}", string.Empty.PadLeft(margin, ' '), headerText);
            var headerFooter = string.Empty.PadLeft(topBottomMargin, '\r');

            var bar = string.Empty.PadLeft(headerText.Length + 2, '-');

            output.Accent2.WriteLine(headerFooter);
            output.Accent2.WriteLine(bar);
            output.Standard.WriteLine("|{0}|", headerText);
            output.Accent2.WriteLine(bar);
            output.Accent2.WriteLine(headerFooter);
        }



    }


}
