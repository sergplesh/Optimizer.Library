using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizer.Library.Models
{
    /// <summary>
    /// Модель задания для планирования
    /// </summary>
    public class Job
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public double Duration { get; set; } = 1.0;
        public bool IsPreemptable { get; set; }
        public List<Stage> Stages { get; } = new List<Stage>();
        public List<Guid> Dependencies { get; } = new List<Guid>();

        /// <summary>
        /// Является ли задание конвейерным
        /// </summary>
        public bool IsPipelineTask => Stages.Count > 0;

        /// <summary>
        /// Имеет ли задание зависимости
        /// </summary>
        public bool HasDependencies => Dependencies.Count > 0;

        /// <summary>
        /// Общая длительность задания
        /// </summary>
        public double TotalDuration => IsPipelineTask ? Stages.Sum(s => s.Duration) : Duration;

        /// <summary>
        /// Проверяет, зависит ли задание от указанного задания
        /// </summary>
        public bool DependsOn(Guid jobId) => Dependencies.Contains(jobId);
    }

    /// <summary>
    /// Модель этапа задания
    /// </summary>
    public class Stage
    {
        public double Duration { get; set; }
        public int Order { get; set; }
    }
}