using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplikacjaTestujaca
{
    public class ExamTask
    {
        private int id;
        private string tresc, a, b, c, d, corretAnswer, difficulty;
        public ExamTask(int iid, string ttresc, string aa, string bb, string cc, string dd, string ccorretAnswer, string ddifficulty)
        {
            id = iid;
            tresc = ttresc;
            a = aa;
            b = bb;
            c = cc;
            d = dd;
            corretAnswer = ccorretAnswer;
            difficulty = ddifficulty;
        }

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
            return b;
        }
        public string getC()
        {
            return c;
        }
        public string getD()
        {
            return d;
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
