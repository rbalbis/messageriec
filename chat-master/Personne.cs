using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Personne
    {
        public string nickname { get; set; }
        public int id { get; set; }
        public List<string> messageList { get; set; }

        public Personne(int id, string nick)
        {
            this.id = id;
            this.nickname = nick;
            this.messageList = new List<string>();
        }

        public Personne(int id, string nick, List<string> msglist)
        {
            this.id = id;
            this.nickname = nick;
            this.messageList = msglist;

        }
    }
}
