using eBirdDataVisualizer.ViewModels;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using eBirdDataVisualizer.Core.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System.ComponentModel;
using CommunityToolkit.WinUI.UI;

namespace eBirdDataVisualizer.Views;

public class DataGridLengthValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return new DataGridLength((double)value, DataGridLengthUnitType.Star);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        DataGridLength converted = (DataGridLength)value;
        return converted.Value;
    }
}

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class DataGridPage : Page
{
    public DataGridViewModel ViewModel
    {
        get;
    }

    public DataGridPage()
    {
        ViewModel = App.GetService<DataGridViewModel>();
        DataContext = ViewModel;
        InitializeComponent();
    }


    private void GroupByGenus_Click(object sender, RoutedEventArgs e)
    {
        if (frequencyDataGrid != null)
        {
            ViewModel.CachedGroupQuery = "Genus";
            frequencyDataGrid.ItemsSource = ViewModel.GroupDataByGenus().View;
        }
    }

    private void GroupByCommonName_Click(object sender, RoutedEventArgs e)
    {
        if (frequencyDataGrid != null)
        {
            ViewModel.CachedGroupQuery = "Common Name";
            frequencyDataGrid.ItemsSource = ViewModel.GroupDataByCommonName().View;
        }
    }

    private void DataGrid_Sorting(object sender, DataGridColumnEventArgs e)
    {
        // Clear previous sorted column if we start sorting a different column
        string previousSortedColumn = ViewModel.CachedSortedColumn;
        if (previousSortedColumn != string.Empty)
        {
            foreach (DataGridColumn dataGridColumn in frequencyDataGrid.Columns)
            {
                if (dataGridColumn.Tag != null && dataGridColumn.Tag.ToString() == previousSortedColumn &&
                    (e.Column.Tag == null || previousSortedColumn != e.Column.Tag.ToString()))
                {
                    dataGridColumn.SortDirection = null;
                }
            }
        }

        // Toggle clicked column's sorting method
        if (e.Column.Tag != null)
        {
            if (e.Column.SortDirection == null)
            {
                frequencyDataGrid.ItemsSource = ViewModel.SortData(e.Column.Tag.ToString(), true);
                e.Column.SortDirection = DataGridSortDirection.Ascending;
            }
            else if (e.Column.SortDirection == DataGridSortDirection.Ascending)
            {
                frequencyDataGrid.ItemsSource = ViewModel.SortData(e.Column.Tag.ToString(), false);
                e.Column.SortDirection = DataGridSortDirection.Descending;
            }
            else
            {
                frequencyDataGrid.ItemsSource = ViewModel.FilterData(DataGridViewModel.FilterOptions.All);
                e.Column.SortDirection = null;
            }
        }
    }

    //Handle the LoadingRowGroup event to alter the grouped header property value to be displayed
    private void dg_loadingRowGroup(object sender, DataGridRowGroupHeaderEventArgs e)
    {
        ICollectionViewGroup group = e.RowGroupHeader.CollectionViewGroup;
        Bird? bird = group.GroupItems?[0] as Bird;
        if (bird == null)
            return;

        switch (ViewModel.CachedGroupQuery)
        {
            case "Genus":
                e.RowGroupHeader.PropertyValue = ViewModel.KeySelectorGenus(bird);
                break;
            case "Common Name":
                e.RowGroupHeader.PropertyValue = ViewModel.KeySelectorCommonName(bird);
                break;
            default:
                e.RowGroupHeader.PropertyValue = "Group";
                break;
        }
    }
}
