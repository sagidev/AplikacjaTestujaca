using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroSet_UI;
using MetroSet_UI.Forms;

namespace AplikacjaTestujaca
{
    public partial class Form1 : MetroSetForm
    {
        public int question = 0;
        public int taskAmount = 0;
        public int points = 0;
        public int index = 0;
        
        public void UpdateExam(string pytanie, string a, string b, string c, string d)
        {
            testQuestionTresc.Text = pytanie;
            testQuestionA.Text = a;
            testQuestionB.Text = b;
            testQuestionC.Text = c;
            testQuestionD.Text = d;
        }
        public List<ExamTask> eTasks = new List<ExamTask>();
        List<int> indexes = new List<int>();
        string taskFilePath = "TaskDatabase.txt";
        public string dane, rank;
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Funkcja tworząca egzamin z aktualnej bazy pytan
        /// </summary>
        /// <param name="taskAmount"></param>
        void CreateExam(int taskAmount)//wip
        {
            List<ExamTask> examTasks = new List<ExamTask>();
            List<int> listNumbers = new List<int>();
            Random rng = new Random();
            int randomTaskId;
            for (int i = 0; i < taskAmount; i++)
            {
                do
                {
                    randomTaskId = rng.Next(1, eTasks.Count + 1);
                } while (listNumbers.Contains(randomTaskId));
                listNumbers.Add(randomTaskId);
                examTasks.Add(eTasks[randomTaskId]);
                
            }
        }

        /// <summary>
        /// Funkcja wczytujaca pytania z pliku
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startTestBtn_Click(object sender, EventArgs e)
        {
            CollectTasks();
            updateExams();
            string fileName = "ExamDatabase.txt";
            string[] lines = File.ReadAllLines(fileName);
            string topic = "";
            int taskamount = 0;
            string[] exam = lines[index].Split(';');
            
            topic = exam[0];
            taskamount = Convert.ToInt32(exam[1]);
            taskAmount = taskamount;
            for (int l = 2; l < taskamount+2; l++)
            {
                indexes.Add(Convert.ToInt32(exam[l]));
            }
            Exam egzamin = new Exam(topic, taskamount);
            egzamin.addIndexes(indexes);
            question = 0;
            askQuestion(indexes[question]);
        }

        /// <summary>
        /// Funkcja wyswietlajaca pytanie
        /// </summary>
        /// <param name="index"></param>
        public void askQuestion(int index)
        {
            maxTasksLbl.Text = taskAmount.ToString();
            currentTaskLbl.Text = (question + 1).ToString();
            testQuestionTresc.Text = eTasks[index].getTresc();
            testQuestionA.Text = eTasks[index].getA();
            testQuestionB.Text = eTasks[index].getB();
            testQuestionC.Text = eTasks[index].getC();
            testQuestionD.Text = eTasks[index].getD();
        }

        /// <summary>
        /// Funkcja zbierajaca wszystkie zadania z pliku txt do listy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nextBtn_Click(object sender, EventArgs e)
        {
            string answer = "";
            if(answerBox.SelectedIndex != -1)
            {
                if (answerBox.SelectedItem.ToString() == "A")
                {
                    answer = "A";
                }
                else if (answerBox.SelectedItem.ToString() == "B")
                {
                    answer = "B";
                }
                else if (answerBox.SelectedItem.ToString() == "C")
                {
                    answer = "C";
                }
                else if (answerBox.SelectedItem.ToString() == "D")
                {
                    answer = "D";
                }
                string correctAnswer = eTasks[indexes[question]].getCorrectAnswer();
                MessageBox.Show(eTasks[indexes[question]].getCorrectAnswer());
                if (correctAnswer == answer)
                {
                    points++;
                }
                question++;
                if (question >= taskAmount)
                {
                    finishTest();
                }
                else
                {
                    askQuestion(indexes[question]);
                }
            }
            else
            {
                MessageBox.Show("Odpowiedz nie zostala wybrana");
            }

        }

        /// <summary>
        /// Funkcja zamykajaca test i wyswietlajaca wynik
        /// </summary>
        public void finishTest()
        {
            MessageBox.Show("finished with " + points + " points");
            points = 0;
            question = 0;
            maxTasksLbl.Text = "0";
            currentTaskLbl.Text = "0";
            testQuestionTresc.Text = "";
            testQuestionA.Text = "";
            testQuestionB.Text = "";
            testQuestionC.Text = "";
            testQuestionD.Text = "";
            answerBox.SelectedIndex = 0;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Funkcja zbierajaca wszystkie zadania z pliku txt do listy w programie
        /// </summary>
        void CollectTasks()
        {
            string fileName = "TaskDatabase.txt";
            int id;
            string tresc, a, b, c, d, correctAnswer, difficulty;
            var lines = File.ReadLines(fileName);

            foreach (var line in lines)
            {
                string[] credientals = line.Split(';');
                id = Int32.Parse(credientals[0]);
                tresc = credientals[1];
                a = credientals[2];
                b = credientals[3];
                c = credientals[4];
                d = credientals[5];
                correctAnswer = credientals[7];
                difficulty = credientals[6];
                ExamTask eTask = new ExamTask(id,tresc,a,b,c,d,correctAnswer,difficulty);
                eTasks.Add(eTask);
            }
        }

        private void addQuestionBox_Click(object sender, EventArgs e)
        {
            string fileName = "TaskDatabase.txt";
            var lines = File.ReadLines(fileName);
            int id = 0;

            foreach (var line in lines)
            {
                id++;
            }

            if (mainQuestionBox.Text != "" && answerABox.Text != "" && answerBBox.Text != "" && answerCBox.Text != "" && answerDBox.Text != "" && correntAnswerBox.Text != "" && difficultyBox.Text != "")
            {
                File.AppendAllText(taskFilePath, (id + ";" +mainQuestionBox.Text + ";" + answerABox.Text + ";" + answerBBox.Text + ";" + answerCBox.Text + ";" + answerDBox.Text + ";" + difficultyBox.Text + ";" + correntAnswerBox.Text) + Environment.NewLine);
                dodanoZadanieLbl.Text = "Dodano Zadanie!";
                dodanoZadanieLbl.Visible = true;
            }
            else
            {
                dodanoZadanieLbl.Text = "Uzupelnij wszystkie pola!";
                dodanoZadanieLbl.Visible = true;
            }
        }

        /// <summary>
        /// Funkcja ladujaca liste studentow do tabeli
        /// </summary>
        void LoadStudentList()
        {
            string fileName = "UserDatabase.txt";
            string numer, imie, nazwisko;
            var lines = File.ReadLines(fileName);

            foreach (var line in lines)
            {
                string[] credientals = line.Split(';');
                numer = credientals[0];
                imie = credientals[2];
                nazwisko = credientals[3];
                rank = credientals[4];

                if(rank == "Student")
                {
                    int rowId = dataGridView1.Rows.Add();
                    DataGridViewRow row = dataGridView1.Rows[rowId];

                    dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    row.Cells["Numer"].Value = numer;
                    row.Cells["Imie"].Value = imie;
                    row.Cells["Nazwisko"].Value = nazwisko;
                }
            }
        }


        private void metroSetButton1_Click(object sender, EventArgs e)
        {
            CollectTasks();
            int taskAmount = Convert.ToInt32(taskAmountBox.Text);
            string topic = testNameBox.Text;
            Exam egzamin = new Exam(topic, taskAmount);
            
            Random rng = new Random();
            List<int> indexes = new List<int>();
            int number;
            for (int i = 0; i < taskAmount; i++)
            {
                do
                {
                    number = rng.Next(0, eTasks.Count);
                    egzamin.addTasks(eTasks[number]);
                    egzamin.addIndex(number);
                } while (indexes.Contains(number));
                indexes.Add(number);
            }
            string fromHour = (fromHourBox.Text + ":" + fromMinBox.Text);
            string toHour = (toHourBox.Text + ":" + toMinBox.Text);
            string fileName = "ExamDatabase.txt";
            using (TextWriter tw = new StreamWriter(fileName, append: true))
            {
                tw.Write("\n"+egzamin.getTopic());
                tw.Write(";" + taskAmount);
                foreach (int i in indexes)
                    tw.Write(";"+i);
                tw.Write(";" + fromHour);
                tw.Write(";" + toHour);
            }
            updateExams();
        }

        /// <summary>
        /// Funkcja aktualizujaca liste egzaminow
        /// </summary>
        public void updateExams()
        {
            examsBox.Items.Clear();
            examsBox.Visible = true;
            string fileName = "ExamDatabase.txt";
            var lines = File.ReadLines(fileName);

            foreach (var line in lines)
            {
                string[] exam = line.Split(';');
                examsBox.Items.Add(exam[0]);
            }
        }

        private void examsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            index = examsBox.SelectedIndex;
        }

        /// <summary>
        /// Inicjalizacja GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            updateExams();
            rankLbl.Text = rank;
            imieLbl.Text = dane;
            mainRankLbl.Text = rank;
            mainDaneLbl.Text = dane;

            mainPage.Style = MetroSet_UI.Enums.Style.Dark;
            createTestPage.Style = MetroSet_UI.Enums.Style.Dark;
            addQuestionsPage.Style = MetroSet_UI.Enums.Style.Dark;
            studentListPage.Style = MetroSet_UI.Enums.Style.Dark;

            if (rank == "Student")
            {
                tabControl.TabPages.Remove(createTestPage);
                tabControl.TabPages.Remove(addQuestionsPage);
            }
            if (rank == "Profesor")
            {
                tabControl.TabPages.Remove(mainPage);
            }
            LoadStudentList();
        }
    }
}
