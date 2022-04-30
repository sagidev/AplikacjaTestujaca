using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplikacjaTestujaca
{
    public class ExamTask
    {
        //deklaracje
        private int id;
        private string tresc, a, b, c, d, corretAnswer, difficulty;
        public ExamTask(int iid, string ttresc, string aa, string bb, string cc, string dd, string ccorretAnswer, string ddifficulty)
        {
            //funkcja tworzenia taska
            id = iid;
            tresc = ttresc;
            a = aa;
            b = bb;
            c = cc;
            d = dd;
            corretAnswer = ccorretAnswer;
            difficulty = ddifficulty;
        }
        
        //funkcje do czytania dancyh z taska
        //przyklad:    string trescZadania = task.getTresc();
        int getID()
        {
            return id;
        }
        public string getTresc()
        {
            return tresc;
        }
        public string getA()
        {
            return a;
        }
        public string getB()
        {
            return a;
        }
        public string getC()
        {
            return a;
        }
        public string getD()
        {
            return a;
        }
        public string getCorrectAnswer()
        {
            return corretAnswer;
        }
        public string getDifficulty()
        {
            return difficulty;
        }
    }
}
