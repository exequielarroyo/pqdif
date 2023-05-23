// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Data.Access;
using Data.Models;
using Gemstone.PQDIF.Logical;
using Gemstone.PQDIF.Physical;
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

namespace WinUI.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SavePage : Page
{
    public SavePage()
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

            var container = new Container()
            {
                CompressionAlgorithm = (int)_container.CompressionAlgorithm,
                CompressionStyle = (int)_container.CompressionStyle,
                Creation = _container.Creation,
                FileName = _container.FileName,
            };

            var source = new Source()
            {
                Container = container,
                ContainerId = container.Id,
                EquipmentId = _sources[0].EquipmentID.ToString(),
                Name = _sources[0].DataSourceName,
                Type =_sources[0].DataSourceTypeID.ToString(),
                VendorId = _sources[0].VendorID.ToString(),
            };

            SQLite.Containers.Add(container);
            SQLite.Sources.Add(source);
            MySQL.Containers.Add(container);
            MySQL.Sources.Add(source);

            foreach (ObservationRecord observation in Observations)
            {
                var newObservation = new Observation() { 
                    Container = container,
                    ContainerId = container.Id,
                    Name = observation.Name,
                    CreateAt = observation.CreateTime,
                    StartAt = observation.StartTime,
                    TriggerMethod = (int)observation.TriggerMethod,
                };
                SQLite.Observations.Add(newObservation);
                MySQL.Observations.Add(newObservation);

                foreach (ChannelInstance channel in observation.ChannelInstances)
                {
                    var newChannel = new Channel()
                    {
                        MeasuredId = (int)channel.Definition.QuantityMeasured,
                        Name = channel.Definition.ChannelName,
                        Observation = newObservation,
                        ObservationId = newObservation.Id,
                        PhaseId = (int)channel.Definition.Phase,
                    };
                    SQLite.Channels.Add(newChannel);
                    MySQL.Channels.Add(newChannel);

                    foreach (SeriesInstance series in channel.SeriesInstances)
                    {
                        var newSeries = new Series() { 
                            Channel = newChannel,
                            ChannelId = newChannel.Id,
                            CharacteristicId = 0,
                            TypeId = 0,
                            UnitsId = (int)series.Definition.QuantityUnits,
                            Values = JsonSerializer.Serialize(series.OriginalValues),
                        };
                        SQLite.Series.Add(newSeries);
                        MySQL.Series.Add(newSeries);

                        if (hasInternet == NetworkConnectivityLevel.InternetAccess)
                        {
                            //this.MySQL.Series.AddRange(newData);
                            //newData.IsSync = true;
                            //MySQL.SaveChanges();
                        }

                    }
                }
            }
            SQLite.SaveChanges();
            MySQL.SaveChanges();
            dialogLoading.IsActive = false;
        }
    }

    private ObservableCollection<ObservationRecord> Observations { get; set; } = new();
    private ContainerRecord _container;
    private List<DataSourceRecord> _sources;

    private async void PickAFileButton_Click(object sender, RoutedEventArgs e)
    {
        // Clear previous returned file name, if it exists, between iterations of this scenario
        PickAFileOutputTextBlock.Text = "";

        // Create a file picker
        var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

        // Retrieve the window handle (HWND) of the current WinUI 3 window.
        //var window = App.Window;
        var window = (Application.Current as App)?.m_window as BlankWindow;
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
            _container = logicalParser.ContainerRecord;
            _sources = logicalParser.DataSourceRecords;
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
