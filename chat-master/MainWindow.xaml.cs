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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
    String token = "";
        ObservableCollection<String> contactsList = new ObservableCollection<String>();
        VMPersonneMessage vm;
        string privk;
        string pubk;
        public MainWindow(String user)
        {
            dynamic jsonUser = JObject.Parse(user);
            token = jsonUser["access_token"];
            vm = new VMPersonneMessage(token, user);
            DataContext = vm;  
            InitializeComponent();
            GetContact();
            
        }

        public void addContact(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = true;
            
        }
        public void addContactOkPopup(object sender, RoutedEventArgs e)
        {
            Object receiver = contact.SelectedItem;
            Personne personne = null;
            if (receiver != null)
            {
                personne = (Personne)receiver;
            }
            if (personne != null)
            {


                String BASE_URL = "http://baobab.tokidev.fr/";

                var client = new RestClient(BASE_URL);

                var request = new RestRequest("api/sendMsg", Method.POST);

                request.AddHeader("Accept", "application/json");
                request.AddHeader("Authorization", "Bearer " + token);


                request.AddJsonBody(new
                {
                    message = vm.getPubK(),
                    receiver = personne.nickname
                });

            }
            vm.addContact(addContactName.Text);
            popup.IsOpen = false;
        }

            public void send(object sender, RoutedEventArgs e)
        {
            Object receiver = contact.SelectedItem;
            Personne personne = null;
            if (receiver != null)
            {
                personne = (Personne) receiver;
            }
            if(personne != null)
            {

            
            String BASE_URL = "http://baobab.tokidev.fr/";

            var client = new RestClient(BASE_URL);

            var request = new RestRequest("api/sendMsg", Method.POST);

            request.AddHeader("Accept", "application/json");
                request.AddHeader("Authorization", "Bearer " + token);


            request.AddJsonBody(new
            {
                message = vm.Encrypt(messageContent.Text),
                receiver = personne.nickname
            });

            IRestResponse response = client.Execute(request);
                var content = response.Content;
                if (content == null || content == "")
                {
                    MessageBox.Show("Error.");

                }
                else
                {

                }
            }
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
            if (content == null || content == "")
            {
                MessageBox.Show("Error.");

            }
            else
            {
                HashSet<String> contacts = new HashSet<String>();
                JArray contact = (JArray) JsonConvert.DeserializeObject(content);
                foreach(var con in contact)
                {
                    contacts.Add((String) con["author"]);
                }
                foreach (var con in contacts)
                {
                    contactsList.Add(con);
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
    }

}

