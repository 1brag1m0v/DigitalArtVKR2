using Microsoft.Win32;
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
using System.IO;
using Path = System.IO.Path;

namespace DigitalArtVKR2
{
    /// <summary>
    /// Логика взаимодействия для LessonModule.xaml
    /// </summary>
    public partial class LessonModule : UserControl
    {
        private string pdflesson;
        private string pdfname;

        public LessonModule()
        {
            InitializeComponent();
        }

        public async void SetLessonData(Lessons lesson)
        {
            if (lesson.Media != "empty")
            {
                try
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(lesson.Media);
                    bitmapImage.EndInit();
                    lessonMedia.Source = bitmapImage;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("фывфыв");
                }
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
            int count = 0;
            var models = await App.madm.supabase.From<PdfLesson>().Get();
            var links = models.Models;
            foreach (var item in links)
            {
                if (item.idLesson == int.Parse(lessonId.Text))
                {
                    count++;
                    pdfListView.Items.Add(item.nameFile);
                }
            }
            nameFileAdmin.Text = "Уроков загружено: " + count.ToString();
            lessonName.Text = lesson.Name;
            //lessonMain.Text = lesson.Text;
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

        public async void LoadFile()
        {
            try
            {
                OpenFileDialog saveFileDialog = new OpenFileDialog();
                if (saveFileDialog.ShowDialog() == true)
                {
                    int pdfid = 0;
                    var basePath = Path.GetDirectoryName(saveFileDialog.FileName);
                    var path = Path.Combine(basePath, saveFileDialog.FileName);
                    await App.madm.supabase.Storage.From("docs/lessons").Upload(path, Path.GetFileName(saveFileDialog.FileName));
                    var models = await App.madm.supabase.From<PdfLesson>().Get();
                    var list = models.Models;
                    var sortlist = from i in list orderby i.Id select i;
                    foreach (var item in sortlist)
                    {
                            pdfid = item.Id + 1;
                    }
                    var newpdf = new PdfLesson()
                    {
                        Id = pdfid,
                        idLesson = int.Parse(lessonId.Text),
                        pdfLink = App.madm.supabase.Storage.From("docs/lessons").GetPublicUrl(Path.GetFileName(saveFileDialog.FileName)),
                        nameFile = Path.GetFileName(saveFileDialog.FileName),
                };
                    await App.madm.supabase.From<PdfLesson>().Insert(newpdf);
                    MessageBox.Show("Загрузка успешна." + App.madm.supabase.Storage.From("docs/lessons").GetPublicUrl(Path.GetFileName(saveFileDialog.FileName)));
                    pdfname = Path.GetFileName(saveFileDialog.FileName);
                    nameFileAdmin.Text = Path.GetFileName(saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке." + ex);
            }
        }

        public async void DeleteFile()
        {
            try
            {
                if (pdflesson == null)
                {
                    MessageBox.Show("Для удаления файла следует выбрать его.");
                    return;
                }
                int id = int.Parse(lessonId.Text);
                await App.madm.supabase.From<PdfLesson>().Where(u => u.idLesson == id && u.nameFile == pdflesson).Delete();
                await App.madm.supabase.Storage.From("docs").Remove(new List<string> { $"lessons/{pdflesson}" });
                MessageBox.Show("Файл удален успешно." + pdflesson);
            }
            catch (Exception ex)
            {
                MessageBox.Show("фывфыв" + ex);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadFile();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DeleteFile();
        }

        private void pdfListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pdflesson = pdfListView.SelectedItem.ToString();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            TestsWindow testsWindow = new TestsWindow(int.Parse(lessonId.Text));
            testsWindow.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            DeleteTests deleteTests = new DeleteTests(int.Parse(lessonId.Text));
            deleteTests.Show();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            CheckPractices checkPractices = new CheckPractices(int.Parse(lessonId.Text));
            checkPractices.Show();
        }
    }
}
