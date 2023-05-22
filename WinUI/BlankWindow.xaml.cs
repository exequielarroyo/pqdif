using System;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUI.Views;
using Windows.Networking.Connectivity;
using Data.Access;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class BlankWindow : Window
{
    public BlankWindow()
    {
        this.InitializeComponent();
        Content = new NavigationPage();
        NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
    }

    private DatabaseContext SQLite;
    private DatabaseContext MySQL;

    private void NetworkInformation_NetworkStatusChanged(object sender)
    {
        DispatcherQueue.TryEnqueue(async () =>
        {
            ConnectionProfile profile = NetworkInformation.GetInternetConnectionProfile();
            var level = profile?.GetNetworkConnectivityLevel() ?? NetworkConnectivityLevel.None;
            if (level == NetworkConnectivityLevel.InternetAccess)
            {
                //AppTitleBarText.Text = "Internet Connected";
                ContentDialogResult result = await dialog2.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    MySQL = new MySQLContext();
                    SQLite = new SQLiteContext();

                    dialogLoading.IsActive = true;

                    var seriesToSync = from series in SQLite.Series.ToList()
                                       where series.Channel.Observation.Container.IsSync == false
                                       select series;


                    MySQL.Series.AddRange(seriesToSync);
                    MySQL.SaveChanges();

                    seriesToSync.ToList().ForEach(s => s.Channel.Observation.Container.IsSync = true);
                    SQLite.SaveChanges();

                    dialogLoading.IsActive = false;
                }
            }
            else
            {
                //AppTitleBarText.Text = "No Internet Connection";
            }
        });
    }

    public string GetAppTitleFromSystem()
    {
        return Windows.ApplicationModel.Package.Current.DisplayName;
    }
}
