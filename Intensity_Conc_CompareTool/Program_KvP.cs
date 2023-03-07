﻿using CsvHelper;
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
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = @"C:\ExportFiles";
            watcher.EnableRaisingEvents = true;
            watcher.Filter = "*.csv";
            watcher.Created += new FileSystemEventHandler(watcher_OnCreated);

            bool intensityListMatch = new bool();
            bool concentrationListMatch = new bool();

            List<List<KeyValuePair<string, string>>> intensityCSVData = DataProvider.Instance.getIntensityCSVData();
            List<List<KeyValuePair<string, string>>> intensityDBData = DataProvider.Instance.getIntensityDBData();

            List<List<KeyValuePair<string, string>>> concentrationCSVData = DataProvider.Instance.getConcentrationCSVData();
            List<List<KeyValuePair<string, string>>> concentrationDBData = DataProvider.Instance.getConcentrationDBData();

            bool intensityListLengthMatch = Utilities.compareListLength(intensityCSVData, intensityDBData);
            bool concentrationListLengthMatch = Utilities.compareListLength(concentrationCSVData, concentrationDBData);

            if (intensityListLengthMatch == true)
            {
                Utilities.sortLists(intensityCSVData, intensityDBData);
                intensityListMatch = Utilities.compareIntensityListContents(intensityCSVData, intensityDBData);
            }
            else
            {
                Console.WriteLine("Intensity Lists are not matching. Further comparisons will not execute.");
            }

            if (concentrationListLengthMatch == true)
            {
                Utilities.sortLists(concentrationCSVData, concentrationDBData);
                concentrationListMatch = Utilities.compareConcListContents(concentrationCSVData, concentrationDBData);
            }
            else
            {
                Console.WriteLine("Concentration Lists are not matching. Further comparisons will not execute.");
            }

            Console.WriteLine("Intensity Comparison evaluation (False = Failed. True = Success): " + intensityListMatch.ToString());

            Console.WriteLine("Concentration Comparison evaluation (False = Failed. True = Success): " + concentrationListMatch.ToString());

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }

        private static void watcher_OnCreated(object source, FileSystemEventArgs e)
        {
            FileInfo file = new FileInfo(e.FullPath);
            Console.WriteLine(file.Name);
        }
    }
}
