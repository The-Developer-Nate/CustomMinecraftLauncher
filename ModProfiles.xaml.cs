using CmlLib.Core.Auth;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Game_Launcher
{
    /// <summary>
    /// Interaction logic for ModProfiles.xaml
    /// </summary>
    public partial class ModProfiles : Window
    {
        public ModProfiles(MSession session)
        {
            InitializeComponent();
            this.settion = session;
            if (!Directory.Exists(Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf\\mods"))
            {
                Directory.CreateDirectory(Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf\\mods");
            }
            if (!Directory.Exists(Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf\\modprofiles"))
            {
                Directory.CreateDirectory(Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf\\modprofiles");
            }
        }

        MSession settion;

        private void s_Click(object sender, RoutedEventArgs e)
        {
            var mdp = Find("mpn");
            TextBox box = mdp as TextBox;
            if (!Directory.Exists(Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf\\modprofiles\\" + box.Text))
            {
                Directory.CreateDirectory(Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf\\modprofiles\\" + box.Text);
            }
            if (Directory.Exists(Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf\\modprofiles\\" + box.Text))
            {
                Directory.Delete(Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf\\modprofiles\\" + box.Text, true);
                Directory.CreateDirectory(Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf\\modprofiles\\" + box.Text);
            }
            DirectoryCopy(Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf\\mods", Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf\\modprofiles\\" + box.Text, true);
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

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }

        private void l_Click(object sender, RoutedEventArgs e)
        {
            var mdp = Find("mpn");
            TextBox box = mdp as TextBox;
            if (Directory.Exists(Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf\\mods"))
            {
                Directory.Delete(Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf\\mods", true);
                Directory.CreateDirectory(Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf\\mods");
            }
            DirectoryCopy(Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf\\modprofiles\\" + box.Text, Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf\\mods", true);
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Launcher launcher = new Launcher(settion);
            launcher.Show();
            DataContext = new WindowBlureffect(launcher, AccentState.ACCENT_ENABLE_BLURBEHIND) { BlurOpacity = 0 };
            this.Close();
        }

        private void d_Click(object sender, RoutedEventArgs e)
        {
            var mdp = Find("mpn");
            TextBox box = mdp as TextBox;
            if (Directory.Exists(Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf\\modprofiles\\" + box.Text))
            {
                Directory.Delete(Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf\\modprofiles\\" + box.Text, true);
            }
        }
    }
}
