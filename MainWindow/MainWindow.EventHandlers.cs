using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MemoBrowser.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.Web.WebView2.Core;
using Windows.System;

namespace MemoBrowser;
public sealed partial class MainWindow
{
    private TabModel TabViewModel
    {
        get; set;
    }

    private async void RootComponent_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            // WebView2の初期化
            await AddNewTab();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading WebView: " + ex.Message);
        }
    }


    private void AddressBar_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Enter)
        {
            SeartingProcess();
            RootComponent.Focus(FocusState.Programmatic);
        }
    }



    private void DOMContentLoaded(CoreWebView2 sender, CoreWebView2DOMContentLoadedEventArgs args)
    {
        Console.WriteLine("DOM読み込み完了");

        DispatcherQueue.TryEnqueue(() => UpdateTabProperty(sender.DocumentTitle, sender.Source.ToString()));
    }

    private void CoreWebView2_NavigationCompleted(CoreWebView2 sender, CoreWebView2NavigationCompletedEventArgs args)
    {
        Console.WriteLine("ナビゲーション完了");
        DispatcherQueue.TryEnqueue(() =>
        {
            if (args.IsSuccess)
            {
                UpdateTabProperty(sender.DocumentTitle, sender.Source.ToString());
            }
            else
            {
                Console.WriteLine("ナビゲーション失敗: " + args.WebErrorStatus);
            }
        });
    }

    private void OnDocumentChanged(CoreWebView2 sender, object args)
    {
        Debug.WriteLine($"Document changed: {sender.DocumentTitle} ({sender.Source})");
        DispatcherQueue.TryEnqueue(() => UpdateTabProperty(sender.DocumentTitle, sender.Source.ToString()));
    }


    private void TabView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectedIndex = TabMainView.SelectedIndex;
        WebView_Swhich();
    }

    private async void TabMainView_AddTabButtonClick(TabView sender, object args)
    {
        await AddNewTab();
    }

    private async void PreferencesButton_Click(object sender, RoutedEventArgs e)
    {
        await AddNewTab("app://preferences");
    }
}
