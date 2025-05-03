using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizer.Library.Models
{
    /// <summary>
    /// Модель задачи для составления расписания
    /// </summary>
    public class SchedulingProblem
    {
        public List<Job> Jobs { get; } = new List<Job>();
        public List<Worker> Workers { get; } = new List<Worker>();

        /// <summary>
        /// Является ли задача конвейерной
        /// </summary>
        public bool IsPipelineTask => Jobs.Any(j => j.IsPipelineTask);

        /// <summary>
        /// Имеет ли задача зависимости между заданиями
        /// </summary>
        public bool HasDependencies => Jobs.Any(j => j.HasDependencies);

        /// <summary>
        /// Разрешены ли прерывания в выполнении заданий
        /// </summary>
        public bool IsPreemptable => Jobs.Any(j => j.IsPreemptable);

        /// <summary>
        /// Проверяет валидность задачи
        /// </summary>
        public void Validate()
        {
            if (!Jobs.Any())
                throw new InvalidOperationException("Не указаны задания");

            if (!Workers.Any())
                throw new InvalidOperationException("Не указаны работники");

            if (IsPipelineTask)
            {
                foreach (var job in Jobs)
                {
                    if (job.Stages.Count != 2)
                        throw new InvalidOperationException(
                            "Конвейерные задания должны иметь 2 этапа");
                }
            }
        }
    }
}
