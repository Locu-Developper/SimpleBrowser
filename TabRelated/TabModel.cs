using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MemoBrowser;

public partial class TabModel: ObservableObject
{
    private ObservableCollection<TabNode> _tabs = [];
    public ObservableCollection<TabNode> Tabs
    {
        get => _tabs;
        set
        {
            if (value != _tabs)
            {
                SetProperty(ref _tabs, value);
            }
        }
    }
}
