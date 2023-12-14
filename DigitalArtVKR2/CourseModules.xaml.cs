﻿using System;
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
    /// Логика взаимодействия для CourseModules.xaml
    /// </summary>
    public partial class CourseModules : UserControl
    {

        public int modId = 0;

        public CourseModules(int Id)
        {
            InitializeComponent();
            modId = Id;
            LoadLessons();
        }

        public async void LoadLessons()
        {
            var model = await App.madm.supabase.From<Lessons>().Get();
            var lessons = model.Models;
            foreach (var item in lessons)
            {
                if (item.moduleID == modId)
                {
                    var itemControl = new LessonModule();
                    itemControl.SetLessonData(item);
                    itemsControlLessons.Items.Add(itemControl);
                }
            }
        }

        public void SetModulesData(Modules module)
        {
            nameModule.Text = module.Name;
        }
    }
}