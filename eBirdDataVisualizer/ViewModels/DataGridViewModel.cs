using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using eBirdDataVisualizer.Contracts.ViewModels;
using eBirdDataVisualizer.Core.Contracts.Services;
using eBirdDataVisualizer.Core.Models;
using eBirdDataVisualizer.Core.Services;

namespace eBirdDataVisualizer.ViewModels;

public class DataGridViewModel : ObservableRecipient, INavigationAware
{
    private readonly BirdDataService birdDataService;

    public ObservableCollection<Bird> Source { get; } = new ObservableCollection<Bird>();

    public DataGridViewModel()
    {
        birdDataService = new BirdDataService();
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // TODO: Replace with real data.
        var data = await birdDataService.GetGridDataAsync();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
