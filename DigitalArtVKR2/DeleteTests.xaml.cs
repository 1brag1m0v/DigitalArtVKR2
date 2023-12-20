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
using System.Windows.Shapes;

namespace DigitalArtVKR2
{
    /// <summary>
    /// Логика взаимодействия для DeleteTests.xaml
    /// </summary>
    public partial class DeleteTests : Window
    {

        private int idles;
        public DeleteTests(int lessonId)
        {
            InitializeComponent();
            idles = lessonId;
            LoadTestsList();
        }

        private async void LoadTestsList()
        {
            comboIds.Items.Clear();
            var model = await App.madm.supabase.From<LessonTests>().Get();
            var list = model.Models;
            foreach (var item in list)
            {
                if (item.idLesson == idles)
                {
                    comboIds.Items.Add(item.Id);
                }
            }
        }

        private async void DeleteTest()
        {
            if (comboIds.SelectedItem == null)
            {
                MessageBox.Show("Перед удалением теста его нужно выбрать из списка.");
                return;
            }
            int id = int.Parse(comboIds.Text);
            await App.madm.supabase.From<LessonTests>().Where(u => u.Id == id).Delete();
            MessageBox.Show("Тест успешно удален.");
            LoadTestsList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DeleteTest();
        }
    }
}
