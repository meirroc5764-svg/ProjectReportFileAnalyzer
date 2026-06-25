using System;
namespace projectRFA
{
    enum ClassReportsType
    {
        Intel,
        Recon,
        Analyze,
        Collect
    }
    
    enum ClassStatus
    {
        Rejected,
        Approved,
        Pending
    }

    class FileAnalize
    {
        const int MAX_REPORTS = 100;
        static void Main()
        {
            string path = @"C:\Users\Aenigma\OneDrive\Desktop\projectFileAnalyzer\ReportFileAnalyzer\reports.txt";

            string[] allData = ReadFile(path);
            
            string[] unit = new string[MAX_REPORTS];
            ClassReportsType[] reportType = new ClassReportsType[MAX_REPORTS];
            int[] priority = new int[MAX_REPORTS];
            double[] score = new double[MAX_REPORTS];
            ClassStatus[] status = new ClassStatus[MAX_REPORTS];
            
            ProcessReports(allData, unit, reportType, priority, score, status);
        }

        static string[] ReadFile(string path)
        {
            string fileName = path.Split("\\")[^1];
            if (File.Exists(path))
            {
                string[] allData = File.ReadAllLines(path);
                if (allData.Length == 0)
                    return null;
                int count = allData.Length;
                Console.WriteLine($"File loaded:{count} lines found");
                return allData;
            }
            else
                return null;

        }
        static void ProcessReports(string[] allData, string[] unit, ClassReportsType[] reportType, int[] priority, double[] score, ClassStatus[] status)
        {

            for (int index = 0; index < allData.Length; index++)
            {
                ClassReportsType ReportType;
                int Priority;
                double Score;
                ClassStatus Status;


                if (allData[index] == null)
                    continue;
                string[] cleanLine = allData[index].Split(",", StringSplitOptions.RemoveEmptyEntries);

                if (cleanLine.Length != 5)
                {
                    Console.WriteLine("Invalid len");
                    continue;
                }
                    

                if (!Enum.TryParse<ClassReportsType>(cleanLine[1].Trim(), true, out ReportType))
                {
                    Console.WriteLine($"Invalid Report Type: {cleanLine[1].Trim()}");
                    continue;
                }

                else if (!Enum.TryParse<ClassStatus>(cleanLine[4].Trim(), true, out Status))
                {
                    Console.WriteLine($"Invalid Status: {cleanLine[4].Trim()}");
                    continue;
                }

                else if (!int.TryParse(cleanLine[2], out Priority))
                {
                    Console.WriteLine($"Invalid Priority: {cleanLine[2]}");
                    continue;
                }

                else if (0 > Priority || Priority > 6)
                {
                    Console.WriteLine($"Invalid Priority: {cleanLine[2].Trim()}");
                    continue;
                }

                else if (!double.TryParse(cleanLine[3].Trim(), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out Score))
                {
                    Console.WriteLine($"Invalid Score: {cleanLine[3]}");
                    continue;
                }

                else if (Score > 100.0 || Score < 0.0)
                {
                    Console.WriteLine($"Invalid Score: {cleanLine[3]}!");
                    continue;
                }

                unit[index] = cleanLine[0];
                reportType[index] = ReportType;
                priority[index] = Priority;
                score[index] = Score;
                status[index] = Status;
                Console.WriteLine("Valid record processed.");
                Console.WriteLine($"unit: {cleanLine[0]}, reportType: {ReportType}, priority: {Priority}, score: {Score}, status: {Status}");

            }
        }
        static double CalculateAverage(double[] Score)
        {
                double average = 0;
                foreach(double score in Score)
                {
                    average += score;
                }
                return average / Score.Length;
            

        }
        static double FindMaxScore(double[] Score)
        {
            double maxScore = 0;
            foreach (double score in Score)
            {
                if (score > maxScore)
                    maxScore = score;
            }
                
            return maxScore;
        }
        static 
    }
}