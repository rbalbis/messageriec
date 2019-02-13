using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class VMPersonneMessage
    {

        public ObservableCollection<Personne> contactList { get; set; }

        public VMPersonneMessage()
        {
            contactList = new ObservableCollection<Personne>();
            Personne p1 = new Personne(1, "mich", new List<string> { "hello", "hi", "ho" });
            Personne p2 = new Personne(2, "didier", new List<string> { "Salut c didier", "hai" });
            contactList.Add(p1);
            contactList.Add(p2);
        }

        

    }
}
