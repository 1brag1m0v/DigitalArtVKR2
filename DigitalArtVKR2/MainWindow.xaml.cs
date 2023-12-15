using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DigitalArtVKR2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Supabase.Client supabase;

        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }

        public async void Initialize()
        {
            try
            {
                var url = Environment.GetEnvironmentVariable("https://gaxlrywbsvtamlbizmjt.supabase.co");
                var key = Environment.GetEnvironmentVariable("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImdheGxyeXdic3Z0YW1sYml6bWp0Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MDIyNzU4NTYsImV4cCI6MjAxNzg1MTg1Nn0.7bVx2U7_9xGbHTQQFfMEfv3OjsE00zfj4k9zSdvepoY");

                var options = new Supabase.SupabaseOptions
                {
                    AutoConnectRealtime = true
                };

                supabase = new Supabase.Client("https://gaxlrywbsvtamlbizmjt.supabase.co", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImdheGxyeXdic3Z0YW1sYml6bWp0Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MDIyNzU4NTYsImV4cCI6MjAxNzg1MTg1Nn0.7bVx2U7_9xGbHTQQFfMEfv3OjsE00zfj4k9zSdvepoY", options);


                await supabase.InitializeAsync();
            }
            catch (Exception ex) {

            }
        }

        public async void AttemptToAuth()
        {
            if ((String.IsNullOrEmpty(emailBox.Text) && String.IsNullOrEmpty(passBox.Text)
                || (String.IsNullOrEmpty(emailBox.Text) || String.IsNullOrEmpty(passBox.Text)))) 
            {
                MessageBox.Show("Введите правильные данные.");
                return;
            }
            var users = await supabase.From<Users>().Get();
            var list = users.Models;
            foreach (var item in list)
            {
                if (item.Email == emailBox.Text && item.Password == passBox.Text)
                {
                    MessageBox.Show($"Добро пожаловать, {item.Name}!");
                    Data.name = item.Name;
                    Data.idemp = item.Id;
                    MenuAdmin menuAdmin = new MenuAdmin();
                    menuAdmin.Show();
                    this.Close();
                    return;
                }
            }
            MessageBox.Show("Введите корректные данные.");
        }

        private void buttonAuth_Click(object sender, RoutedEventArgs e)
        {
            AttemptToAuth();
        }
    }
}