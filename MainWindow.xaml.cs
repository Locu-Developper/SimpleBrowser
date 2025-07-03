using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.Web.WebView2.Core;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MemoBrowser
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private TabModel tabViewModel;
        private TabNode ?currentWebView { get; set; }

        public MainWindow()
        {
            this.InitializeComponent();

            tabViewModel = new TabModel();

            //await Add_Tab(); // 初期タブを追加
            
        }


        private Uri ParseAddressBarInput(string input)
        {
            // 既にプロトコルがある場合
            if (Regex.IsMatch(input, @"^[a-zA-Z][a-zA-Z0-9+.-]*://"))
                return new Uri(input);

            // ドメイン形式の判定（より厳密）
            var domainPattern = @"^(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?\.)+[a-zA-Z]{2,}(?:/.*)?$";

            if (Regex.IsMatch(input, domainPattern))
            {
                return new Uri("https://" + input);
            }

            // IPアドレスの場合
            if (Regex.IsMatch(input, @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(?::\d+)?(?:/.*)?$"))
            {
                return new Uri("http://" + input); // IPアドレスはhttpをデフォルト
            }


            // URIじゃなかった場合
            return new Uri("https://google.com/search?q=" + Uri.EscapeDataString(input));
        }

        private void SeartingProcess()
        {
            string input = AddressTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) return;

            Uri url = ParseAddressBarInput(input);
            //tabViewModel.Tabs[tabListView.SelectedIndex].WebView.Source = url;
        }

        private void AddressBar_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == VirtualKey.Enter)
            {
                SeartingProcess();
                RootComponent.Focus(FocusState.Programmatic);
            }
        }

        private async Task Add_Tab()
        {
            WebView2 webView = new();
            await webView.EnsureCoreWebView2Async();

            TabNode tabNode = new(webView);
            tabViewModel.Tabs.Add(tabNode);

            TabViewItem tabItem = new TabViewItem
            {
                Header = tabNode.Title,
                Content = tabNode.WebView,
                Tag = tabNode
            };

            TabMainView.TabItems.Add(tabItem); // 新しいWebViewを設定
            TabMainView.SelectedItem = tabItem; // 新しいタブを選択

            //var index = tabViewModel.Tabs.Count - 1;
            //Debug.WriteLine($"Adding new tab at index: {index}");
            //if (index < 0)
            //{
            //    index = 0; // タブがない場合は最初のタブを選択
            //} else if (currentWebView != null)
            //{
            //    WebViewContainer.TabItems.Remove(currentWebView.WebView); // 既存のWebViewをクリア
            //}

            //tabListView.SelectedIndex = index; // 新しいタブを選択
            //currentWebView = tabViewModel.Tabs[index];
            
        }

        private async void AddNewTab_Click(object sender, RoutedEventArgs e)
        {
            await Add_Tab();
        }


        private async void RootComponent_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await Add_Tab();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading WebView: " + ex.Message);
            }
        }

        private void CoreWebView2_DOMContentLoaded(CoreWebView2 sender, CoreWebView2DOMContentLoadedEventArgs args)
        {
            Console.WriteLine("DOM読み込み完了");

            this.DispatcherQueue.TryEnqueue(async () =>
            {
                // 少し待ってからタイトルを取得
                await Task.Delay(200);
                var url = sender.Source.ToString().Replace(" ", "%20");
                UpdateTabProperty(sender.DocumentTitle, url);
                AddressTextBox.Text = url;
            });
        }

        private void WebView_PropertyChanged()
        {
            //var index = tabListView.SelectedIndex;
            //Debug.WriteLine($"Selected Index: {index}");

            //var selectedTab = tabViewModel.Tabs[index];
            //TabMainView.SelectedItem = selectedTab;
            //AddressTextBox.Text = selectedTab.Url;
        }

        private void UpdateTabProperty(string title, string url)
        {
            //var selectedTab = tabViewModel.Tabs[tabListView.SelectedIndex];
            //selectedTab.Title = title;
            //selectedTab.Url = url;
            //Debug.WriteLine(tabViewModel.Tabs[tabListView.SelectedIndex].Title);
        }

        private void tabListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WebView_PropertyChanged();
        }
    }
}
