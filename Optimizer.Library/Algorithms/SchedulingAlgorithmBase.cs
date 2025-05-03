using Optimizer.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizer.Library.Algorithms
{
    public abstract class SchedulingAlgorithmBase : ISchedulingAlgorithm
    {
        public abstract Guid Id { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }

        public virtual void Validate(SchedulingProblem problem) { }

        public abstract Schedule Solve(SchedulingProblem problem);
    }
}
