using System.Diagnostics;
using eBirdDataVisualizer.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Windows.Storage.Pickers;

namespace eBirdDataVisualizer.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }

    public async Task spawnFilePicker()
    {
        FileOpenPicker fileOpenPicker = new();
        fileOpenPicker.FileTypeFilter.Add("*");

        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(fileOpenPicker, hwnd);

        var file = await fileOpenPicker.PickSingleFileAsync();

        if (file is not null)
        {
            Debug.WriteLine("Success");
        }

    }

    private async void ImportHistogramDataButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await spawnFilePicker();
    }
}
