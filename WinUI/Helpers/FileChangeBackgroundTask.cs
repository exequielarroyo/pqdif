using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Dispatching;
using Windows.ApplicationModel.Background;

namespace WinUI.Helpers;

public sealed class FileChangeBackgroundTask : IBackgroundTask
{
    public void Run(IBackgroundTaskInstance taskInstance)
    {

        var folder = @"C:\Users\zekie\Documents\pqdif\example-pqdif-native";

        if (folder != null)
        {
            // Create a file system watcher to monitor changes in the folder
            FileSystemWatcher watcher = new FileSystemWatcher(folder);
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.IncludeSubdirectories = true;

            // Set up event handlers and start monitoring the folder
            watcher.Created += Watcher_Created;
            watcher.EnableRaisingEvents = true;
        }
    }

    private void Watcher_Created(object sender, FileSystemEventArgs e)
    {
        //DispatcherQueue.TryEnqueue(() =>
        //{
        //    chooseLabel.Text = $"New file: {e.Name}";
        //});
        Console.WriteLine("Hello, World!");
    }
}
