using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplikacjaTestujaca
{
    class Exam
    {
        private string Topic;
        private int taskAmount;

        public Exam(string TTopic, int ttaskAmount)
        {
            Topic = TTopic;
            taskAmount = ttaskAmount;
        }
    }
}
