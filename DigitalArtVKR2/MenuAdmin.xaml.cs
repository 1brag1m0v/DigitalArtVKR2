using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace DigitalArtVKR2
{
    /// <summary>
    /// Логика взаимодействия для MenuAdmin.xaml
    /// </summary>
    public partial class MenuAdmin : Window
    {
        public Supabase.Client supabase;
        public int itemId = 0;
        public List<Users> users;
        public MenuAdmin()
        {
            InitializeComponent();
            Initialize();
            App.madm = this;
            this.Title = $"Меню администратора ({Data.name})";
            ClearStackPanel();
            LoadCourses();
        }

        public async void Initialize()
        {
            var options = new Supabase.SupabaseOptions
            {
                AutoConnectRealtime = true
            };

            supabase = new Supabase.Client("https://gaxlrywbsvtamlbizmjt.supabase.co", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImdheGxyeXdic3Z0YW1sYml6bWp0Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MDIyNzU4NTYsImV4cCI6MjAxNzg1MTg1Nn0.7bVx2U7_9xGbHTQQFfMEfv3OjsE00zfj4k9zSdvepoY", options);


            await supabase.InitializeAsync();

            var model = await supabase.From<Users>().Get();
            users = model.Models;
        }

        private void ClearBackgrounds()
        {
            courseButton.Background = null;
            testButton.Background = null;
            usersButton.Background = null;
            chatButton.Background = null;
            materialButton.Background = null;
        }

        private void ClearTextBoxes()
        {
            courseName.Text = "";
            courseDesc.Text = "";
            courseLevel.SelectedItem = null;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClearBackgrounds();
            LoadStackPanel();
            coursesList.Visibility = Visibility.Visible;
            chat.Visibility = Visibility.Hidden;
            courseButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF596C75"));
            LoadCourses();
        }

        private async void LoadCourses()
        {
            itemsControlMain.Items.Clear();
            var courses = await supabase.From<Courses>().Get();
            var list = courses.Models;
            foreach (var item in list)
            {
                var itemControl = new CourseItem();
                itemControl.SetItemData(item);
                itemsControlMain.Items.Add(itemControl);
            }
        }

        private void ClearStackPanel()
        {
            addNewUser.Visibility = Visibility.Visible;
            courseTitle.Visibility = Visibility.Hidden;
            courseName.Visibility = Visibility.Hidden;
            courseDesc.Visibility = Visibility.Hidden;
            courseLevel.Visibility = Visibility.Hidden;
            addCourseButton.Visibility = Visibility.Hidden;
            editCourseButton.Visibility = Visibility.Hidden;
            deleteCourseButton.Visibility = Visibility.Hidden;
            courseStackPanel.Visibility = Visibility.Hidden;
        }

        private void LoadStackPanel()
        {
            addNewUser.Visibility = Visibility.Hidden;
            courseStackPanel.Visibility = Visibility.Visible;
            courseTitle.Visibility = Visibility.Visible;
            courseName.Visibility = Visibility.Visible;
            courseDesc.Visibility = Visibility.Visible;
            courseLevel.Visibility = Visibility.Visible;
            addCourseButton.Visibility = Visibility.Visible;
            editCourseButton.Visibility = Visibility.Visible;
            deleteCourseButton.Visibility = Visibility.Visible;
        }

        private async void DeleteCourse()
        {
            if (itemId == 0)
            {
                MessageBox.Show("Для удаления необходимо выбрать курс.");
                return;
            }
            await supabase.From<Courses>().Where(u => u.Id == itemId).Delete();
            LoadCourses();
            ClearTextBoxes();
            MessageBox.Show("Курс успешно удален.");
        }

        private async void AddCourse()
        {
            int idcor = 0;
            int iddif = 0;
            if ((String.IsNullOrEmpty(courseName.Text) && String.IsNullOrEmpty(courseDesc.Text) && courseLevel.SelectedItem == null)
                || String.IsNullOrEmpty(courseName.Text) || String.IsNullOrEmpty(courseDesc.Text) || courseLevel.SelectedItem == null)
            {
                MessageBox.Show("Одно из полей было пустым, пожалуйста, заполните все поля.");
                return;
            }
            var courses = await supabase.From<Courses>().Get();
            var list = courses.Models;
            foreach (var item in list)
            {
                if (item.Id > idcor)
                {
                    idcor = item.Id;
                }
            }
            if (courseLevel.Text == "НАЧАЛЬНЫЙ")
            {
                iddif = 1;
            }
            else if (courseLevel.Text == "ПРОДВИНУТЫЙ")
            {
                iddif = 2;
            }
            var newCourse = new Courses()
            {
                Id = idcor + 1,
                Name = courseName.Text,
                Description = courseDesc.Text,
                Photo = "https://w.forfun.com/fetch/c4/c4278a31d1820f1df09421a893726338.jpeg",
                difficultID = iddif
            };
            await supabase.From<Courses>().Insert(newCourse);
            LoadCourses();
            ClearTextBoxes();
            MessageBox.Show("Курс успешно добавлен.");
        }
        private async void EditCourse()
        {
            if ((String.IsNullOrEmpty(courseName.Text) && String.IsNullOrEmpty(courseDesc.Text) && courseLevel.SelectedItem == null)
                || String.IsNullOrEmpty(courseName.Text) || String.IsNullOrEmpty(courseDesc.Text) || courseLevel.SelectedItem == null)
            {
                MessageBox.Show("Одно из полей было пустым, пожалуйста, заполните все поля.");
                return;
            }
            await supabase.From<Courses>().Where(u => u.Id == itemId).Set(u => u.Name, courseName.Text).Update();
            if (courseLevel.Text == "ПРОДВИНУТЫЙ")
            {
                await supabase.From<Courses>().Where(u => u.Id == itemId).Set(u => u.difficultID, 2).Update();
            }
            else if (courseLevel.Text == "НАЧАЛЬНЫЙ")
            {
                await supabase.From<Courses>().Where(u => u.Id == itemId).Set(u => u.difficultID, 1).Update();
            }
            await supabase.From<Courses>().Where(u => u.Id == itemId).Set(u => u.Description, courseDesc.Text).Update();
            LoadCourses();
            ClearTextBoxes();
            MessageBox.Show("Редактирование прошло успешно.");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EditCourse();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddCourse();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DeleteCourse();
        }

        private void testButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            itemsControlMain.Items.Clear();
            ClearStackPanel();
            ClearBackgrounds();
            testButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF596C75"));
        }

        private void chatButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClearStackPanel();
            ClearBackgrounds();
            coursesList.Visibility = Visibility.Hidden;
            chat.Visibility = Visibility.Visible;
            chatButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF596C75"));
            LoadChat();
        }

        public async void LoadChat()
        {
            itemsControlMessage.Items.Clear();
            var chat = await supabase.From<Messages>().Get();
            var list = chat.Models;
            foreach (var item in list)
            {
                itemsControlMessage.Items.Add(new Message(item));
            }
        }

        public async void SendMessage()
        {
            int iddd = 0;
            if (String.IsNullOrEmpty(sendMessageBox.Text))
            {
                MessageBox.Show("Нельзя отправить пустое сообщение.");
                return;
            }
            var model = await supabase.From<Messages>().Get();
            var chat = model.Models;
            foreach (var item in chat)
            {
                if (item.Id > iddd)
                {
                    iddd = item.Id + 1;
                }
            }
            var newMessage = new Messages()
            {
                Id = iddd,
                dateCreated = DateTime.Now,
                senderID = Data.idemp,
                Message = sendMessageBox.Text,
                takerID = 1
            };
            await supabase.From<Messages>().Insert(newMessage);
            LoadChat();
            MessageBox.Show("Сообщение отправлено.");
            sendMessageBox.Clear();
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        public async void LoadUsers()
        {
            itemsControlUsers.Items.Clear();
            var model = await supabase.From<Users>().Get();
            var users = model.Models;
            foreach (var item in users)
            {
                if (item.RoleID != 2)
                {
                    var userItem = new UserItem();
                    userItem.SetUserData(item);
                    itemsControlUsers.Items.Add(userItem);
                }
            }
        }

        private void usersButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClearStackPanel();
            ClearBackgrounds();
            coursesList.Visibility = Visibility.Hidden;
            chat.Visibility = Visibility.Hidden;
            usersList.Visibility = Visibility.Visible;
            usersButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF596C75"));
            LoadUsers();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            addNewUser.Visibility = Visibility.Visible;
            addUserStackPanel.Visibility = Visibility.Hidden;
            userBoxName.Visibility = Visibility.Visible;
            userBoxEmail.Visibility = Visibility.Visible;
            userBoxPass.Visibility = Visibility.Visible;
            addUserButton.Visibility = Visibility.Visible;
            cancelUserButton.Visibility = Visibility.Visible;
        }

        private void cancelUserButton_Click(object sender, RoutedEventArgs e)
        {
            courseStackPanel.Visibility = Visibility.Visible;
            ClearStackPanel();
            addUserStackPanel.Visibility = Visibility.Visible;
            userBoxName.Visibility = Visibility.Hidden;
            userBoxEmail.Visibility = Visibility.Hidden;
            userBoxPass.Visibility = Visibility.Hidden;
            addUserButton.Visibility = Visibility.Hidden;
            cancelUserButton.Visibility = Visibility.Hidden;
        }

        private async void addUserButton_Click(object sender, RoutedEventArgs e)
        {
            if ((String.IsNullOrEmpty(userBoxName.Text) && String.IsNullOrEmpty(userBoxEmail.Text) && String.IsNullOrEmpty(userBoxPass.Text))
                || (String.IsNullOrEmpty(userBoxName.Text) || String.IsNullOrEmpty(userBoxEmail.Text) || String.IsNullOrEmpty(userBoxPass.Text)))
            {
                MessageBox.Show("Нельзя добавить пользователя с пустыми данными.");
                return;
            }
            int idus = 0;
            var model = await supabase.From<Users>().Get();
            var users = model.Models;
            foreach (var item in users)
            {
                if (item.Id > idus)
                {
                    idus = item.Id + 1;
                }
            }

            var newUser = new Users()
            {
                Id = idus,
                avatarURL = "https://i.imgur.com/yO3a9BJ.png",
                Password = userBoxPass.Text,
                Email = userBoxEmail.Text,
                registerDate = DateTime.Now,
                RoleID = 1,
                Name = userBoxName.Text
            };

            await supabase.From<Users>().Insert(newUser);
            MessageBox.Show("Пользователь успешно добавлен.");
            userBoxName.Clear();
            userBoxEmail.Clear();
            userBoxPass.Clear();
            LoadUsers();
            cancelUserButton_Click(sender, e);
        }

        private void TextBlock_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
