using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBirdDataVisualizer.Core.Models;

/// <summary>
/// Class for dispalying (initially) frequency of observations data
/// </summary>
/// 

public class Bird : IComparable
{
    public int BirdId
    {
        get; set;
    }

    public string CommonName
    {
        get; set;
    }

    public string ScientificName
    {
        get; set;
    }

    public double JanuaryQ1
    {
        get; set;
    }

    public double JanuaryQ2
    {
        get; set;
    }

    public double JanuaryQ3
    {
        get; set;
    }

    public double JanuaryQ4
    {
        get; set;
    }

    public double FebruaryQ1
    {
        get; set;
    }

    public double FebruaryQ2
    {
        get; set;
    }

    public double FebruaryQ3
    {
        get; set;
    }

    public double FebruaryQ4
    {
        get; set;
    }

    public double MarchQ1
    {
        get; set;
    }

    public double MarchQ2
    {
        get; set;
    }

    public double MarchQ3
    {
        get; set;
    }

    public double MarchQ4
    {
        get; set;
    }

    public double AprilQ1
    {
        get; set;
    }

    public double AprilQ2
    {
        get; set;
    }

    public double AprilQ3
    {
        get; set;
    }

    public double AprilQ4
    {
        get; set;
    }

    public double MayQ1
    {
        get; set;
    }

    public double MayQ2
    {
        get; set;
    }

    public double MayQ3
    {
        get; set;
    }

    public double MayQ4
    {
        get; set;
    }

    public double JuneQ1
    {
        get; set;
    }

    public double JuneQ2
    {
        get; set;
    }

    public double JuneQ3
    {
        get; set;
    }

    public double JuneQ4
    {
        get; set;
    }

    public double JulyQ1
    {
        get; set;
    }

    public double JulyQ2
    {
        get; set;
    }

    public double JulyQ3
    {
        get; set;
    }

    public double JulyQ4
    {
        get; set;
    }

    public double AugustQ1
    {
        get; set;
    }

    public double AugustQ2
    {
        get; set;
    }

    public double AugustQ3
    {
        get; set;
    }

    public double AugustQ4
    {
        get; set;
    }

    public double SeptemberQ1
    {
        get; set;
    }

    public double SeptemberQ2
    {
        get; set;
    }

    public double SeptemberQ3
    {
        get; set;
    }

    public double SeptemberQ4
    {
        get; set;
    }

    public double OctoberQ1
    {
        get; set;
    }

    public double OctoberQ2
    {
        get; set;
    }

    public double OctoberQ3
    {
        get; set;
    }

    public double OctoberQ4
    {
        get; set;
    }

    public double NovemberQ1
    {
        get; set;
    }

    public double NovemberQ2
    {
        get; set;
    }

    public double NovemberQ3
    {
        get; set;
    }

    public double NovemberQ4
    {
        get; set;
    }

    public double DecemberQ1
    {
        get; set;
    }

    public double DecemberQ2
    {
        get; set;
    }

    public double DecemberQ3
    {
        get; set;
    }

    public double DecemberQ4
    {
        get; set;
    }

    public ICollection<double> Frequency
    {
        get; set;
    }

    public int CompareTo(object obj)
    {
        return BirdId.CompareTo((obj as Bird).BirdId);
    }

    public override string ToString() => $"{CommonName} ({ScientificName})";
}
