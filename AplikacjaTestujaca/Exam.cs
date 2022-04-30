using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AplikacjaTestujaca
{
    class Exam
    {
        private string Topic;
        private int taskAmount;
        Form1 f11orm = new Form1();
        List<ExamTask> allTasks = new List<ExamTask>();
        List<ExamTask> examTasks = new List<ExamTask>();
        public Exam(string TTopic, int ttaskAmount)
        {
            Topic = TTopic;
            taskAmount = ttaskAmount;
        }
        public void getTasks(List<ExamTask> tasks)
        {
            allTasks = tasks;
        }
        public void createExam(List<ExamTask> tasks)
        {
            allTasks = tasks;
            List<int> listNumbers = new List<int>();
            Random rng = new Random();
            int randomTaskId;
            for (int i = 0; i < taskAmount; i++)
            {
                //do
                //{
                //    randomTaskId = rng.Next(1, allTasks.Count + 1);
                //} while (listNumbers.Contains(randomTaskId));
                randomTaskId = rng.Next(0, allTasks.Count);
                //listNumbers.Add(randomTaskId);
                examTasks.Add(allTasks[randomTaskId]);
            }
            MessageBox.Show(examTasks[0].getTresc());
            //Form1.pytanie = examTasks[0].getTresc();
            //Form1 form = new Form1();
            //form.UpdateExam();
            startExam();


        }
        public void startExam()
        {
            for(int i = 0; i < taskAmount;i++)
            {
                Form1 form = new Form1();
                form.pytanie = examTasks[i].getTresc();
                form.a = examTasks[i].getA();
                form.b = examTasks[i].getB();
                form.c = examTasks[i].getC();
                form.d = examTasks[i].getD();
                form.UpdateExam();
            }
        }
    }
}
