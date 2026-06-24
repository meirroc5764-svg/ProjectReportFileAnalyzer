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
                return allData;
            }
            else
                return null;

        }
        static string ProcessReports(string[] allData, string[] unit, ClassReportsType[] reportType, int[] priority, double[] score, ClassStatus[] status)
        {

            for (int index = 0; index == allData.Length; index++)
            {
                ClassReportsType ReportType;
                int Priority;
                double Score;
                ClassStatus Status;


                if (line == null)
                    continue;
                string cleanLine = string.Join(",", allData[index].Split(" ", StringSplitOptions.RemoveEmptyEntries));

                if (!ClassReportsType.Tryparse(cleanLine[1], true, out ReportType))
                    continue;

                else if (!ClassStatus.TryParse(cleanLine[4], true, out Status))
                    continue;

                else if (!int.TryParse(cleanLine[2], true, out Priority))
                    continue;

                else if (0 > priority || priority > 6)
                    continue;

                else if (!double.TryParse(cleanLine[3], true, out Score))
                    continue;

                else if (Score > 100.0 || Score < 0.0)
                    continue;

                unit[index] = cleanLine[0];
                reportType[index] = ReportType;
                priority[index] = Priority;
                score[index] = Score;
                status[index] = Status;



            }
            return $"File loaded:{uint.Length} lines found";
                
        }
            
        
    }
}