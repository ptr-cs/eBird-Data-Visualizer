using System.Collections.ObjectModel;
using System.ComponentModel;
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

public class DataGridViewModel : ObservableRecipient, INavigationAware
{
    private readonly IBirdDataService _birdDataService;

    public ObservableCollection<Bird> BirdsCollection { get; set; } = new ObservableCollection<Bird>();
    public CollectionViewSource BirdsCollectionViewSource { get; set; }

    private ICollectionView itemsSource;
    public ICollectionView ItemsSource
    {
        get => itemsSource;
        set => SetProperty(ref itemsSource, value);
    }

    public ICommand GroupByGenusCommand
    {
        get;
    }

    public ICommand GroupByCommonNameCommand
    {
        get;
    }

    private static CollectionViewSource groupedItems;

    public DataGridViewModel(IBirdDataService birdDataService)
    {
        _birdDataService = birdDataService;
        BirdsCollectionViewSource = new CollectionViewSource() { Source = BirdsCollection };

        GroupByGenusCommand = new RelayCommand(() =>
        {
            if (CachedGroupQuery != nameof(KeySelectorGenus))
            {
                CachedGroupQuery = nameof(KeySelectorGenus);
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
                CachedGroupQuery = nameof(KeySelectorCommonName);
                ItemsSource = GroupDataByCommonName().View;
            }
            else
            {
                CreateDefaultView();
                CachedGroupQuery = "";
                return;
            }
        });
    }

    // Sorting implementation using LINQ
    private string _cachedSortedColumn = string.Empty;
    public string CachedSortedColumn
    {
        get => _cachedSortedColumn;
        set => _cachedSortedColumn = value;
    }

    // Grouping implementation using LINQ
    private string _cachedGroupQuery = string.Empty;
    public string CachedGroupQuery
    {
        get => _cachedGroupQuery;
        set => _cachedGroupQuery = value;
    }

    public async void OnNavigatedTo(object parameter)
    {
        BirdsCollection.Clear();

        // TODO: Replace with real data.
        var data = await _birdDataService.GetGridDataAsync();

        foreach (var item in data)
        {
            BirdsCollection.Add(item);
        }
        BirdsCollectionViewSource = new CollectionViewSource() { Source = BirdsCollection };
        ItemsSource = BirdsCollectionViewSource.View;
    }

    public void CreateDefaultView()
    {
        BirdsCollectionViewSource = new CollectionViewSource() { Source = BirdsCollection };
        ItemsSource = BirdsCollectionViewSource.View;
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
    public ObservableCollection<Bird> SortData(string sortBy, bool ascending)
    {
        _cachedSortedColumn = sortBy;

        var propertyInfo = typeof(Bird).GetProperty(sortBy);
        if (propertyInfo == null)
            return BirdsCollection;

        if (ascending)
            return new ObservableCollection<Bird>(BirdsCollection.OrderBy(bird => propertyInfo.GetValue(bird, null)));
        else
            return new ObservableCollection<Bird>(BirdsCollection.OrderByDescending(bird => propertyInfo.GetValue(bird, null)));
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
        return GroupData(BirdsCollection, KeySelectorGenus);
    }

    public CollectionViewSource GroupDataByCommonName()
    {
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
        groupedItems.Source = groups;

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
