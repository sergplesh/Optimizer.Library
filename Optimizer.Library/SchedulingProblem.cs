using ScheduleLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizer.Library
{
    public class SchedulingProblem
    {
        public List<Job> Jobs { get; }
        public List<Worker> Workers { get; }
        public JobGraph? DependencyGraph { get; }

        public SchedulingProblem(List<Job> jobs, List<Worker> workers)
        {
            Jobs = jobs;
            Workers = workers;
        }

        public SchedulingProblem(List<Job> jobs, List<Worker> workers, JobGraph graph)
            : this(jobs, workers)
        {
            DependencyGraph = graph;
        }
    }
}
