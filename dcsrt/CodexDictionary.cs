using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace dcsrt
{
    [JsonObject]
    public class CodexDictionary : IEnumerable<Codex>
    {
        public CodexDictionary()
        {
            this.Dictionary = new List<Codex>();
        }
        public string RootPath { get; set; }

        public IEnumerable<Codex> Dictionary { get; set; }

        public IEnumerator<Codex> GetEnumerator()
        {
            return this.Dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
