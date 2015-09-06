using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthTwoDemo.XForms
{
    public class BulkMatch
    {
        public string name { get; set; }
        public string description { get; set; }
        public JObject matchData { get; set; }
        public int overallMatch
        {
            get
            {
                if(matchData != null)
                    return matchData["overall"].Value<int>();
                return 0;
            }
        }
    }
}
