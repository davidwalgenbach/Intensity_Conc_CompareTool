using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intensity_Conc_CompareTool.Resources
{
    public sealed class DataProvider
    {
        private static DataProvider instance = null;
        private static readonly object padlock = new object();

        //Contains a Mode - Ran on an instrument
        public static string _intensityCSVFileLocation = @"Resources\Testing CSV Files\Intensities_WithDetails_011723092726.csv";
        //Does not contain a Mode - Ran using 5 Second Timer
        //public static string intensityCSVFileLocation = "C:\\Users\\dwalgenbach\\Documents\\Resources\\intensitiesLEL.csv";
        public static string _concentrationCSVFileLocation = @"C:\Users\dwalgenbach\Documents\concentrationsWDetails.csv";

        private DataProvider()
        {
        }

        public string intensityCSVFileLocation
        {
            get
            {
                return _intensityCSVFileLocation;
            }
            set
            {
                _intensityCSVFileLocation = value;
            }
        }

        public string concentrationCSVFileLocation
        {
            get
            {
                return _concentrationCSVFileLocation;
            }
            set
            {
                _concentrationCSVFileLocation = value;
            }
        }

        //Reads in Intensity Data Rows from input CSV
        public List<List<KeyValuePair<string, string>>> getIntensityCSVData()
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
                    while (i < csv.HeaderRecord.Count() - 4)
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

        //Reads in Concentration Data Rows from input CSV
        public List<List<KeyValuePair<string, string>>> getConcentrationCSVData()
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

        //Reads in Intensity Data Rows from Database
        public List<List<KeyValuePair<string, string>>> getIntensityDBData()
        {
            //Read in all intensity rows from DB
            try
            {
                using (SqlConnection connection = new SqlConnection("Server=plasmatraxDB\\PLASMATRAXQA;Database=PLASMATRAX;Trusted_Connection=True"))
                {
                    using (SqlCommand command = new SqlCommand(string.Format(File.ReadAllText("Resources\\SQL Scripts\\Create_IntensityDBData_PivotTable.sql"), getAnalyteList_fromCSV(intensityCSVFileLocation), getAnalysisID_fromCSV(intensityCSVFileLocation)), connection))
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

        //Reads in Concentration Data Rows from Database
        public List<List<KeyValuePair<string, string>>> getConcentrationDBData()
        {
            //Read in all intensity rows from DB
            try
            {
                using (SqlConnection connection = new SqlConnection("Server=plasmatraxDB\\PLASMATRAXQA;Database=PLASMATRAX;Trusted_Connection=True"))
                {
                    using (SqlCommand command = new SqlCommand(string.Format(File.ReadAllText("Resources\\SQL Scripts\\Create_ConcentrationDBData_PivotTable.sql"), getAnalyteList_fromCSV(concentrationCSVFileLocation), getAnalysisID_fromCSV(concentrationCSVFileLocation)), connection))
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

        //Gets Analysis ID from CSV to be used in SQL Query to create Pivot tables
        public string getAnalysisID_fromCSV(string CSVFileLocation)
        {
            string analysisID = "";

            using (var reader = new StreamReader(CSVFileLocation))
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
        public string getAnalyteList_fromCSV(string CSVFileLocation)
        {
            string analyteList = "";

            using (var reader = new StreamReader(CSVFileLocation))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CurrentCulture))
            {
                List<List<KeyValuePair<string, string>>> rowList = new List<List<KeyValuePair<string, string>>>();
                csv.Read();
                csv.ReadHeader();

                int i = 13;
                while (i < csv.HeaderRecord.Count() - 4)
                {
                    if (!csv.HeaderRecord[14].Contains("["))
                    {
                        analyteList = analyteList + ',' + '"' + csv.HeaderRecord[i] + " [ " + " ]" + '"';
                    }
                    else
                    {
                        analyteList = analyteList + ',' + '"' + csv.HeaderRecord[i] + '"';
                    }
                    i++;
                }
                analyteList = analyteList.TrimStart(',');
                analyteList = analyteList.Replace("ppb", "");
                analyteList = analyteList.Replace("ppt", "");
                analyteList = analyteList.Replace("ppm", "");
                analyteList = analyteList.Replace("\r", "");
                analyteList = analyteList.Replace("\n", "");

                return analyteList;
            }
        }

        public static DataProvider Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DataProvider();
                    }
                    return instance;
                }
            }
        }
    }
}
