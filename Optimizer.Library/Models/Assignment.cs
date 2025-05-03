using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizer.Library.Models
{
    /// <summary>
    /// Назначение задания работнику
    /// </summary>
    public class Assignment
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Guid JobId { get; set; }
        public Guid WorkerId { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public int? StageOrder { get; set; } // Только для конвейерной задачи 1 или 2

        /// <summary>
        /// Длительность выполнения
        /// </summary>
        public double Duration => EndTime - StartTime;

        /// <summary>
        /// Проверяет пересечение по времени с другим назначением
        /// </summary>
        public bool OverlapsWith(Assignment other)
        {
            return WorkerId == other.WorkerId &&
                   StartTime < other.EndTime &&
                   EndTime > other.StartTime;
        }
    }
}