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
using CmlLib.Core;
using CmlLib.Core.Version;
using CmlLib.Core.Downloader;
using CmlLib.Utils;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.IO;
using System.Net;
using CmlLib.Core.Auth;
using System.Security.Authentication;

namespace Game_Launcher
{
    /// <summary>
    /// Interaction logic for Launcher.xaml
    /// </summary>
    public partial class Launcher : Window
    {
        MSession Session;
        
        public Launcher(MSession session)
        {
            InitializeComponent();
            this.Session = session;
            var version = Find("version");
            TextBox ver = version as TextBox;
            ver.Text = Properties.Settings.Default.version;
        }
        MinecraftPath game = new MinecraftPath(Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf");

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

        public ProgressChangedEventHandler Downloader_ChangeProgress { get; private set; }
        public DownloadFileChangedHandler Downloader_ChangeFile { get; private set; }

        

        void UpdateVersion(string ver)
        {
            Properties.Settings.Default.version = ver;
            Properties.Settings.Default.Save();
        }

        void Start(MSession session)
        {
            
            // Initializing Launcher

            // Set minecraft home directory
            // MinecraftPath.GetOSDefaultPath() return default minecraft BasePath of current OS.
            // https://github.com/AlphaBs/CmlLib.Core/blob/master/CmlLib/Core/MinecraftPath.cs

            // You can set this path to what you want like this :
            // var path = Environment.GetEnvironmentVariable("APPDATA") + "\\.mylauncher";

            // Create CMLauncher instance
            var launcher = new CMLauncher(game);
            launcher.ProgressChanged += Downloader_ChangeProgress;
            launcher.FileChanged += Downloader_ChangeFile;
            launcher.LogOutput += (s, e) => Console.WriteLine(e); // forge installer log

            Console.WriteLine($"Initialized in {launcher.MinecraftPath.BasePath}");

            var versions = launcher.GetAllVersions(); // Get all installed profiles and load all profiles from mojang server
            foreach (var item in versions) // Display all profiles 
            {
                // You can filter snapshots and old versions to add if statement : 
                // if (item.MType == MProfileType.Custom || item.MType == MProfileType.Release)
                Console.WriteLine(item.Type + " " + item.Name);
            }

            var launchOption = new MLaunchOption
            {
                

                MaximumRamMb = 4096,
                Session = session,

                // More options:
                // https://github.com/AlphaBs/CmlLib.Core/wiki/MLaunchOption
            };

            // (A) checks forge installation and install forge if it was not installed.
            // (B) just launch any versions without installing forge, but it can still launch forge already installed.
            // Both methods automatically download essential files (ex: vanilla libraries) and create game process.

            // (A) download forge and launch
            // var process = launcher.CreateProcess("1.12.2", "14.23.5.2768", launchOption);

            // (B) launch vanilla version
            // var process = launcher.CreateProcess("1.15.2", launchOption);

            // If you have already installed forge, you can launch it directly like this.
            // var process = launcher.CreateProcess("1.12.2-forge1.12.2-14.23.5.2838", launchOption);

            // launch by user input
            var version = Find("version");
            TextBox ver = version as TextBox;
            UpdateVersion(ver.Text);
            var process = launcher.CreateProcess(ver.Text, launchOption);

            

            // or just start it without print logs
            process.Start();
            this.WindowState = WindowState.Minimized;
            this.ShowInTaskbar = false;
            process.WaitForExit();

            this.WindowState = WindowState.Maximized;
            this.ShowInTaskbar = true;
            return;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Start(Session);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Forge forge = new Forge(Session);
            forge.Show();
            DataContext = new WindowBlureffect(forge, AccentState.ACCENT_ENABLE_BLURBEHIND) { BlurOpacity = 0 };
            this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            
            ModProfiles modProfiles = new ModProfiles(Session);
            modProfiles.Show();
            DataContext = new WindowBlureffect(modProfiles, AccentState.ACCENT_ENABLE_BLURBEHIND) { BlurOpacity = 0 };
            this.Close();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow("");
            mainWindow.Show();
            DataContext = new WindowBlureffect(mainWindow, AccentState.ACCENT_ENABLE_BLURBEHIND) { BlurOpacity = 0 };
            this.Close();
        }
    }
}
