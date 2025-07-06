using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace MemoBrowser;
public sealed partial class MainWindow
{
    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        NavigationProcess("back");
    }

    private void ForwardButton_Click(object sender, RoutedEventArgs e)
    {
        NavigationProcess("forward");
    }

    private void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        NavigationProcess("reload");
    }

    private void NavigationProcess(string tag)
    {
        
        if (Tabs[SelectedIndex] == null)
        {
            return;
        }

        if (!Tabs[SelectedIndex].IsInternalPage)
        {
            switch (tag)
            {
                case "back":
                    Tabs[SelectedIndex]?.WebView?.GoBack();
                    break;
                case "forward":
                    Tabs[SelectedIndex]?.WebView?.GoForward();
                    break;
                case "reload":
                    Tabs[SelectedIndex]?.WebView?.Reload();
                    break;
                default:
                    break;
            }
        }
    }
}
