using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthTwoDemo.XForms
{
    public class User
    {
        public string id { get; set; }
        public string name { get; set; }
        public Posts posts { get; set; }
        public string bio { get; set; }

        public string personality { get; set; }
    }
    public class Datum
    {
        public string message { get; set; }
        public string id { get; set; }
    }

    public class Paging
    {
        public string previous { get; set; }
        public string next { get; set; }
    }

    public class Posts
    {
        public List<Datum> data { get; set; }
        public Paging paging { get; set; }
    }



}
