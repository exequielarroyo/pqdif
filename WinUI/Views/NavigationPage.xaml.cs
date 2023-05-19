// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class NavigationPage : Page
{
    public NavigationPage()
    {
        this.InitializeComponent();

        NavigationViewControl.SelectedItem = NavigationViewControl.MenuItems.OfType<NavigationViewItem>().First();
        ContentFrame.Navigate(
                   typeof(Views.SyncPage),
                   null,
                   new Microsoft.UI.Xaml.Media.Animation.EntranceNavigationTransitionInfo()
                   );
    }

    public string ProfileImage = "./../Assets/IMG-2276.png";

    private void NavigationViewControl_ItemInvoked(NavigationView sender,
                  NavigationViewItemInvokedEventArgs args)
    {
        if (args.IsSettingsInvoked == true)
        {
            ContentFrame.Navigate(typeof(Views.PreviewPage), null, args.RecommendedNavigationTransitionInfo);
        }
        else if (args.InvokedItemContainer != null && (args.InvokedItemContainer.Tag != null))
        {
            Type newPage = Type.GetType(args.InvokedItemContainer.Tag.ToString());
            ContentFrame.Navigate(
                   newPage,
                   null,
                   args.RecommendedNavigationTransitionInfo
                   );
        }
    }

    private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
    {
        NavigationViewControl.IsBackEnabled = ContentFrame.CanGoBack;

        if (ContentFrame.SourcePageType == typeof(Views.NavigationPage))
        {
            // SettingsItem is not part of NavView.MenuItems, and doesn't have a Tag.
            NavigationViewControl.SelectedItem = (NavigationViewItem)NavigationViewControl.SettingsItem;
        }
        else if (ContentFrame.SourcePageType != null)
        {
            NavigationViewControl.SelectedItem = NavigationViewControl.MenuItems.Concat(SecondNav.MenuItems)
                .OfType<NavigationViewItem>()
                .First(n =>
                {
                    if (n.Tag != null)
                    {
                        return n.Tag.Equals(ContentFrame.SourcePageType.FullName.ToString());
                    }
                    return false;
                });
        }

        NavigationViewControl.Header = ((NavigationViewItem)NavigationViewControl.SelectedItem)?.Content?.ToString();
    }

    private void NavigationViewControl_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
    {
        AppTitleBar.Margin = new Thickness()
        {
            Left = sender.CompactPaneLength * (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? 2 : 1),
            Top = AppTitleBar.Margin.Top,
            Right = AppTitleBar.Margin.Right,
            Bottom = AppTitleBar.Margin.Bottom
        };

        //personPicture.Margin = new Thickness()
        //{
        //    Left = (sender.DisplayMode == NavigationViewDisplayMode.Minimal ? -8 : 0),
        //    Top = personPicture.Margin.Top,
        //    Right = personPicture.Margin.Right,
        //    Bottom = personPicture.Margin.Bottom
        //};
    }

    private void NavigationViewControl_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
    {
        if (ContentFrame.CanGoBack) ContentFrame.GoBack();
    }
}
