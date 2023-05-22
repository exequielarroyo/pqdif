using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using Data.Access;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;
using LiveChartsCore.Kernel.Sketches;

namespace WinUI.ViewModels;
public class PreviewViewModel
{
    public ISeries[] Series
    {
        get;
        set;
    }

    readonly SQLiteContext db;

    public PreviewViewModel()
    {
        db = new SQLiteContext();

        var xSeries = db.Series.ToList()[0].Values.Trim('[', ']').Split(',');
        var ySeries = db.Series.ToList()[1].Values.Trim('[', ']').Split(',');
        var xSeries2 = db.Series.ToList()[2].Values.Trim('[', ']').Split(',');
        var ySeries2 = db.Series.ToList()[3].Values.Trim('[', ']').Split(',');

        List<DateTime> x = new();
        List<double> y = new();
        List<DateTime> x2 = new();
        List<double> y2 = new();

        for (var i = 0; i < xSeries.Length; i++)
        {
            x.Add(new DateTime(2023, 10, 9).AddSeconds(double.Parse(xSeries[i].Trim())));
            //x.Add(double.Parse(xSeries[i].Trim()));
            y.Add(double.Parse(ySeries[i].Trim()));
            //x2.Add(double.Parse(xSeries2[i].Trim()));
            x2.Add(new DateTime(2023, 10, 9).AddSeconds(double.Parse(xSeries2[i].Trim())));
            y2.Add(double.Parse(ySeries2[i].Trim()));
        }

        var lineSeries = new LineSeries<DateTimePoint>();
        var lineSeries2 = new LineSeries<DateTimePoint>();
        var observablePoints = new List<DateTimePoint>();
        var observablePoints2 = new List<DateTimePoint>();

        for (var i = 0; i < x.Count; i++)
        {
            observablePoints.Add(new DateTimePoint(x[i], y[i]));
            observablePoints2.Add(new DateTimePoint(x2[i], y2[i]));
        }

        lineSeries.Values = observablePoints;
        lineSeries.Fill = new SolidColorPaint(SKColor.FromHsl(207, 90, 54, 100));
        lineSeries.Name = "V Peak A";
        lineSeries.LineSmoothness = 0.2;
        lineSeries.GeometrySize = 4;
        lineSeries.Stroke = new SolidColorPaint(SKColor.FromHsl(207, 90, 54, 100)) { StrokeThickness = 2 };
        lineSeries2.Values = observablePoints2;
        lineSeries2.Fill = new SolidColorPaint(SKColor.FromHsl(4, 90, 58, 100)); ;
        lineSeries2.Name = "V Peak B";
        lineSeries2.LineSmoothness = 0.2;
        lineSeries2.GeometrySize = 4;
        lineSeries2.Stroke = new SolidColorPaint(SKColor.FromHsl(4, 90, 58, 100)) { StrokeThickness = 2 };

        this.Series = new ISeries[2];
        this.Series[0] = lineSeries;
        this.Series[1] = lineSeries2;
    }

    public LabelVisual Title
    {
        get; set;
    } =
        new LabelVisual
        {
            Text = "Service Entrance 7100",
            TextSize = 25,
            Padding = new LiveChartsCore.Drawing.Padding(15),
            Paint = new SolidColorPaint(SKColors.White)
        };

    public ICartesianAxis[] cartesianAxes
    {
        get; set;
    } = {new Axis
    {
        Labeler = value => new DateTime((long)value).ToString("ddd h:mm tt"),
        LabelsRotation = 80,

        // when using a date time type, let the library know your unit 
        UnitWidth = TimeSpan.FromDays(1).Seconds,

        // if the difference between our points is in hours then we would:
        // UnitWidth = TimeSpan.FromHours(1).Ticks,

        // since all the months and years have a different number of days
        // we can use the average, it would not cause any visible error in the user interface
        // Months: TimeSpan.FromDays(30.4375).Ticks
        // Years: TimeSpan.FromDays(365.25).Ticks

        // The MinStep property forces the separator to be greater than 1 day.
        MinStep = TimeSpan.FromDays(1).Seconds
    } };
}
