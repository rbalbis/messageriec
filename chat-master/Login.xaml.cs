using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
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

namespace WpfApp1
{
    /// <summary>
    /// Logique d'interaction pour Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        //IsolatedStorageFile ISOFile = IsolatedStorageFile.GetUserStoreForApplication();
        List<UserData> ObjUserDataList = new List<UserData>();
        public Login()
        {
            InitializeComponent();
        }

        public void Login_Click(object sender, RoutedEventArgs e)
        {
            if (UserName.Text != "" && PassWord.Password != "")
            {

                String BASE_URL = "http://baobab.tokidev.fr/";

                var client = new RestClient(BASE_URL);

                var request = new RestRequest("api/login", Method.POST);

                request.AddHeader("Accept", "application/json");
                request.AddJsonBody(new
                {
                    username = UserName.Text,
                    password = PassWord.Password
                });

                IRestResponse response = client.Execute(request);
                var content = response.Content;
                Console.WriteLine(content);
                if (content == null || content == "")
                {
                    MessageBox.Show("Enter UserID/Password");
                }
                else
                {
                    MessageBox.Show("Identitfiant correct");

                }
            }
            else
            {
                MessageBox.Show("Enter UserID/Password");
            }
        }


        public void SignUp_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/SignUpPage.xaml", UriKind.Relative));
        }
        //protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        //{
        //    var Result = MessageBox.Show("Are you sure you want to exit?", "", MessageBoxButton.OKCancel);
        //    if (Result == MessageBoxResult.OK)
        //    {
        //        if (NavigationService.CanGoBack)
        //        {
        //            while (NavigationService.RemoveBackEntry() != null)
        //            {
        //                NavigationService.RemoveBackEntry();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        e.Cancel = true;
        //    }
        //}
        private void UserName_GotFocus(object sender, RoutedEventArgs e)
        {
            UserName.BorderBrush = new SolidColorBrush(Colors.LightGray);
            PassWord.BorderBrush = new SolidColorBrush(Colors.LightGray);
        }

    }
}
