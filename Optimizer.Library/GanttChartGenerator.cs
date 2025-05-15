using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizer.Library
{
    public class GanttChartGenerator
    {
        public static string GenerateTextChart(Schedule schedule)
        {
            var sb = new StringBuilder();
            var workers = schedule.Items.Select(i => i.Worker).Distinct().OrderBy(w => w.Id).ToList();

            sb.AppendLine("Диаграмма Ганта");
            sb.AppendLine(new string('-', 50));

            foreach (var worker in workers)
            {
                sb.AppendLine($"Работник {worker.Id} ({worker.Name}):");

                var workerItems = schedule.Items
                    .Where(i => i.Worker.Id == worker.Id)
                    .OrderBy(i => i.StartTime)
                    .ToList();

                var maxTime = (int)Math.Ceiling(schedule.TotalDuration);
                var timeScale = "Время: |" + string.Join("|", Enumerable.Range(0, maxTime + 1)
                    .Select(t => t.ToString().PadLeft(3))) + "|";

                sb.AppendLine(timeScale);

                foreach (var item in workerItems)
                {
                    var start = (int)item.StartTime;
                    var duration = (int)Math.Ceiling(item.EndTime - item.StartTime);

                    var taskLine = new string(' ', start * 4 + 1) +
                                   new string('=', duration * 4 - 1) +
                                   $" [Job {item.Job.Id}, Stage {item.Stage.StageNumber}]";

                    sb.AppendLine(taskLine);
                }

                sb.AppendLine();
            }

            sb.AppendLine($"Общая длительность: {schedule.TotalDuration}");
            return sb.ToString();
        }
        public static Dictionary<string, object> GenerateChartData(Schedule schedule)
        {
            var result = new Dictionary<string, object>();

            // Защита от null
            if (schedule?.Items == null)
            {
                result.Add("error", "Schedule data is empty");
                return result;
            }

            var workersData = new List<object>();
            var maxTime = schedule.TotalDuration;

            // Группировка по работникам
            var workerGroups = schedule.Items
                .GroupBy(item => item.Worker)
                .OrderBy(g => g.Key?.Id ?? 0);

            foreach (var workerGroup in workerGroups)
            {
                if (workerGroup.Key == null) continue;

                var workerItems = workerGroup
                    .OrderBy(item => item.StartTime)
                    .Select(item => new
                    {
                        jobId = item.Job?.Id ?? 0,
                        stageId = item.Stage?.Id ?? 0,
                        stageNum = item.Stage?.StageNumber ?? 0,
                        start = item.StartTime,
                        end = item.EndTime,
                        duration = item.EndTime - item.StartTime
                    })
                    .ToList();

                workersData.Add(new
                {
                    workerId = workerGroup.Key.Id,
                    workerName = workerGroup.Key.Name ?? $"Worker {workerGroup.Key.Id}",
                    stages = workerItems
                });
            }

            result.Add("workers", workersData);
            result.Add("totalDuration", maxTime);
            result.Add("timeScale", GenerateTimeScale(maxTime));

            return result;
        }

        private static List<object> GenerateTimeScale(double maxDuration)
        {
            var scale = new List<object>();
            int steps = Math.Min(20, (int)Math.Ceiling(maxDuration));

            for (int i = 0; i <= steps; i++)
            {
                double time = maxDuration * i / steps;
                scale.Add(new { time = time, label = time.ToString("0.#") });
            }

            return scale;
        }
    }
}
