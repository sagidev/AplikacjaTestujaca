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
    public partial class Login : MetroSetForm
    {
        string fileName = "UserDatabase.txt";
        public Login()
        {
            InitializeComponent();
        }

        private void metroSetButton1_Click(object sender, EventArgs e)
        {
            string login, password, rank, imie, nazwisko;

            var lines = File.ReadLines(fileName);
            foreach (var line in lines)
            {
                string[] credientals = line.Split(';');
                login = credientals[0];
                password = credientals[1];
                imie = credientals[2];
                nazwisko = credientals[3];
                rank = credientals[4];

                if (usernameTxt.Text == login && passwordTxt.Text == password)
                {
                    this.Hide();
                    Form1 form = new Form1();

                    form.dane = (imie + " " + nazwisko);
                    form.rank = rank;
                    
                    form.ShowDialog();
                }
            }
        }
    }
}
