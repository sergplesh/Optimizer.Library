using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizer.Library
{
    public enum DependencyType { Tree, Graph, None }
    public class JobGraph
    {
        private readonly Dictionary<Job, List<Job>> _dependencies;
        public List<Job> Jobs { get; }
        public DependencyType DependencyType { get; private set; }

        public JobGraph(List<Job> jobs)
        {
            Jobs = jobs;
            _dependencies = new Dictionary<Job, List<Job>>();
            DependencyType = DependencyType.None;
        }

        public void AddDependency(Job from, Job to)
        {
            if (!_dependencies.ContainsKey(to))
            {
                _dependencies[to] = new List<Job>();
            }

            if (!_dependencies[to].Contains(from))
            {
                _dependencies[to].Add(from);
            }

            UpdateDependencyType();
        }

        public List<Job> GetDependencies(Job job)
        {
            return _dependencies.ContainsKey(job) ? _dependencies[job] : new List<Job>();
        }

        public List<Job> GetDirectDependencies(Job job)
        {
            return GetDependencies(job);
        }

        public bool IsTreeToRoot()
        {
            var roots = Jobs.Where(j => !_dependencies.ContainsKey(j) || !_dependencies[j].Any()).ToList();
            if (roots.Count != 1) return false;

            var visited = new HashSet<Job>();
            var queue = new Queue<Job>();
            queue.Enqueue(roots[0]);

            while (queue.Any())
            {
                var current = queue.Dequeue();
                if (visited.Contains(current)) return false;

                visited.Add(current);

                foreach (var dep in _dependencies.Where(kv => kv.Value.Contains(current)))
                {
                    queue.Enqueue(dep.Key);
                }
            }

            return visited.Count == Jobs.Count;
        }

        public void RemoveTransitiveEdges()
        {
            var transitiveEdges = new List<(Job from, Job to)>();

            foreach (var job in Jobs)
            {
                var allDependencies = GetAllTransitiveDependencies(job).ToList();
                foreach (var directDep in GetDependencies(job).ToList())
                {
                    foreach (var transitiveDep in allDependencies)
                    {
                        if (directDep != transitiveDep &&
                            GetDependencies(directDep).Contains(transitiveDep))
                        {
                            transitiveEdges.Add((transitiveDep, job));
                        }
                    }
                }
            }

            foreach (var (from, to) in transitiveEdges)
            {
                _dependencies[to].Remove(from);
            }

            UpdateDependencyType();
        }

        private IEnumerable<Job> GetAllTransitiveDependencies(Job job)
        {
            var visited = new HashSet<Job>();
            var stack = new Stack<Job>(GetDependencies(job));

            while (stack.Any())
            {
                var current = stack.Pop();
                if (visited.Contains(current)) continue;

                visited.Add(current);
                yield return current;

                foreach (var dep in GetDependencies(current))
                {
                    stack.Push(dep);
                }
            }
        }

        public List<Job> GetSources()
        {
            return Jobs.Where(j => !_dependencies.ContainsKey(j) || !_dependencies[j].Any()).ToList();
        }

        public List<Job> GetJobsWithAllDependenciesCompleted(IEnumerable<Job> completedJobs)
        {
            var completedSet = new HashSet<Job>(completedJobs);
            return Jobs.Where(j =>
                !completedSet.Contains(j) &&
                (!_dependencies.ContainsKey(j) || _dependencies[j].All(d => completedSet.Contains(d))))
                .ToList();
        }

        private void UpdateDependencyType()
        {
            if (IsTreeToRoot())
            {
                DependencyType = DependencyType.Tree;
            }
            else if (_dependencies.Any())
            {
                DependencyType = DependencyType.Graph;
            }
            else
            {
                DependencyType = DependencyType.None;
            }
        }
    }
}
