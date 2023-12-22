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
using Newtonsoft.Json;

namespace DigitalArtVKR2
{
    /// <summary>
    /// Логика взаимодействия для TestsWindow.xaml
    /// </summary>
    public partial class TestsWindow : Window
    {
        private List<int> ids = new List<int>();
        private int lesid;
        private List<Question> question2 = new List<Question>();
        private int questCount = 0;
        public TestsWindow(int lessonId)
        {
            InitializeComponent();
            lesid = lessonId;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(headerQuestion.Text))
            {
                MessageBox.Show("Для добавления вопроса нужен его заголовок.");
                return;
            }
            if (String.IsNullOrEmpty(quest1.Text) && String.IsNullOrEmpty(quest2.Text) && String.IsNullOrEmpty(quest3.Text) 
                && String.IsNullOrEmpty(quest4.Text) && String.IsNullOrEmpty(quest4.Text) && String.IsNullOrEmpty(quest5.Text) && String.IsNullOrEmpty(quest6.Text))
            {
                MessageBox.Show("Для добавления вопроса нужен хотя бы один правильный ответ.");
                return;
            }
            if (correct1.IsChecked == false && correct2.IsChecked == false && correct3.IsChecked == false
                && correct4.IsChecked == false && correct5.IsChecked == false && correct6.IsChecked == false)
            {
                MessageBox.Show("Для добавления вопроса справа нужно выбрать, какой ответ будет правильным.");
                return;
            }
            int idcount = 0;
            if (typeQuestion.SelectedIndex == 0)
            {
                    if (correct1.IsChecked == true)
                    {
                        idcount++;
                    }
                    if (correct2.IsChecked == true)
                    {
                        idcount++;
                    }
                    if (correct3.IsChecked == true)
                    {
                        idcount++;
                    }
                    if (correct4.IsChecked == true)
                    {
                        idcount++;
                    }
                    if (correct5.IsChecked == true)
                    {
                        idcount++;
                    }
                    if (correct6.IsChecked == true)
                    {
                        idcount++;
                    }
                if (idcount > 1)
                {
                    MessageBox.Show("Если тип вопроса выбран 'Одиночный ответ', то выбранный правильный ответ на вопрос должен быть один.");
                    return;
                }
            }
            Question question = new Question();
            question.text = headerQuestion.Text;
            if (typeQuestion.SelectedIndex == 0)
            {
                question.type = "single";
            }
            else if (typeQuestion.SelectedIndex == 1)
            {
                question.type = "multiple";
            }
            Answers answer = new Answers();
            List<string> list = new List<string>()
            {
                quest1.Text, quest2.Text, quest3.Text, quest4.Text, quest5.Text, quest6.Text
            };
            int i = 1;
            foreach (var item in list)
            {
                if (!String.IsNullOrEmpty(item))
                {
                    answer.text = item;
                    answer.id = i;
                    question.answers.Add(answer);
                    i++;
                    answer = new Answers();
                }
            }
            if (correct1.IsChecked == true)
            {
                question.corrects.Add(1);
            }
            if (correct2.IsChecked == true)
            {
                question.corrects.Add(2);
            }
            if (correct3.IsChecked == true)
            {
                question.corrects.Add(3);
            }
            if (correct4.IsChecked == true)
            {
                question.corrects.Add(4);
            }
            if (correct5.IsChecked == true)
            {
                question.corrects.Add(5);
            }
            if (correct6.IsChecked == true)
            {
                question.corrects.Add(6);
            }
            question2.Add(question);
            questCount++;
            addedQuestions.Text = "Количество добавленных вопросов: " + questCount.ToString();
            finishTest.IsEnabled = true;
            ClearQuest();
        }

        private void correct1_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ClearQuest()
        {
            headerQuestion.Text = "";
            typeQuestion.SelectedIndex = 0;
            quest1.Text = "";
            quest2.Text = "";
            quest3.Text = "";
            quest4.Text = "";
            quest5.Text = "";
            quest6.Text = "";
            correct1.IsChecked = false;
            correct2.IsChecked = false;
            correct3.IsChecked = false;
            correct4.IsChecked = false;
            correct5.IsChecked = false;
            correct6.IsChecked = false;
        }
        public async void SaveTestInDb()
        {
            var json = JsonConvert.SerializeObject(question2);
            int idtest = 0;
            var model = await App.madm.supabase.From<LessonTests>().Get();
            var list = model.Models;
            var sortlist = from i in list orderby i.Id select i;
            foreach (var item in sortlist)
            {
                idtest = item.Id + 1;
            }
            var newTest = new LessonTests()
            {
                Id = idtest,
                idLesson = lesid,
                jsonData = json
            };
            await App.madm.supabase.From<LessonTests>().Insert(newTest);
            MessageBox.Show("Новый тест успешно добавлен.");
            this.Close();
        }
        private void finishTest_Click(object sender, RoutedEventArgs e)
        {
            SaveTestInDb();
        }
    }
}
