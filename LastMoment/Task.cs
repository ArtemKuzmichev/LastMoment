using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LastMoment
{
    class Task
    {
        private string description;
        private DateTime deadline;
        private DateTime startDateWork;
        private DateTime endDateWork;
        private int importance;
        private int days;
        public Task(string description, DateTime deadline, int importance, int days)
        {
            this.description = description;
            this.deadline = deadline;
            this.importance = importance;
            this.days = days;
            this.startDateWork = deadline.AddDays(-days);
            this.endDateWork = deadline.AddDays(-1);
        }

        public string GetDescription() { return description; }
        public DateTime GetDeadline() { return deadline; }

        public DateTime GetStartDateWork() { return startDateWork; }
        public DateTime GetEndDateWork() { return endDateWork; }
        public int GetImportance() {  return importance; }
        public int GetDays() {  return days; }
        public void SetDescription(string  description) { this.description = description; }
        public void SetDeadline(DateTime deadline) { this.deadline = deadline; }
        public void SetStartDateWork(DateTime startDateWork) { this.startDateWork = startDateWork; }
        public void SetEndDateWork(DateTime endDateWork) { this.endDateWork = endDateWork; }
        public void SetImportance(int importance) { this.importance = importance;}
        public void SetDays(int days) { this.days = days; }
    }
}
