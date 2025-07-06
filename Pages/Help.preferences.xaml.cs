using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MemoBrowser.Pages;
public sealed partial class Help : UserControl
{
    public Help()
    {
        InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        var repo = "https://github.com/Locu-Developper/MemoBrowser.git";

        version.Text = $"Version: {App.Current.GetType().Assembly.GetName().Version?.ToString() ?? "1.0.0"}";
        developper.Text = "Developper: Locu";
        repositoryUrl.NavigateUri = new Uri(repo);
    }
}
