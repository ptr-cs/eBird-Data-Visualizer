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

public class BarChartValueHeightConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return (double)value * 32.0;
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return (double)value / 32.0;
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

    private void SelectAll_Checked(object sender, RoutedEventArgs e)
    {
        // Option1CheckBox.IsChecked = Option2CheckBox.IsChecked = Option3CheckBox.IsChecked = true;
    }

    private void SelectAll_Unchecked(object sender, RoutedEventArgs e)
    {
        // Option1CheckBox.IsChecked = Option2CheckBox.IsChecked = Option3CheckBox.IsChecked = false;
    }

    private void SelectAll_Indeterminate(object sender, RoutedEventArgs e)
    {
        // If the SelectAll box is checked (all options are selected),
        // clicking the box will change it to its indeterminate state.
        // Instead, we want to uncheck all the boxes,
        // so we do this programatically. The indeterminate state should
        // only be set programatically, not by the user.

        //if (Option1CheckBox.IsChecked == true &&
        //    Option2CheckBox.IsChecked == true &&
        //    Option3CheckBox.IsChecked == true)
        //{
        //    // This will cause SelectAll_Unchecked to be executed, so
        //    // we don't need to uncheck the other boxes here.
        //    OptionsAllCheckBox.IsChecked = false;
        //}
    }

    private void SetCheckedState()
    {
        // Controls are null the first time this is called, so we just
        // need to perform a null check on any one of the controls.
        //if (Option1CheckBox != null)
        //{
        //    if (Option1CheckBox.IsChecked == true &&
        //        Option2CheckBox.IsChecked == true &&
        //        Option3CheckBox.IsChecked == true)
        //    {
        //        OptionsAllCheckBox.IsChecked = true;
        //    }
        //    else if (Option1CheckBox.IsChecked == false &&
        //        Option2CheckBox.IsChecked == false &&
        //        Option3CheckBox.IsChecked == false)
        //    {
        //        OptionsAllCheckBox.IsChecked = false;
        //    }
        //    else
        //    {
        //        // Set third state (indeterminate) by setting IsChecked to null.
        //        OptionsAllCheckBox.IsChecked = null;
        //    }
        //}
    }

    private void Option_Checked(object sender, RoutedEventArgs e)
    {
        SetCheckedState();
    }

    private void Option_Unchecked(object sender, RoutedEventArgs e)
    {
        SetCheckedState();
    }
}
