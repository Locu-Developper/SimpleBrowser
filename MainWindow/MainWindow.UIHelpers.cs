using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MemoBrowser.Helpers;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.WebUI;

namespace MemoBrowser;
public sealed partial class MainWindow
{
    private void WebView_Swhich()
    {
        var index = TabMainView.SelectedIndex;

        if (index < 0 || index >= TabViewModel.Tabs.Count) return;

        this.DispatcherQueue.TryEnqueue(() =>
        {
            UpdateMainContentArea(Tabs[SelectedIndex]);
        });
    }

    private void UpdateTabProperty(string title, string url)
    {
        Debug.WriteLine($"Updating tab: {title} ({url})");
        if (Tabs[SelectedIndex] == null) return;
        Tabs[SelectedIndex].Title = title;
        Tabs[SelectedIndex].Url = url;
        UpdateAddressBar(Tabs[SelectedIndex]);

        Debug.WriteLine($"Tab updated: {Tabs[SelectedIndex].Title} ({Tabs[SelectedIndex].Url})");
        //AddressTextBox.Text = url;
    }

    private void UpdateMainContentArea(TabNode tabNode)
    {
        TabContentArea.Children.Clear();
        if (SelectedIndex >= 0 && SelectedIndex < Tabs.Count)
        {
            if(tabNode.IsInternalPage)
            {
                TabContentArea.Children.Add(tabNode.UserControl);
            }
            else if (tabNode.WebView != null)
            {
                TabContentArea.Children.Add(tabNode.WebView);
            }
        }

        UpdateAddressBar(tabNode);
    }

    private void UpdateAddressBar(TabNode tabNode)
    {
        if (string.IsNullOrWhiteSpace(tabNode.Url)) return;

        AddressTextBox.Text = tabNode.Url;
    }

    private void SeartingProcess()
    {
        var input = AddressTextBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(input)) return;

        var url = OtherHelper.ParseAddressBarInput(input).OriginalString;
        var webView = Tabs[SelectedIndex].WebView;
        if (Tabs[SelectedIndex] != null && webView != null)
        {
            Tabs[SelectedIndex].Url = url;

            if (!Tabs[SelectedIndex].IsInternalPage)
            {
                webView.CoreWebView2.Navigate(url);
            }

        }
        else
        {
            Debug.WriteLine("現在のWebViewが設定されていません。");
        }
        UpdateAddressBar(Tabs[SelectedIndex]);
    }

    private async Task AddNewTab(string? url = null)
    {
        TabNode tabNode;

        if (OtherHelper.IsInternalUrl(url))
        {
            UserControl userControl = new InternalPageControl();
            tabNode = new TabNode(userControl, "アプリケーション設定", url ?? "app://preferences");
        }
        else
        {
            WebView2 webView = new();
            await webView.EnsureCoreWebView2Async();

            var coreWebView2 = webView.CoreWebView2;
            coreWebView2.DOMContentLoaded += DOMContentLoaded;
            coreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
            coreWebView2.DocumentTitleChanged += OnDocumentChanged;

            tabNode = new(ref webView);
        }

        Tabs.Add(tabNode);
        TabMainView.SelectedIndex = Tabs.Count - 1; // 新しいタブを選択
        SelectedIndex = TabMainView.SelectedIndex; // 現在のインデックスを更新  

        UpdateMainContentArea(tabNode);
        UpdateAddressBar(tabNode);
    }


}
