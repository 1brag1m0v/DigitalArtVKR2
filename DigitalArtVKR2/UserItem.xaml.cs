using System;
using System.Collections.Generic;
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

namespace DigitalArtVKR2
{
    /// <summary>
    /// Логика взаимодействия для UserItem.xaml
    /// </summary>
    public partial class UserItem : UserControl
    {

        public UserItem()
        {
            InitializeComponent();
        }

        public void SetUserData(Users user)
        {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(user.avatarURL);
                bitmapImage.EndInit();
                userId.Text = user.Id.ToString();
                userImage.Source = bitmapImage;
                userName.Text = user.Name;
                userEmail.Text = user.Email;
                userPass.Text = user.Password;
        }

        public async void ChangeUserData()
        {
            var users = App.madm.users;
            if ((String.IsNullOrEmpty(userName.Text) && String.IsNullOrEmpty(userEmail.Text) && String.IsNullOrEmpty(userPass.Text))
                || (String.IsNullOrEmpty(userName.Text) || String.IsNullOrEmpty(userEmail.Text) || String.IsNullOrEmpty(userPass.Text)))
            {
                MessageBox.Show("При редактировании нельзя оставлять пустые поля.");
                return;
            }
            int userIdInt = int.Parse(userId.Text);
            await App.madm.supabase.From<Users>().Where(u => u.Id == userIdInt).Set(u => u.Name, userName.Text).Update();
            await App.madm.supabase.From<Users>().Where(u => u.Id == userIdInt).Set(u => u.Email, userEmail.Text).Update();
            await App.madm.supabase.From<Users>().Where(u => u.Id == userIdInt).Set(u => u.Password, userPass.Text).Update();
            MessageBox.Show("Редактирование прошло успешно.");
            //await supabase.From<Courses>().Where(u => u.Id == itemId).Set(u => u.Description, courseDesc.Text).Update();
        }

        public async void DeleteUser()
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены в том, что хотите удалить пользователя?", "Предупреждение", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                var users = App.madm.users;
                int userIdInt = int.Parse(userId.Text);
                await App.madm.supabase.From<Users>().Where(u => u.Id == userIdInt).Delete();
                MessageBox.Show("Пользователь успешно удален.");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChangeUserData();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DeleteUser();
            App.madm.LoadUsers();
        }
    }
}
