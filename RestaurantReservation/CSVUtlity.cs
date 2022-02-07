using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace RestaurantReservation
{
    public static class CSVUtlity
    {
        public static string fileName = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "예약.csv";
        public static void ToCSV(DataTable dtDataTable)
        {

            StreamWriter sw = new StreamWriter(fileName, false, Encoding.UTF8);
            //headers    
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }
        public static DataTable ConvertCSVtoDataTable(DataTable dt)
        {
            try
            {
                TextFieldParser parser = new TextFieldParser(fileName);
                parser.HasFieldsEnclosedInQuotes = true;
                parser.SetDelimiters(",");
                string[] headers;
                headers = parser.ReadFields();
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        switch (i)
                        {
                            case (int)MainProgram.DataIdx.num:
                                dr[i] = Convert.ToInt32(fields[i]);
                                break;
                            case (int)MainProgram.DataIdx.entered:
                                bool entered = false;
                                if (fields[i] == true.ToString())
                                {
                                    entered = true;
                                }
                                dr[i] = entered;
                                break;
                            default:
                                dr[i] = fields[i];
                                break;

                        }
                    }
                    dt.Rows.Add(dr);
                }
                parser.Close();
            }
            catch
            {

            }
            return dt;
        }
    }
}