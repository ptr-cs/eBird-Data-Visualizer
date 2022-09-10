using System.Diagnostics;
using System.Numerics;
using eBirdDataVisualizer.Core.Services;
using eBirdDataVisualizer.ViewModels;
using Microsoft.UI;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Navigation;
using Windows.Storage.Pickers;

namespace eBirdDataVisualizer.Views;

public sealed partial class MainPage : Page
{
    // Binding for the TeachingTip that displays the import result success/failure message:
    readonly Binding importResultBinding;

    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
        importResultBinding = new Binding() { Path = new(nameof(ViewModel.ShowImportResult)), Source = ViewModel, Mode = BindingMode.OneWay };
    }

    private void TeachingTip_Closed(TeachingTip sender, TeachingTipClosedEventArgs args)
    {
        ViewModel.ShowImportResult = false;
        // Since activating the close button on the TeachingTip removes the binding set
        // upon the IsOpenProperty, re-set the binding whenever the TeachingTip is closed:
        BindingOperations.SetBinding(sender, TeachingTip.IsOpenProperty, importResultBinding);
    }
}
