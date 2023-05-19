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

namespace WinUI.ViewModels;
public class PreviewViewModel
{
    public ISeries[] Series
    {
        get;
        set;
    }

    SQLiteContext db;

    public PreviewViewModel()
    {
        db = new SQLiteContext();

        var xSeries = db.Series.ToList()[0].Values.Trim('[', ']').Split(',');
        var ySeries = db.Series.ToList()[1].Values.Trim('[', ']').Split(',');
        var xSeries2 = db.Series.ToList()[2].Values.Trim('[', ']').Split(',');
        var ySeries2 = db.Series.ToList()[3].Values.Trim('[', ']').Split(',');

        List<double> x = new();
        List<double> y = new();
        List<double> x2 = new();
        List<double> y2 = new();

        for (var i = 0; i < xSeries.Length; i++)
        {
            x.Add(double.Parse(xSeries[i].Trim()));
            y.Add(double.Parse(ySeries[i].Trim()));
            x2.Add(double.Parse(xSeries2[i].Trim()));
            y2.Add(double.Parse(ySeries2[i].Trim()));
        }

        var lineSeries = new LineSeries<ObservablePoint>();
        var lineSeries2 = new LineSeries<ObservablePoint>();
        var observablePoints = new List<ObservablePoint>();
        var observablePoints2 = new List<ObservablePoint>();

        for (var i = 0; i < x.Count; i++)
        {
            observablePoints.Add(new ObservablePoint(x[i], y[i]));
            observablePoints2.Add(new ObservablePoint(x2[i], y2[i]));
        }

        lineSeries.Values = observablePoints;
        lineSeries.Fill = new SolidColorPaint(SKColor.FromHsl(207, 90, 54, 100));
        lineSeries.Name = "V Peak A";
        lineSeries.LineSmoothness = 0.2;
        lineSeries2.Values = observablePoints2;
        lineSeries2.Fill = new SolidColorPaint(SKColor.FromHsl(4, 90, 58, 100)); ;
        lineSeries2.Name = "V Peak B";
        lineSeries2.LineSmoothness = 0.2;

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
}
