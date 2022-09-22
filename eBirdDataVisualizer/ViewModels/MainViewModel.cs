using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using eBirdDataVisualizer.Core.Services;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace eBirdDataVisualizer.ViewModels;

public class MainViewModel : ObservableRecipient
{
    private readonly IBirdDataService _birdDataService;

    public ICommand ImportHistogramDataCommand
    {
        get;
    }

    public ICommand CloseImportResultCommand
    {
        get;
    }

    private bool showImportResult = false;
    public bool ShowImportResult
    {
        get => showImportResult;
        set => SetProperty(ref showImportResult, value);
    }

    private string importResultText = "Import Result";
    public string ImportResultText
    {
        get => importResultText;
        set => SetProperty(ref importResultText, value);
    }

    private Symbol importResultSymbol = Symbol.Help;
    public Symbol ImportResultSymbol
    {
        get => importResultSymbol;
        set => SetProperty(ref importResultSymbol, value);
    }

    public async void LoadSampleData()
    {
        var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/sample_US-HI__1900_2022_1_12_barchart.txt"));
        if (file is not null)
        {
            await _birdDataService.ParseMetadata(file.Name);
            var text = System.IO.File.ReadAllText(file.Path);
            await _birdDataService.ParseData(text);
        }
    }

    public MainViewModel(IBirdDataService birdDataService)
    {
        _birdDataService = birdDataService;

        LoadSampleData();

        ImportHistogramDataCommand = new RelayCommand(async () =>
        {
            await spawnFilePicker();
        });

        CloseImportResultCommand = new RelayCommand(() =>
        {
            ShowImportResult = false;
        });
    }

    public async Task<bool?> spawnFilePicker()
    {
        FileOpenPicker fileOpenPicker = new();
        fileOpenPicker.FileTypeFilter.Add("*");

        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(fileOpenPicker, hwnd);

        var file = await fileOpenPicker.PickSingleFileAsync();

        bool? result = false;

        if (file is not null)
        {
            var importResult = false;
            try
            {
                await _birdDataService.ParseMetadata(file.Name);

                var text = await Windows.Storage.FileIO.ReadTextAsync(file);
                importResult = await _birdDataService.ParseData(text);
                result = importResult;

                if (importResult)
                    ImportResultText = $"{file.Name} successfully imported!";
                else
                    ImportResultText = $"Failed to parse {file.Name}.";
            }
            catch (Exception)
            {
                ImportResultText = $"Failed to import {file.Name}.";
                ShowImportResult = true;
            }

            if (importResult)
                ImportResultSymbol = Symbol.Accept;
            else
                ImportResultSymbol = Symbol.Important;

            ShowImportResult = true;
        }
        return result;
    }
}
