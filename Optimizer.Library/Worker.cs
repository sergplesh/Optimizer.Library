using ScheduleLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizer.Library
{
    public enum WorkerType { General, StageSpecific }
    public class Worker
    {
        public int Id { get; }
        public string Name { get; set; }
        public double Productivity { get; set; }
        public WorkerType Type { get; set; }
        public int? SupportedStageType { get; set; }

        public Worker(int id)
        {
            Id = id;
            Productivity = 1.0;
            Type = WorkerType.General;
        }
    }
}