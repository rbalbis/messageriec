using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace WpfApp1
{
    public class VMPersonneMessage
    {
        int id = 0;
        String token = "";
        public ObservableCollection<Personne> contactList { get; set; }

        public VMPersonneMessage(String token)
        {
            contactList = new ObservableCollection<Personne>();
            //Personne p1 = new Personne(1, "mich", new List<string> { "hello", "hi", "ho" });
            //Personne p2 = new Personne(2, "didier", new List<string> { "Salut c didier", "hai" });
            //contactList.Add(p1);
            //contactList.Add(p2);
            this.token = token;
            GetContact();
        }


        public void addContact(string nom)
        {
            if(contactList == null)
            {
                contactList = new ObservableCollection<Personne>();
            }
            Personne newP = new Personne(contactList.Count(), nom);
            contactList.Add(newP);
        }


        public void GetContact()
        {


            String BASE_URL = "http://baobab.tokidev.fr/";

            var client = new RestClient(BASE_URL);

            var request = new RestRequest("api/fetchMessages", Method.GET);

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);

            IRestResponse response = client.Execute(request);
            var content = response.Content;
            Console.WriteLine(content);
            if (content == null || content == "")
            {
                MessageBox.Show("Error.");

            }
            else
            {
                HashSet<String> contacts = new HashSet<String>();
                Console.WriteLine("{0}", content);
                JArray contact = (JArray)JsonConvert.DeserializeObject(content);
                foreach (var con in contact)
                {
                    Personne p = getPersonneByAuthor((String) con["author"]);
                    if(p == null)
                    {
                        p = new Personne((int)con["id"], (String)con["author"], new List<string> { (String)con["msg"] });
                        contactList.Add(p);
                    }
                    else
                    {
                        p.messageList.Add((String)con["msg"]);
                    }
                    
                    id++;
                    //contacts.Add((String)con["author"]);
                }
                //foreach (var name in contacts)
                //{
                    
                //}

                Console.WriteLine("{0}", contactList);
                foreach (var c in contactList)
                {
                    Console.WriteLine("{0}", c);
                }
                //vm.contactList = JObject.Parse(content);


            }

            //String BASE_URL = "http://baobab.tokidev.fr/";
            //WebRequest request = WebRequest.Create(BASE_URL + "api/fetchMessages");
            //request.Method = "GET";
            //WebResponse dataStream = request.GetResponse();
            //Stream responseStream = dataStream.GetResponseStream();
            //XmlTextReader reader = new XmlTextReader(responseStream);
            //while (reader.Read())
            //{
            //    if (reader.NodeType == XmlNodeType.Text)
            //        Console.WriteLine("Message : {0}", reader.Value.Trim());
            //}

            //Console.ReadLine();
            //dataStream.Close();

        }

        public Personne getPersonneByAuthor(String author)
        {
            foreach (Personne contact in contactList)
            {
                if(contact.nickname.Trim() == author.Trim())
                {
                    return contact;
                }
            }
            return null;
        }

        public void sendMessage(Personne from, Personne to,string msg)
        {
            to.addMessage(msg);

        }

        

    }
}
