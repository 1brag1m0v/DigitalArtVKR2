using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalArtVKR2
{
    public class Question
    {
       public string text { get; set; }
        public string type { get; set; }
        public List<Answers> answers = new List<Answers>();
        public List<int> corrects = new List<int>();
    }
}
