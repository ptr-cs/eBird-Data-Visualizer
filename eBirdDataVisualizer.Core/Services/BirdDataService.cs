using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using eBirdDataVisualizer.Core.Models;
using static System.Net.Mime.MediaTypeNames;

namespace eBirdDataVisualizer.Core.Services;

public interface IBirdDataService
{
    public Task<IEnumerable<Bird>> GetBirdDataAsync();
    public Task<IEnumerable<MonthData>> GetMonthDataAsync();
    public Task<DataSourceMetadata> GetMetadataAsync();
    public void ClearData();
    public Task<bool> ParseData(string data);
    public Task ParseMetadata(string name);
}

public class BirdDataService : IBirdDataService
{
    static DataSet DataSet = new DataSet();
    static DataTable Birds = new DataTable(nameof(Birds));
    static DataColumn BirdId = new DataColumn(nameof(BirdId), typeof(Int32));
    static DataColumn CommonName = new DataColumn(nameof(CommonName), typeof(string));
    static DataColumn ScientificName = new DataColumn(nameof(ScientificName), typeof(string));
    static DataColumn Frequency = new DataColumn(nameof(Frequency), typeof(List<double>));
    static DataColumn JanuaryQ1 = new DataColumn(nameof(JanuaryQ1), typeof(double));
    static DataColumn JanuaryQ2 = new DataColumn(nameof(JanuaryQ2), typeof(double));
    static DataColumn JanuaryQ3 = new DataColumn(nameof(JanuaryQ3), typeof(double));
    static DataColumn JanuaryQ4 = new DataColumn(nameof(JanuaryQ4), typeof(double));

    static DataTable Months = new DataTable(nameof(Months));
    static DataColumn Month = new DataColumn(nameof(Month), typeof(Month));
    static DataColumn SampleSizes = new DataColumn(nameof(SampleSizes), typeof(List<double>));

    static DataTable Metadata = new DataTable(nameof(Metadata));
    static DataColumn Name = new DataColumn(nameof(Name), typeof(string));
    static DataColumn YearStart = new DataColumn(nameof(YearStart), typeof(int));
    static DataColumn YearEnd = new DataColumn(nameof(YearEnd), typeof(int));
    static DataColumn MonthStart = new DataColumn(nameof(MonthStart), typeof(Month));
    static DataColumn MonthEnd = new DataColumn(nameof(MonthEnd), typeof(Month));
    static DataColumn Location = new DataColumn(nameof(Location), typeof(string));

    private static List<Bird> allBirds = new List<Bird>();
    private static List<MonthData> allMonths = new List<MonthData>();

    static BirdDataService()
    {
        Birds.Columns.Add(BirdId);
        Birds.Columns.Add(CommonName);
        Birds.Columns.Add(ScientificName);
        Birds.Columns.Add(Frequency);
        Birds.Columns.Add(JanuaryQ1);
        Birds.Columns.Add(JanuaryQ2);
        Birds.Columns.Add(JanuaryQ3);
        Birds.Columns.Add(JanuaryQ4);

        Months.Columns.Add(Month);
        Months.Columns.Add(SampleSizes);

        Metadata.Columns.Add(Name);
        Metadata.Columns.Add(Location);
        Metadata.Columns.Add(YearStart);
        Metadata.Columns.Add(YearEnd);
        Metadata.Columns.Add(MonthStart);
        Metadata.Columns.Add(MonthEnd);

        DataSet.Tables.Add(Birds);
        DataSet.Tables.Add(Months);
        DataSet.Tables.Add(Metadata);
    }

    public void ClearData()
    {
        for (int i = 0; i < DataSet.Tables.Count; ++i)
            DataSet.Tables[i].Rows.Clear();
    }

    private static DataSourceMetadata AllMetadata()
    {
        DataSourceMetadata metadata = new DataSourceMetadata();
        foreach (DataRow row in DataSet.Tables[DataSet.Tables.IndexOf(Metadata)].Rows)
        {
            metadata.Name = (string)row[Name];
            metadata.YearStart = (int)row[YearStart];
            metadata.YearEnd = (int)row[YearEnd];
            metadata.MonthStart = (Month)row[MonthStart];
            metadata.MonthEnd = (Month)row[MonthEnd];
            metadata.Location = (string)row[Location];
        }
        return metadata;
    }

    private static IEnumerable<MonthData> AllMonths()
    {
        List<MonthData> months = new List<MonthData>();
        foreach (DataRow row in DataSet.Tables[DataSet.Tables.IndexOf(Months)].Rows)
        {
            months.Add(new MonthData()
            {
                Month = (Month)row[Month],
                SampleSizes = (List<double>)row[SampleSizes]
            });
        }
        return months;
    }

    private static IEnumerable<Bird> AllBirds()
    {
        List<Bird> birds = new List<Bird>();
        foreach (DataRow row in DataSet.Tables[DataSet.Tables.IndexOf(Birds)].Rows)
        {
            var frequency = (List<double>)row[Frequency];
            birds.Add(new Bird()
            {
                BirdId = (int)row[BirdId],
                CommonName = (string)row[CommonName],
                ScientificName = (string)row[ScientificName],
                JanuaryQ1 = (double)frequency[0],
                JanuaryQ2 = (double)frequency[1],
                JanuaryQ3 = (double)frequency[2],
                JanuaryQ4 = (double)frequency[3],
                FebruaryQ1 = (double)frequency[4],
                FebruaryQ2 = (double)frequency[5],
                FebruaryQ3 = (double)frequency[6],
                FebruaryQ4 = (double)frequency[7],
                MarchQ1 = (double)frequency[8],
                MarchQ2 = (double)frequency[9],
                MarchQ3 = (double)frequency[10],
                MarchQ4 = (double)frequency[11],
                AprilQ1 = (double)frequency[12],
                AprilQ2 = (double)frequency[13],
                AprilQ3 = (double)frequency[14],
                AprilQ4 = (double)frequency[15],
                MayQ1 = (double)frequency[16],
                MayQ2 = (double)frequency[17],
                MayQ3 = (double)frequency[18],
                MayQ4 = (double)frequency[19],
                JuneQ1 = (double)frequency[20],
                JuneQ2 = (double)frequency[21],
                JuneQ3 = (double)frequency[22],
                JuneQ4 = (double)frequency[23],
                JulyQ1 = (double)frequency[24],
                JulyQ2 = (double)frequency[25],
                JulyQ3 = (double)frequency[26],
                JulyQ4 = (double)frequency[27],
                AugustQ1 = (double)frequency[28],
                AugustQ2 = (double)frequency[29],
                AugustQ3 = (double)frequency[30],
                AugustQ4 = (double)frequency[31],
                SeptemberQ1 = (double)frequency[32],
                SeptemberQ2 = (double)frequency[33],
                SeptemberQ3 = (double)frequency[34],
                SeptemberQ4 = (double)frequency[35],
                OctoberQ1 = (double)frequency[36],
                OctoberQ2 = (double)frequency[37],
                OctoberQ3 = (double)frequency[38],
                OctoberQ4 = (double)frequency[39],
                NovemberQ1 = (double)frequency[40],
                NovemberQ2 = (double)frequency[41],
                NovemberQ3 = (double)frequency[42],
                NovemberQ4 = (double)frequency[43],
                DecemberQ1 = (double)frequency[44],
                DecemberQ2 = (double)frequency[45],
                DecemberQ3 = (double)frequency[46],
                DecemberQ4 = (double)frequency[47]
            });
        }
        return birds;
    }

    public async Task<IEnumerable<Bird>> GetBirdDataAsync()
    {
        allBirds = new List<Bird>(AllBirds());

        await Task.CompletedTask;
        return allBirds;
    }

    public async Task<IEnumerable<MonthData>> GetMonthDataAsync()
    {
        allMonths = new List<MonthData>(AllMonths());

        await Task.CompletedTask;
        return allMonths;
    }

    public async Task<DataSourceMetadata> GetMetadataAsync()
    {
        var data = AllMetadata();
        await Task.CompletedTask;
        return data;
    }

    /// <summary>
    /// Parse data loaded from an eBird barchart .txt file.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public async Task<bool> ParseData(string data)
    {
        //Months.Rows.Clear();
        //Birds.Rows.Clear();

        const string frequencyKey = "Frequency of observations in the selected location(s)";
        const string numberTaxaKey = "Number of taxa";
        const string sampleSizeKey = "Sample Size";

        bool importResult = false;

        try
        {
            // Parse the monthly sample-size data: 
            var sampleSizes = data.Trim().Split('\n') // split text into lines
                .Where(l => l != String.Empty) // skip empty lines
                .SkipWhile(l => !l.Contains(sampleSizeKey)).First()  // skip forward to the line containing sample size data
                .Split('\t')  // separate the line by the value delimiter
                .Where(v => v != String.Empty && Double.TryParse(v, out _))  // exclude empty items and non-numeric values
                .Select(v => System.Convert.ToDouble(v)).ToList(); // cast the remaining items to doubles and return as list

            // Parse the histogram data for bird entries:
            var lines = data.Trim().Split('\n') // split text into lines
                .Where(l => l != String.Empty) // skip empty lines
                .SkipWhile(l => !l.Contains(sampleSizeKey)) // skip forward to the line containing sample size data
                .Skip(1).ToList(); // skip the sample size data line; subsequent lines should be bird entries

            Months.Rows.Clear();
            Birds.Rows.Clear();

            int monthCounter = 0;
            List<double> samples = new List<double>();
            for (var i = 0; i < sampleSizes.Count() + 1; ++i)
            {
                if (i > 0 && i % 4 == 0)
                {
                    Months.Rows.Add((Month)Enum.GetValues(typeof(Month)).GetValue(monthCounter), samples);
                    monthCounter++;
                    samples = new List<double>();
                }
                if (i < sampleSizes.Count())
                    samples.Add(sampleSizes[i]);
            }

            for (var i = 0; i < lines.Count(); ++i)
            {
                // the common name appears first in the data entry:
                var commonNameSplit = lines[i].Split("(<em class=\"sci\">");
                // the scientific name appears second in the data entry, between an <em> tag pair:
                var scientificNameSplit = commonNameSplit.Last().Split("</em>)");
                var commonName = commonNameSplit.First();
                var scientificName = scientificNameSplit.First();
                var frequencyData = scientificNameSplit.Last().Trim().Split('\t').Where(x => x != String.Empty).Select(x => System.Convert.ToDouble(x)).ToList();

                Birds.Rows.Add(i, commonName, scientificName, frequencyData);
            }
            importResult = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing data: {ex}");
            importResult = false;
        }

        await Task.CompletedTask;
        return importResult;
    }

    public async Task ParseMetadata(string filename)
    {
        var yearStart = 0;
        var yearEnd = 0;
        var monthStart = Models.Month.January;
        var monthEnd = Models.Month.January;
        var location = filename.Split("__").First().Split('_').Last();
        var timeInfo = filename.Split("__").Last().Split('_').Where(x => x != String.Empty);
        if (timeInfo.Count() >= 4)
        {
            try
            {
                yearStart = int.Parse(timeInfo.ElementAt(0));
                yearEnd = int.Parse(timeInfo.ElementAt(1));
                monthStart = (Month)(int.Parse(timeInfo.ElementAt(2)) - 1);
                monthEnd = (Month)(int.Parse(timeInfo.ElementAt(3)) - 1);
            }
            catch (Exception) { }
        }

        Metadata.Rows.Clear();
        Metadata.Rows.Add(filename, location, yearStart, yearEnd, monthStart, monthEnd);
        await Task.CompletedTask;
        return;
    }
}
