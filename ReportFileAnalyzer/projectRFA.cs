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
            if (allData == null)
                Console.WriteLine("file not found");

            else
            {
                string[] unit = new string[MAX_REPORTS];
                ClassReportsType[] reportType = new ClassReportsType[MAX_REPORTS];
                int[] priority = new int[MAX_REPORTS];
                double[] score = new double[MAX_REPORTS];
                ClassStatus[] status = new ClassStatus[MAX_REPORTS];


                int lendata = allData.Length;
                ProcessReports(allData, unit, reportType, priority, score, status);
                DisplayBasicStatistics(lendata, score);
                DisplayStatusCounts(lendata, status);
                DisplayTypeCounts(lendata, reportType);
                DisplayHighestPriorityApproved(lendata, unit, reportType, priority, score, status);
                DisplayAverageByPriority(lendata, priority, score);
            }
            
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
            int validIndex = 0;

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

                else if (!int.TryParse(cleanLine[2].Trim(), out Priority))
                {
                    Console.WriteLine($"Invalid Priority: {cleanLine[2]}");
                    continue;
                }

                else if (1 > Priority || Priority > 5)
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

                unit[validIndex] = cleanLine[0].Trim();
                reportType[validIndex] = ReportType;
                priority[validIndex] = Priority;
                score[validIndex] = Score;
                status[validIndex] = Status;
                Console.WriteLine("Valid record processed.");
                Console.WriteLine($"unit: {cleanLine[0]}, reportType: {ReportType}, priority: {Priority}, score: {Score}, status: {Status}");
                validIndex++;
            }
            Console.WriteLine($"File loaded:{validIndex} correct lines found");
            Console.WriteLine($"File loaded:{allData.Length - validIndex} not correct lines found");
        }
        static double CalculateAverage(double[] Score, int datalen)
        {
            double average = 0;
            for (int i = 0; i < datalen; i++)
            {
                average += Score[i];
            }
            return (average / datalen);
            

        }
        static double FindMaxScore(double[] Score, int datalen)
        {
            double maxScore = 0;
            for (int i = 0; i < datalen; i++)
            {
                if (Score[i] > maxScore)
                    maxScore = Score[i];
            }
                
            return maxScore;
        }
        static double FindMinScore(double[] Score, int datalen)
        {
            double minScore = Score[0];
            for (int i = 0; i < datalen; i++)
            {
                if (Score[i] < minScore)
                {
                    minScore = Score[i];
                }

            }
            return minScore;
        }
        static int CountByStatus(ClassStatus[] StatusAr, ClassStatus userStatus, int datalen)
        { 
                int count = 0;
            for (int i = 0; i < datalen; i++)
            {
                    if (StatusAr[i] == userStatus)
                        count++;

            }
                return count;
        }
        static int CountByType(ClassReportsType[] ReportTypeAr, ClassReportsType userType, int datalen)
        {
            int count = 0;
            for (int i = 0; i < datalen; i++)
            {
                if (ReportTypeAr[i] == userType)
                    count++;

            }
            return count;
        }
        static void DisplayBasicStatistics(int lendata, double[] Score)
        {
            Console.WriteLine("=== Report Statistics ===");
            Console.WriteLine($"Total Reports:{lendata} ");
            Console.WriteLine($"Average Score: {CalculateAverage(Score, lendata):F2}");
            Console.WriteLine($"Highest Score: {FindMaxScore(Score, lendata)}");
            Console.WriteLine($"Lowest Score: {FindMinScore(Score, lendata)}");
        }
        static void DisplayStatusCounts(int lendata, ClassStatus[] Status)
        {
            foreach (ClassStatus status in Enum.GetValues<ClassStatus>())
            {
                Console.WriteLine($"{status}: {CountByStatus(Status, status, lendata)}");
            }
        }
        static void DisplayTypeCounts(int lendata, ClassReportsType[] ReportType)
        {
            foreach (ClassReportsType type in Enum.GetValues<ClassReportsType>())
            {
                Console.WriteLine($"{type}: {CountByType(ReportType, type, lendata)}");
            }
        }
        static void DisplayHighestPriorityApproved(int lendata, string[] unit, ClassReportsType[] reportType, int[] Priority, double[] score, ClassStatus[] Status)
        {
            int index = 0;
            for (int i = 0; i < lendata; i++)
            {
                if (Status[i] == ClassStatus.Approved)
                {
                    if (Priority[index] < Priority[i])
                        index = i;
                }
            }
            Console.WriteLine($"unit:{unit[index]},reportType: {reportType[index]},Priority: {Priority[index]},score:{score[index]},Status: {Status[index]}");
        }
        static double AvergeByPrioryty(int lendata, double[] Score, int[] Priority, int userPriority)
        {
            int count = 0;
            double score = 0;
            for (int i = 0; i < lendata; i++)
            {

                if (Priority[i] == userPriority)
                {
                    score += Score[i];
                    count++;
                }
            }
            if (count == 0)
                return -1;
            return score / count;


        }
        
        static void DisplayAverageByPriority(int lendata, int[] Priority, double[] Score)
        {
            Console.WriteLine("=== Average Score by Priority ===");

            for (int i = 1; i <= 5; i++)
            {
                double average = AvergeByPrioryty(lendata, Score, Priority, i);

                if (average == -1)
                    Console.WriteLine($"Priority {i}: No reports");
                else
                    Console.WriteLine($"Priority {i}: {average:F2}");
            }
        }
    }
}