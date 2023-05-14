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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            //TrySetSystemBackdrop();
            this.get_Data();
            this.get_PQDIF_Data();
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            //myButton.Content = "Saved";
            this.save_To_XML();
        }

        public Blog Blog { get; set; }
        public ObservableCollection<Blog> Blogs { get; set; } = new();
        public DatabaseContext Database = new DatabaseContext();
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

        async private void get_PQDIF_Data()
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

        WindowsSystemDispatcherQueueHelper m_wsdqHelper; // See below for implementation.
        MicaController m_backdropController;
        SystemBackdropConfiguration m_configurationSource;
        AppWindow m_AppWindow;
        bool TrySetSystemBackdrop()
        {
            if (Microsoft.UI.Composition.SystemBackdrops.MicaController.IsSupported())
            {
                m_wsdqHelper = new WindowsSystemDispatcherQueueHelper();
                m_wsdqHelper.EnsureWindowsSystemDispatcherQueueController();

                // Create the policy object.
                m_configurationSource = new SystemBackdropConfiguration();
                this.Activated += Window_Activated;
                this.Closed += Window_Closed;
                ((FrameworkElement)this.Content).ActualThemeChanged += Window_ThemeChanged;

                // Initial configuration state.
                m_configurationSource.IsInputActive = true;
                SetConfigurationSourceTheme();

                m_backdropController = new Microsoft.UI.Composition.SystemBackdrops.MicaController();

                // Enable the system backdrop.
                // Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
                m_backdropController.AddSystemBackdropTarget(this.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
                m_backdropController.SetSystemBackdropConfiguration(m_configurationSource);
                return true; // succeeded
            }

            return false; // Mica is not supported on this system
        }

        private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            m_configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
        }

        private void Window_Closed(object sender, WindowEventArgs args)
        {
            // Make sure any Mica/Acrylic controller is disposed
            // so it doesn't try to use this closed window.
            if (m_backdropController != null)
            {
                m_backdropController.Dispose();
                m_backdropController = null;
            }
            this.Activated -= Window_Activated;
            m_configurationSource = null;
        }

        private void Window_ThemeChanged(FrameworkElement sender, object args)
        {
            if (m_configurationSource != null)
            {
                SetConfigurationSourceTheme();
            }
        }

        private void SetConfigurationSourceTheme()
        {
            switch (((FrameworkElement)this.Content).ActualTheme)
            {
                case ElementTheme.Dark: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Dark; break;
                case ElementTheme.Light: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Light; break;
                case ElementTheme.Default: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Default; break;
            }
        }
    }

    class WindowsSystemDispatcherQueueHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        struct DispatcherQueueOptions
        {
            internal int dwSize;
            internal int threadType;
            internal int apartmentType;
        }

        [DllImport("CoreMessaging.dll")]
        private static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object dispatcherQueueController);

        object m_dispatcherQueueController = null;
        public void EnsureWindowsSystemDispatcherQueueController()
        {
            if (Windows.System.DispatcherQueue.GetForCurrentThread() != null)
            {
                // one already exists, so we'll just use it.
                return;
            }

            if (m_dispatcherQueueController == null)
            {
                DispatcherQueueOptions options;
                options.dwSize = Marshal.SizeOf(typeof(DispatcherQueueOptions));
                options.threadType = 2;    // DQTYPE_THREAD_CURRENT
                options.apartmentType = 2; // DQTAT_COM_STA

                CreateDispatcherQueueController(options, ref m_dispatcherQueueController);
            }
        }
    }
}
