using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoBrowser
{
    class TabModel
    {
        public ObservableCollection<TabNode> Tabs = new ObservableCollection<TabNode>();
        public TabModel()
        {}
    }
}
