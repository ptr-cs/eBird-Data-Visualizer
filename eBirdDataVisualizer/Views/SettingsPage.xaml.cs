﻿using eBirdDataVisualizer.Core.Services;
using eBirdDataVisualizer.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace eBirdDataVisualizer.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
    }

    //private void ClearSampleDataButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    //{
    //    ViewModel._birdDataService.ClearData();
    //}
}
