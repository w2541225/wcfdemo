using System;
using System.Collections.Generic;
using System.Globalization;

namespace OperationProfiler
{
    public class OperationProfilerManager
    {
        Dictionary<string, List<double>> callsPerOperation;
        public OperationProfilerManager()
        {
            this.callsPerOperation = new Dictionary<string, List<double>>();
        }

        public void AddOneWayCall(string operationName)
        {
            List<double> callTimes;
            if (this.callsPerOperation.ContainsKey(operationName))
            {
                callTimes = this.callsPerOperation[operationName];
            }
            else
            {
                callTimes = new List<double>();
                this.callsPerOperation.Add(operationName, callTimes);
            }

            callTimes.Add(-1);
        }

        public void AddCall(string operationName, double duration)
        {
            List<double> callTimes;
            if (this.callsPerOperation.ContainsKey(operationName))
            {
                callTimes = this.callsPerOperation[operationName];
            }
            else
            {
                callTimes = new List<double>();
                this.callsPerOperation.Add(operationName, callTimes);
            }

            callTimes.Add(duration);
        }

        public void PrintSummary()
        {
            Console.WriteLine("  *** Operation call summary ***");
            Console.WriteLine("Operation name      #calls    avg       min       max");
            foreach (string operationName in this.callsPerOperation.Keys)
            {
                List<double> durations = this.callsPerOperation[operationName];
                double min, max, avg;
                this.SummarizeList(durations, out min, out max, out avg);
                if (max < 0)
                {
                    // one way operation
                    Console.WriteLine("{0}: {1}     no data because operation is one-way", PadRight(operationName, 19), PadLeft(durations.Count, 3));
                }
                else
                {
                    Console.WriteLine(
                        "{0}: {1}  {2} {3} {4}",
                        PadRight(operationName, 19),
                        PadLeft(durations.Count, 3),
                        PadLeft(avg, 9),
                        PadLeft(min, 9),
                        PadLeft(max, 9));
                }
            }
        }

        private string PadRight(string text, int size)
        {
            if (text.Length < size)
            {
                text = text + new string(' ', size - text.Length);
            }

            return text;
        }

        private string PadLeft(double value, int size)
        {
            return this.PadLeft(value.ToString("###.000", CultureInfo.InvariantCulture), size);
        }

        private string PadLeft(int value, int size)
        {
            return this.PadLeft(value.ToString(CultureInfo.InvariantCulture), size);
        }

        private string PadLeft(string text, int size)
        {
            if (text.Length < size)
            {
                text = new string(' ', size - text.Length) + text;
            }

            return text;
        }

        private void SummarizeList(List<double> durations, out double min, out double max, out double avg)
        {
            min = double.MaxValue;
            max = double.MinValue;
            double total = 0;
            foreach (double value in durations)
            {
                total += value;
                min = Math.Min(min, value);
                max = Math.Max(max, value);
            }

            avg = total / durations.Count;
        }
    }
}
