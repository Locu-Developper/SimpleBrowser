using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Text.RegularExpressions;
using Windows.System;
using System.Collections.ObjectModel;

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

            // Ç∆ÇËÇ†Ç¶Ç∏http or httpsÇ©ÇÁénÇ‹ÇÈÇ‡ÇÃÇÃÇ›URIâª
            if (urlRegex.IsMatch(inputText))
            {
                return new Uri(inputText); 
            }

            // URIÇ∂Ç·Ç»Ç©Ç¡ÇΩèÍçá
            return new Uri("https://google.com/search?q=" + Uri.EscapeDataString(inputText));
        }

        private void SeartingMain()
        {
            string inputText = AddressTextBox.Text.Trim();
            if (inputText.Equals("")) return;

            Uri url = ParseAddressBarInput(inputText);
            webView.Source = url;
        }


        private void SearchingButton_Click(object sender, RoutedEventArgs e){
            SeartingMain();
        }

        private void AddressBar_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == VirtualKey.Enter)
            {
                SeartingMain();
                AddressTextBox.Focus(FocusState.Unfocused);
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
    }
}
