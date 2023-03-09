using CsvHelper;
using Intensity_Conc_CompareTool.Resources;
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
        static void Main(string[] args)
        {
            //May need two file watchers. One for ConcentrationCSV and one for IntensityCSV
            FileSystemWatcher intensity_watcher = new FileSystemWatcher();
            intensity_watcher.Path = @"C:\ExportFiles";
            intensity_watcher.EnableRaisingEvents = true;
            intensity_watcher.Filter = "Intensities.csv";
            intensity_watcher.Created += new FileSystemEventHandler(intensity_watcher_OnCreated);

            FileSystemWatcher conc_watcher = new FileSystemWatcher();
            conc_watcher.Path = @"C:\ExportFiles";
            conc_watcher.EnableRaisingEvents = true;
            conc_watcher.Filter = "Concentrations.csv";
            conc_watcher.Created += new FileSystemEventHandler(conc_watcher_OnCreated);

            Console.WriteLine("Please drop CSV files to be validated in the directory: C:\\ExportFiles.");
            Console.WriteLine("Concentration Exports should be named Concentrations.csv, Intensity Exports should be named Intensities.csv");
            Console.WriteLine("Press Enter to Exit the program." + "\n");
            Console.ReadLine();

        }

        public static void compareIntensity()
        {
            bool intensityListMatch = new bool();

            List<List<KeyValuePair<string, string>>> intensityCSVData = DataProvider.Instance.getIntensityCSVData();
            List<List<KeyValuePair<string, string>>> intensityDBData = DataProvider.Instance.getIntensityDBData();

            bool intensityListLengthMatch = Utilities.compareListLength(intensityCSVData, intensityDBData);

            if (intensityListLengthMatch == true)
            {
                Utilities.sortLists(intensityCSVData, intensityDBData);
                intensityListMatch = Utilities.compareIntensityListContents(intensityCSVData, intensityDBData);
            }
            else
            {
                Console.WriteLine("Intensity Lists are not matching. Further comparisons will not execute.");
            }

            Console.WriteLine("Intensity Comparison evaluation (False = Failed. True = Success): " + intensityListMatch.ToString() + "\n");

            Console.WriteLine("If you would like to continue, drop another CSV file into the directory: C:\\ExportFiles.");
            Console.WriteLine("Concentration Exports should be named Concentrations.csv, Intensity Exports should be named Intensities.csv");
            Console.WriteLine("Press Enter to Exit the program." + "\n");
        }

        public static void compareConc()
        {
            bool concentrationListMatch = new bool();

            List<List<KeyValuePair<string, string>>> concentrationCSVData = DataProvider.Instance.getConcentrationCSVData();
            List<List<KeyValuePair<string, string>>> concentrationDBData = DataProvider.Instance.getConcentrationDBData();

            bool concentrationListLengthMatch = Utilities.compareListLength(concentrationCSVData, concentrationDBData);

            if (concentrationListLengthMatch == true)
            {
                Utilities.sortLists(concentrationCSVData, concentrationDBData);
                concentrationListMatch = Utilities.compareConcListContents(concentrationCSVData, concentrationDBData);
            }
            else
            {
                Console.WriteLine("Concentration Lists are not matching. Further comparisons will not execute.");
            }

            Console.WriteLine("Concentration Comparison evaluation (False = Failed. True = Success): " + concentrationListMatch.ToString() + "\n");

            Console.WriteLine("If you would like to continue, drop another CSV file into the directory: C:\\ExportFiles.");
            Console.WriteLine("Concentration Exports should be named Concentrations.csv, Intensity Exports should be named Intensities.csv");
            Console.WriteLine("Press Enter to Exit the program." + "\n");
        }

        private static void intensity_watcher_OnCreated(object source, FileSystemEventArgs e)
        {
            FileInfo file = new FileInfo(e.FullPath);
            Console.WriteLine(e.FullPath);
            DataProvider.Instance.intensityCSVFileLocation = e.FullPath;
            compareIntensity();
        }

        private static void conc_watcher_OnCreated(object source, FileSystemEventArgs e)
        {
            FileInfo file = new FileInfo(e.FullPath);
            Console.WriteLine(e.FullPath);
            DataProvider.Instance.concentrationCSVFileLocation = e.FullPath;
            compareConc();
        }
    }
}
