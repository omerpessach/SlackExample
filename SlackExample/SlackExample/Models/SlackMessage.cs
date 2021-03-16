using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlackExample.Models
{
    public class SlackMessage
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("channel")]
        public string UserID { get; set; }
    }
}
