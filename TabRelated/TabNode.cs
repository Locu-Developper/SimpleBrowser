using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;

namespace MemoBrowser;

public partial class TabNode : ObservableObject
{
    private static readonly string DEFAULT_URI = "https://www.google.com";

    /**
     * WebView2用 コンストラクタ
     * */
    public TabNode(ref WebView2 webView)
    {
        Url = DEFAULT_URI;
        Title = "New Tab";
        WebView = webView;
        WebView.Source = new Uri(Url);

        IsInternalPage = false;
    }

    /* *
     * 内部ページ用 コンストラクタ
     * */
    public TabNode(UserControl userControl, string title, string url)
    {
        IsInternalPage = true;
        Title = title;
        Url = url;
        UserControl = userControl;
    }

    public bool IsInternalPage
    {
        get; private set;
    }

    public string Title
    {
        get; set;
    }

    public string Url
    {
        get; set;
    }

    public WebView2? WebView
    {
        get; private set;
    } = null;

    public UserControl? UserControl
    {
        get; private set;
    } = null;
}
