using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizer.Library
{
    public enum DependencyType { Tree, Graph, None }
    public class JobGraph
    {
        public Dictionary<Job, List<Job>> _dependencies;
        //private readonly Dictionary<Job, List<Job>> _dependencies;
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
            // Находим все корни (работы, от которых никто не зависит)
            var roots = GetRoots();

            // Для каждого корня проверяем, что все остальные работы ведут к нему
            foreach (var root in roots)
            {
                var visited = new HashSet<Job>();
                var queue = new Queue<Job>();
                queue.Enqueue(root);
                visited.Add(root);

                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();

                    // Идем в обратном направлении - от корня к зависимостям
                    foreach (var dependent in Jobs.Where(j => GetDependencies(current).Contains(j)))
                    {
                        if (visited.Contains(dependent))
                            return false; // Нашли цикл

                        visited.Add(dependent);
                        queue.Enqueue(dependent);
                    }
                }

                // Если все работы посещены - это корректное дерево
                if (visited.Count == Jobs.Count)
                    return true;
            }

            // Ни одно из деревьев не покрыло все работы
            return false;
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

        public List<Job> GetRoots()
        {
            return Jobs.Where(j => !Jobs.Any(other => GetDependencies(other).Contains(j))).ToList();
        }

        public List<Job> GetToAssign(HashSet<Job> jobs, Dictionary<Job, int> dict)
        {
            var jobsToAssign = new List<Job>();
            // Находим работы, все зависимости которых уже имеют приоритет
            foreach (var job in jobs)
            {
                var dependencies = Jobs.Where(j => GetDependencies(j).Contains(job)).ToList();
                if (dependencies.All(d => dict.ContainsKey(d)))
                {
                    jobsToAssign.Add(job);
                }
            }
            return jobsToAssign;
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
