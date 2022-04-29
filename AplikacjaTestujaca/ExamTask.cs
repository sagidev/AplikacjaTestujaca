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
        string getTresc()
        {
            return tresc;
        }
        string getA()
        {
            return a;
        }
        string getB()
        {
            return a;
        }
        string getC()
        {
            return a;
        }
        string getD()
        {
            return a;
        }
        string getCorrectAnswer()
        {
            return corretAnswer;
        }
        string getDifficulty()
        {
            return difficulty;
        }
    }
}
