using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using MemoBrowser.Helpers;

namespace MemoBrowser;

public partial class TabNode : ObservableObject
{

    public static string DefaultUrl ()
    {
        var url = AppSettingsManager.Get("DefaultSearchEngineUrl", "www.google.com").Split("/")[0];
        return $"https://{url}";
    }

    /**
     * WebView2用 コンストラクタ
     * */
    public TabNode(ref WebView2 webView)
    {
        Url = DefaultUrl();
        _title = "New Tab";
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
        _title = title;
        Url = url;
        UserControl = userControl;
    }

    private string _title;

    public bool IsInternalPage
    {
        get; private set;
    }

    public string Title
    {
        get => _title; set => SetProperty(ref _title, value);
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
