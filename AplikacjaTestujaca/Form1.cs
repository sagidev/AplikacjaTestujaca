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
        //public string pytanie, a, b, c, d;
        public static int maxPoints = 0;
        public static System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
        public int question = 0;
        public int taskAmount = 0;
        public int points = 0;
        public int index = 0;
        public static int timeLeft;
        public static string time;
        public void UpdateExam(string pytanie, string a, string b, string c, string d)
        {
            testQuestionTresc.Text = pytanie;
            testQuestionA.Text = a;
            testQuestionB.Text = b;
            testQuestionC.Text = c;
            testQuestionD.Text = d;
        }
        //casualowe dodawanie zmiennych i innej sraki
        public List<ExamTask> eTasks = new List<ExamTask>();
        public List<string> listaZdajacych = new List<string>();
        List<int> indexes = new List<int>();
        string taskFilePath = "TaskDatabase.txt";
        public static string dane = "", rank;
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
            try
            {
                CollectTasks();
                updateExams();
                //int index = examsBox.SelectedIndex;
                string fileName = "ExamDatabase.txt";
                maxPoints = 0;
                string[] lines = File.ReadAllLines(fileName);
                //var lines = File.ReadLines(fileName); //czytanie z pliku
                string topic = "";

                int taskamount = 0;
                string[] exam = lines[index].Split(';');
                timeLeft = Convert.ToInt32(exam.Last());
                topic = exam[0];
                taskamount = Convert.ToInt32(exam[1]);
                taskAmount = taskamount;
                for (int l = 2; l < taskamount + 2; l++)
                {
                    indexes.Add(Convert.ToInt32(exam[l]));
                }


                t.Interval = 1000; // specify interval time as you want
                t.Tick += new EventHandler(timer_Tick);
                t.Start();

                Exam egzamin = new Exam(topic, taskamount);
                egzamin.addIndexes(indexes);
                question = 0;

                askQuestion(indexes[question]);
                examsBox.Visible = false;
                startTestBtn.Visible = false;

                currentTaskLbl.Visible = true;
                maxTasksLbl.Visible = true;
                metroSetLabel27.Visible = true;

                metroSetLabel25.Visible = true;
                timeLbl.Visible = true;

                metroSetLabel24.Visible = true;
                answerBox.Visible = true;
                nextBtn.Visible = true;
            }
            catch
            {
                MessageBox.Show("Nie wybrano testu.");
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {

            //timeLbl.Text = timeLeft.ToString();

            TimeSpan ts = TimeSpan.FromSeconds(timeLeft);
            timeLbl.Text = string.Format("{0}:{1}:{2}", ts.Hours, ts.Minutes, ts.Seconds);
            timeLeft -= 1;

        }

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
                int pointsForTask = Convert.ToInt32(eTasks[indexes[question]].getDifficulty());
                maxPoints += pointsForTask;
                //MessageBox.Show(eTasks[indexes[question]].getCorrectAnswer());
                if (correctAnswer == answer)
                {
                    points = points + pointsForTask;
                }
                question++;
                if (question >= taskAmount)
                {
                    finishTest(maxPoints);
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
            
            //askQuestion(indexes[question]);
            //check czy koniec
        }

        public void finishTest(int maxpoints)
        {
            MessageBox.Show("finished with " + points + "/" + maxpoints+ " points");

            CreateRaport(timeLeft, points, testQuestionTresc.Text, maxpoints);

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

            examsBox.Visible = true;
            startTestBtn.Visible = true;

            currentTaskLbl.Visible = false;
            maxTasksLbl.Visible = false;
            metroSetLabel27.Visible = false;

            metroSetLabel25.Visible = false;
            timeLbl.Visible = false;

            metroSetLabel24.Visible = false;
            answerBox.Visible = false;
            nextBtn.Visible = false;

            timeLeft = 0;
            timeLbl.Text = "00:00";
            t.Stop();

            LoadStudentsHistory();
        }

        public void CreateRaport(int timeLeft, int points, string topic, int maxpoints)
        {
            System.IO.Directory.CreateDirectory(dane);

            double pt = Convert.ToDouble(points);
            double mxpt = Convert.ToDouble(maxpoints);

            DialogResult d;
            
            TimeSpan ts = TimeSpan.FromSeconds(timeLeft);
            time = string.Format("{0}:{1}:{2}", ts.Hours, ts.Minutes, ts.Seconds);
            int ocena = 3;
            double percent = (pt / mxpt) * 100;
            MessageBox.Show(percent.ToString());
            if (percent > 90)
            {
                ocena = 5;
            }
            else if (percent > 70)
            {
                ocena = 4;
            }
            else if (percent >= 50)
            {
                ocena = 3;
            }
            else if(percent < 50)
            {
                ocena = 2;
            }

            d = MessageBox.Show("Imie i nazwisko: " + dane+"\nLiczba punktow: "+points+"/"+maxpoints+"\nPozostaly czas: "+time + "\nOcena: " + ocena +"\nCzy chcesz wydrukowac wynik?", ("Egzamin: " + topic), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            

            if (d == DialogResult.Yes)
            {
                DrukujWynik();
            }
            string dir = dane;
            string filename = "Historia.txt";
            string fullPath = Path.Combine(dir, filename);
            using (TextWriter tw = new StreamWriter(fullPath, append: true))
            {
                tw.Write("\n" + dane);
                tw.Write(";" + topic);
                tw.Write(";" + points);
                tw.Write(";" + timeLeft);
                tw.Write(";" + ocena);
                tw.Write(";" + maxPoints);
            }

            using (TextWriter tw = new StreamWriter(filename, append: true))
            {
                tw.Write("\n" + dane);
                tw.Write(";" + topic);
                tw.Write(";" + points);
                tw.Write(";" + timeLeft);
                tw.Write(";" + ocena);
                tw.Write(";" + maxPoints);
            }
        }

        public void DrukujWynik()
        {
            printPreviewDialog1.ShowDialog();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
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
                correctAnswer = credientals[7];
                difficulty = credientals[6];
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
                MessageBox.Show("Dodano zadanie!");
            }
            else//eror
            {
                dodanoZadanieLbl.Text = "Uzupelnij wszystkie pola!";
                dodanoZadanieLbl.Visible = true;
                MessageBox.Show("Nie udalo sie dodac zadania.");
            }
        }

        void LoadStudentsHistory()
        {
            //deklaracje
            if(rank == "Student")
            {
                try
                {
                    string dir = Form1.dane;
                    string filename = "Historia.txt";
                    string fullPath = Path.Combine(dir, filename);

                    //string fileName = "History.txt";
                    string temat, dane, punkty, ocena, czas, maxpoints;
                    var lines = File.ReadLines(fullPath);

                    foreach (var line in lines)
                    {
                        //wyswietlanie studentow w bazie danych

                        string[] credientals = line.Split(';');
                        temat = credientals[1];
                        dane = credientals[0];
                        punkty = credientals[2];
                        czas = credientals[3];
                        ocena = credientals[4];
                        maxpoints = credientals[5];

                        string pkt = (punkty + "/" + maxpoints);
                        TimeSpan ts = TimeSpan.FromSeconds(Convert.ToInt32(czas));
                        time = string.Format("{0}:{1}:{2}", ts.Hours, ts.Minutes, ts.Seconds);

                        if (true)
                        {
                            //deklaracja jakiegos gowna
                            int rowId = dataGridView2.Rows.Add();
                            DataGridViewRow row = dataGridView2.Rows[rowId];

                            //dodawanie rowow z wyzej wypisanymi danymi
                            dataGridView2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dataGridView2.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dataGridView2.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dataGridView2.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            row.Cells["Dane1"].Value = dane;
                            row.Cells["Tytul"].Value = temat;
                            row.Cells["Punkty"].Value = pkt;
                            row.Cells["Czas"].Value = time;
                            row.Cells["Ocena"].Value = ocena;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Nie znaleziono testow do zaladowania");
                }
            }
            else
            {
                try
                {
                    string filename = "Historia.txt";

                    //string fileName = "History.txt";
                    string temat, dane, punkty, ocena, czas, maxpoints;
                    var lines = File.ReadLines(filename);

                    foreach (var line in lines)
                    {
                        //wyswietlanie studentow w bazie danych

                        string[] credientals = line.Split(';');
                        temat = credientals[1];
                        dane = credientals[0];
                        punkty = credientals[2];
                        czas = credientals[3];
                        ocena = credientals[4];
                        maxpoints = credientals[5];

                        string pkt = (punkty + "/" + maxpoints);
                        TimeSpan ts = TimeSpan.FromSeconds(Convert.ToInt32(czas));
                        time = string.Format("{0}:{1}:{2}", ts.Hours, ts.Minutes, ts.Seconds);

                        if (true)
                        {
                            //deklaracja jakiegos gowna
                            int rowId = dataGridView2.Rows.Add();
                            DataGridViewRow row = dataGridView2.Rows[rowId];

                            //dodawanie rowow z wyzej wypisanymi danymi
                            dataGridView2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dataGridView2.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dataGridView2.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            dataGridView2.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            row.Cells["Dane1"].Value = dane;
                            row.Cells["Tytul"].Value = temat;
                            row.Cells["Punkty"].Value = pkt;
                            row.Cells["Czas"].Value = time;
                            row.Cells["Ocena"].Value = ocena;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Nie znaleziono testow do zaladowania");
                }
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
            try
            {
                CollectTasks();
                int taskAmount = Convert.ToInt32(taskAmountBox.Text);
                string topic = testNameBox.Text;
                int mediumTaskAmount = Convert.ToInt32(mediumTaskAmountBox.Text);
                int hardTaskAmount = Convert.ToInt32(hardTaskAmountBox.Text);
                Exam egzamin = new Exam(topic, taskAmount);

                Random rng = new Random();
                List<int> indexes = new List<int>();
                //wez n liczbe pytan do temp tablicy
                int number;
                //int medTasks = 0;
                int hardTasks = 0;
                //egzamin.addTasks(eTasks);
                int allZadania = taskAmount - mediumTaskAmount - hardTaskAmount;
                //trudne
                for (int a = 0; a < hardTaskAmount; a++)
                {
                    do
                    {
                        number = rng.Next(0, eTasks.Count);
                        if (eTasks[number].getDifficulty() == "3")
                        {
                            egzamin.addTasks(eTasks[number]);
                            egzamin.addIndex(number);
                            hardTasks++;
                        }
                    } while (indexes.Contains(number));
                    indexes.Add(number);
                }

                //srednie
                for (int a = 0; a < mediumTaskAmount; a++)
                {
                    do
                    {
                        number = rng.Next(0, eTasks.Count);
                        if (eTasks[number].getDifficulty() == "2")
                        {
                            egzamin.addTasks(eTasks[number]);
                            egzamin.addIndex(number);
                            hardTasks++;
                        }
                    } while (indexes.Contains(number));
                    indexes.Add(number);
                }

                //reszta
                for (int a = 0; a < allZadania; a++)
                {
                    do
                    {
                        number = rng.Next(0, eTasks.Count);
                        egzamin.addTasks(eTasks[number]);
                        egzamin.addIndex(number);
                        hardTasks++;
                    } while (indexes.Contains(number));
                    indexes.Add(number);
                }
                int examDuration = (Convert.ToInt32(HourBox.Text) * 3600) + (Convert.ToInt32(MinuteBox.Text) * 60);
                string fileName = "ExamDatabase.txt";
                using (TextWriter tw = new StreamWriter(fileName, append: true))
                {
                    tw.Write("\n" + egzamin.getTopic());
                    tw.Write(";" + taskAmount);
                    foreach (int i in indexes)
                        tw.Write(";" + i);
                    tw.Write(";" + examDuration);
                }
                updateExams();

                MessageBox.Show("Dodano nowy test!");
                //File.AppendAllText()
                //dodano wszystkie losowe zadania do egzaminu
            }
            catch
            {
                MessageBox.Show("Dodawanie nowego testu nie powiodlo sie. Sprawdz czy wszystkie pola zostaly uzupelnione poprawnie.");
            }
        }

        public void updateExams()
        {
            examsBox.Items.Clear();
            examsBox.Visible = true;
            string fileName = "ExamDatabase.txt";
            var lines = File.ReadLines(fileName); //czytanie z pliku

            foreach (var line in lines)//petla wykonujaca sie w KAZDEJ LINIJCE pliku tekstowego
            {
                string[] exam = line.Split(';');
                examsBox.Items.Add(exam[0]);
            }
        }

        private void examPage_Click(object sender, EventArgs e)
        {

        }

        private void examsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            index = examsBox.SelectedIndex;
        }

        private void metroSetButton2_Click(object sender, EventArgs e)
        {
            try
            {
                listaZdajacych.Clear();

                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.ShowDialog(this);
                openFileDialog1.RestoreDirectory = true;
                string path = openFileDialog1.FileName;
                //string directoryPath = System.IO.Path.GetDirectoryName(openFileDialog1.FileName);
                //MessageBox.Show(path);

                using (var reader = new StreamReader(path))
                {
                    List<string> Nazwisko = new List<string>();
                    List<string> Imie = new List<string>();
                    List<string> Imie2 = new List<string>();
                    List<string> Skreslony = new List<string>();
                    List<string> Rezygnacja = new List<string>();
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');

                        Nazwisko.Add(values[0]);
                        Imie.Add(values[1]);
                        Imie2.Add(values[2]);
                        Skreslony.Add(values[3]);
                        Rezygnacja.Add(values[4]);

                        listaZdajacych.Add(Nazwisko + " " + Imie);
                    }
                }
                MessageBox.Show("Udalo sie dodac studentow do listy.");
            }
            catch
            {
                MessageBox.Show("Nie dalo sie dodac studentow do listy.");
            }

            
        }

        private void fromHourBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString(("Imie i nazwisko: " + dane + "\nLiczba punktow: " + points + "\nPozostaly czas: " + time),new Font("Arial",32,FontStyle.Regular), Brushes.Black, new Point(10,10));
        }

        private void metroSetButton3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Wylogowano!");
            this.Hide();
            Login login = new Login();

            //update gui
            Form1.dane = "";
            Form1.rank = "";

            login.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //podstawowa deklaracja gui po zaladowaniu sie programu
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
                //to sie robi gdy laduje sie student
                //tabControl.TabPages.Remove(mainProfesorPage);
            }
            if (rank == "Profesor")
            {
                //a to gdy profesor
                //tabControl.TabPages.Remove(mainPage);
            }
            LoadStudentList();//ladowanie studenciakow do listy
            LoadStudentsHistory();

        }
    }
}
