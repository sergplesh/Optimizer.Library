using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizer.Library.Models
{
    /// <summary>
    /// Модель расписания
    /// </summary>
    public class Schedule
    {
        public Guid Id { get; } = Guid.NewGuid();
        public List<Assignment> Assignments { get; } = new List<Assignment>();

        /// <summary>
        /// Общее время выполнения
        /// </summary>
        public double TotalDuration => Assignments.Count > 0
            ? Assignments.Max(a => a.EndTime - a.StartTime) : 0;

        /// <summary>
        /// Добавляет назначение
        /// </summary>
        public void AddAssignment(Assignment assignment)
        {
            if (Assignments.Any(a => a.OverlapsWith(assignment)))
            {
                throw new InvalidOperationException(
                    "Новое назначение конфликтует с существующими");
            }
            Assignments.Add(assignment);
        }

        /// <summary>
        /// Получает все назначения для указанного работника
        /// </summary>
        public IEnumerable<Assignment> GetWorkerAssignments(Guid workerId)
        {
            return Assignments
                .Where(a => a.WorkerId == workerId)
                .OrderBy(a => a.StartTime);
        }

        /// <summary>
        /// Получает все назначения для указанного задания
        /// </summary>
        public IEnumerable<Assignment> GetJobAssignments(Guid jobId)
        {
            return Assignments
                .Where(a => a.JobId == jobId)
                .OrderBy(a => a.StartTime);
        }
    }
}