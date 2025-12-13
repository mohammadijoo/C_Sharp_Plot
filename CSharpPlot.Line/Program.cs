// line.cs
// C# console application for generating line plots using ScottPlot 5.
//
// NuGet:
//   dotnet add package ScottPlot
//
// Outputs:
//   PNG images saved into ./output
//
// Notes:
// - To overlay multiple series on the same axes, add multiple plottables to the same Plot instance.
// - For subplot-style layouts, ScottPlot.Multiplot can render multiple Plot objects into one image.
// - If you want deterministic output placement (especially under Visual Studio), consider using:
//     string outDir = Path.Combine(AppContext.BaseDirectory, "output");

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using ScottPlot;

public class Program
{
    public static int Main()
    {
        Directory.CreateDirectory("output");

        // 1) Multiple line plots on the same axes (different styles and markers)
        {
            double[] x = Linspace(0, 2 * Math.PI, 100);
            double[] y = x.Select(Math.Sin).ToArray();

            var plt = new Plot();

            // Series 1: solid line with filled-circle markers
            var s1 = plt.Add.Scatter(x, y);
            s1.LineWidth = 2;
            s1.MarkerSize = 7;
            s1.MarkerShape = MarkerShape.FilledCircle;

            // Series 2: dashed line, red color, "X" markers
            var s2 = plt.Add.Scatter(x, y.Select(v => -v).ToArray());
            s2.LineWidth = 2;
            s2.LinePattern = LinePattern.DenselyDashed;
            s2.Color = Colors.Red;
            s2.MarkerSize = 7;
            s2.MarkerShape = MarkerShape.Eks;

            // Series 3: custom dash-dot pattern, green color, square markers
            var dashDot = new LinePattern(new float[] { 8, 4, 2, 4 }, 0, "DashDot");
            var s3 = plt.Add.Scatter(x, x.Select(v => v / Math.PI - 1.0).ToArray());
            s3.LineWidth = 2;
            s3.LinePattern = dashDot;
            s3.Color = Colors.Green;
            s3.MarkerSize = 7;
            s3.MarkerShape = MarkerShape.FilledSquare;

            // Series 4: Y-only values using index-based X values (0..N-1)
            double[] y4 = { 1.0, 0.7, 0.4, 0.0, -0.4, -0.7, -1.0 };
            double[] x4 = Enumerable.Range(0, y4.Length).Select(i => (double)i).ToArray();
            var s4 = plt.Add.Scatter(x4, y4);
            s4.LineWidth = 2;
            s4.Color = Colors.Black;
            s4.MarkerSize = 0;

            plt.XLabel("x");
            plt.YLabel("y");
            plt.Title("Multiple line plots");

            plt.SavePng(Path.Combine("output", "line_1.png"), 1200, 800);
        }

        // 2) Plot multiple vectors as separate series (with legend)
        {
            // Multiple sequences plotted as independent series
            var Y = new List<double[]>
            {
                new double[] {16, 5, 9, 4},
                new double[] {2, 11, 7, 14},
                new double[] {3, 10, 6, 15},
                new double[] {13, 8, 12, 1}
            };

            var plt = new Plot();

            for (int i = 0; i < Y.Count; i++)
            {
                double[] ys = Y[i];
                double[] xs = Enumerable.Range(0, ys.Length).Select(v => (double)v).ToArray();

                var sp = plt.Add.Scatter(xs, ys);
                sp.MarkerSize = 7;
                sp.LineWidth = 2;
                sp.LegendText = $"Series {i + 1}";
            }

            plt.ShowLegend();
            plt.XLabel("x");
            plt.YLabel("y");
            plt.Title("Multiple line plots (set of vectors)");

            plt.SavePng(Path.Combine("output", "line_2.png"), 1200, 800);
        }

        // 3) Trigonometric line plots (line styles distinguish series)
        {
            var plt = new Plot();

            double[] x = Linspace(0, 2 * Math.PI, 200);
            double[] y1 = x.Select(Math.Sin).ToArray();
            double[] y2 = x.Select(v => Math.Sin(v - 0.25)).ToArray();
            double[] y3 = x.Select(v => Math.Sin(v - 0.5)).ToArray();

            var a = plt.Add.Scatter(x, y1);
            a.MarkerSize = 0;
            a.LineWidth = 2;

            var b = plt.Add.Scatter(x, y2);
            b.MarkerSize = 0;
            b.LineWidth = 2;
            b.LinePattern = LinePattern.DenselyDashed;

            var c = plt.Add.Scatter(x, y3);
            c.MarkerSize = 0;
            c.LineWidth = 2;
            c.LinePattern = LinePattern.Dotted;

            plt.XLabel("x");
            plt.YLabel("y");
            plt.Title("Sin() function line plots");

            plt.SavePng(Path.Combine("output", "line_3.png"), 1200, 800);
        }

        // 4) Trigonometric line plots with markers (style + marker combinations)
        {
            var plt = new Plot();

            double[] x = Linspace(0, 2 * Math.PI, 200);
            double[] y1 = x.Select(Math.Sin).ToArray();
            double[] y2 = x.Select(v => Math.Sin(v - 0.25)).ToArray();
            double[] y3 = x.Select(v => Math.Sin(v - 0.5)).ToArray();

            var a = plt.Add.Scatter(x, y1);
            a.Color = Colors.Green;
            a.LineWidth = 2;
            a.MarkerSize = 7;
            a.MarkerShape = MarkerShape.FilledCircle;

            var b = plt.Add.Scatter(x, y2);
            b.Color = Colors.Blue;
            b.LineWidth = 2;
            b.LinePattern = LinePattern.DenselyDashed;
            b.MarkerSize = 7;
            b.MarkerShape = MarkerShape.OpenCircle;

            var c = plt.Add.Scatter(x, y3);
            c.Color = Colors.Cyan;
            c.LineWidth = 0; // render markers only
            c.MarkerSize = 9;
            c.MarkerShape = MarkerShape.Asterisk;

            plt.XLabel("x");
            plt.YLabel("y");
            plt.Title("Sin() function line plots with markers");

            plt.SavePng(Path.Combine("output", "line_4.png"), 1200, 800);
        }

        // 5) Two plots in a 2x1 grid using ScottPlot.Multiplot
        {
            double[] x = Linspace(0, 3, 200);
            double[] y1 = x.Select(v => Math.Sin(5 * v)).ToArray();
            double[] y2 = x.Select(v => Math.Sin(15 * v)).ToArray();

            var mp = new ScottPlot.Multiplot();
            mp.AddPlots(2);
            mp.Layout = new ScottPlot.MultiplotLayouts.Grid(rows: 2, columns: 1);

            Plot ax1 = mp.Subplots.GetPlot(0);
            var p1 = ax1.Add.Scatter(x, y1);
            p1.MarkerSize = 0;
            p1.LineWidth = 2;
            ax1.Title("Top Plot");
            ax1.YLabel("sin(5x)");

            Plot ax2 = mp.Subplots.GetPlot(1);
            var p2 = ax2.Add.Scatter(x, y2);
            p2.MarkerSize = 0;
            p2.LineWidth = 2;
            ax2.Title("Bottom Plot");
            ax2.YLabel("sin(15x)");

            mp.SavePng(Path.Combine("output", "line_5.png"), 1200, 800);
        }

        // 6) Multiplot: 3x2 grid (6 subplots total) exported as a single PNG
        {
            var mp = new ScottPlot.Multiplot();
            mp.AddPlots(6);
            mp.Layout = new ScottPlot.MultiplotLayouts.Grid(rows: 3, columns: 2);

            // Subplot 0: sin(x) with emphasized markers at regular indices
            {
                Plot ax = mp.Subplots.GetPlot(0);

                double[] x_a = Linspace(0, 10, 100);
                double[] y_a = x_a.Select(Math.Sin).ToArray();

                var line = ax.Add.Scatter(x_a, y_a);
                line.LineWidth = 2;
                line.MarkerSize = 0;

                int[] markerIdx = Enumerable.Range(0, x_a.Length).Where(i => i % 5 == 0).ToArray();
                double[] mx = markerIdx.Select(i => x_a[i]).ToArray();
                double[] my = markerIdx.Select(i => y_a[i]).ToArray();

                var pts = ax.Add.ScatterPoints(mx, my);
                pts.MarkerSize = 7;
                pts.MarkerShape = MarkerShape.FilledCircle;

                ax.Title("sin(x) with marker indices");
                ax.XLabel("x");
                ax.YLabel("y");
            }

            // Subplot 1: composite function with custom marker fill
            {
                Plot ax = mp.Subplots.GetPlot(1);

                double[] x_b = Linspace(-Math.PI, +Math.PI, 20);
                double[] y_b = x_b.Select(v => Math.Tan(Math.Sin(v)) - Math.Sin(Math.Tan(v))).ToArray();

                var sp = ax.Add.Scatter(x_b, y_b);
                sp.LineWidth = 2;
                sp.LinePattern = LinePattern.DenselyDashed;
                sp.Color = Colors.Green;
                sp.MarkerSize = 10;
                sp.MarkerShape = MarkerShape.FilledSquare;
                sp.MarkerFillColor = Colors.Gray.WithAlpha(.5);

                ax.Title("tan(sin(x)) - sin(tan(x))");
                ax.XLabel("x");
                ax.YLabel("y");
            }

            // Subplot 2: cos(5x) with labeled axes
            {
                Plot ax = mp.Subplots.GetPlot(2);

                double[] x_c = Linspace(0, 10, 150);
                double[] y_c = x_c.Select(v => Math.Cos(5 * v)).ToArray();

                var sp = ax.Add.Scatter(x_c, y_c);
                sp.LineWidth = 2;
                sp.MarkerSize = 0;
                sp.Color = Colors.Cyan;

                ax.Title("2-D Line Plot");
                ax.XLabel("x");
                ax.YLabel("cos(5x)");
            }

            // Subplot 3: custom time tick labels
            {
                Plot ax = mp.Subplots.GetPlot(3);

                double[] x_d = Linspace(0, 180, 7);
                double[] y_d = { 0.8, 0.9, 0.1, 0.9, 0.6, 0.1, 0.3 };

                var sp = ax.Add.Scatter(x_d, y_d);
                sp.LineWidth = 2;
                sp.MarkerSize = 7;
                sp.MarkerShape = MarkerShape.FilledCircle;

                ax.Title("Time Plot");
                ax.XLabel("Time");
                ax.Axes.SetLimitsY(bottom: 0, top: 1);

                double[] tickPositions = { 0, 30, 60, 90, 120, 150, 180 };
                string[] tickLabels = { "00:00s", "30:00", "01:00", "01:30", "02:00", "02:30", "03:00" };
                ax.Axes.Bottom.SetTicks(tickPositions, tickLabels);
            }

            // Subplot 4: sin(5x)
            {
                Plot ax = mp.Subplots.GetPlot(4);

                double[] x_e = Linspace(0, 3, 200);
                double[] y_e = x_e.Select(v => Math.Sin(5 * v)).ToArray();

                var sp = ax.Add.Scatter(x_e, y_e);
                sp.LineWidth = 2;
                sp.MarkerSize = 0;

                ax.Title("sin(5x)");
                ax.XLabel("x");
                ax.YLabel("y");
            }

            // Subplot 5: parametric circle with square units
            {
                Plot ax = mp.Subplots.GetPlot(5);

                double r = 2.0;
                double xc = 4.0;
                double yc = 3.0;

                double[] theta = Linspace(0, 2 * Math.PI, 200);
                double[] x_f = theta.Select(t => r * Math.Cos(t) + xc).ToArray();
                double[] y_f = theta.Select(t => r * Math.Sin(t) + yc).ToArray();

                var sp = ax.Add.Scatter(x_f, y_f);
                sp.LineWidth = 2;
                sp.MarkerSize = 0;

                // Enforce square axis units so circles render proportionally
                ax.Axes.SquareUnits();

                ax.Title("Circle");
                ax.XLabel("x");
                ax.YLabel("y");
            }

            mp.SavePng(Path.Combine("output", "line_6.png"), 1600, 900);
        }

        // 7) Real-world CSV -> plot (Diamonds dataset)
        {
            // If you have the CSV locally, you can load it like this:
            // string csvText = File.ReadAllText(@"C:\path\to\diamonds.csv");

            string url = "https://raw.githubusercontent.com/mohammadijoo/Datasets/refs/heads/main/diamonds.csv";
            string csvText = DownloadString(url);

            // Plot: average price vs carat (carat binned to 0.1)
            var rows = ReadCsv(csvText);

            int idxCarat = FindColumn(rows.Header, "carat");
            int idxPrice = FindColumn(rows.Header, "price");

            // Sample the first N rows to keep the plot responsive and uncluttered
            int N = Math.Min(rows.Data.Count, 15000);

            var groups = new Dictionary<double, List<double>>();
            for (int i = 0; i < N; i++)
            {
                string[] r = rows.Data[i];
                if (!TryParseDouble(r[idxCarat], out double carat)) continue;
                if (!TryParseDouble(r[idxPrice], out double price)) continue;

                double bin = Math.Round(carat, 1); // 0.1-carat bins
                if (!groups.TryGetValue(bin, out var list))
                {
                    list = new List<double>();
                    groups[bin] = list;
                }
                list.Add(price);
            }

            double[] xs = groups.Keys.OrderBy(v => v).ToArray();
            double[] ys = xs.Select(x => groups[x].Average()).ToArray();

            var plt = new Plot();
            var sp = plt.Add.Scatter(xs, ys);
            sp.LineWidth = 2;
            sp.MarkerSize = 7;
            sp.MarkerShape = MarkerShape.FilledCircle;

            plt.Title("Diamonds CSV: Average Price vs Carat (0.1-carat bins)");
            plt.XLabel("Carat");
            plt.YLabel("Average Price");

            plt.SavePng(Path.Combine("output", "line_7_csv.png"), 1200, 800);
        }

        Console.WriteLine("Done. See ./output for generated images.");
        return 0;
    }

    // -----------------------
    // Helpers
    // -----------------------

    // Create an array of evenly spaced values between start and end (inclusive).
    private static double[] Linspace(double start, double end, int count)
    {
        if (count < 2) return new[] { start };
        double step = (end - start) / (count - 1);
        double[] xs = new double[count];
        for (int i = 0; i < count; i++)
            xs[i] = start + step * i;
        return xs;
    }

    // Download a text resource (e.g., CSV) from a URL.
    private static string DownloadString(string url)
    {
        using var http = new HttpClient();
        return http.GetStringAsync(url).GetAwaiter().GetResult();
    }

    // Parse floating-point values safely using invariant culture (consistent decimal parsing).
    private static bool TryParseDouble(string s, out double value)
    {
        s = s.Trim().Trim('"');
        return double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
    }

    // Locate a column index by name (case-insensitive); throws if not found.
    private static int FindColumn(string[] header, string name)
    {
        int idx = Array.FindIndex(header, h => h.Trim().Trim('"')
            .Equals(name, StringComparison.OrdinalIgnoreCase));
        if (idx < 0)
            throw new InvalidOperationException($"CSV column '{name}' not found. Available: {string.Join(", ", header)}");
        return idx;
    }

    // Read CSV text into a header array and a list of row arrays.
    private static (string[] Header, List<string[]> Data) ReadCsv(string csvText)
    {
        using var sr = new StringReader(csvText);

        string? headerLine = sr.ReadLine();
        if (headerLine is null) throw new InvalidOperationException("CSV is empty.");

        string[] header = SplitCsvLine(headerLine).ToArray();
        var data = new List<string[]>();

        string? line;
        while ((line = sr.ReadLine()) is not null)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var fields = SplitCsvLine(line).ToArray();
            if (fields.Length != header.Length) continue; // skip malformed lines
            data.Add(fields);
        }

        return (header, data);
    }

    // Minimal CSV splitter that respects quoted fields.
    private static IEnumerable<string> SplitCsvLine(string line)
    {
        bool inQuotes = false;
        var token = new System.Text.StringBuilder();

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                inQuotes = !inQuotes;
                token.Append(c);
                continue;
            }

            if (c == ',' && !inQuotes)
            {
                yield return token.ToString();
                token.Clear();
            }
            else
            {
                token.Append(c);
            }
        }

        yield return token.ToString();
    }
}
