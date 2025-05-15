using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizer.Library
{
    // Основные классы
    public class Job
    {
        public int Id { get; }
        public string Name { get; set; }
        public List<Stage> Stages { get; }
        public double Priority { get; set; }
        public double RemainingDuration { get; set; }

        public Job(int id)
        {
            Id = id;
            Stages = new List<Stage>();
            RemainingDuration = 0;
        }

        public void AddStage(Stage stage)
        {
            Stages.Add(stage);
            RemainingDuration += stage.Duration;
        }
    }

    public class Stage
    {
        public int Id { get; }
        public string Name { get; set; }
        public double Duration { get; set; }
        public int StageNumber { get; set; }
        public List<Stage> Dependencies { get; }

        public Stage(int id)
        {
            Id = id;
            Dependencies = new List<Stage>();
            Duration = 0;
        }

        public void AddDependency(Stage stage)
        {
            if (!Dependencies.Contains(stage))
            {
                Dependencies.Add(stage);
            }
        }

        public bool IsReadyToExecute(IEnumerable<Stage> completedStages)
        {
            return Dependencies.All(d => completedStages.Contains(d));
        }
    }
}