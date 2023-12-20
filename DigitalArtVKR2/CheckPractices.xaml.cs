using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.IO;

namespace DigitalArtVKR2
{
    /// <summary>
    /// Логика взаимодействия для CheckPractices.xaml
    /// </summary>
    public partial class CheckPractices : Window
    {

        private UserPractices userPractice;

        private int lesidid = 0;
        public CheckPractices(int lesId)
        {
            InitializeComponent();
            lesidid = lesId;
            LoadPractices();
        }

        private async void LoadPractices()
        {
            var model = await App.madm.supabase.From<UserPractices>().Where(u => u.lessonId == lesidid).Get();
            var model2 = await App.madm.supabase.From<Users>().Get();
            var users = model2.Models;
            var practices = model.Models;
            foreach (var item in practices)
            {
                foreach (var item2 in users)
                {
                    if (item.idUser == item2.Id)
                    {
                        item.nameUser = item2.Name;
                        break;
                    }
                }
            }
            listViewPractices.ItemsSource = practices;
        }

        private void listViewPractices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            userPractice = (UserPractices)listViewPractices.SelectedItem;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            DeletePractice();
        }

        private async void DeletePractice()
        {
            if (userPractice != null)
            {
                await App.madm.supabase.From<UserPractices>().Where(u => u.Id == userPractice.Id).Delete();
                MessageBox.Show("Ответ отклонен.");
                LoadPractices();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AcceptPractice();
        }

        private async void AcceptPractice()
        {
            if (userPractice != null)
            {
                int id = 0;
                await App.madm.supabase.From<UserPractices>().Where(u => u.Id == userPractice.Id).Delete();
                var model = await App.madm.supabase.From<LessonsProgress>().Get();
                var list = model.Models;
                var sortlist = from i in list orderby i.Id select i;
                foreach (var item in sortlist)
                {
                    id = item.Id + 1;
                }
                var newLessonProgress = new LessonsProgress()
                {
                    Id = id,
                    lessonId = lesidid,
                    userId = userPractice.idUser
                };
                await App.madm.supabase.From<LessonsProgress>().Insert(newLessonProgress);
                MessageBox.Show("Ответ был принят.");
                LoadPractices();
            }
        }

        private async void DownloadFiles()
        {
            if (userPractice != null)
            {
                byte[] file;
                string[] links = null;
                var model = await App.madm.supabase.From<UserPractices>().Get();
                var list = model.Models;
                foreach (var item in list)
                {
                    if (item.idUser == userPractice.idUser)
                    {
                        links = item.fileLinks.Trim().Split(',');
                    }
                }
                for (int i = 0; i < links.Length; i++)
                {
                    string namefile = "file" + i;
                    MessageBox.Show(links[i]);
                    file = await App.madm.supabase.Storage.From("docs").DownloadPublicFile($"practices/{links[i]}");
                    File.WriteAllBytes(namefile, file);
                    MessageBox.Show("Файл " + links[i] + " успешно скачан.");
                    file = null;
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DownloadFiles();
        }
    }
}
