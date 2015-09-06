using Xamarin.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
public class AppUser
    {
        public string name { get; set; }
        public UriImageSource profileIcon { get; set; }
        public string description { get; set; }
        public JObject matchData { get; set; }
        public int Rating { get; set; }
        public int overallMatchScore
        {
            get
            {
                if (matchData != null)
                    return matchData["overall"].Value<int>();
                return 0;
            }
        }
    }