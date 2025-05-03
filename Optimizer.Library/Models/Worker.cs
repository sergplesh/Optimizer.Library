using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizer.Library.Models
{
    ///// <summary>
    ///// Тип работника
    ///// </summary>
    //public enum WorkerType
    //{
    //    Universal,    // Может выполнять любые задания
    //    Specialized   // Выполняет только задания определенного типа
    //}

    /// <summary>
    /// Модель работника
    /// </summary>
    public class Worker
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        //public WorkerType Type { get; set; } = WorkerType.Universal;
        public double? Speed { get; set; }

        public int SpecializedStage { get; set; } // Только для конвейерной задачи 1 или 2

        /// <summary>
        /// Рассчитывает фактическое время выполнения с учетом производительности
        /// </summary>
        public double CalculateActualDuration(double baseDuration)
        {
            return Speed.HasValue ? baseDuration / Speed.Value : baseDuration;
        }
    }
}