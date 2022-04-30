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
        public string pytanie, a, b, c, d;
        public void UpdateExam()
        {
            testQuestionTresc.Text = pytanie;
            testQuestionA.Text = a;
            testQuestionB.Text = b;
            testQuestionC.Text = c;
            testQuestionD.Text = d;
        }
        //casualowe dodawanie zmiennych i innej sraki
        public List<ExamTask> eTasks = new List<ExamTask>();
        string taskFilePath = "TaskDatabase.txt";
        public string dane, rank;
        public Form1()
        {
            InitializeComponent();
        }

        private void answerABox_Click(object sender, EventArgs e)
        {

        }

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

        private void startTestBtn_Click(object sender, EventArgs e)
        {
            CollectTasks();
            Exam exam1 = new Exam("Matma", 2);
            exam1.createExam(eTasks);
            //exam1.startExam();
        }

        void CollectTasks()//funkcja zbierajaca wszystkie zadania z pliku txt do listy w programie
        {
            //deklaracja zmiennych
            string fileName = "TaskDatabase.txt";
            int id;
            string tresc, a, b, c, d, correctAnswer, difficulty;
            var lines = File.ReadLines(fileName); //czytanie z pliku


            foreach (var line in lines)//petla wykonujaca sie w KAZDEJ LINIJCE pliku tekstowego
            {
                string[] credientals = line.Split(';');//dzielenie linijki na poszczegolne zmienne oddzielajac je ';'
                id = Int32.Parse(credientals[0]);//zamiana stringa na id
                tresc = credientals[1];
                a = credientals[2];
                b = credientals[3];
                c = credientals[4];
                d = credientals[5];
                correctAnswer = credientals[6];
                difficulty = credientals[7];
                ExamTask eTask = new ExamTask(id,tresc,a,b,c,d,correctAnswer,difficulty); // tworzenie nowego zadania uzywajac powyzszych danych
                eTasks.Add(eTask);//dodanie do listy
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

            //jesli wszystkie okna sa zapelnione to dodaje zadanie
            if (mainQuestionBox.Text != "" && answerABox.Text != "" && answerBBox.Text != "" && answerCBox.Text != "" && answerDBox.Text != "" && correntAnswerBox.Text != "" && difficultyBox.Text != "")
            {
                File.AppendAllText(taskFilePath, (id + ";" +mainQuestionBox.Text + ";" + answerABox.Text + ";" + answerBBox.Text + ";" + answerCBox.Text + ";" + answerDBox.Text + ";" + difficultyBox.Text + ";" + correntAnswerBox.Text) + Environment.NewLine);
                dodanoZadanieLbl.Text = "Dodano Zadanie!";
                dodanoZadanieLbl.Visible = true;
            }
            else//eror
            {
                dodanoZadanieLbl.Text = "Uzupelnij wszystkie pola!";
                dodanoZadanieLbl.Visible = true;
            }
        }


        void LoadStudentList()
        {
            //deklaracje
            string fileName = "UserDatabase.txt";
            string numer, imie, nazwisko;
            var lines = File.ReadLines(fileName);


            foreach (var line in lines)
            {
                //wyswietlanie studentow w bazie danych

                string[] credientals = line.Split(';');
                numer = credientals[0];
                imie = credientals[2];
                nazwisko = credientals[3];
                rank = credientals[4];

                if(rank == "Student")
                {
                    //deklaracja jakiegos gowna
                    int rowId = dataGridView1.Rows.Add();
                    DataGridViewRow row = dataGridView1.Rows[rowId];

                    //dodawanie rowow z wyzej wypisanymi danymi
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
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //podstawowa deklaracja gui po zaladowaniu sie programu
            
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
                //to sie robi gdy laduje sie student
                //tabControl.TabPages.Remove(mainProfesorPage);
            }
            if (rank == "Profesor")
            {
                //a to gdy profesor
                //tabControl.TabPages.Remove(mainPage);
            }
            LoadStudentList();//ladowanie studenciakow do listy


        }
    }
}
