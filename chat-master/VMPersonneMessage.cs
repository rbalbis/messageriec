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
using System.Security.Cryptography;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WpfApp1
{
    public class VMPersonneMessage
    {
        private Database db;

        int id = 0;
        String token = "";
        private dynamic jsonUser;
        public ObservableCollection<Personne> contactList { get; set; }

        private static string _privateKey;
        private static string _publicKey;


        public VMPersonneMessage(String token, string user)
        {
            db = new Database();

            db.Test(db.GetConn());

            jsonUser = JObject.Parse(user);

            string username = jsonUser["username"];

            string key = db.userKey(username);

            if(key == ""){
                RSA();
                db.insertUser(username, _privateKey, _publicKey);
            }
            else {
                _privateKey = key;
                _publicKey = db.userPubKey(username);

                System.Console.WriteLine("aaaaaaaaaaaaaaaa");
                System.Console.WriteLine(key); }

            contactList = new ObservableCollection<Personne>();
            this.token = token;
            GetContact();
        }

        public string getPubK()
        {
            return _publicKey;
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
            if (content == null || content == "")
            {
                MessageBox.Show("Error.");

            }
            else
            {
                HashSet<String> contacts = new HashSet<String>();
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
                }
    

            }

         

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

        private static UnicodeEncoding _encoder = new UnicodeEncoding();

        private static void RSA()
        {
            var rsa = new RSACryptoServiceProvider();
            _privateKey = rsa.ToXmlString(true);
            _publicKey = rsa.ToXmlString(false);

           /* var text = "Test1";
            Console.WriteLine("RSA // Text to encrypt: " + text);
            var enc = Encrypt(text);
            Console.WriteLine("RSA // Encrypted Text: " + enc);
            var dec = Decrypt(enc);
            Console.WriteLine("RSA // Decrypted Text: " + dec);

    */
        }

        public string Decrypt(string data)
        {
            var rsa = new RSACryptoServiceProvider();
            var dataArray = data.Split(new char[] { ',' });
            byte[] dataByte = new byte[dataArray.Length];
            for (int i = 0; i < dataArray.Length; i++)
            {
                dataByte[i] = Convert.ToByte(dataArray[i]);
            }

            rsa.FromXmlString(_privateKey);
            var decryptedByte = rsa.Decrypt(dataByte, false);
            return _encoder.GetString(decryptedByte);
        }

        public string Encrypt(string data)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(_publicKey);
            var dataToEncrypt = _encoder.GetBytes(data);
            var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();
            var length = encryptedByteArray.Count();
            var item = 0;
            var sb = new StringBuilder();
            foreach (var x in encryptedByteArray)
            {
                item++;
                sb.Append(x);

                if (item < length)
                    sb.Append(",");
            }

            return sb.ToString();
        }



    }
}
