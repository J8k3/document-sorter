using Newtonsoft.Json;
using System.Collections.Generic;

namespace dcsrt
{
    [JsonObject]
    public class Codex
    {
        public Codex()
        {
            this.Words = new List<string>();
        }
        public IEnumerable<string> Words { get; set; }
        public string Destination { get; set; }
        public string RuleId { get; set; }
    }
}
