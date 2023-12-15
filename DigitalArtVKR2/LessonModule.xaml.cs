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
    /// Логика взаимодействия для LessonModule.xaml
    /// </summary>
    public partial class LessonModule : UserControl
    {
        public LessonModule()
        {
            InitializeComponent();
        }

        public void SetLessonData(Lessons lesson)
        {
            if (lesson.Media != "empty")
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(lesson.Media);
                bitmapImage.EndInit();
                lessonMedia.Source = bitmapImage;
            }
            else
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri("https://mebeliero.ru/images/photos/medium/no_image.png");
                bitmapImage.EndInit();
                lessonMedia.Source = bitmapImage;
            }
            lessonId.Text = lesson.Id.ToString();
            lessonUrl.Text = lesson.Media;
            lessonName.Text = lesson.Name;
            lessonMain.Text = lesson.Text;
            if (lesson.Type == 1)
            {
                lessonComboBox.Text = "Лекция";
            }
            else if (lesson.Type == 2)
            {
                lessonComboBox.Text = "Тест";
            }
            else if (lesson.Type == 3)
            {
                lessonComboBox.Text = "Видеоматериал";
            }
        }

        private async void ChangeLessonData()
        {
            var model = await App.madm.supabase.From<Lessons>().Get();
            var lessons = model.Models;
            if ((String.IsNullOrEmpty(lessonName.Text) && String.IsNullOrEmpty(lessonMain.Text) && lessonComboBox.SelectedItem == null)
                || (String.IsNullOrEmpty(lessonName.Text) || String.IsNullOrEmpty(lessonMain.Text) || lessonComboBox.SelectedItem == null))
            {
                MessageBox.Show("Урок должен содержать название, наполнение и тип урока.");
                return;
            }
            int id = int.Parse(lessonId.Text);
            await App.madm.supabase.From<Lessons>().Where(u => u.Id == id).Set(u => u.Media, lessonUrl.Text).Update();
            await App.madm.supabase.From<Lessons>().Where(u => u.Id == id).Set(u => u.Name, lessonName.Text).Update();
            await App.madm.supabase.From<Lessons>().Where(u => u.Id == id).Set(u => u.Text, lessonMain.Text).Update();
            if (lessonComboBox.Text == "Лекция")
            {
                await App.madm.supabase.From<Lessons>().Where(u => u.Id == id).Set(u => u.Type, 1).Update();
            }
            else if (lessonComboBox.Text == "Тест")
            {
                await App.madm.supabase.From<Lessons>().Where(u => u.Id == id).Set(u => u.Type, 2).Update();
            }
            else if (lessonComboBox.Text == "Видеоматериал")
            {
                await App.madm.supabase.From<Lessons>().Where(u => u.Id == id).Set(u => u.Type, 3).Update();
            }
            MessageBox.Show("Урок успешно отредактирован.");
            App.ListModules.LoadModules();
        }

        public async void DeleteLesson()
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены в том, что хотите удалить урок?", "Предупреждение", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                int id = int.Parse(lessonId.Text);
                await App.madm.supabase.From<Lessons>().Where(u => u.Id == id).Delete();
                MessageBox.Show("Урок успешно удален.");
                App.ListModules.LoadModules();
            }
        }

        private void lessonEditButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeLessonData();
        }

        private void lessonDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteLesson();
        }
    }
}
