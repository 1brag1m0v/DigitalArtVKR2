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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DigitalArtVKR2
{
    /// <summary>
    /// Логика взаимодействия для CourseItem.xaml
    /// </summary>
    public partial class CourseItem : UserControl
    {
        public int crsId = 0;
        public string description;

        public CourseItem()
        {
            InitializeComponent();
        }

        public void SetItemData(Courses course)
        {
            courseId.Text = course.Id.ToString();
            nameCourse.Text = course.Name.ToUpper();
            description = course.Description;
            if (course.difficultID == 1)
            {
                difficultCourse.Text = "УРОВЕНЬ: НАЧАЛЬНЫЙ";
            }
            else if (course.difficultID == 2)
            {
                difficultCourse.Text = "УРОВЕНЬ: ПРОДВИНУТЫЙ";
            }
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(course.Photo);
            bitmapImage.EndInit();
            imageCourse.Source = bitmapImage;
        }

        private void buttonCourse_Click(object sender, RoutedEventArgs e)
        {
            App.madm.itemId = int.Parse(courseId.Text);
            App.madm.courseName.Text = nameCourse.Text;
            App.madm.courseDesc.Text = description;
            if (difficultCourse.Text.Contains("НАЧАЛЬНЫЙ"))
            {
                App.madm.courseLevel.Text = "НАЧАЛЬНЫЙ";
            }
            else if (difficultCourse.Text.Contains("ПРОДВИНУТЫЙ"))
            {
                App.madm.courseLevel.Text = "ПРОДВИНУТЫЙ";
            }
        }

        private void buttonModules_Click(object sender, RoutedEventArgs e)
        {
            crsId = int.Parse(courseId.Text);
            ListModules listModules = new ListModules(crsId);
            listModules.Show();
        }
    }
}
