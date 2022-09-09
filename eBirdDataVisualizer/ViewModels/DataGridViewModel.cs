using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using ColorCode.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using eBirdDataVisualizer.Contracts.ViewModels;
using eBirdDataVisualizer.Core.Contracts.Services;
using eBirdDataVisualizer.Core.Models;
using eBirdDataVisualizer.Core.Services;
using Microsoft.UI.Xaml.Data;

namespace eBirdDataVisualizer.ViewModels;

public class DataGridViewModel : ObservableRecipient, INavigationAware
{
    private readonly BirdDataService birdDataService = new BirdDataService();

    public ObservableCollection<Bird> BirdsCollection { get; set; } = new ObservableCollection<Bird>();
    public CollectionViewSource BirdsCollectionViewSource { get; set; }
    public ICollectionView BirdsCollectionView { get; set; }

    private static CollectionViewSource groupedItems;

    public DataGridViewModel()
    {
        BirdsCollectionViewSource = new CollectionViewSource() { Source = BirdsCollection };
        BirdsCollectionView = BirdsCollectionViewSource.View;
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
        var data = await birdDataService.GetGridDataAsync();

        foreach (var item in data)
        {
            BirdsCollection.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }

    public ObservableCollection<Bird> SortData(string sortBy, bool ascending)
    {
        _cachedSortedColumn = sortBy;
        switch (sortBy)
        {
            case "BirdId":
                if (ascending)
                    return new ObservableCollection<Bird>(BirdsCollection.OrderBy(bird => bird.BirdId));
                else
                    return new ObservableCollection<Bird>(BirdsCollection.OrderByDescending(bird => bird.BirdId));
            case "CommonName":
                if (ascending)
                    return new ObservableCollection<Bird>(BirdsCollection.OrderBy(bird => bird.CommonName));
                else
                    return new ObservableCollection<Bird>(BirdsCollection.OrderByDescending(bird => bird.CommonName));
            case "ScientificName":
                if (ascending)
                    return new ObservableCollection<Bird>(BirdsCollection.OrderBy(bird => bird.ScientificName));
                else
                    return new ObservableCollection<Bird>(BirdsCollection.OrderByDescending(bird => bird.ScientificName));
            case "JanuaryQ1":
                if (ascending)
                    return new ObservableCollection<Bird>(BirdsCollection.OrderBy(bird => bird.JanuaryQ1));
                else
                    return new ObservableCollection<Bird>(BirdsCollection.OrderByDescending(bird => bird.JanuaryQ1));
            case "JanuaryQ2":
                if (ascending)
                    return new ObservableCollection<Bird>(BirdsCollection.OrderBy(bird => bird.JanuaryQ2));
                else
                    return new ObservableCollection<Bird>(BirdsCollection.OrderByDescending(bird => bird.JanuaryQ2));
            case "JanuaryQ3":
                if (ascending)
                    return new ObservableCollection<Bird>(BirdsCollection.OrderBy(bird => bird.JanuaryQ3));
                else
                    return new ObservableCollection<Bird>(BirdsCollection.OrderByDescending(bird => bird.JanuaryQ3));
            case "JanuaryQ4":
                if (ascending)
                    return new ObservableCollection<Bird>(BirdsCollection.OrderBy(bird => bird.JanuaryQ4));
                else
                    return new ObservableCollection<Bird>(BirdsCollection.OrderByDescending(bird => bird.JanuaryQ4));
        }

        return BirdsCollection;
    }

    public string keySelector(Bird bird)
    {
        return bird.ScientificName.Split(' ')[0];
    }

    public Func<Bird, string> KeySelectorGenus = new (item => item.ScientificName.Split(' ').First());
    public Func<Bird, string> KeySelectorCommonName = new(item => item.CommonName.Split(' ').Last());

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
