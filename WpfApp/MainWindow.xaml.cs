using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        NotifyIcon notifyIcon = new NotifyIcon();
        notifyIcon.Icon = new System.Drawing.Icon("Resources/Icon1.ico");
        notifyIcon.Visible = true;
        notifyIcon.Text = "PQDIF";
        notifyIcon.Click += NotifyIcon_Click;
        
        var contextMenu = new ContextMenuStrip();
        var option1 = new ToolStripMenuItem("Option 1");
        var option2 = new ToolStripMenuItem("Option 2");
        var exitOption = new ToolStripMenuItem("Exit");
        contextMenu.Items.Add(option1);
        contextMenu.Items.Add(option2);
        contextMenu.Items.Add(exitOption);

        notifyIcon.ContextMenuStrip = contextMenu;
    }

    private void MainWindow_Click(object? sender, EventArgs e)
    {
        this.Close();
    }

    private void NotifyIcon_Click(object? sender, EventArgs e)
    {
        //MainWindow
    }
}
