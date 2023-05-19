using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using WinRT;

namespace WinUI;

public partial class App : Application
{

    public App()
    {
        this.InitializeComponent();
        //serviceProvider = new ServiceProvider
    }

    //IServiceProvider serviceProvider;
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

    public Window m_window;
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        m_window = new BlankWindow();

        if (AppWindowTitleBar.IsCustomizationSupported()) //Run only on Windows 11
        {
            m_window.SizeChanged += SizeChanged; //Register handler for setting draggable rects

            appWindow = GetAppWindow(m_window); //Set ExtendsContentIntoTitleBar for the AppWindow not the window
            appWindow.TitleBar.ExtendsContentIntoTitleBar = true;
            appWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            appWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }
        m_window.Activate();
        TrySetMicaBackdrop();
    }
    public Window Window;

    private AppWindow appWindow;
    WindowsSystemDispatcherQueueHelper m_wsdqHelper; // See separate sample below for implementation
    Microsoft.UI.Composition.SystemBackdrops.MicaController m_micaController;
    Microsoft.UI.Composition.SystemBackdrops.SystemBackdropConfiguration m_configurationSource;
    bool TrySetMicaBackdrop()
    {
        if (Microsoft.UI.Composition.SystemBackdrops.MicaController.IsSupported())
        {
            m_wsdqHelper = new WindowsSystemDispatcherQueueHelper();
            m_wsdqHelper.EnsureWindowsSystemDispatcherQueueController();

            // Hooking up the policy object
            m_configurationSource = new Microsoft.UI.Composition.SystemBackdrops.SystemBackdropConfiguration();
            m_window.Activated += Window_Activated;
            m_window.Closed += Window_Closed;
            ((FrameworkElement)m_window.Content).ActualThemeChanged += Window_ThemeChanged;

            // Initial configuration state.
            m_configurationSource.IsInputActive = true;
            SetConfigurationSourceTheme();

            m_micaController = new Microsoft.UI.Composition.SystemBackdrops.MicaController();

            // Enable the system backdrop.
            // Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
            m_micaController.AddSystemBackdropTarget(m_window.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
            m_micaController.SetSystemBackdropConfiguration(m_configurationSource);
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
        // Make sure any Mica/Acrylic controller is disposed so it doesn't try to
        // use this closed window.
        if (m_micaController != null)
        {
            m_micaController.Dispose();
            m_micaController = null;
        }
        m_window.Activated -= Window_Activated;
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
        switch (((FrameworkElement)m_window.Content).ActualTheme)
        {
            case ElementTheme.Dark: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Dark; break;
            case ElementTheme.Light: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Light; break;
            case ElementTheme.Default: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Default; break;
        }
    }
}
