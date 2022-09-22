using eBirdDataVisualizer.ViewModels;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using eBirdDataVisualizer.Core.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System.ComponentModel;
using CommunityToolkit.WinUI.UI;
using System;
using Windows.UI.Core;
using Microsoft.UI.Dispatching;

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

public class BarChartValueHeightConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return (double)value * 32.0; // 32.0 Is Default MinHeight of DataGridRow
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return (double)value / 32.0;
    }
}

public class HistogramViewTypeComparisonConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        HistogramViewType view = (HistogramViewType)value;
        HistogramViewType param;
        string parsed = parameter.ToString();
        if (parsed == "Bars")
            param = HistogramViewType.Bars;
        else if (parsed == "Values")
            param = HistogramViewType.Values;
        else
            return false;

        return (view == param) ? Visibility.Visible : Visibility.Collapsed;
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return HistogramViewType.Values;
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

    private void DataGrid_Sorting(object sender, DataGridColumnEventArgs e)
    {
        //DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        //Task.Run(() =>
        //{
        //    dispatcherQueue.TryEnqueue(() =>
        //    {

        //    });
        //});
        ViewModel.ShowProgress = true;
        DataGrid dg = (DataGrid)sender;

        // Clear previous sorted column if we start sorting a different column
        string previousSortedColumn = ViewModel.CachedSortedColumn.Item1;
        if (previousSortedColumn != string.Empty)
        {
            foreach (DataGridColumn dataGridColumn in dg.Columns)
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
                dg.ItemsSource = ViewModel.SortData(e.Column.Tag.ToString(), true).View;
                e.Column.SortDirection = DataGridSortDirection.Ascending;
            }
            else if (e.Column.SortDirection == DataGridSortDirection.Ascending)
            {
                dg.ItemsSource = ViewModel.SortData(e.Column.Tag.ToString(), false).View;
                e.Column.SortDirection = DataGridSortDirection.Descending;
            }
            else
            {
                dg.ItemsSource = ViewModel.FilterData(DataGridViewModel.FilterOptions.All);
                e.Column.SortDirection = null;
            }
        }

        ViewModel.ShowProgress = false;
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
            case nameof(ViewModel.KeySelectorGenus):
                e.RowGroupHeader.PropertyValue = ViewModel.KeySelectorGenus(bird);
                break;
            case nameof(ViewModel.KeySelectorCommonName):
                e.RowGroupHeader.PropertyValue = ViewModel.KeySelectorCommonName(bird);
                break;
            default:
                e.RowGroupHeader.PropertyValue = "Group";
                break;
        }
    }

    private void RadioButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.ShowProgress = true;
    }
}
