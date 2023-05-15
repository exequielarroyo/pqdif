// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using WinRT; // required to support Window.As<ICompositionSupportsSystemBackdrop>()
using Microsoft.UI.Composition.SystemBackdrops;
using System.Runtime.InteropServices; // For DllImport
using CommunityToolkit.WinUI.UI.Triggers;
using Data.Access;
using Data.Models;
using Gemstone.Collections.CollectionExtensions;
using Gemstone.PQDIF.Logical;
using GSF;
using GSF.Xml;
using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Windowing;
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
using System.Xml;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.ApplicationModel.Core;
using System.Drawing;
using WinUIEx;
using CommunityToolkit.WinUI;
using Windows.Networking.Connectivity;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        this.InitializeComponent();

        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);
        //Content = null;
        //AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/StoreLogo.png"));
        //Title = "PQDIF";
        //TrySetSystemBackdrop();
        NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;

        this.get_Data();
        this.get_PQDIF_Data();
        
    }

    private void NetworkInformation_NetworkStatusChanged(object sender) 
    {
        DispatcherQueue.TryEnqueue(() =>
        {
            ConnectionProfile profile = NetworkInformation.GetInternetConnectionProfile();
            var level = profile?.GetNetworkConnectivityLevel() ?? NetworkConnectivityLevel.None;
            if (level == NetworkConnectivityLevel.InternetAccess)
            {
                AppTitleBarText.Text = "Internet Connected";
            }
            else
            {
                AppTitleBarText.Text = "No Internet Connection";
            }
        });
    }

    private void myButton_Click(object sender, RoutedEventArgs e)
    {
        //myButton.Content = "Saved";
        this.save_To_XML();
    }

    public Blog Blog { get; set; }
    public ObservableCollection<Blog> Blogs { get; set; } = new();
    public DatabaseContext Database = new();
    private ObservableCollection<ObservationRecord> Observations{ get; set; } = new();

    private void load(object sender, WindowActivatedEventArgs e)
    {

    }

    private void get_Data()
    {
        DbSet<Series> series = Database.Series;
        foreach (var data in series)
        {
            //string values = JsonSerializer.Deserialize<string>(data.Values);

            string[] stringNumbers = data.Values.Trim('[', ']').Split(',');

            double[] numbers = new double[stringNumbers.Length];
            for (int i = 0; i < stringNumbers.Length; i++)
            {
                //double.TryParse(stringNumbers[i].Trim(), out numbers[i]);
                numbers[i] = double.Parse(stringNumbers[i].Trim());
            }
        }

        //this.series = series.First();

        //foreach (Blog blog in blogs)
        //{
        //    this.Blogs.Add(blog);
        //}

        ////var data = from blog in blogs
        ////           where blog.BlogId == 1
        ////           select blog;

        //textBox.Text = blogs.First().Url;
    }

    private async void get_PQDIF_Data()
    {
        string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string path = Path.Combine(documents, "example-pqdif-native/example 01.pqd");

        LogicalParser logicalParser = new LogicalParser(path);
        await logicalParser.OpenAsync();
        //ObservationRecord observationRecord = await logicalParser.NextObservationRecordAsync();

        do
        {
            this.Observations.Add(await logicalParser.NextObservationRecordAsync());
        } while (await logicalParser.HasNextObservationRecordAsync());


        // SAVE TO DATABASE
        //foreach (ChannelInstance channel in observationRecord.ChannelInstances)
        //{
        //    foreach (SeriesInstance series in channel.SeriesInstances)
        //    {
        //        this.Database.Series.Add(new Series()
        //        {
        //            Offset = series.SeriesOffset.GetInt4(),
        //            Scale = series.SeriesScale.GetInt4(),
        //            Values = JsonSerializer.Serialize(series.OriginalValues),
        //        });
        //    }
        //}
        //Database.SaveChanges();
    }

    private void save_To_XML()
    {
        string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string path = Path.Combine(documents, @"example-pqdif-native\sample.xml");

        XmlDocument xmlDocument = new XmlDocument();
        //xmlDocument.Load(path);
        xmlDocument.LoadXml("<?xml version=\"1.0\"?> \n" +
"<books xmlns=\"http://www.contoso.com/books\"> \n" +
"  <book genre=\"novel\" ISBN=\"1-861001-57-8\" publicationdate=\"1823-01-28\"> \n" +
"    <title>Pride And Prejudice</title> \n" +
"    <price>24.95</price> \n" +
"  </book> \n" +
"  <book genre=\"novel\" ISBN=\"1-861002-30-1\" publicationdate=\"1985-01-01\"> \n" +
"    <title>The Handmaid's Tale</title> \n" +
"    <price>29.95</price> \n" +
"  </book> \n" +
"</books>");

        //XmlElement root = xmlDocument.CreateElement("Root");
        //root.AppendChild(xmlDocument.CreateNode(XmlNodeType.Element, "Child", null));
        //xmlDocument.AppendChild(root);

        //xmlDocument.Save(path);
        XmlNode xmlNode = XmlExtensions.GetXmlNode(xmlDocument, path);
    }

    private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs e)
    {
        var tag = sender;
        //Type _page = null;
        //if (e.SelectedItemContainer != null)
        //{
        //    var item = _pages.FirstOrDefault(p => p.Tag.Equals(e.SelectedItem));
        //    _page = item.Page;
        //    contentFrame.Navigate("SamplePage1", e.RecommendedNavigationTransitionInfo);
        //}
    }

    private void navigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        Type to = null;
        switch (args.InvokedItem)
        {
            case "SQLite":
                to = typeof(BlankPage1);
                navigationView.Header = "Working with SQLite";
                navigationView.PaneTitle = "Working with SQLite";
                break;
            case "XML":
                to = typeof(BlankPage2);
                navigationView.Header = "Working with XML";
                navigationView.PaneTitle = "Working with XML";
                break;
            default:
                break;
        }

        contentFrame.Navigate(to, args.RecommendedNavigationTransitionInfo);
    }

    private void navigationView_Loaded(object sender, RoutedEventArgs e)
    {
        contentFrame.Navigate(typeof(BlankPage1));
    }
}
