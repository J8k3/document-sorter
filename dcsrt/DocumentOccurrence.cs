namespace dcsrt
{
    public class DocumentOccurrence
    {
        public DocumentOccurrence(ref WordBag parent, string word, string documentPath)
        {
            this.ParentWordBag = parent;
            this.Word = word;
            this.DocumentPath = documentPath;
            this.OccurrenceCount = 0;
        }
        private string Word { get; set; }
        internal WordBag ParentWordBag { get; private set; }
        internal Token ParentToken
        {
            get
            {
                int tokenIndex = this.ParentWordBag.IndexOf(this.Word);

                return this.ParentWordBag[tokenIndex];
            }
        }

        public string DocumentPath { get; private set; }
        public double OccurrenceCount { get; set; }

        public double MostFrequentWordInDocumentCount { get; set; }

        public double NormalizedFrequencyInDocument
        {
            get
            {
                //TF
                return this.OccurrenceCount / this.MostFrequentWordInDocumentCount;
            }
        }
    }
}
