using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using MemoBrowser.Pages;
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

namespace MemoBrowser;
public sealed partial class InternalPageControl : UserControl
{
    public InternalPageControl()
    {
        InitializeComponent();
    }


    private void navInternalMenu_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        var tag = args.SelectedItemContainer.Tag;
        
        switch (tag)
        {
            case "Generic":
                contentFrame.Content = new Generic();
                break;
            case "Other":
                contentFrame.Content = new Other();
                break;
            case "Help":
                contentFrame.Content = new Help();
                break;
            default:
                Debug.WriteLine("Unknown page selected");
                break;
        }
    }
}
