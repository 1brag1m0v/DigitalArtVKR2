using Postgrest.Attributes;
using Supabase.Gotrue;
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
    /// Логика взаимодействия для NewModule.xaml
    /// </summary>
    public partial class NewModule : Window
    {
        private int idc = 0;
        private Supabase.Client supabase;

        public NewModule(int idCourse)
        {
            InitializeComponent();
            idc = idCourse;
            Initialize();
        }

        public async void Initialize()
        {
            var options = new Supabase.SupabaseOptions
            {
                AutoConnectRealtime = true
            };

            supabase = new Supabase.Client("https://gaxlrywbsvtamlbizmjt.supabase.co", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImdheGxyeXdic3Z0YW1sYml6bWp0Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MDIyNzU4NTYsImV4cCI6MjAxNzg1MTg1Nn0.7bVx2U7_9xGbHTQQFfMEfv3OjsE00zfj4k9zSdvepoY", options);


            await supabase.InitializeAsync();
        }

        public async void AddModule()
        {
                    int idmod = 0;
                    int idles = 0;
                    int lessonscount = 0;
                    if ((String.IsNullOrEmpty(moduleName.Text)))
                    {
                        MessageBox.Show("Для добавления модуля нужно указать его название.");
                        return;
                    }
                    var model = await supabase.From<Modules>().Get();
                    var modules = model.Models;
                    foreach (var module in modules)
                    {
                        if (module.Id > idmod)
                        {
                            idmod = module.Id + 1;
                        }
                    }
                    var newModule = new Modules()
                    {
                        Id = idmod,
                        courseID = idc,
                        Name = moduleName.Text,
                    };
                    await supabase.From<Modules>().Insert(newModule);
                    var model2 = await supabase.From<Lessons>().Get();
                    var lessons = model2.Models;

                    foreach (var item in lessons)
                    {
                        if (item.Id > idles)
                        {
                            idles = item.Id + 1;
                        }
                    }
                    if (String.IsNullOrEmpty(moduleLessonsCount.Text))
                    {
                        lessonscount = 1;
                    }
                    else if (int.Parse(moduleLessonsCount.Text) > 0)
                    {
                        lessonscount = int.Parse(moduleLessonsCount.Text);
                    }
                    for (int i = 1; i <= lessonscount; i++)
                    {
                        var newLesson = new Lessons()
                        {
                            Id = idles,
                            moduleID = newModule.Id,
                            Name = "empty",
                            Type = 1,
                            Media = "empty",
                            Text = "empty"
                        };
                        idles++;
                        await supabase.From<Lessons>().Insert(newLesson);
                    }
                    MessageBox.Show("Модуль успешно добавлен.");
                    App.ListModules.LoadModules();
                    this.Close();
        }

        private void lessonEditButton_Click(object sender, RoutedEventArgs e)
        {
            AddModule();
        }
    }
}
