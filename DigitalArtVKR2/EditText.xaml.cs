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
    /// Логика взаимодействия для EditText.xaml
    /// </summary>
    public partial class EditText : Window
    {

        private int lesid;
        public EditText(int lessonId)
        {
            InitializeComponent();
            lesid = lessonId;
            LoadText();
        }

        async void LoadText()
        {
            try
            {
                var model = await App.madm.supabase.From<Lessons>().Where(u => u.Id == lesid).Get();
                var lesson = model.Models;
                foreach (var item in lesson)
                {
                    lessonText.Text = item.Text;
                }
            }
            catch (Exception ex)
            {

            }
        }

        async void ChangeText()
        {
            await App.madm.supabase.From<Lessons>().Where(u => u.Id == lesid).Set(u => u.Text, lessonText.Text).Update();
            MessageBox.Show("Наполнение урока успешно изменено.");
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChangeText();
        }
    }
}
