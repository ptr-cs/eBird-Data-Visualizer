using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Input;
using ColorCode.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI.Controls;
using eBirdDataVisualizer.Contracts.ViewModels;
using eBirdDataVisualizer.Core.Contracts.Services;
using eBirdDataVisualizer.Core.Models;
using eBirdDataVisualizer.Core.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace eBirdDataVisualizer.ViewModels;

public enum HistogramViewType
{
    Values= 0,
    Bars = 1
}

public class DataGridViewModel : ObservableRecipient, INavigationAware
{
    private readonly IBirdDataService _birdDataService;

    public ObservableCollection<Bird> BirdsCollection { get; set; } = new ObservableCollection<Bird>();
    public CollectionViewSource BirdsCollectionViewSource { get; set; }

    public ObservableCollection<MonthData> MonthsCollection { get; set; } = new ObservableCollection<MonthData>();

    public Guid id { get; set; }

    private string dataSourceName = "default";
    public string DataSourceName
    {
        get => dataSourceName;
        set => SetProperty(ref dataSourceName, value);
    }

    private string dataSourceLocation = "abc";
    public string DataSourceLocation
    {
        get => dataSourceLocation;
        set => SetProperty(ref dataSourceLocation, value);
    }

    private int dataSourceYearStart = 0;
    public int DataSourceYearStart
    {
        get => dataSourceYearStart;
        set => SetProperty(ref dataSourceYearStart, value);
    }

    private int dataSourceYearEnd = 0;
    public int DataSourceYearEnd
    {
        get => dataSourceYearEnd;
        set => SetProperty(ref dataSourceYearEnd, value);
    }

    private Month dataSourceMonthStart = 0;
    public Month DataSourceMonthStart
    {
        get => dataSourceMonthStart;
        set => SetProperty(ref dataSourceMonthStart, value);
    }

    private Month dataSourceMonthEnd = 0;
    public Month DataSourceMonthEnd
    {
        get => dataSourceMonthEnd;
        set => SetProperty(ref dataSourceMonthEnd, value);
    }

    public void NotifyDataSourceMetadataChanged(string s)
    {
        DataSourceName = s;
        OnPropertyChanged(nameof(DataSourceName));
    }

    private bool isJanuaryVisible = true;
    public bool IsJanuaryVisible
    {
        get => isJanuaryVisible;
        set
        {
            SetProperty(ref isJanuaryVisible, value);
            OnPropertyChanged(nameof(IsJanuaryVisibleValues));
            OnPropertyChanged(nameof(IsJanuaryVisibleBars));
        }
    }

    private bool isFebruaryVisible = true;
    public bool IsFebruaryVisible
    {
        get => isFebruaryVisible;
        set
        {
            SetProperty(ref isFebruaryVisible, value);
            OnPropertyChanged(nameof(IsFebruaryVisibleValues));
            OnPropertyChanged(nameof(IsFebruaryVisibleBars));
        }
    }

    private bool isMarchVisible = true;
    public bool IsMarchVisible
    {
        get => isMarchVisible;
        set
        {
            SetProperty(ref isMarchVisible, value);
            OnPropertyChanged(nameof(IsMarchVisibleValues));
            OnPropertyChanged(nameof(IsMarchVisibleBars));
        }
    }

    private bool isAprilVisible = true;
    public bool IsAprilVisible
    {
        get => isAprilVisible;
        set
        {
            SetProperty(ref isAprilVisible, value);
            OnPropertyChanged(nameof(IsAprilVisibleValues));
            OnPropertyChanged(nameof(IsAprilVisibleBars));
        }
    }

    private bool isMayVisible = true;
    public bool IsMayVisible
    {
        get => isMayVisible;
        set
        {
            SetProperty(ref isMayVisible, value);
            OnPropertyChanged(nameof(IsMayVisibleValues));
            OnPropertyChanged(nameof(IsMayVisibleBars));
        }
    }

    private bool isJuneVisible = true;
    public bool IsJuneVisible
    {
        get => isJuneVisible;
        set
        {
            SetProperty(ref isJuneVisible, value);
            OnPropertyChanged(nameof(IsJuneVisibleValues));
            OnPropertyChanged(nameof(IsJuneVisibleBars));
        }
    }

    private bool isJulyVisible = true;
    public bool IsJulyVisible
    {
        get => isJulyVisible;
        set
        {
            SetProperty(ref isJulyVisible, value);
            OnPropertyChanged(nameof(IsJulyVisibleValues));
            OnPropertyChanged(nameof(IsJulyVisibleBars));
        }
    }

    private bool isAugustVisible = true;
    public bool IsAugustVisible
    {
        get => isAugustVisible;
        set
        {
            SetProperty(ref isAugustVisible, value);
            OnPropertyChanged(nameof(IsAugustVisibleValues));
            OnPropertyChanged(nameof(IsAugustVisibleBars));
        }
    }

    private bool isSeptemberVisible = true;
    public bool IsSeptemberVisible
    {
        get => isSeptemberVisible;
        set
        {
            SetProperty(ref isSeptemberVisible, value);
            OnPropertyChanged(nameof(IsSeptemberVisibleValues));
            OnPropertyChanged(nameof(IsSeptemberVisibleBars));
        }
    }

    private bool isOctoberVisible = true;
    public bool IsOctoberVisible
    {
        get => isOctoberVisible;
        set
        {
            SetProperty(ref isOctoberVisible, value);
            OnPropertyChanged(nameof(IsOctoberVisibleValues));
            OnPropertyChanged(nameof(IsOctoberVisibleBars));
        }
    }

    private bool isNovemberVisible = true;
    public bool IsNovemberVisible
    {
        get => isNovemberVisible;
        set
        {
            SetProperty(ref isNovemberVisible, value);
            OnPropertyChanged(nameof(IsNovemberVisibleValues));
            OnPropertyChanged(nameof(IsNovemberVisibleBars));
        }
    }

    private bool isDecemberVisible = true;
    public bool IsDecemberVisible
    {
        get => isDecemberVisible;
        set
        {
            SetProperty(ref isDecemberVisible, value);
            OnPropertyChanged(nameof(IsDecemberVisibleValues));
            OnPropertyChanged(nameof(IsDecemberVisibleBars));
        }
    }

    private bool? areAllMonthsVisible = true;
    public bool? AreAllMonthsVisible
    {
        get => areAllMonthsVisible;
        set => SetProperty(ref areAllMonthsVisible, value);
    }

    public bool IsHistogramViewTypeValues => (DataGridBarChartMode == HistogramViewType.Values) ? true : false;
    public bool IsHistogramViewTypeBars => (DataGridBarChartMode == HistogramViewType.Bars) ? true : false;

    private HistogramViewType dataGridBarChartMode = HistogramViewType.Values;
    public HistogramViewType DataGridBarChartMode
    {
        get => dataGridBarChartMode;
        set
        {
            SetProperty(ref dataGridBarChartMode, value);

            OnPropertyChanged(nameof(IsJanuaryVisibleValues));
            OnPropertyChanged(nameof(IsFebruaryVisibleValues));
            OnPropertyChanged(nameof(IsMarchVisibleValues));
            OnPropertyChanged(nameof(IsAprilVisibleValues));
            OnPropertyChanged(nameof(IsMayVisibleValues));
            OnPropertyChanged(nameof(IsJuneVisibleValues));
            OnPropertyChanged(nameof(IsJulyVisibleValues));
            OnPropertyChanged(nameof(IsAugustVisibleValues));
            OnPropertyChanged(nameof(IsSeptemberVisibleValues));
            OnPropertyChanged(nameof(IsOctoberVisibleValues));
            OnPropertyChanged(nameof(IsNovemberVisibleValues));
            OnPropertyChanged(nameof(IsDecemberVisibleValues));

            OnPropertyChanged(nameof(IsJanuaryVisibleBars));
            OnPropertyChanged(nameof(IsFebruaryVisibleBars));
            OnPropertyChanged(nameof(IsMarchVisibleBars));
            OnPropertyChanged(nameof(IsAprilVisibleBars));
            OnPropertyChanged(nameof(IsMayVisibleBars));
            OnPropertyChanged(nameof(IsJuneVisibleBars));
            OnPropertyChanged(nameof(IsJulyVisibleBars));
            OnPropertyChanged(nameof(IsAugustVisibleBars));
            OnPropertyChanged(nameof(IsSeptemberVisibleBars));
            OnPropertyChanged(nameof(IsOctoberVisibleBars));
            OnPropertyChanged(nameof(IsNovemberVisibleBars));
            OnPropertyChanged(nameof(IsDecemberVisibleBars));

            OnPropertyChanged(nameof(IsHistogramViewTypeValues));
            OnPropertyChanged(nameof(IsHistogramViewTypeBars));
        }
    }

    public bool IsJanuaryVisibleValues => IsJanuaryVisible && (DataGridBarChartMode == HistogramViewType.Values);
    public bool IsFebruaryVisibleValues => IsFebruaryVisible && (DataGridBarChartMode == HistogramViewType.Values);
    public bool IsMarchVisibleValues => IsMarchVisible && (DataGridBarChartMode == HistogramViewType.Values);
    public bool IsAprilVisibleValues => IsAprilVisible && (DataGridBarChartMode == HistogramViewType.Values);
    public bool IsMayVisibleValues => IsMayVisible && (DataGridBarChartMode == HistogramViewType.Values);
    public bool IsJuneVisibleValues => IsJuneVisible && (DataGridBarChartMode == HistogramViewType.Values);
    public bool IsJulyVisibleValues => IsJulyVisible && (DataGridBarChartMode == HistogramViewType.Values);
    public bool IsAugustVisibleValues => IsAugustVisible && (DataGridBarChartMode == HistogramViewType.Values);
    public bool IsSeptemberVisibleValues => IsSeptemberVisible && (DataGridBarChartMode == HistogramViewType.Values);
    public bool IsOctoberVisibleValues => IsOctoberVisible && (DataGridBarChartMode == HistogramViewType.Values);
    public bool IsNovemberVisibleValues => IsNovemberVisible && (DataGridBarChartMode == HistogramViewType.Values);
    public bool IsDecemberVisibleValues => IsDecemberVisible && (DataGridBarChartMode == HistogramViewType.Values);

    public bool IsJanuaryVisibleBars => IsJanuaryVisible && (DataGridBarChartMode == HistogramViewType.Bars);
    public bool IsFebruaryVisibleBars => IsFebruaryVisible && (DataGridBarChartMode == HistogramViewType.Bars);
    public bool IsMarchVisibleBars => IsMarchVisible && (DataGridBarChartMode == HistogramViewType.Bars);
    public bool IsAprilVisibleBars => IsAprilVisible && (DataGridBarChartMode == HistogramViewType.Bars);
    public bool IsMayVisibleBars => IsMayVisible && (DataGridBarChartMode == HistogramViewType.Bars);
    public bool IsJuneVisibleBars => IsJuneVisible && (DataGridBarChartMode == HistogramViewType.Bars);
    public bool IsJulyVisibleBars => IsJulyVisible && (DataGridBarChartMode == HistogramViewType.Bars);
    public bool IsAugustVisibleBars => IsAugustVisible && (DataGridBarChartMode == HistogramViewType.Bars);
    public bool IsSeptemberVisibleBars => IsSeptemberVisible && (DataGridBarChartMode == HistogramViewType.Bars);
    public bool IsOctoberVisibleBars => IsOctoberVisible && (DataGridBarChartMode == HistogramViewType.Bars);
    public bool IsNovemberVisibleBars => IsNovemberVisible && (DataGridBarChartMode == HistogramViewType.Bars);
    public bool IsDecemberVisibleBars => IsDecemberVisible && (DataGridBarChartMode == HistogramViewType.Bars);

    private string dataGridModeLabel = "Grid";
    public string DataGridModeLabel
    {
        get => dataGridModeLabel;
        set => SetProperty(ref dataGridModeLabel, value);
    }

    private ICollectionView itemsSource;
    public ICollectionView ItemsSource
    {
        get => itemsSource;
        set => SetProperty(ref itemsSource, value);
    }

    private ICollectionView monthDataSource;
    public ICollectionView MonthDataSource
    {
        get => monthDataSource;
        set => SetProperty(ref monthDataSource, value);
    }

    public ICommand GroupByGenusCommand
    {
        get;
    }

    public ICommand GroupByCommonNameCommand
    {
        get;
    }

    public ICommand ToggleMonthVisibility
    {
        get;
    }

    public ICommand ToggleAllMonthsVisibility
    {
        get;
    }

    public ICommand ToggleDataGridBarChartMode
    {
        get;
    }

    private static CollectionViewSource groupedItems;

    public bool MonthsAllVisible()
    {
        return IsJanuaryVisible &&
        IsFebruaryVisible &&
        IsMarchVisible &&
        IsAprilVisible &&
        IsMayVisible &&
        IsJuneVisible &&
        IsJulyVisible &&
        IsAugustVisible &&
        IsSeptemberVisible &&
        IsOctoberVisible &&
        IsNovemberVisible &&
        IsDecemberVisible;
    }

    public bool AtLeastOneMonthVisible()
    {
        return IsJanuaryVisible ||
        IsFebruaryVisible ||
        IsMarchVisible ||
        IsAprilVisible ||
        IsMayVisible ||
        IsJuneVisible ||
        IsJulyVisible ||
        IsAugustVisible ||
        IsSeptemberVisible ||
        IsOctoberVisible ||
        IsNovemberVisible ||
        IsDecemberVisible;
    }

    public DataGridViewModel(IBirdDataService birdDataService)
    {
        id = Guid.NewGuid();
        _birdDataService = birdDataService;
        BirdsCollectionViewSource = new CollectionViewSource() { Source = BirdsCollection };

        GroupByGenusCommand = new RelayCommand(() =>
        {
            if (CachedGroupQuery != nameof(KeySelectorGenus))
            {
                ItemsSource = GroupDataByGenus().View;
            }
            else
            {
                CreateDefaultView();
                CachedGroupQuery = "";
                return;
            }
        });

        GroupByCommonNameCommand = new RelayCommand(() =>
        {
            if (CachedGroupQuery != nameof(KeySelectorCommonName))
            {
                ItemsSource = GroupDataByCommonName().View;
            }
            else
            {
                CreateDefaultView();
                CachedGroupQuery = "";
                return;
            }
        });

        ToggleMonthVisibility = new RelayCommand<Month>((param) =>
        {
            switch (param)
            {
                case Month.January:
                    IsJanuaryVisible = !IsJanuaryVisible;
                    break;
                case Month.February:
                    IsFebruaryVisible = !IsFebruaryVisible;
                    break;
                case Month.March:
                    IsMarchVisible = !IsMarchVisible;
                    break;
                case Month.April:
                    IsAprilVisible = !IsAprilVisible;
                    break;
                case Month.May:
                    IsMayVisible = !IsMayVisible;
                    break;
                case Month.June:
                    IsJuneVisible = !IsJuneVisible;
                    break;
                case Month.July:
                    IsJulyVisible = !IsJulyVisible;
                    break;
                case Month.August:
                    IsAugustVisible = !IsAugustVisible;
                    break;
                case Month.September:
                    IsSeptemberVisible = !IsSeptemberVisible;
                    break;
                case Month.October:
                    IsOctoberVisible = !IsOctoberVisible;
                    break;
                case Month.November:
                    IsNovemberVisible = !IsNovemberVisible;
                    break;
                case Month.December:
                    IsDecemberVisible = !IsDecemberVisible;
                    break;
            }

            if (MonthsAllVisible())
                AreAllMonthsVisible = true;
            else if (AtLeastOneMonthVisible())
                AreAllMonthsVisible = null;
            else
                AreAllMonthsVisible = false;
        });

        ToggleAllMonthsVisibility = new RelayCommand(() =>
        {
            AreAllMonthsVisible = (AreAllMonthsVisible == true || AreAllMonthsVisible == null) ? false : true;

            if (AreAllMonthsVisible == true)
            {
                IsJanuaryVisible =
                IsFebruaryVisible =
                IsMarchVisible =
                IsAprilVisible =
                IsMayVisible =
                IsJuneVisible =
                IsJulyVisible =
                IsAugustVisible =
                IsSeptemberVisible =
                IsOctoberVisible =
                IsNovemberVisible =
                IsDecemberVisible = true;
            }
            else if (AreAllMonthsVisible == false)
            {
                IsJanuaryVisible =
                IsFebruaryVisible =
                IsMarchVisible =
                IsAprilVisible =
                IsMayVisible =
                IsJuneVisible =
                IsJulyVisible =
                IsAugustVisible =
                IsSeptemberVisible =
                IsOctoberVisible =
                IsNovemberVisible =
                IsDecemberVisible = false;
            }
        });

        ToggleDataGridBarChartMode = new RelayCommand<HistogramViewType>((param) =>
        {
            DataGridBarChartMode = param;
            //ShowProgress = false;
        });
    }

    private bool showProgress = false;
    public bool ShowProgress
    {
        get => showProgress;
        set => SetProperty(ref showProgress, value);
    }

    // Sorting implementation using LINQ
    private Tuple<string, bool>  _cachedSortedColumn = new Tuple<string, bool>(string.Empty, false);
    public Tuple<string, bool> CachedSortedColumn
    {
        get => _cachedSortedColumn;
        set => _cachedSortedColumn = value;
    }

    // Grouping implementation using LINQ
    private string _cachedGroupQuery = string.Empty;
    public string CachedGroupQuery
    {
        get => _cachedGroupQuery;
        set
        {
            SetProperty(ref _cachedGroupQuery, value);
            OnPropertyChanged(nameof(IsGroupedByGenus));
            OnPropertyChanged(nameof(IsGroupedByCommonName));
        }
    }

    public bool IsGroupedByGenus => (CachedGroupQuery == nameof(KeySelectorGenus)) ? true : false;
    public bool IsGroupedByCommonName => (CachedGroupQuery == nameof(KeySelectorCommonName)) ? true : false;

    public async void OnNavigatedTo(object parameter)
    {
        BirdsCollection.Clear();

        var birdData = await _birdDataService.GetBirdDataAsync();

        foreach (var item in birdData)
        {
            BirdsCollection.Add(item);
        }
        BirdsCollectionViewSource = new CollectionViewSource() { Source = BirdsCollection };
        ItemsSource = BirdsCollectionViewSource.View;

        MonthsCollection.Clear();

        var monthData = await _birdDataService.GetMonthDataAsync();

        foreach (var item in monthData)
        {
            MonthsCollection.Add(item);
        }

        var metadata = await _birdDataService.GetMetadataAsync();
        DataSourceName = metadata.Name;
        DataSourceLocation = metadata.Location;
        DataSourceYearStart = metadata.YearStart;
        DataSourceYearEnd = metadata.YearEnd;
        DataSourceMonthStart = metadata.MonthStart;
        DataSourceMonthEnd = metadata.MonthEnd;
    }

    public void CreateDefaultView()
    {
        BirdsCollectionViewSource = new CollectionViewSource() { Source = BirdsCollection };
        ItemsSource = BirdsCollectionViewSource.View;

        //MonthsCollectionViewSource = new CollectionViewSource() { Source = MonthsCollection };
        //MonthDataSource = MonthsCollectionViewSource.View;
    }

    public void OnNavigatedFrom()
    {
    }

    /// <summary>
    /// SortData function that uses a DataGrid Column's Tag as the OrderBy keySelector.
    /// Note that the tag must exactly match the name of a property in the type being sorted.
    /// </summary>
    /// <param name="sortBy"></param>
    /// <param name="ascending"></param>
    /// <returns></returns>
    public CollectionViewSource SortData(string sortBy, bool ascending)
    {
        CachedSortedColumn = new Tuple<string, bool>(sortBy, ascending);
        CachedGroupQuery = String.Empty;
        //_cachedGroupQuery = string.Empty;

        CollectionViewSource collectionViewSource = new();

        var propertyInfo = typeof(Bird).GetProperty(sortBy);
        if (propertyInfo == null)
        {
            collectionViewSource.Source = BirdsCollection;
            return collectionViewSource;
        }

        //if (CachedGroupQuery != string.Empty)
        //{
        //    switch (CachedGroupQuery)
        //    {
        //        case nameof(KeySelectorGenus):
        //            return GroupDataByGenus();
        //        case nameof(KeySelectorCommonName):
        //            return GroupDataByCommonName();
        //        default:
        //            break;
        //    }
        //}

        if (ascending)
            collectionViewSource.Source = new ObservableCollection<Bird>(BirdsCollection.OrderBy(bird => propertyInfo.GetValue(bird, null)));
        else
            collectionViewSource.Source = new ObservableCollection<Bird>(BirdsCollection.OrderByDescending(bird => propertyInfo.GetValue(bird, null)));

        return collectionViewSource;
    }

    public string keySelector(Bird bird)
    {
        return bird.ScientificName.Split(' ')[0];
    }

    public Func<Bird, string> KeySelectorGenus = new (item => item.ScientificName.Split(' ').First());
    public Func<Bird, string> KeySelectorCommonName = new(item => item.CommonName.Trim()
        .Split(' ')
        .TakeWhile(x => !x.Contains('(') && !x.Contains("sp.")) // Stop at the first string that contains either '(' or "sp."
        .Select(x => Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(x)) // Adjust capitalization to unify group name
        .Last()); // Select the last item remaining, which should be the type of bird (Goose, Duck, etc.)

    public CollectionViewSource GroupDataByGenus()
    {
        CachedGroupQuery = nameof(KeySelectorGenus);
        // bool? sort = (CachedSortedColumn.Item1 == string.Empty) ? null : CachedSortedColumn.Item2;
        return GroupData(BirdsCollection, KeySelectorGenus);
    }

    public CollectionViewSource GroupDataByCommonName()
    {
        CachedGroupQuery = nameof(KeySelectorCommonName);
        // bool? sort = (CachedSortedColumn.Item1 == string.Empty) ? null : CachedSortedColumn.Item2;
        return GroupData(BirdsCollection, KeySelectorCommonName);
    }

    // Grouping implementation using LINQ
    public CollectionViewSource GroupData(IEnumerable<Bird> list, Func<Bird, string> keySelector)
    {
        ObservableCollection<GroupInfoCollection<Bird>> groups = new ObservableCollection<GroupInfoCollection<Bird>>();
        var query = from item in list orderby item group item by keySelector(item) into g select new
        {
            GroupName = g.Key,
            Items = g
        };
        foreach (var g in query)
        {
            GroupInfoCollection<Bird> info = new GroupInfoCollection<Bird>();
            info.Key = g.GroupName;


            foreach (var item in g.Items)
            {
                info.Add(item);
            }

            groups.Add(info);
        }

        groupedItems = new CollectionViewSource();
        groupedItems.IsSourceGrouped = true;

        //if (CachedSortedColumn.Item1 != string.Empty)
        //    groupedItems.Source = (CachedSortedColumn.Item2 == true) ? groups.OrderBy(x => x.Key) : groups.OrderByDescending(x => x.Key);
        //else
        //    groupedItems.Source = groups;

        groupedItems.Source = groups.OrderBy(x => x.Key);

        return groupedItems;
    }

    public class GroupInfoCollection<T> : ObservableCollection<T>
    {
        public object Key
        {
            get; set;
        }

        public new IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)base.GetEnumerator();
        }
    }

    // Filtering implementation using LINQ
    public enum FilterOptions
    {
        All = -1,
        Rank_Low = 0,
        Rank_High = 1,
        Height_Low = 2,
        Height_High = 3
    }

    public ObservableCollection<Bird> FilterData(FilterOptions filterBy)
    {
        switch (filterBy)
        {
            case FilterOptions.All:
                return new ObservableCollection<Bird>(BirdsCollection);

            //case FilterOptions.Rank_Low:
            //    return new ObservableCollection<DataGridDataItem>(from item in _items
            //                                                      where item.Rank < 50
            //    select item);

            //case FilterOptions.Rank_High:
            //    return new ObservableCollection<DataGridDataItem>(from item in _items
            //                                                      where item.Rank > 50
            //                                                      select item);

            //case FilterOptions.Height_High:
            //    return new ObservableCollection<DataGridDataItem>(from item in _items
            //                                                      where item.Height_m > 8000
            //                                                      select item);

            //case FilterOptions.Height_Low:
            //    return new ObservableCollection<DataGridDataItem>(from item in _items
            //                                                      where item.Height_m < 8000
            //                                                      select item);
        }

        return BirdsCollection;
    }
}
