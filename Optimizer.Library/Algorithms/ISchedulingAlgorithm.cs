using Optimizer.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizer.Library.Algorithms
{
    public interface ISchedulingAlgorithm
    {
        Guid Id { get; }
        string Name { get; }
        string Description { get; }

        Schedule Solve(SchedulingProblem problem);
        void Validate(SchedulingProblem problem);
    }
}
