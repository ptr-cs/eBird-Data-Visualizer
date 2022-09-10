using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eBirdDataVisualizer.Core.Models;
using static System.Net.Mime.MediaTypeNames;

namespace eBirdDataVisualizer.Core.Services;

public interface IBirdDataService
{
    public Task<IEnumerable<Bird>> GetGridDataAsync();
    public void ClearData();
    public Task<bool> ParseData(string data);
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

    private static List<Bird> allBirds = new List<Bird>();

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

        Birds.Rows.Add(0, "Black-bellied Whistling-Duck", "Dendrocygna autumnalis", new List<double>() { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1.655E-4, 2.082E-4, 4.693E-4, 4.624E-4, 8.357E-4, 9.092E-4, 8.3E-5, 0.0012603, 0.0012944, 4.97E-5, 0.0015, 0.0010597, 4.18E-4, 7.671E-4, 1.218E-4, 6.09E-5, 0.0, 0.0, 0.0019542, 0.0, 4.4E-5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });

        Birds.Rows.Add(3, "West Indian Whistling-Duck", "Dendrocygna arborea", new List<double>() { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0015, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });

        Birds.Rows.Add(4, "Fulvous Whistling-Duck", "Dendrocygna bicolor", new List<double>() { 0.0015, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0015, 0.0, 0.0015, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0015, 3.95E-5, 0.0015, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0015, 0.0015, 0.0015, 0.0015, 0.0, 0.0, 0.0, 0.0, 0.0015, 0.0015, 0.0, 0.0015, 0.0, 0.0015, 0.0, 0.0, 0.0015 });

        Birds.Rows.Add(1, "Emu", "Dromaius novaehollandiae", new List<double>() { 0.0, 0.0, 0.0, 0.0, 4.44E-5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });

        Birds.Rows.Add(2, "Snow Goose", "Anser caerulescens", new List<double>() { 0.0164127, 0.0130752, 0.0147944, 0.0115589, 0.0134523, 0.0070272, 0.0060272, 0.0100091, 0.0097213, 0.0075751, 0.0053493, 0.0031462, 0.0010052, 6.83E-4, 6.52E-4, 4.76E-4, 4.164E-4, 7.41E-5, 3.391E-4, 2.279E-4, 1.186E-4, 8.3E-5, 1.8E-4, 0.0, 1.491E-4, 5.39E-5, 5.89E-5, 4.18E-5, 1.18E-4, 1.828E-4, 1.218E-4, 7.95E-5, 2.555E-4, 6.514E-4, 9.051E-4, 0.0011868, 0.0010745, 0.0026718, 0.0040783, 0.0059102, 0.0064931, 0.0110383, 0.0122762, 0.0232245, 0.0200967, 0.0151272, 0.0184791, 0.030209 });

        Birds.Rows.Add(5, "Barnacle Goose", "Branta leucopsis", new List<double>() { 3.17E-5, 4.22E-5, 4.12E-5, 0.0015, 3.108E-4, 5.69E-5, 3.57E-5, 0.0015, 0.0015, 1.209E-4, 1.207E-4, 0.0015, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0015, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 3.205E-4, 5.19E-5, 0.0 });

        Birds.Rows.Add(5, "Egyptian Goose", "Alopochen aegyptiaca", new List<double>() { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0015, 0.0, 0.0, 0.0, 0.0, 0.0, 4.152E-4, 1.35E-4, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 3.97E-5, 5.11E-5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 4.48E-5, 0.0, 0.0, 0.0, 0.0 });

        DataSet.Tables.Add(Birds);
    }

    public BirdDataService()
    {

    }

    public void ClearData()
    {
        DataSet.Clear();
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
                Frequency = frequency,
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

    public async Task<IEnumerable<Bird>> GetGridDataAsync()
    {
        allBirds = new List<Bird>(AllBirds());

        await Task.CompletedTask;
        return allBirds;
    }

    public async Task<bool> ParseData(string data)
    {
        const string frequencyKey = "Frequency of observations in the selected location(s)";
        const string numberTaxaKey = "Number of taxa";
        const string sampleSizeKey = "Sample Size";

        bool importResult = false;

        try
        {
            // Parse the histogram data text:
            var lines = data.Trim().Split('\n') // split text into lines
                .Where(l => l != "") // skip empty lines
                .SkipWhile(l => !l.Contains(sampleSizeKey)) // skip forward to the line containing sample size data
                .Skip(1).ToList(); // skip the sample size data line; subsequent lines should be bird entries
            for (var i = 0; i < lines.Count(); ++i)
            {
                // the common name appears first in the data entry:
                var commonNameSplit = lines[i].Split("(<em class=\"sci\">");
                // the scientific name appears second in the data entry, between an <em> tag pair:
                var scientificNameSplit = commonNameSplit.Last().Split("</em>)");
                var commonName = commonNameSplit.First();
                var scientificName = scientificNameSplit.First();
                var frequencyData = scientificNameSplit.Last().Trim().Split('\t').Where(x => x != "").Select(x => System.Convert.ToDouble(x)).ToList();

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
}
