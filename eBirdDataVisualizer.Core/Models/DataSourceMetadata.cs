using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBirdDataVisualizer.Core.Models;

public class DataSourceMetadata
{
    public string Name
    {
        get; set;
    }

    public string Location
    {
        get; set;
    }

    public int YearStart
    {
        get; set;
    }

    public int YearEnd
    {
        get; set;
    }

    public Month MonthStart
    {
        get; set;
    }

    public Month MonthEnd
    {
        get; set;
    }
}
