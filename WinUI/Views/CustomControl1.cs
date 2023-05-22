// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using H.NotifyIcon;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI.Views;
public sealed class CustomControl1 : Control
{
    public CustomControl1()
    {
        this.DefaultStyleKey = typeof(CustomControl1);


        
        //TrySetMicaBackdrop();


        var command = new XamlUICommand();
        var showHide = new XamlUICommand();
        command.ExecuteRequested += Command_ExecuteRequested;
        showHide.ExecuteRequested += ShowHide_ExecuteRequested;

        TaskbarIcon taskbarIcon = new TaskbarIcon();

        var contextFlyout = new MenuFlyout();
        contextFlyout.Items.Add(new MenuFlyoutItem() { Command = showHide, Text = "Show/Hide Window" });
        contextFlyout.Items.Add(new MenuFlyoutItem() { Command = command, Text = "Exit" });
        taskbarIcon.ContextFlyout = contextFlyout;

        taskbarIcon.ForceCreate();
    }

    private void Command_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
    {
        App.Current.Exit();
    }

    private AppWindow appWindow;

    private void ShowHide_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
    {
        var window = (Application.Current as App).m_window;
        if (window.AppWindow.IsVisible)
        {
            window.Hide();
        }
        else
        {
            window.Activate();
        }
    }
    private void SizeChanged(object sender, WindowSizeChangedEventArgs args)
    {
        //Update the title bar draggable region. We need to indent from the left both for the nav back button and to avoid the system menu
        Windows.Graphics.RectInt32[] rects = new Windows.Graphics.RectInt32[] { new Windows.Graphics.RectInt32(96, 0, (int)args.Size.Width - 48, 48) };
        appWindow.TitleBar.SetDragRectangles(rects);
    }

    private AppWindow GetAppWindow(Window window)
    {
        IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        WindowId windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
        return AppWindow.GetFromWindowId(windowId);
    }

}
