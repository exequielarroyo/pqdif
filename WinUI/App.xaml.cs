using Microsoft.UI.Xaml;

namespace WinUI;

public partial class App : Application
{

    public App()
    {
        this.InitializeComponent();
    }

    public static Window Window;
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        Window = new MainWindow();
        Window.Activate();
    }
}
