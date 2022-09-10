using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using eBirdDataVisualizer.Core.Services;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Windows.Storage.Pickers;

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

    public MainViewModel(IBirdDataService birdDataService)
    {
        _birdDataService = birdDataService;

        ImportHistogramDataCommand = new RelayCommand(async () =>
        {
            await spawnFilePicker();
        });

        CloseImportResultCommand = new RelayCommand(() =>
        {
            ShowImportResult = false;
        });
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
            var importResult = false;
            try
            {
                var text = await Windows.Storage.FileIO.ReadTextAsync(file);
                importResult = await _birdDataService.ParseData(text);
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
    }
}
