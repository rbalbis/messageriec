using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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

namespace WpfApp1
{
    /// <summary>
    /// Logique d'interaction pour SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Window
    {
        string gender = "";

        //IsolatedStorageFile ISOFile = IsolatedStorageFile.GetUserStoreForApplication();
        List<UserData> ObjUserDataList = new List<UserData>();
        public SignUpPage()
        {
            InitializeComponent();
        }
        public void Gender_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rtn = (RadioButton)sender;
            gender = rtn.Content.ToString();
        }
        private void SignUpPage_Loaded(object sender, RoutedEventArgs e)
        {
            //if (ISOFile.FileExists("RegistrationDetails"))//loaded previous items into list
            //{
            //    using (IsolatedStorageFileStream fileStream = ISOFile.OpenFile("RegistrationDetails", FileMode.Open))
            //    {
            //        //DataContractSerializer serializer = new DataContractSerializer(typeof(List<UserData>));
            //        //ObjUserDataList = (List<UserData>)serializer.ReadObject(fileStream);

            //    }
            //}
        }
        private void Txt_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.BorderBrush = new SolidColorBrush(Colors.LightGray);
        }
        private void pwd_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox pbox = (PasswordBox)sender;
            pbox.BorderBrush = new SolidColorBrush(Colors.LightGray);
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            //UserName Validation
            if (!Regex.IsMatch(TxtUserName.Text.Trim(), @"^[A-Za-z_][a-zA-Z0-9_\s]*$"))
            {
                MessageBox.Show("Invalid UserName");
            }

            //Password length Validation
            else if (TxtPwd.Password.Length < 4)
            {
                MessageBox.Show("Password length should be minimum of 4 characters!");
            }


            //After validation success ,store user detials in isolated storage
            else if (TxtUserName.Text != "" && TxtPwd.Password != "")
            {
                Console.WriteLine(TxtUserName.Text);

                String BASE_URL = "http://baobab.tokidev.fr/";

                var client = new RestClient(BASE_URL);

                var request = new RestRequest("api/createUser", Method.POST);

                request.AddHeader("Accept", "application/json");
                request.AddJsonBody(new
                {
                    username = TxtUserName.Text,
                    password = TxtPwd.Password
                });

                IRestResponse response = client.Execute(request);
                var content = response.Content;
                Console.WriteLine(content);
                if(content == null || content == "")
                {
                    MessageBox.Show("Sorry! user name is already existed.");

                }
                else
                {
                    var newPage = new Login();
                    newPage.Show();
                    this.Close();
                    //NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
                }
            }
            else
            {
                MessageBox.Show("Sorry! user name is already existed.");
            }

        }
    }

}
