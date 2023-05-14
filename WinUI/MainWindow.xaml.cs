// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Data.Access;
using Data.Models;
using Gemstone.PQDIF.Logical;
using GSF.Xml;
using Microsoft.EntityFrameworkCore;
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
            this.get_Data();
            this.get_PQDIF_Data();
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";
        }

        public Blog Blog { get; set; }
        public ObservableCollection<Blog> Blogs { get; set; } = new();
        public DatabaseContext Database = new DatabaseContext();

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
            ObservationRecord observationRecord = await logicalParser.NextObservationRecordAsync();

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

        //private void save_To_XML()
        //{
        //    XmlDocument xmlDocument = new XmlDocument();
        //    XmlExtensions.GetXmlNode()
        //}
    }
}
