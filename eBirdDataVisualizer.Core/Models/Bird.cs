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
