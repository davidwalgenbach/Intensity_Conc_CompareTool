using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intensity_Conc_CompareTool.Resources
{
    internal class Utilities
    {
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
            for (int i = 0; i < CSVData.Count; i++)
            {
                CSVData[i] = CSVData[i].OrderBy(x => removeFirstCharacter(x.Key)).ToList();
            }
            for (int j = 0; j < DBData.Count; j++)
            {
                DBData[j] = DBData[j].OrderBy(x => removeFirstCharacter(x.Key)).ToList();
            }
        }

        //Compares length of keyvaluepair lists initially to make sure they are the same
        public static bool compareListLength(List<List<KeyValuePair<string, string>>> CSVData, List<List<KeyValuePair<string, string>>> DBData)
        {
            if (CSVData.Count == DBData.Count)
            {
                Console.WriteLine("Row counts match.");
                for (int i = 0; i < CSVData.Count; i++)
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

        //Compares actual Intensity key value pair data
        public static bool compareIntensityListContents(List<List<KeyValuePair<string, string>>> CSVData, List<List<KeyValuePair<string, string>>> DBData)
        {
            bool equal = new bool();

            for (int i = 0; i < CSVData.Count; i++)
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

        //Compares actual Concentration key value pair data
        public static bool compareConcListContents(List<List<KeyValuePair<string, string>>> CSVData, List<List<KeyValuePair<string, string>>> DBData)
        {
            bool equal = new bool();

            for (int i = 0; i < CSVData.Count; i++)
            {
                //Temporary until the RS2 issue is alleviated. For now, will just skip all RS2 rows. (Every fourth row starting from 0) 
                if ((i+1) % 4 != 0)
                {
                    using (var e1 = CSVData[i].GetEnumerator())
                    using (var e2 = DBData[i].GetEnumerator())
                    {
                        while (e1.MoveNext() && e2.MoveNext())
                        {
                            try
                            {
                                if (Math.Abs(Double.Parse(e1.Current.Value) - Double.Parse(e2.Current.Value)) < 0.05)
                                {
                                    equal = true;
                                    continue;
                                }
                                else
                                {
                                    if (e1.Current.Value == "NaN" && e2.Current.Value == "0")
                                    {
                                        equal = true;
                                        continue;
                                    }
                                    else
                                    {
                                        return false;
                                    }
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
                else
                {
                    continue;
                }
            }
            return equal;
        }
    }
}
