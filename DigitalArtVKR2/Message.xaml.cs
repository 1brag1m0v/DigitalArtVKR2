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
    /// Логика взаимодействия для Message.xaml
    /// </summary>
    public partial class Message : UserControl
    {
        public Message(Messages message)
        {
            InitializeComponent();
            var users = App.madm.users;
            foreach (var user in users)
            {
                if (message.senderID == user.Id)
                {
                    nameUser.Text = user.Name;
                }
                if (message.senderID == -1 || message.senderID == 2)
                {
                    rec.Fill = Brushes.Aquamarine;
                }
            }
            messageUser.Text = message.Message;
        }
    }
}
