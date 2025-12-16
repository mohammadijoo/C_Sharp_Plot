<div style="font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif; line-height: 1.6;">

  <h1 align="center" style="margin-bottom: 0.2em;">C# Plotting Playground</h1>

  <p style="font-size: 0.95rem; max-width: 820px; margin: 0 auto;">
    A minimal, extensible C# project showcasing scientific plotting with
    <strong>ScottPlot</strong>, starting with <strong>line plots</strong> and
    <strong>histograms</strong>. The repository is structured to make it easy to add
    additional plot types (e.g., pie charts, bar charts, scatter plots, heatmaps) over time.
  </p>

  <p align="center" style="font-size: 1rem; color: #666; margin-top: 0.5em;">
    Built with .NET (Console Apps), NuGet, and Visual Studio 2022.
  </p>

</div>

<hr />

<!-- ========================================================= -->
<!-- Table of Contents                                        -->
<!-- ========================================================= -->

<ul style="list-style: none; padding-left: 0; font-size: 0.95rem;">
  <li> <a href="#about-this-repository">About this repository</a></li>
  <li> <a href="#library-scottplot">Library: ScottPlot</a></li>
  <li> <a href="#prerequisites">Prerequisites</a></li>
  <li> <a href="#setup-and-run-visual-studio-2022">Setup and run (Visual Studio 2022)</a></li>
  <li> <a href="#setup-and-run-dotnet-cli">Setup and run (.NET CLI)</a></li>
  <li> <a href="#project-structure">Project structure</a></li>
  <li> <a href="#output-files-and-working-directory">Output files and working directory</a></li>
  <li> <a href="#line-plots-module">Line plots module</a></li>
  <li> <a href="#histogram-plots-module">Histogram plots module</a></li>
  <li> <a href="#implementation-tutorial-video">Implementation tutorial video</a></li>
</ul>

<hr />

<!-- ========================================================= -->
<!-- About                                                    -->
<!-- ========================================================= -->

<h2 id="about-this-repository">About this repository</h2>

<p>
  This repository is a focused playground for plotting in <strong>C#</strong> using
  <strong>ScottPlot</strong>. Each plot category is implemented as a standalone, runnable
  <strong>Console App</strong> project to keep examples independent and easy to expand.
</p>

<p style="margin-bottom: 0.3rem;"><strong>Current examples:</strong></p>
<ul>
  <li><code>CSharpPlot.Line</code> — line plots, multi-series overlays, subplots (multiplot layouts), and a CSV-based plot</li>
  <li><code>CSharpPlot.Histogram</code> — histogram binning/normalization patterns, overlays, PDF comparison, and a CSV-based histogram</li>
</ul>

<p style="margin-top: 0.7rem;">
  New plot categories can be added later by creating additional Console App projects
  (e.g., <code>CSharpPlot.Pie</code>, <code>CSharpPlot.Scatter</code>) and adding modular
  documentation sections similar to the ones below.
</p>

<hr />

<!-- ========================================================= -->
<!-- ScottPlot                                                -->
<!-- ========================================================= -->

<h2 id="library-scottplot">Library: ScottPlot</h2>

<h3 style="margin-bottom: 0.3rem;">What is ScottPlot?</h3>

<p>
  ScottPlot is a free and open-source plotting library for .NET. It supports generating
  publication-quality static images and interactive plots across common .NET application types.
  This repository uses ScottPlot to create plots and export them as PNG images from console apps.
</p>

<h3 style="margin-bottom: 0.3rem;">Installation</h3>

<p>
  ScottPlot is added via NuGet to each Console App project:
</p>

<pre><code>dotnet add package ScottPlot</code></pre>

<p style="font-size: 0.92rem; color: #555;">
  In Visual Studio, this is done using the NuGet Package Manager UI (steps shown below).
</p>

<hr />

<!-- ========================================================= -->
<!-- Prerequisites                                            -->
<!-- ========================================================= -->

<h2 id="prerequisites">Prerequisites</h2>

<ul>
  <li><strong>Visual Studio 2022</strong> with the <strong>Desktop development with .NET</strong> workload</li>
  <li><strong>.NET SDK</strong> (recommended: .NET 6+)</li>
  <li>Internet access for CSV URL examples (optional; local CSV loading is supported too)</li>
</ul>

<hr />

<!-- ========================================================= -->
<!-- Visual Studio Setup                                      -->
<!-- ========================================================= -->

<h2 id="setup-and-run-visual-studio-2022">Setup and run (Visual Studio 2022)</h2>

<p>
  This repository follows a clean Visual Studio workflow: a single solution with multiple independent
  Console App projects. Each project has its own <code>Program.cs</code> entry point and runs independently.
</p>

<h3 style="margin-bottom: 0.3rem;">Step-by-step: Create a Blank Solution + Two Console Apps</h3>

<ol>
  <li>
    <strong>Create the solution</strong>
    <ul>
      <li>Open Visual Studio 2022</li>
      <li><em>File</em> → <em>New</em> → <em>Project</em></li>
      <li>Choose <strong>Blank Solution</strong></li>
      <li>Name it something like <code>CSharpPlot</code></li>
    </ul>
  </li>

  <li>
    <strong>Add the line plotting project</strong>
    <ul>
      <li>Right-click the Solution → <em>Add</em> → <em>New Project...</em></li>
      <li>Select <strong>Console App (.NET)</strong></li>
      <li>Name the project: <code>CSharpPlot.Line</code></li>
      <li>Install ScottPlot:
        <ul>
          <li>Right-click <code>CSharpPlot.Line</code> → <em>Manage NuGet Packages...</em></li>
          <li><em>Browse</em> → search <code>ScottPlot</code> → <em>Install</em></li>
        </ul>
      </li>
      <li>Replace the content of <code>Program.cs</code> with the code from your <code>line</code> module file</li>
    </ul>
  </li>

  <li>
    <strong>Add the histogram plotting project</strong>
    <ul>
      <li>Right-click the Solution → <em>Add</em> → <em>New Project...</em></li>
      <li>Select <strong>Console App (.NET)</strong></li>
      <li>Name the project: <code>CSharpPlot.Histogram</code></li>
      <li>Install ScottPlot (same steps as above)</li>
      <li>Replace the content of <code>Program.cs</code> with the code from your <code>histogram</code> module file</li>
    </ul>
  </li>

  <li>
    <strong>Run a specific module</strong>
    <ul>
      <li>Right-click the project you want to run (Line or Histogram) → <em>Set as Startup Project</em></li>
      <li>Run: <em>Debug</em> → <em>Start Without Debugging</em> (Ctrl+F5) or press F5</li>
    </ul>
  </li>
</ol>

<p style="margin-top: 0.6rem;">
  <strong>Tip:</strong> Each project produces images under its own output folder. See the
  <a href="#output-files-and-working-directory">Output files and working directory</a> section for details.
</p>

<hr />

<!-- ========================================================= -->
<!-- CLI Setup                                                -->
<!-- ========================================================= -->

<h2 id="setup-and-run-dotnet-cli">Setup and run (.NET CLI)</h2>

<p>
  If you prefer not to use Visual Studio, you can build and run the projects using the .NET CLI.
  This repository supports a simple console-app workflow:
</p>

<pre><code>dotnet new console -n CSharpPlot
cd CSharpPlot
dotnet add package ScottPlot
# replace Program.cs with the desired module file (line or histogram), then:
dotnet run</code></pre>

<p style="font-size: 0.92rem; color: #555;">
  For this repository, the Visual Studio solution structure is recommended because it keeps each module
  runnable without swapping files.
</p>

<hr />

<!-- ========================================================= -->
<!-- Project Structure                                        -->
<!-- ========================================================= -->

<h2 id="project-structure">Project structure</h2>

<p>
  A typical structure for this repository looks like this:
</p>

<pre><code>CSharpPlot.sln
CSharpPlot.Line/
  Program.cs
  CSharpPlot.Line.csproj
CSharpPlot.Histogram/
  Program.cs
  CSharpPlot.Histogram.csproj</code></pre>

<p style="margin-top: 0.6rem;">
  When you add new plot categories later, create new Console App projects:
  <code>CSharpPlot.Pie</code>, <code>CSharpPlot.Bar</code>, <code>CSharpPlot.Scatter</code>, etc.
</p>

<hr />

<!-- ========================================================= -->
<!-- Output Behavior                                          -->
<!-- ========================================================= -->

<h2 id="output-files-and-working-directory">Output files and working directory</h2>

<p>
  The examples save plots as image files (PNG) into an <code>output</code> directory.
  In Visual Studio, the process working directory can differ from the project folder, so
  output files may appear under the build directory (e.g., <code>bin\Debug\net8.0\output</code>).
</p>

<p style="margin-bottom: 0.3rem;"><strong>Recommended pattern:</strong></p>
<p style="margin-top: 0;">
  For deterministic output placement (next to the executable), set the output folder using:
</p>

<pre><code>string outDir = Path.Combine(AppContext.BaseDirectory, "output");
Directory.CreateDirectory(outDir);</code></pre>

<p style="font-size: 0.92rem; color: #555;">
  This ensures images always go to the same place regardless of how the program is launched.
</p>

<hr />

<!-- ========================================================= -->
<!-- Line Module                                              -->
<!-- ========================================================= -->

<section id="line-plots-module">

<h2 id="line-plots-module">Line plots module</h2>

<p>
  The line plotting module demonstrates core plotting patterns using ScottPlot:
  multi-series overlays, line styling, markers, subplots (multiplot), and producing static PNG outputs.
  It also includes a real-world example that downloads a CSV dataset and generates a plot from it.
</p>

<h3 style="margin-bottom: 0.3rem;">Key namespaces / imports</h3>

<ul>
  <li><code>ScottPlot</code> — plotting API</li>
  <li><code>System</code>, <code>System.Linq</code> — numeric generation and transformations</li>
  <li><code>System.IO</code> — creating output directories and local file loading</li>
  <li><code>System.Net.Http</code> — downloading CSV files from a URL (optional)</li>
  <li><code>System.Globalization</code> — locale-safe numeric parsing for CSV values</li>
</ul>

<h3 style="margin-bottom: 0.3rem;">What the module does</h3>

<ul>
  <li>
    <strong>Multiple line series on one plot</strong><br />
    Adds several line series to the same axes (overlays) and customizes line patterns and markers.
  </li>
  <li>
    <strong>Plotting multiple sequences</strong><br />
    Plots multiple vectors (arrays/lists) as independent series with optional legend entries.
  </li>
  <li>
    <strong>Trigonometric function plots</strong><br />
    Generates smooth curves using a helper like <code>Linspace()</code> and applies different styles.
  </li>
  <li>
    <strong>Subplots using Multiplot</strong><br />
    Uses <code>ScottPlot.Multiplot</code> with a grid layout to render multiple subplots into one image.
  </li>
  <li>
    <strong>CSV-based real-world plot</strong><br />
    Downloads a CSV dataset and plots a derived relationship (e.g., aggregation such as mean vs. binned x-values).
  </li>
</ul>

<h3 style="margin-bottom: 0.3rem;">CSV workflow</h3>

<ul>
  <li>Download CSV text using <code>HttpClient</code> (URL-based datasets)</li>
  <li>Parse header + rows with a lightweight CSV splitter (quote-aware)</li>
  <li>Select columns by name (e.g., <code>carat</code>, <code>price</code>)</li>
  <li>Transform/aggregate values (e.g., binning x-values, averaging y-values)</li>
  <li>Plot the result and export a PNG</li>
</ul>

<p style="margin-bottom: 0.3rem;"><strong>Local CSV loading:</strong></p>
<p style="margin-top: 0;">
  If you want to read CSV from your local drive instead of a URL, use:
</p>

<pre><code>// string csvText = File.ReadAllText(@"C:\path\to\yourfile.csv");</code></pre>

<h3 style="margin-bottom: 0.3rem;">Extending this module</h3>

<ul>
  <li>Add additional line-series examples by appending new blocks</li>
  <li>Introduce annotations (titles, labels, legends) consistently</li>
  <li>Add new CSV datasets and plot strategies (scatter, box plots, rolling averages, etc.)</li>
</ul>

</section>

<hr />

<!-- ========================================================= -->
<!-- Histogram Module                                         -->
<!-- ========================================================= -->

<section id="histogram-plots-module">

<h2 id="histogram-plots-module">Histogram plots module</h2>

<p>
  The histogram module focuses on distribution visualization using ScottPlot. It covers:
  bin selection strategies, normalization variants, overlays for comparison, and an example that reads a CSV dataset.
</p>

<h3 style="margin-bottom: 0.3rem;">Key namespaces / imports</h3>

<ul>
  <li><code>ScottPlot</code> and <code>ScottPlot.Statistics</code> — histogram bins and rendering</li>
  <li><code>System</code>, <code>System.Linq</code> — data transformation and numeric operations</li>
  <li><code>System.IO</code> — output directory and optional local CSV loading</li>
  <li><code>System.Threading</code> — optional delays for iterative rendering demonstrations</li>
  <li><code>System.Net.Http</code>, <code>System.Globalization</code> — URL-based CSV downloads and safe parsing</li>
</ul>

<h3 style="margin-bottom: 0.3rem;">What the module does</h3>

<ul>
  <li>
    <strong>Basic histograms</strong><br />
    Creates histograms from generated data and exports the result as PNG.
  </li>
  <li>
    <strong>Bin count strategies</strong><br />
    Demonstrates how bin counts can be chosen using different rules (e.g., square-root, Sturges, Scott, Freedman–Diaconis).
  </li>
  <li>
    <strong>Iterative bin updates</strong><br />
    Shows how changing bin count affects the histogram (useful when tuning visualization).
  </li>
  <li>
    <strong>Custom bin edges</strong><br />
    Builds histograms from explicit bin edges and renders bars with bin-width-aware sizing.
  </li>
  <li>
    <strong>Categorical bar histogram</strong><br />
    Counts occurrences of string categories and renders them as a bar chart with labeled ticks.
  </li>
  <li>
    <strong>Overlay comparisons</strong><br />
    Overlays two distributions using consistent binning so shapes can be compared directly.
  </li>
  <li>
    <strong>PDF normalization + theoretical curve</strong><br />
    Normalizes histogram bars to represent a probability density estimate and overlays a theoretical PDF curve.
  </li>
  <li>
    <strong>CSV-based real-world histogram</strong><br />
    Reads a numeric column from a CSV dataset (e.g., price) and plots its distribution.
  </li>
</ul>

<h3 style="margin-bottom: 0.3rem;">Random data generation</h3>

<p>
  The module includes a standard normal generator (commonly implemented using the Box–Muller transform)
  to create reproducible distributions for histogram demonstrations.
</p>

<h3 style="margin-bottom: 0.3rem;">Extending this module</h3>

<ul>
  <li>Add additional normalization modes (count, probability, density)</li>
  <li>Introduce stacked histograms or grouped comparisons</li>
  <li>Expand CSV examples (multiple columns, filters, subsets, stratified distributions)</li>
</ul>

</section>

<hr />

<!-- ========================================================= -->
<!-- Video                                                    -->
<!-- ========================================================= -->

<h2 id="implementation-tutorial-video">Implementation tutorial video</h2>

<p>
  A complete walkthrough is available on YouTube (click the image below).
</p>

<!-- Replace VIDEO_ID with your YouTube video ID -->
<a href="https://www.youtube.com/watch?v=ItI79hGPehg" target="_blank">
  <img
    src="https://i.ytimg.com/vi/ItI79hGPehg/maxresdefault.jpg"
    alt="C# Plotting with ScottPlot - Implementation Tutorial"
    style="max-width: 100%; border-radius: 8px; box-shadow: 0 4px 16px rgba(0,0,0,0.15); margin-top: 0.5rem;"
  />
</a>
