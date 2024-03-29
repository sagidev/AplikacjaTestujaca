﻿using System;
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
        List<int> indexes = new List<int>();
        List<ExamTask> allTasks = new List<ExamTask>();
        List<ExamTask> examTasks = new List<ExamTask>();
        public Exam(string TTopic, int ttaskAmount)
        {
            Topic = TTopic;
            taskAmount = ttaskAmount;
        }
        public void addIndexes(List<int> iindexes)
        {
            indexes = iindexes;
        }
        public void addTasks(ExamTask tasks)
        {
            allTasks.Add(tasks);
        }
        public void addIndex(int index)
        {
            indexes.Add(index);
        }
        public void getTasks(List<ExamTask> tasks)
        {
            allTasks = tasks;
        }
        public string getTopic()
        {
            return Topic;
        }
        public void createExam(List<ExamTask> tasks)
        {
            allTasks = tasks;
            List<int> listNumbers = new List<int>();
            Random rng = new Random();
            int randomTaskId;
            for (int i = 0; i < taskAmount; i++)
            {
                randomTaskId = rng.Next(0, allTasks.Count);
                examTasks.Add(allTasks[randomTaskId]);
            }
        }

        public string printExam()
        {
            return (Topic + ";" + taskAmount + ";");
        }
    }
}
