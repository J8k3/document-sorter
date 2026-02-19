using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace dcsrt
{
    public class Token : List<DocumentOccurrence>
    {
        public Token(ref WordBag parent, string rawWord)
        {
            this.ParentWordBag = parent;
            this.Word = rawWord;
        }

        public Token(ref WordBag parent, string rawWord, bool stem)
            : this(ref parent, stem ? rawWord.StemToLowerInvariant() : rawWord)
        { }

        internal WordBag ParentWordBag { get; private set; }

        public string Word { get; private set; }

        public string RawWord { get; private set; }
                
        private double NumberOfDocumentsContainingToken
        {
            get
            {
                //CN
                return this.Count;
            }
        }

        public double DocumentFrequency
        {
            get
            {
                return (double)this.NumberOfDocumentsContainingToken / this.ParentWordBag.DocumentCount;
            }
        }

        public double IDF
        {
            get
            {
                // IDF [Log(DN/CN)]
                return Math.Log((double)this.ParentWordBag.DocumentCount / this.NumberOfDocumentsContainingToken);
            }
        }

        public double NormalizedFrequencyAcrossDocuments
        {
            get
            {
                return this.Select(d => d.NormalizedFrequencyInDocument).Sum() / this.ParentWordBag.DocumentCount;
            }
        }

        public double TFIDF
        {
            get
            {
                //TF*IDF
                return this.NormalizedFrequencyAcrossDocuments * this.IDF;
            }
        }

        public int IndexOf(string documentPath)
        {
            var item = this.Where(t => string.Equals(t.DocumentPath, documentPath, StringComparison.Ordinal)).SingleOrDefault();
            return this.IndexOf(item);
        }
    }
}
