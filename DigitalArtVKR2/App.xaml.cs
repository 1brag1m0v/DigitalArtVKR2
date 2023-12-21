using System.Configuration;
using System.Data;
using System.Windows;

namespace DigitalArtVKR2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MenuAdmin madm { get; set; }
        public static ListModules ListModules { get; set; }
        public static CourseModules CourseModules { get; set; }
    }

}
