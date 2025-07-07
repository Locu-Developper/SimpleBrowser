using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace MemoBrowser;
public sealed partial class MainWindow
{
    private void NewTabKeyboardAccelerator_Invoked(KeyboardAccelerator sender,
                                      KeyboardAcceleratorInvokedEventArgs args)
    {
        // Create new tab.
        TabView senderTabView = (TabView)args.Element;
        if (senderTabView is not null)
        {
            // (Click handler defined in previous example.)
            TabMainView_AddTabButtonClick(senderTabView, new EventArgs());
        }
        args.Handled = true;
    }

    private void CloseSelectedTabKeyboardAccelerator_Invoked(KeyboardAccelerator sender,
                                                    KeyboardAcceleratorInvokedEventArgs args)
    {
        CloseSelectedTab(TabMainView.SelectedIndex);
        args.Handled = true;
    }

    private void TabMainView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
    {
        CloseSelectedTab(TabMainView.SelectedIndex);
    }

    private async void CloseSelectedTab(int index)
    {
        TabViewModel.Tabs.RemoveAt(index);

        if (TabViewModel.Tabs.Count == 0)
        {
            // If no tabs left, add a new tab.
            await AddNewTab();
        }
        else if (index >= TabViewModel.Tabs.Count)
        {
            // If the closed tab was the last one, select the previous tab.
            TabMainView.SelectedIndex = TabViewModel.Tabs.Count - 1;
        }
        else
        {
            // Select the tab that was closed.
            TabMainView.SelectedIndex = index;
        }
    }



}
