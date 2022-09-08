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



public class Bird
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

    public ICollection<double> Frequency
    {
        get; set;
    }

    public override string ToString() => $"{CommonName} ({ScientificName})";
}
