// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Data.Access;
using Data.Models;
using Gemstone.PQDIF.Logical;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class BlankPage1 : Page
{
    public BlankPage1()
    {
        this.InitializeComponent();
        NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
    }

    private NetworkConnectivityLevel hasInternet;
    private void NetworkInformation_NetworkStatusChanged(object sender) 
    {
        DispatcherQueue.TryEnqueue(() =>
        {
            ConnectionProfile profile = NetworkInformation.GetInternetConnectionProfile();
            hasInternet = profile?.GetNetworkConnectivityLevel() ?? NetworkConnectivityLevel.None;
            if (hasInternet == NetworkConnectivityLevel.InternetAccess)
            {
                MySQL = new MySQLContext();
            }
            else
            {
                SQLite = new SQLiteContext();
            }
        });
    }

    private DatabaseContext MySQL = new MySQLContext();
    private DatabaseContext SQLite = new SQLiteContext();
    private StorageFile file;
    private LogicalParser logicalParser;

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        ContentDialogResult result= await dialog.ShowAsync();

        if (result.Equals(ContentDialogResult.Primary))
        {
            dialogLoading.IsActive = true;

            foreach (ObservationRecord observation in Observations)
            {
                foreach (ChannelInstance channel in observation.ChannelInstances)
                {
                    foreach (SeriesInstance series in channel.SeriesInstances)
                    {
                        Series newData = new Series()
                        {
                            Offset = series.SeriesOffset.GetInt4(),
                            Scale = series.SeriesScale.GetInt4(),
                            Values = JsonSerializer.Serialize(series.OriginalValues),
                        };
                        SQLite.Series.Add(newData);

                        if (hasInternet == NetworkConnectivityLevel.InternetAccess)
                        {
                            this.MySQL.Series.AddRange(newData);
                            newData.IsSync = true;
                            MySQL.SaveChanges();
                        }

                        SQLite.SaveChanges();
                    }
                }
            }
            dialogLoading.IsActive = false;
        }
    }

    private ObservableCollection<ObservationRecord> Observations { get; set; } = new();

    private async void PickAFileButton_Click(object sender, RoutedEventArgs e)
    {
        // Clear previous returned file name, if it exists, between iterations of this scenario
        PickAFileOutputTextBlock.Text = "";

        // Create a file picker
        var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

        // Retrieve the window handle (HWND) of the current WinUI 3 window.
        var window = App.Window;
        //var window = (Application.Current as App)?.m_window as MainWindow;
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

        // Initialize the file picker with the window handle (HWND).
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

        // Set options for your file picker
        openPicker.ViewMode = PickerViewMode.Thumbnail;
        openPicker.FileTypeFilter.Add("*");

        // Open the picker for the user to pick a file
        file = await openPicker.PickSingleFileAsync();
        if (file != null)
        {
            PickAFileOutputTextBlock.Text = "Picked file: " + file.Name;

            logicalParser = new LogicalParser(file.Path);
            await logicalParser.OpenAsync();
            this.Observations.Clear();
            do
            {
                this.Observations.Add(await logicalParser.NextObservationRecordAsync());
            } while (await logicalParser.HasNextObservationRecordAsync());
            await logicalParser.CloseAsync();
        }
        else
        {
            PickAFileOutputTextBlock.Text = "Operation cancelled.";
        }
    }
}
