using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace MyWebService
{
    class ChartMaker
    {
        /// <summary>
        /// Generates chart based on downloaded data.
        /// </summary>
        /// <param name="dataFile">File containing data.</param>
        /// <returns>Path to chart image.</returns>
        public static string CreateChart(string dataFile)
        {
            Dictionary<string, int> data = new Dictionary<string, int>();
            using (StreamReader reader = new StreamReader("service_log.log"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = line.Split(';');

                    if (DateTime.TryParse(values[0], out DateTime time) && int.TryParse(values[1], out int count))
                    {
                        if (!data.ContainsKey(time.ToLongTimeString()))
                            data.Add(time.ToLongTimeString(), count);
                    }
                }
            }

            Chart c = new Chart();

            c.ChartAreas.Add("TestArea");
            c.ChartAreas[0].AxisX.Title = "Time";
            c.ChartAreas[0].AxisX.TitleFont = new Font("Verdana", 11, FontStyle.Bold);

            c.ChartAreas[0].AxisY.Title = "Number of players";
            c.ChartAreas[0].AxisY.TitleFont = new Font("Verdana", 11, FontStyle.Bold);

            c.ChartAreas[0].BorderDashStyle = ChartDashStyle.Dot;
            c.ChartAreas[0].BorderWidth = 0;
            c.ChartAreas[0].BackGradientStyle = GradientStyle.Center;
            c.ChartAreas[0].AxisX.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep45;

            c.Legends.Add("Legend");
            c.Series.Add("Player count");

            c.Series[0].Font = new Font("Verdana", 8);
            c.Series[0].ChartType = SeriesChartType.Line;
            c.Series[0].Points.DataBindXY(data.Keys, data.Values);
            c.Series[0].Color = Color.Red;
            c.Series[0].BorderWidth = 1;

            string fileName = "graph_" + DateTime.Now.ToShortTimeString().Replace(':', '_') + ".png";
            c.SaveImage(fileName, ChartImageFormat.Png);

            return fileName;
        }
    }
}
