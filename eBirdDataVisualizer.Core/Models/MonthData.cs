using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBirdDataVisualizer.Core.Models;

public enum Month
{
    January = 0,
    February = 1,
    March = 2,
    April = 3,
    May = 4,
    June = 5,
    July = 6,
    August = 7,
    September = 8,
    October = 9,
    November = 10,
    December = 11
}

public class MonthData : IComparable
{
    public Month Month
    {
        get; set;
    }

    public ICollection<double> SampleSizes
    {
        get; set;
    }

    public string Q1String => $"{Month} Q1: {SampleSizes.ElementAt(0)}";
    public string Q2String => $"{Month} Q2: {SampleSizes.ElementAt(1)}";
    public string Q3String => $"{Month} Q3: {SampleSizes.ElementAt(2)}";
    public string Q4String => $"{Month} Q4: {SampleSizes.ElementAt(3)}";

    public string Q1Tag => $"{Month}Q1";
    public string Q2Tag => $"{Month}Q2";
    public string Q3Tag => $"{Month}Q3";
    public string Q4Tag => $"{Month}Q4";

    public int CompareTo(object obj)
    {
        return Month.CompareTo((obj as MonthData).Month);
    }
}
