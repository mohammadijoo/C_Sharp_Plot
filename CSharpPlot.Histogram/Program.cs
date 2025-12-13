// histogram.cs
// C# console application for generating histogram plots using ScottPlot 5.
//
// NuGet:
//   dotnet add package ScottPlot
//
// Outputs:
//   PNG images saved into ./output
//
// Notes:
// - ScottPlot can render histograms using ScottPlot.Statistics.Histogram together with Plot.Add.Histogram().
// - Overlay comparisons are typically done by drawing multiple bar series using consistent binning.
// - If you want deterministic output placement (especially under Visual Studio), consider using:
//     string outDir = Path.Combine(AppContext.BaseDirectory, "output");

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using ScottPlot;

public class Program
{
    public static int Main()
    {
        Directory.CreateDirectory("output");

        // 1) Basic histogram from generated normal data (automatic bin selection)
        {
            double[] x1 = Randn(10000, mean: 0, stdDev: 1);

            int kAuto = AutoBinCount(x1);

            var plt = new Plot();

            double min = x1.Min();
            double max = x1.Max();

            var hist = ScottPlot.Statistics.Histogram.WithBinCount(count: kAuto, minValue: min, maxValue: max);
            hist.AddRange(x1);

            var hp = plt.Add.Histogram(hist);
            hp.BarWidthFraction = 0.9;

            plt.XLabel("Value");
            plt.YLabel("Frequency");
            plt.Title($"Histogram of standard normal data ({kAuto} bins)");

            plt.SavePng(Path.Combine("output", "histogram_1.png"), 1200, 800);
        }

        // 2) Compare multiple bin-count rules side-by-side (2x3 multiplot)
        {
            double[] x2 = Randn(10000, mean: 0, stdDev: 1);
            double min = x2.Min();
            double max = x2.Max();

            var mp = new ScottPlot.Multiplot();
            mp.AddPlots(6);
            mp.Layout = new ScottPlot.MultiplotLayouts.Grid(rows: 2, columns: 3);

            (string name, Func<double[], int> rule)[] rules =
            {
                ("Automatic", AutoBinCount),
                ("Scott's rule", ScottBinCount),
                ("Freedman-Diaconis", FdBinCount),
                ("Integers rule", IntegersBinCount),
                ("Sturges' rule", SturgesBinCount),
                ("Square root rule", SqrtBinCount),
            };

            for (int i = 0; i < rules.Length; i++)
            {
                Plot ax = mp.Subplots.GetPlot(i);

                int k = Math.Max(1, rules[i].rule(x2));

                var hist = ScottPlot.Statistics.Histogram.WithBinCount(count: k, minValue: min, maxValue: max);
                hist.AddRange(x2);

                var hp = ax.Add.Histogram(hist);
                hp.BarWidthFraction = 0.9;

                ax.XLabel("Value");
                ax.YLabel("Frequency");
                ax.Title($"{rules[i].name} (k={k})");
            }

            // Multiplot renders to a single raster image (PNG)
            mp.SavePng(Path.Combine("output", "histogram_2.png"), 1600, 900);
        }

        // 3) Generate multiple histogram images with different bin counts (useful for tuning)
        {
            double[] x3 = Randn(1000, mean: 0, stdDev: 1);
            double min = x3.Min();
            double max = x3.Max();

            int[] binCounts = { 10, 15, 20, 30, 50 };

            for (int i = 0; i < binCounts.Length; i++)
            {
                int k = binCounts[i];

                var plt = new Plot();

                var hist = ScottPlot.Statistics.Histogram.WithBinCount(count: k, minValue: min, maxValue: max);
                hist.AddRange(x3);

                var hp = plt.Add.Histogram(hist);
                hp.BarWidthFraction = 0.9;

                plt.XLabel("Value");
                plt.YLabel("Frequency");
                plt.Title($"{k} bins");

                plt.SavePng(Path.Combine("output", $"histogram_3_{k}bins.png"), 1200, 800);

                // Short delay to make rapid file generation easier to observe during execution
                Thread.Sleep(400);
            }
        }

        // 4) Histogram with custom bin edges and density-style normalization (count / binWidth)
        {
            double[] x4 = Randn(10000, mean: 0, stdDev: 1);

            double[] edges =
            {
                -10.0000, -2.0000, -1.7500, -1.5000, -1.2500,
                -1.0000, -0.7500, -0.5000, -0.2500,  0.0000,
                 0.2500,  0.5000,  0.7500,  1.0000,  1.2500,
                 1.5000,  1.7500,  2.0000, 10.0000
            };

            // Compute counts per custom bin, then normalize by bin width
            double[] counts = HistogramCountsWithEdges(x4, edges);
            double[] widths = edges.Zip(edges.Skip(1), (a, b) => b - a).ToArray();
            double[] density = counts.Zip(widths, (c, w) => c / w).ToArray();
            double[] centers = edges.Zip(edges.Skip(1), (a, b) => 0.5 * (a + b)).ToArray();

            var plt = new Plot();
            var bars = plt.Add.Bars(centers, density);

            // Set each bar width to match its bin width (slightly reduced for visual separation)
            for (int i = 0; i < bars.Bars.Count; i++)
            {
                bars.Bars[i].Size = widths[i] * 0.95;
                bars.Bars[i].LineWidth = 0;
            }

            plt.XLabel("Value");
            plt.YLabel("Count density");
            plt.Title("Histogram with custom bin edges");

            plt.SavePng(Path.Combine("output", "histogram_4.png"), 1600, 900);
        }

        // 5) Categorical histogram implemented as a bar chart (group counts by label)
        {
            string[] categories =
            {
                "no", "no",  "yes",       "yes",       "yes", "no",  "no",
                "no", "no",  "undecided", "undecided", "yes", "no",  "no",
                "no", "yes", "no",        "yes",       "no",  "yes", "no",
                "no", "no",  "yes",       "yes",       "yes", "yes"
            };

            var counts = categories
                .GroupBy(c => c)
                .OrderBy(g => g.Key)
                .ToArray();

            string[] labels = counts.Select(g => g.Key).ToArray();
            double[] values = counts.Select(g => (double)g.Count()).ToArray();
            double[] positions = Enumerable.Range(0, labels.Length).Select(i => (double)i).ToArray();

            var plt = new Plot();
            var barPlot = plt.Add.Bars(positions, values);

            // Adjust bar width (visual)
            foreach (var bar in barPlot.Bars)
                bar.Size = 0.5;

            plt.Axes.Bottom.SetTicks(positions, labels);
            plt.XLabel("Category");
            plt.YLabel("Count");
            plt.Title("Histogram of categorical responses");

            plt.SavePng(Path.Combine("output", "histogram_5.png"), 1600, 900);
        }

        // 6) Overlay two distributions using aligned bins and probability normalization
        {
            var plt = new Plot();

            double[] x5 = Randn(2000, mean: 0, stdDev: 1);
            double[] y5 = Randn(5000, mean: 1, stdDev: 1);

            // Fixed bin size ensures bars align for visual comparison
            double binSize = 0.25;
            double globalMin = Math.Min(x5.Min(), y5.Min());
            double globalMax = Math.Max(x5.Max(), y5.Max());

            var h1 = ScottPlot.Statistics.Histogram.WithBinSize(binSize, firstBin: globalMin, lastBin: globalMax);
            h1.AddRange(x5);

            var h2 = ScottPlot.Statistics.Histogram.WithBinSize(binSize, firstBin: globalMin, lastBin: globalMax);
            h2.AddRange(y5);

            // Probability normalization allows comparing shapes even with different sample sizes
            var b1 = plt.Add.Bars(h1.Bins, h1.GetProbability());
            foreach (var bar in b1.Bars)
            {
                bar.Size = h1.FirstBinSize;
                bar.LineWidth = 0;
                bar.FillColor = Colors.Blue.WithAlpha(.2);
            }

            var b2 = plt.Add.Bars(h2.Bins, h2.GetProbability());
            foreach (var bar in b2.Bars)
            {
                bar.Size = h2.FirstBinSize;
                bar.LineWidth = 0;
                bar.FillColor = Colors.Red.WithAlpha(.2);
            }

            plt.XLabel("Value");
            plt.YLabel("Probability (%)");
            plt.Title("Overlaid normalized histograms");

            plt.SavePng(Path.Combine("output", "histogram_6.png"), 1200, 800);
        }

        // 7) PDF-normalized histogram with a theoretical normal PDF overlay
        {
            var plt = new Plot();

            double mu = 5.0;
            double sigma = 2.0;
            double[] x6 = Randn(5000, mean: mu, stdDev: sigma);

            double binSize = 0.25;
            var hist = ScottPlot.Statistics.Histogram.WithBinSize(binSize, x6);

            // PDF normalization: density = counts / (N * binWidth)
            double N = x6.Length;
            double[] pdfHeights = hist.Counts.Select(c => c / (N * hist.FirstBinSize)).ToArray();

            var bars = plt.Add.Bars(hist.Bins, pdfHeights);
            foreach (var bar in bars.Bars)
            {
                bar.Size = hist.FirstBinSize;
                bar.LineWidth = 0;
                bar.FillColor = Colors.Gray.WithAlpha(.35);
            }

            // Analytical normal PDF function
            double NormalPdf(double x) =>
                Math.Exp(-Math.Pow(x - mu, 2.0) / (2.0 * sigma * sigma)) / (sigma * Math.Sqrt(2.0 * Math.PI));

            var f = plt.Add.Function(NormalPdf);
            f.MinX = mu - 5 * sigma;
            f.MaxX = mu + 5 * sigma;
            f.LineWidth = 2;

            plt.XLabel("Value");
            plt.YLabel("Probability density");
            plt.Title("Histogram with theoretical normal PDF");

            plt.SavePng(Path.Combine("output", "histogram_7.png"), 1200, 800);
        }

        // 8) Real-world CSV -> histogram (Diamonds dataset)
        {
            // If you have the CSV locally, you can load it like this:
            // string csvText = File.ReadAllText(@"C:\path\to\diamonds.csv");

            string url = "https://raw.githubusercontent.com/mohammadijoo/Datasets/refs/heads/main/diamonds.csv";
            string csvText = DownloadString(url);

            var rows = ReadCsv(csvText);
            int idxPrice = FindColumn(rows.Header, "price");

            int N = Math.Min(rows.Data.Count, 30000);
            var prices = new List<double>(capacity: N);

            for (int i = 0; i < N; i++)
            {
                string[] r = rows.Data[i];
                if (TryParseDouble(r[idxPrice], out double price))
                    prices.Add(price);
            }

            double[] data = prices.ToArray();

            int kAuto = AutoBinCount(data);
            double min = data.Min();
            double max = data.Max();

            var plt = new Plot();
            var hist = ScottPlot.Statistics.Histogram.WithBinCount(count: kAuto, minValue: min, maxValue: max);
            hist.AddRange(data);

            var hp = plt.Add.Histogram(hist);
            hp.BarWidthFraction = 0.9;

            plt.Title($"Diamonds CSV: Price Histogram (k={kAuto}, N={data.Length})");
            plt.XLabel("Price");
            plt.YLabel("Frequency");

            plt.SavePng(Path.Combine("output", "histogram_8_csv.png"), 1200, 800);
        }

        Console.WriteLine("Done. See ./output for generated images.");
        return 0;
    }

    // -----------------------
    // Random + binning helpers
    // -----------------------

    private static readonly Random Rng = new(0);

    // Generate normally distributed values using the Box–Muller transform.
    private static double[] Randn(int n, double mean, double stdDev)
    {
        double[] data = new double[n];
        int i = 0;

        while (i < n)
        {
            double u1 = 1.0 - Rng.NextDouble();
            double u2 = 1.0 - Rng.NextDouble();
            double r = Math.Sqrt(-2.0 * Math.Log(u1));
            double theta = 2.0 * Math.PI * u2;

            double z0 = r * Math.Cos(theta);
            double z1 = r * Math.Sin(theta);

            data[i++] = mean + stdDev * z0;
            if (i < n)
                data[i++] = mean + stdDev * z1;
        }

        return data;
    }

    // Choose a robust bin count by combining multiple heuristics and clamping the result.
    private static int AutoBinCount(double[] data)
    {
        int fd = FdBinCount(data);
        int st = SturgesBinCount(data);
        int k = Math.Max(fd, st);
        return Clamp(k, 1, 500);
    }

    // Scott's rule bin count (based on standard deviation and sample size).
    private static int ScottBinCount(double[] data)
    {
        int n = data.Length;
        if (n < 2) return 1;

        double sigma = StdDev(data);
        double h = 3.5 * sigma / Math.Pow(n, 1.0 / 3.0);
        if (h <= 0) return 1;

        double range = data.Max() - data.Min();
        return Clamp((int)Math.Ceiling(range / h), 1, 500);
    }

    // Freedman–Diaconis rule bin count (based on IQR and sample size).
    private static int FdBinCount(double[] data)
    {
        int n = data.Length;
        if (n < 2) return 1;

        double q1 = Quantile(data, 0.25);
        double q3 = Quantile(data, 0.75);
        double iqr = q3 - q1;

        if (iqr <= 0) return SturgesBinCount(data);

        double h = 2.0 * iqr / Math.Pow(n, 1.0 / 3.0);
        if (h <= 0) return 1;

        double range = data.Max() - data.Min();
        return Clamp((int)Math.Ceiling(range / h), 1, 500);
    }

    // Sturges' rule bin count (simple log-based heuristic).
    private static int SturgesBinCount(double[] data)
    {
        int n = Math.Max(1, data.Length);
        return Clamp((int)Math.Ceiling(Math.Log(n, 2) + 1), 1, 500);
    }

    // Square-root rule bin count (fast heuristic).
    private static int SqrtBinCount(double[] data)
    {
        int n = Math.Max(1, data.Length);
        return Clamp((int)Math.Ceiling(Math.Sqrt(n)), 1, 500);
    }

    // Integer binning: if values are integer-like, use one bin per integer value; otherwise fall back.
    private static int IntegersBinCount(double[] data)
    {
        const double tol = 1e-9;
        bool integerLike = data.All(v => Math.Abs(v - Math.Round(v)) < tol);
        if (!integerLike) return SturgesBinCount(data);

        int min = (int)Math.Floor(data.Min());
        int max = (int)Math.Ceiling(data.Max());
        return Clamp(max - min + 1, 1, 500);
    }

    private static double StdDev(double[] data)
    {
        double mean = data.Average();
        double var = data.Select(v => (v - mean) * (v - mean)).Average();
        return Math.Sqrt(var);
    }

    // Quantile with linear interpolation (p in [0, 1]).
    private static double Quantile(double[] data, double p)
    {
        double[] sorted = data.OrderBy(v => v).ToArray();
        if (sorted.Length == 1) return sorted[0];

        double pos = (sorted.Length - 1) * p;
        int lo = (int)Math.Floor(pos);
        int hi = (int)Math.Ceiling(pos);
        if (lo == hi) return sorted[lo];

        double frac = pos - lo;
        return sorted[lo] * (1 - frac) + sorted[hi] * frac;
    }

    private static int Clamp(int v, int lo, int hi) => Math.Min(hi, Math.Max(lo, v));

    // Count samples into user-defined bin edges.
    private static double[] HistogramCountsWithEdges(double[] data, double[] edges)
    {
        int bins = edges.Length - 1;
        double[] counts = new double[bins];

        foreach (double x in data)
        {
            // Include the rightmost edge in the last bin
            if (x < edges[0] || x > edges[^1]) continue;

            int idx = Array.BinarySearch(edges, x);
            if (idx >= 0)
            {
                // If x lands exactly on an edge, assign it to the bin on the left (except for the first edge)
                idx = Math.Max(1, idx) - 1;
            }
            else
            {
                // Convert "insertion index" into the containing bin index
                idx = ~idx - 1;
            }

            if (idx >= 0 && idx < bins)
                counts[idx]++;
        }

        return counts;
    }

    // -----------------------
    // CSV helpers
    // -----------------------

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
