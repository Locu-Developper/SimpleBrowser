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
        private TabNode currentWebView;

        public MainWindow()
        {
            this.InitializeComponent();

            tabViewModel = new TabModel();

            Add_Tab(); // �����^�u��ǉ�
            //tabStack.DataContext = tabModel.Tabs;
        }


        private Uri ParseAddressBarInput(string input)
        {
            // ���Ƀv���g�R��������ꍇ
            if (Regex.IsMatch(input, @"^[a-zA-Z][a-zA-Z0-9+.-]*://"))
                return new Uri(input);

            // �h���C���`���̔���i��茵���j
            var domainPattern = @"^(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?\.)+[a-zA-Z]{2,}(?:/.*)?$";

            if (Regex.IsMatch(input, domainPattern))
            {
                return new Uri("https://" + input);
            }

            // IP�A�h���X�̏ꍇ
            if (Regex.IsMatch(input, @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(?::\d+)?(?:/.*)?$"))
            {
                return new Uri("http://" + input); // IP�A�h���X��http���f�t�H���g
            }


            // URI����Ȃ������ꍇ
            return new Uri("https://google.com/search?q=" + Uri.EscapeDataString(input));
        }

        private void SeartingProcess()
        {
            string input = AddressTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) return;

            Uri url = ParseAddressBarInput(input);
            tabViewModel.Tabs[tabListView.SelectedIndex].WebView.Source = url;
        }

        private void AddressBar_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == VirtualKey.Enter)
            {
                SeartingProcess();
                RootComponent.Focus(FocusState.Programmatic);
            }
        }

        private void Add_Tab()
        {
            tabViewModel.Tabs.Add(new TabNode());

            var index = tabViewModel.Tabs.Count - 1;
            if (index < 0)
            {
                index = 0; // �^�u���Ȃ��ꍇ�͍ŏ��̃^�u��I��
            }
            
            WebViewContainer.Children.Clear(); // ������WebView���N���A

            currentWebView = tabViewModel.Tabs[index];

            tabListView.SelectedIndex = index; // �V�����^�u��I��
            WebViewContainer.Children.Add(currentWebView.WebView); // �V����WebView��ݒ�
        }

        private void AddNewTab_Click(object sender, RoutedEventArgs e)
        {
            Add_Tab();
        }


        private async void RootComponent_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (WebViewContainer.Children[0] is WebView2 webView)
                {
                    await webView.EnsureCoreWebView2Async();
                    webView.CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded;

                } else
                {
                    Add_Tab();
                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading WebView: " + ex.Message);
            }
        }

        private void CoreWebView2_DOMContentLoaded(CoreWebView2 sender, CoreWebView2DOMContentLoadedEventArgs args)
        {
            Console.WriteLine("DOM�ǂݍ��݊���");

            this.DispatcherQueue.TryEnqueue(async () =>
            {
                // �����҂��Ă���^�C�g�����擾
                await Task.Delay(200);
                var url = sender.Source.ToString().Replace(" ", "%20");
                UpdateTabProperty(sender.DocumentTitle, url);
                AddressTextBox.Text = url;
            });
        }

        private void WebView_PropertyChanged()
        {
            var index = tabListView.SelectedIndex;
            Debug.WriteLine($"Selected Index: {index}");
            WebViewContainer.Children.Clear(); // ������WebView���N���A

            var selectedTab = tabViewModel.Tabs[index];
            WebViewContainer.Children.Add(selectedTab.WebView);
            currentWebView = selectedTab;
            AddressTextBox.Text = selectedTab.Url;
        }

        private void UpdateTabProperty(string title, string url)
        {
            var selectedTab = tabViewModel.Tabs[tabListView.SelectedIndex];
            selectedTab.Title = title;
            selectedTab.Url = url;
            Debug.WriteLine(tabViewModel.Tabs[tabListView.SelectedIndex].Title);
        }

        private void tabListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WebView_PropertyChanged();
        }
    }
}
