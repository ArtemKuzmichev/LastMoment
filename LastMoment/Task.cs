using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LastMoment
{
    public class Task
    {
        [JsonInclude]
        public string description;
        [JsonInclude]
        public DateTime deadline;
        [JsonInclude]
        private DateTime startDateWork;
        [JsonInclude]
        private DateTime endDateWork;
        [JsonInclude]
        private int importance;
        [JsonInclude]
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

        public string ToJSON()
        {
            //string json = String.Format("{{\n" +
            //                            "\"description\": \"{0}\",\n" +
            //                            "\"deadline\": \"{1}\",\n" +
            //                            "\"startDateWork\": \"{2}\",\n" +
            //                            "\"endDateWork\": \"{3}\",\n" +
            //                            "\"importance\": {4},\n" +
            //                            "\"days\": {5}" +
            //                            "\n}}", description, deadline.ToString("yyyy-MM-dd"),
            //                            startDateWork.ToString("yyyy-MM-dd"), endDateWork.ToString("yyyy-MM-dd"),
            //                            importance, days);
            return JsonSerializer.Serialize(this);
        }
    }
}
