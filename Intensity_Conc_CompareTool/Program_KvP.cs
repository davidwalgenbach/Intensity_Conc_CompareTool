using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Intensity_Conc_CompareTool
{
    internal class Program_KvP
    {
        public static string intensityCSVFileLocation = @"Resources\Intensities_WithDetails_011723092726.csv";
        public static string concentrationCSVFileLocation = @"Resources\Concentrations_WithDetails_011723092726.csv";


        static void Main(string[] args)
        {
            bool intensityListMatch = new bool();
            bool concentrationListMatch = new bool();

            List<List<KeyValuePair<string, string>>> intensityCSVData = getIntensityCSVData();
            List<List<KeyValuePair<string, string>>> intensityDBData = getIntensityDBData();

            List<List<KeyValuePair<string, string>>> concentrationCSVData = getConcentrationCSVData();
            List<List<KeyValuePair<string, string>>> concentrationDBData = getConcentrationDBData();

            bool intensityListLengthMatch = compareListLength(intensityCSVData, intensityDBData);
            bool concentrationListLengthMatch = compareListLength(concentrationCSVData, concentrationDBData);

            if (intensityListLengthMatch == true)
            {
                sortLists(intensityCSVData, intensityDBData);
                intensityListMatch = compareListContents(intensityCSVData, intensityDBData);
            }
            else
            {
                Console.WriteLine("Intensity Lists are not matching. Further comparisons will not execute.");
            }

            if (concentrationListLengthMatch == true)
            {
                sortLists(concentrationCSVData, concentrationDBData);
                concentrationListMatch = compareListContents(concentrationCSVData, concentrationDBData);
            }
            else
            {
                Console.WriteLine("Concentration Lists are not matching. Further comparisons will not execute.");
            }

            Console.WriteLine("Intensity Comparison evaluation (False = Failed. True = Success): " + intensityListMatch.ToString());

            Console.WriteLine("Concentration Comparison evaluation (False = Failed. True = Success): " + concentrationListMatch.ToString());
        }

        //Gets Analysis ID from CSV to be used in SQL Query to create Pivot tables
        public static string getAnalysisID_fromCSV()
        {
            string analysisID = "";

            using (var reader = new StreamReader(intensityCSVFileLocation))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CurrentCulture))
            {
                csv.Read();
                csv.ReadHeader();
                csv.Read();

                analysisID = "'" + csv.GetField(csv.HeaderRecord.Count() - 4) + "'";
            }

            return analysisID;
        }

        //Gets list of Analytes to be inserted into SQL Query to create Pivot tables
        public static string getAnalyteList_fromCSV()
        {
            string analyteList = "";

            using (var reader = new StreamReader(intensityCSVFileLocation))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CurrentCulture))
            {
                List<List<KeyValuePair<string, string>>> rowList = new List<List<KeyValuePair<string, string>>>();
                csv.Read();
                csv.ReadHeader();

                int i = 13;
                while (i < csv.HeaderRecord.Count() - 4)
                {
                    analyteList = analyteList + ',' + '"' + csv.HeaderRecord[i] + '"';
                    i++;
                }
                analyteList = analyteList.TrimStart(',');

                return analyteList;
            }
        }
        //Compares actual key value pair data
        public static bool compareListContents(List<List<KeyValuePair<string, string>>> CSVData, List<List<KeyValuePair<string, string>>> DBData)
        {
            bool equal = new bool();

            for (int i=0; i<CSVData.Count; i++)
            {
                using (var e1 = CSVData[i].GetEnumerator())
                using (var e2 = DBData[i].GetEnumerator())
                {
                    while (e1.MoveNext() && e2.MoveNext())
                    {
                        try
                        {
                            if (Double.Parse(e1.Current.Value).Equals(Double.Parse(e2.Current.Value)))
                            {
                                equal = true;
                                continue;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        catch (Exception e)
                        {
                            if (e1.Current.Value == "NaN" && e2.Current.Value == "")
                            {
                                continue;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return equal;
        }
        //Compares length of keyvaluepair lists initially to make sure they are the same
        public static bool compareListLength(List<List<KeyValuePair<string, string>>> CSVData, List<List<KeyValuePair<string, string>>> DBData)
        {
            if (CSVData.Count == DBData.Count)
            {
                Console.WriteLine("Row counts match.");
                for (int i=0; i<CSVData.Count; i++)
                {
                    if (CSVData[i].Count == DBData[i].Count)
                    {
                        Console.WriteLine("Analyte counts per row match.");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Analyte counts per row do not match.");
                        return false;
                    }
                }
                //this will never be hit?
                return false;
            }
            else
            {
                Console.WriteLine("Row counts do not match.");
                return false;
            }
        }

        //Used when sorting keyvaluepair lists
        public static string removeFirstCharacter(string s)
        {
            if (s.StartsWith("[", StringComparison.CurrentCultureIgnoreCase))
            {
                return s.Substring(1).TrimStart();
            }
            return s;
        }

        //Sorts the keyvaluepair lists
        public static void sortLists(List<List<KeyValuePair<string, string>>> CSVData, List<List<KeyValuePair<string, string>>> DBData)
        {
            for (int i=0; i<CSVData.Count; i++)
            {
                CSVData[i] = CSVData[i].OrderBy(x => removeFirstCharacter(x.Key)).ToList();
            }
            for (int j=0; j<DBData.Count; j++)
            {
                DBData[j] = DBData[j].OrderBy(x => removeFirstCharacter(x.Key)).ToList();
            }
        }

        public static List<List<KeyValuePair<string, string>>> getIntensityDBData()
        {
            //Read in all intensity rows from DB
            try
            {
                using (SqlConnection connection = new SqlConnection("Server=plasmatraxDB\\PLASMATRAXQA;Database=PLASMATRAX;Trusted_Connection=True"))
                {
                    using (SqlCommand command = new SqlCommand(string.Format(File.ReadAllText("Resources\\Create_IntensityDBData_PivotTable.sql"), getAnalyteList_fromCSV(), getAnalysisID_fromCSV()), connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        List<List<KeyValuePair<string, string>>> rowList = new List<List<KeyValuePair<string, string>>>();

                        while (reader.Read())
                        {
                            var row = new List<KeyValuePair<string, string>>();

                            int i = 6;
                            while (i < reader.FieldCount)
                            {
                                row.Add(new KeyValuePair<string, string>(reader.GetName(i), reader.GetValue(i).ToString()));
                                i++;
                            }
                            rowList.Add(row);
                        }
                        return rowList;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public static List<List<KeyValuePair<string, string>>> getConcentrationDBData()
        {
            //Read in all intensity rows from DB
            try
            {
                using (SqlConnection connection = new SqlConnection("Server=plasmatraxDB\\PLASMATRAXQA;Database=PLASMATRAX;Trusted_Connection=True"))
                {
                    using (SqlCommand command = new SqlCommand(File.ReadAllText("Resources\\Create_ConcentrationDBData_PivotTable.sql"), connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        List<List<KeyValuePair<string, string>>> rowList = new List<List<KeyValuePair<string, string>>>();

                        while (reader.Read())
                        {
                            var row = new List<KeyValuePair<string, string>>();

                            int i = 6;
                            while (i < reader.FieldCount)
                            {
                                row.Add(new KeyValuePair<string, string>(reader.GetName(i), reader.GetValue(i).ToString()));
                                i++;
                            }
                            rowList.Add(row);
                        }
                        return rowList;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public static List<List<KeyValuePair<string, string>>> getConcentrationCSVData()
        {
            //Read in all concentration rows from CSV
            using (var reader = new StreamReader(concentrationCSVFileLocation))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CurrentCulture))
            {
                List<List<KeyValuePair<string, string>>> rowList = new List<List<KeyValuePair<string, string>>>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var row = new List<KeyValuePair<string, string>>();

                    int i = 13;
                    while (i < csv.HeaderRecord.Count() - 4)
                    {
                        if (csv.GetField(csv.HeaderRecord[4]) != "Known Concentration" && csv.GetField(csv.HeaderRecord[4]) != "BEC" && csv.GetField(csv.HeaderRecord[4]) != "Concentration Recovery (%)")
                        {
                            row.Add(new KeyValuePair<string, string>(csv.HeaderRecord[i], csv.GetField(csv.HeaderRecord[i])));
                        }
                        i++;
                    }
                    if (csv.GetField(csv.HeaderRecord[4]) != "Known Concentration" && csv.GetField(csv.HeaderRecord[4]) != "BEC" && csv.GetField(csv.HeaderRecord[4]) != "Concentration Recovery (%)")
                    {
                        rowList.Add(row);
                    }
                }
                return rowList;
            }
        }

        public static List<List<KeyValuePair<string, string>>> getIntensityCSVData()
        {
            //Read in all intensity rows from CSV
            using (var reader = new StreamReader(intensityCSVFileLocation))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CurrentCulture))
            {
                List<List<KeyValuePair<string, string>>> rowList = new List<List<KeyValuePair<string, string>>>();

                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var row = new List<KeyValuePair<string, string>>();

                    int i = 13;
                    while (i <csv.HeaderRecord.Count() - 4)
                    {
                        //Removes the percent symbol from RSD Rows
                        if (csv.GetField(csv.HeaderRecord[i]).Contains('%'))
                        {
                            row.Add(new KeyValuePair<string, string>(csv.HeaderRecord[i], csv.GetField(csv.HeaderRecord[i]).TrimEnd('%')));
                            i++;
                        }
                        else
                        {
                            row.Add(new KeyValuePair<string, string>(csv.HeaderRecord[i], csv.GetField(csv.HeaderRecord[i])));
                            i++;
                        }
                    }
                    rowList.Add(row);
                }
                return rowList;
            }
        }
    }
}
