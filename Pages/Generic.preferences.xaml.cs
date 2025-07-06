using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using MemoBrowser.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MemoBrowser.Pages;
public sealed partial class Generic : UserControl
{
    public Generic()
    {
        InitializeComponent();
    }

    private void DefaultSearchEngineComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DefaultSearchEngineComboBox.SelectedItem is ComboBoxItem selectedItem)
        {
            Debug.WriteLine($"Selected search engine: {selectedItem.Tag}");
            AppSettingsManager.DefaultSearchEngine = (string)selectedItem.Tag;
        }
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        DefaultSearchEngineComboBox.SelectedValue = AppSettingsManager.DefaultSearchEngine; // èâä˙ílÇÃê›íË

        var downloadPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Downloads"
        );
        DownloadPath.PlaceholderText = downloadPath;
    }

    private async void PickFolderButton_Click(object sender, RoutedEventArgs e)
    {
        PickFolderButton.IsEnabled = false;

        // Clear previous returned file name, if it exists, between iterations of this scenario

        // Create a folder picker
        var openPicker = new Windows.Storage.Pickers.FolderPicker();

        // Retrieve the window handle (HWND) of the current WinUI 3 window.
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(MainWindow.window);

        // Initialize the folder picker with the window handle (HWND).
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

        // Set options for your folder picker
        openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
        openPicker.FileTypeFilter.Add("*");

        // Open the picker for the user to pick a folder
        StorageFolder folder = await openPicker.PickSingleFolderAsync();
        if (folder != null)
        {
            //StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
            DownloadPath.Text = folder.Path;
        }

        //re-enable the button
        PickFolderButton.IsEnabled = true;
    }

    private void DownloadPath_TextChanged(object sender, TextChangedEventArgs e)
    {
        AppSettingsManager.Set("DownloadPath", DownloadPath.Text);
    }
}
