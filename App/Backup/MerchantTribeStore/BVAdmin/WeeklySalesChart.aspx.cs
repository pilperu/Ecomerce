using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using MerchantTribe.Commerce.Reporting;
using System.Drawing;
using System.Web.UI.DataVisualization;
using System.Web.UI.DataVisualization.Charting;

namespace MerchantTribeStore
{

    public partial class BVAdmin_WeeklySalesChart : BaseAdminJsonPage
    {
        public MemoryStream Stream { get; set; }
        public string OutputContentType { get; set; }
        public string ETag { get; set; }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Stream = null;
            OutputContentType = "image/png";
            ETag = string.Empty;

            SalesSummary summary = new SalesSummary(MTApp.CurrentStore.Id);
            GenerateChart(summary.GetWeeklySummary(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, MTApp.CurrentStore.Settings.TimeZone), MTApp.OrderServices), 
                "Sales This Week", 600, 200);
            RenderChart();
        }

        public void GenerateChart(WeeklySummary data, string chartTitle, int width, int height)
        {
            Chart c = new Chart();

            // Default Chart Styles
            c.ID = "DynamicChart";
            c.ImageType = ChartImageType.Png;
            c.RenderType = RenderType.BinaryStreaming;
            c.AntiAliasing = AntiAliasingStyles.All;
            c.Height = height;
            c.Width = width;
            c.BackColor = Color.White;
            //c.BorderSkin.SkinStyle = BorderSkinStyle.Sunken;
            c.BorderSkin.SkinStyle = BorderSkinStyle.None;

            // Default Chart Area
            ChartArea ca = c.ChartAreas.Add("Default");
            ca.BackGradientStyle = GradientStyle.TopBottom;
            ca.BackSecondaryColor = Color.White;
            ca.BackColor = Color.LightSteelBlue;
            ca.AxisX.IsMarginVisible = true;
            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisX.LineColor = Color.FromArgb(255, 200, 200, 200);
            ca.AxisX.LineWidth = 2;

            ca.AxisY.LineColor = ca.AxisX.LineColor;
            ca.AxisY.LineWidth = ca.AxisX.LineWidth;
            ca.AxisY.MajorGrid.LineColor = Color.FromArgb(200, 255, 255, 255);
            ca.AxisY.MajorGrid.LineWidth = 2;
            ca.AxisY.MinorGrid.LineColor = Color.FromArgb(100, 255, 255, 255);
            ca.AxisY.MinorGrid.LineWidth = 1;
            ca.AxisY.MinorGrid.Enabled = true;

            // Formating of Labels
            ca.AxisY.LabelStyle.Format = "c";

            // Default Title Settings
            Title t = c.Titles.Add("MainTitle");
            t.Text = chartTitle;
            t.Visible = true;
            t.Font = new Font("Verdana", 14, FontStyle.Bold);

            Legend l = new Legend("Last Week Legend");
            c.Legends.Add(l);

            // Custom Data Points
            Series s2 = c.Series.Add("Last Week");
            s2.ChartType = SeriesChartType.Column;

            s2.Color = Color.FromArgb(148, 168, 187);
            s2.BackSecondaryColor = Color.FromArgb(50, 148, 168, 187);
            s2.BackGradientStyle = GradientStyle.TopBottom;
            s2.BorderColor = Color.FromArgb(121, 137, 153);



            s2.BorderWidth = 1;
            s2.MarkerStyle = MarkerStyle.None;
            s2.MarkerSize = 1;
            s2.MarkerColor = Color.White;
            s2.MarkerBorderWidth = 2;
            s2.MarkerBorderColor = Color.FromArgb(200, 200, 0);


            if (data != null)
            {
                s2.Points.Add(SetDataPoint(data.SundayLast, "Sun"));
                s2.Points.Add(SetDataPoint(data.MondayLast, "Mon"));
                s2.Points.Add(SetDataPoint(data.TuesdayLast, "Tue"));
                s2.Points.Add(SetDataPoint(data.WednesdayLast, "Wed"));
                s2.Points.Add(SetDataPoint(data.ThursdayLast, "Thr"));
                s2.Points.Add(SetDataPoint(data.FridayLast, "Fri"));
                s2.Points.Add(SetDataPoint(data.SaturdayLast, "Sat"));

                //s1.Points.Add(SetDataPoint(data.Week, "TOTAL"));
            }


            // Custom Data Points
            Series s1 = c.Series.Add("This Week");
            //s1.ChartType = SeriesChartType.Area;
            s1.ChartType = SeriesChartType.Column;
            s1.Color = Color.FromArgb(235, 137, 9);
            s1.BackSecondaryColor = Color.FromArgb(50, 235, 137, 9);
            s1.BackGradientStyle = GradientStyle.TopBottom;
            s1.BorderColor = Color.FromArgb(158, 100, 27);
            s1.BorderWidth = 1;
            s1.MarkerStyle = MarkerStyle.None;// MarkerStyle.Circle;
            s1.MarkerSize = 1;
            s1.MarkerColor = Color.White;
            s1.MarkerBorderWidth = 2;
            s1.MarkerBorderColor = Color.FromArgb(200, 200, 0);


            if (data != null)
            {
                s1.Points.Add(SetDataPoint(data.Sunday, "Sun"));
                s1.Points.Add(SetDataPoint(data.Monday, "Mon"));
                s1.Points.Add(SetDataPoint(data.Tuesday, "Tue"));
                s1.Points.Add(SetDataPoint(data.Wednesday, "Wed"));
                s1.Points.Add(SetDataPoint(data.Thursday, "Thr"));
                s1.Points.Add(SetDataPoint(data.Friday, "Fri"));
                s1.Points.Add(SetDataPoint(data.Saturday, "Sat"));

                //s1.Points.Add(SetDataPoint(data.Week, "TOTAL"));
            }



            ca.AxisX.IntervalType = DateTimeIntervalType.Auto;
            //ca.AxisX.IntervalType = DateTimeIntervalType.Days;
            //ca.AxisX.LabelStyle.Format = "d";


            // Render Chart            
            MemoryStream img = new MemoryStream();
            c.SaveImage(img);
            OutputContentType = "image/png";
            Stream = img;
        }

        private DataPoint SetDataPoint(decimal revenue, string day)
        {
            DataPoint p = new DataPoint();

            p.SetValueXY(day, revenue);
            return p;
        }

        public void RenderChart()
        {
            Response.Clear();
            Response.ContentType = OutputContentType;
            if (ETag != null) Response.AddHeader("ETag", ETag);
            Response.BinaryWrite(Stream.GetBuffer());
        }

    }
}