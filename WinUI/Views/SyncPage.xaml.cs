// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Data.Access;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Win32;
using Microsoft.Windows.AppLifecycle;
using MySqlX.XDevAPI;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SyncPage : Page
{
    public SyncPage()
    {
        this.InitializeComponent();
    }

    private async void chooseButton_Click(object sender, RoutedEventArgs e)
    {
        FolderPicker folderPicker = new FolderPicker();

        //var window = App.Window;
        var window = (Application.Current as App)?.m_window as BlankWindow;
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

        WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hWnd);

        StorageFolder folder = await folderPicker.PickSingleFolderAsync();

        if (folder != null)
        {
            StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
            chooseLabel.Text = $"Picked folder: {folder.Path}";

            FileSystemWatcher watcher = new FileSystemWatcher(folder.Path);
            watcher.Filter = "*.*";
            watcher.Created += Watcher_Created;
            watcher.EnableRaisingEvents = true;
        }

        await SaveReg();
    }

    private async Task SaveReg()
    {
        using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
        if (registryKey != null)
        {
            dialog.Content = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            await dialog.ShowAsync();
        }
        registryKey.SetValue("Winui", $"{System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName}", RegistryValueKind.String);
        registryKey.Close();
    }
    private void Watcher_Created(object sender, FileSystemEventArgs e)
    {

        DispatcherQueue.TryEnqueue(() =>
        {
            chooseLabel.Text = $"New file: {e.Name}";
        });
        
    }
}
