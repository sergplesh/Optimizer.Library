using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizer.Library
{
    public class Schedule
    {
        public List<ScheduleItem> Items { get; }
        public double TotalDuration { get; private set; }

        public Schedule()
        {
            Items = new List<ScheduleItem>();
            TotalDuration = 0;
        }

        public void AddItem(ScheduleItem item)
        {
            Items.Add(item);
        }

        public void CalculateTotalDuration()
        {
            TotalDuration = Items.Any() ? Items.Max(i => i.EndTime) : 0;
        }
    }

    public class ScheduleItem
    {
        public Job Job { get; }
        public Stage Stage { get; }
        public Worker Worker { get; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }

        public ScheduleItem(Job job, Stage stage, Worker worker)
        {
            Job = job;
            Stage = stage;
            Worker = worker;
        }
    }
}