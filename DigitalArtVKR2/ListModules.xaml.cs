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
    /// Логика взаимодействия для ListModules.xaml
    /// </summary>
    public partial class ListModules : Window
    {

        private int courseId = 0;
        public ListModules(int crsId)
        {
            InitializeComponent();
            courseId = crsId;
            App.ListModules = this;
            LoadModules();
        }

        public async void LoadModules()
        {
            itemsControlModules.Items.Clear();
            var model = await App.madm.supabase.From<Modules>().Get();
            var modules = model.Models;
            var sortmodules = from i in modules orderby i.Id select i;
            foreach (var item in sortmodules)
            {
                if (item.courseID == courseId)
                {
                    var itemControl = new CourseModules(item.Id);
                    itemControl.SetModulesData(item);
                    itemsControlModules.Items.Add(itemControl);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewModule newModule = new NewModule(courseId);
            newModule.Show();
        }
    }
}
