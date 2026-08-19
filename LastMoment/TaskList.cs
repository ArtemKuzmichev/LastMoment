using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LastMoment
{
    public class TaskNode
    {
        private Task task;
        private TaskNode next = null;
        private TaskNode prev = null;
        public TaskNode(Task task)
        {
            this.task = task;
        }
        public string GetDescription() { return task.GetDescription(); }
        public DateTime GetDeadline() { return task.GetDeadline(); }
        public DateTime GetStartDateWork() { return task.GetStartDateWork(); }
        public DateTime GetEndDateWork() { return task.GetEndDateWork(); }
        public int GetImportance() { return task.GetImportance(); }
        public int GetDays() { return task.GetDays(); }
        public TaskNode GetNext() { return next; }
        public TaskNode GetPrev() { return prev; }
        public void SetStartDateWork(DateTime startDateWork) { task.SetStartDateWork(startDateWork); }
        public void SetEndDateWork(DateTime endDateWork) { task.SetEndDateWork(endDateWork); }
        public void SetNext(TaskNode next) { this.next = next; }
        public void SetPrev(TaskNode prev) { this.prev = prev; }
        public string ToJSON()
        {
            return task.ToJSON();
        }
    }
    public class TaskList
    {
        private TaskNode header = null;

        private void DragLeft(TaskNode prev, TaskNode curr)
        {
            int drag = 0;
            
            while (prev != null && prev.GetEndDateWork() >= curr.GetStartDateWork()) //пока возникает "нахлест", сдвигаем время
            {                                                                        //начала и окончания задачи на более ранний срок
                drag = (prev.GetEndDateWork() - curr.GetStartDateWork().AddDays(-1)).Days;
                prev.SetStartDateWork(prev.GetStartDateWork().AddDays(-drag));
                prev.SetEndDateWork(prev.GetEndDateWork().AddDays(-drag));
                prev = prev.GetPrev();
                curr = curr.GetPrev();
            }
        }
        private void DragRight(TaskNode prev, TaskNode curr)
        {
            int drag;
            while (prev != null && !prev.GetEndDateWork().Equals(prev.GetDeadline().AddDays(-1)))
            {
                drag = 0;
                if (prev.GetDeadline() > curr.GetStartDateWork())
                {
                    drag = (prev.GetDeadline() - curr.GetStartDateWork()).Days;
                }
                prev.SetEndDateWork(prev.GetDeadline().AddDays(-(1 + drag)));
                prev.SetStartDateWork(prev.GetEndDateWork().AddDays(-(prev.GetDays() - 1)));
                curr = prev;
                prev = prev.GetPrev();
            }
            
        }

        //добавление новой задачи
        public TaskNode AddTask(Task newTask, out int indexInsert)
        {
            indexInsert = 0;
            TaskNode task = new TaskNode(newTask); 
            if (header != null) //если список не пуст, то хотя бы одна задача будет "слева" или "справа" в очереди на выполнение
            {
                TaskNode next = header;
                TaskNode prev = null;
                int days = 0;
                while (next != null && task.GetDeadline() >= next.GetDeadline()) //ищем задачу (назовем следующей),
                {                                                                //у которой дедлайн будет правее
                    days += next.GetDays(); //считаем количество дней, которые уже заняты
                    prev = next;
                    next = next.GetNext();
                    indexInsert++;
                }
                while (prev != null && prev.GetDeadline() == task.GetDeadline() && prev.GetImportance() < task.GetImportance())
                {
                    days -= prev.GetDays();
                    next = prev;
                    prev = prev.GetPrev();
                    indexInsert--;
                }

                if (DateTime.Today > header.GetStartDateWork()) //убираем дни работы до сегодня
                {
                    days -= (DateTime.Today - header.GetStartDateWork()).Days;
                }

                int drag = 0;
                if (prev != null) //вышли из цикла по причине 
                {                 //"предыдущая задача важнее, добавлена ранее или дедлайн раньше"
                    if (next != null) //если следующая задача нашлась
                    {
                        if (task.GetEndDateWork() >= next.GetStartDateWork())
                        {//колво дней пересеч новой задачи и след задачи
                            drag = (task.GetEndDateWork() - next.GetStartDateWork().AddDays(-1)).Days;
                        }
                        //задача не "помещается" если
                        //количество занятых дней больше, чем количество дней
                        //между сегодняшним днем и первым днем выполнения
                        //новой задачи после сдвига
                        if ((task.GetStartDateWork().AddDays(-drag) - DateTime.Today).Days < days)
                        {
                            throw new Exception("Слишком много дней на выполнение задачи");
                        }
                        task.SetStartDateWork(task.GetStartDateWork().AddDays(-drag)); //сдвигаем начальный и конечный дни
                        task.SetEndDateWork(task.GetEndDateWork().AddDays(-drag));

                        task.SetNext(next); //вставляем задачу в список
                        prev = next.GetPrev();
                        next.SetPrev(task);
                    }
                    else
                    {
                        if ((task.GetStartDateWork() - DateTime.Today).Days < days)
                        {
                            throw new Exception("Слишком много дней на выполнение задачи");
                        }
                    }
                    prev.SetNext(task);
                    task.SetPrev(prev);
                    DragLeft(prev, task);
                }
                else
                {
                    if (task.GetEndDateWork() >= next.GetStartDateWork())
                    {//колво дней пересеч новой задачи и след задачи
                        drag = (task.GetEndDateWork() - next.GetStartDateWork().AddDays(-1)).Days;
                    }
                    //задача не "помещается" если
                    //количество занятых дней больше, чем количество дней
                    //между сегодняшним днем и первым днем выполнения
                    //новой задачи после сдвига
                    if ((task.GetStartDateWork().AddDays(-drag) - DateTime.Today).Days < days)
                    {
                        throw new Exception("Слишком много дней на выполнение задачи");
                    }
                    header = task;
                    task.SetStartDateWork(task.GetStartDateWork().AddDays(-drag)); //сдвигаем начальный и конечный дни
                    task.SetEndDateWork(task.GetEndDateWork().AddDays(-drag));

                    task.SetNext(next); //вставляем задачу в список
                    next.SetPrev(task);
                }
            }
            else
            {
                if ((task.GetStartDateWork() - DateTime.Today).Days < 0)
                {
                    throw new Exception("Слишком много дней на выполнение задачи");
                }
                header = task;
            }
            return task;
        }
        public TaskNode FromJSON(string json)
        {
            try
            {
                List<Task> tasks = JsonSerializer.Deserialize<List<Task>>(json);
                if (tasks != null && tasks.Count > 0)
                {
                    TaskNode lastAdd = null;
                    foreach (Task task in tasks)
                    {
                        TaskNode node = new TaskNode(task);
                        if (header == null)
                        {
                            lastAdd = node;
                            header = node;
                        }
                        else
                        {
                            lastAdd.SetNext(node);
                            node.SetPrev(lastAdd);
                            lastAdd = node;
                        }
                    }
                }
                return header;
            }
            catch(JsonException)
            {
                throw;
            }
        }
        public string ToJSON()
        {
            if (header == null) { return "[]"; }
            TaskNode node = header;
            string json = "[";
            while (node.GetNext() != null)
            {
                json += node.ToJSON() + ",\n";
                node = node.GetNext();
            }
            json += node.ToJSON();
            json += "]";
            return json;
        }
        public void ShowTask()
        {
            if (header == null) { return; }

        }
        public bool DeleteTask(TaskNode node)
        {
            if (header == null || node == null) { return false; }
            TaskNode prev = node.GetPrev();
            TaskNode next = node.GetNext();
            
            if (prev != null)
            {
                prev.SetNext(next);
                if (next != null)
                {
                    next.SetPrev(prev);
                    DragRight(prev, next);
                }
                else
                {
                    prev.SetEndDateWork(prev.GetDeadline().AddDays(-1));
                    prev.SetStartDateWork(prev.GetDeadline().AddDays(-prev.GetDays()));
                    if (prev.GetPrev() != null)
                    {
                        DragRight(prev.GetPrev(), prev);
                    }
                }
                
            }
            else
            {
                if (next != null)
                {
                    next.SetPrev(null);
                    header = next;
                    node.SetNext(null);
                }
                else
                {
                    header = null;
                }
            }
            node.SetPrev(null);
            node.SetNext(null);
            return true;
        }
    }
}
