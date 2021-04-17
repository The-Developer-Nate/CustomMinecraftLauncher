using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.Concurrent;
using System.Threading;
using CmlLib.Core.Downloader;
using CmlLib.Core;
using CmlLib.Core.Auth;

namespace Game_Launcher
{
    /// <summary>
    /// Interaction logic for Forge.xaml
    /// </summary>
    public partial class Forge : Window
    {
        public Forge(MSession session)
        {
            InitializeComponent();
            this.settion = session;
        }

        string LastInstalledVersion;
        MSession settion;

        public EventHandler<string> Forge_InstallerOutput { get; private set; }
        public DownloadFileChangedHandler Forge_FileChanged { get; private set; }

        MinecraftPath game = new MinecraftPath(Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf");

        private void if_Click(object sender, RoutedEventArgs e)
        {
            var fv = Find("fv");
            TextBox fvl = fv as TextBox;
            var v = Find("v");
            TextBox vl = v as TextBox;
            var forge = new MForge(game, Environment.GetEnvironmentVariable("JAVA_HOME") + "\\bin\\java.exe");
            forge.FileChanged += Forge_FileChanged;
            forge.InstallerOutput += Forge_InstallerOutput;
            var versionName = forge.InstallForge(vl.Text, fvl.Text);
            LastInstalledVersion = versionName;
            Properties.Settings.Default.version = versionName;
        }
        public FrameworkElement Find(String node)
        {
            object wantedNode = FindName(node);
            if (wantedNode is FrameworkElement)
            {
                // Following executed if Text element was found.
                FrameworkElement wantedChild = wantedNode as FrameworkElement;
                return wantedChild;
            }
            else
            {
                return null;
            }
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Launcher launcher = new Launcher(settion);
            launcher.Show();
            DataContext = new WindowBlureffect(launcher, AccentState.ACCENT_ENABLE_BLURBEHIND) { BlurOpacity = 0 };
            this.Close();
        }
    }
}
