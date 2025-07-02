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

        public MainWindow()
        {
            this.InitializeComponent();

            tabViewModel = new TabModel();

            tabViewModel.Tabs.Add(new TabNode());
            //tabStack.DataContext = tabModel.Tabs;
        }


        private Uri ParseAddressBarInput(string inputText)
        {
            string urlPattern = @"^https?://.*";
            Regex urlRegex = new Regex(urlPattern);

            // �Ƃ肠����http or https����n�܂���̂̂�URI��
            if (urlRegex.IsMatch(inputText))
            {
                return new Uri(inputText); 
            }

            // URI����Ȃ������ꍇ
            return new Uri("https://google.com/search?q=" + Uri.EscapeDataString(inputText));
        }

        private void SeartingProcess()
        {
            string inputText = AddressTextBox.Text.Trim();
            if (inputText.Equals("")) return;

            Uri url = ParseAddressBarInput(inputText);
            webView.Source = url;
        }

        private void AddressBar_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == VirtualKey.Enter)
            {
                SeartingProcess();
                RootComponent.Focus(FocusState.Programmatic);
            }
        }

        void Add_Tab()
        {
            tabViewModel.Tabs.Add(new TabNode());
        }

        private void AddNewTab_Click(object sender, RoutedEventArgs e)
        {
            Add_Tab();
        }


        private async void RootComponent_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await webView.EnsureCoreWebView2Async();

                webView.CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded;
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
                await Task.Delay(500);
                Debug.WriteLine("�^�C�g��: " + webView.CoreWebView2.DocumentTitle);
                Debug.WriteLine("URL: " + tabListView.SelectedIndex);
                tabViewModel.Tabs[tabListView.SelectedIndex].Url = webView.CoreWebView2.DocumentTitle;
            });
        }
    }
}
