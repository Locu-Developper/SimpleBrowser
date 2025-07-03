using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;

namespace MemoBrowser
{
    class TabNode : ObservableObject
    {
        private static string DEFAULT_URI = "https://www.google.com";
        public TabNode(WebView2 webView)
        {
            this.webView =webView;
            this.webView.Source = new Uri(DEFAULT_URI); // 初期URLを設定
            title = "New Tab";
            url = DEFAULT_URI; // 初期URLを設定
        }

        private string title;
        private string url;
        private WebView2 webView;

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public string Url
        {
            get { return url; }
            set
            {
                SetProperty(ref url, value);
                webView.Source = new Uri(value); // URLが変更されたらWebViewのソースも更新
            }
        }

        public WebView2 WebView
        {
            get { return webView; }
            set { SetProperty(ref webView, value); }
        }
    }
}
