using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.Web.WebView2.Core;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MemoBrowser;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    public int SelectedIndex = 0;
    private ObservableCollection<TabNode> Tabs 
    {
        get; set;
    }
    public static Window? window;

    public MainWindow()
    {
        this.InitializeComponent();

        this.ExtendsContentIntoTitleBar = true;

        window = this;

        TabViewModel = new TabModel();
        Tabs = TabViewModel.Tabs;
    }

}
