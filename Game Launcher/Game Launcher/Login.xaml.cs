using System;
using System.Linq;
using static CmlLib.Core.Version.MVersionLoader;
using System.Windows.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Downloader;
using CmlLib.Core.Version;
using System.Windows;
using System.IO;

namespace Game_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();
        }

        MinecraftPath minecraft = new MinecraftPath(Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf");
        public string userlogin;
        public string passlogin;
        public MSession Session;
        MLogin logn = new MLogin();

        public FrameworkElement Find(String node)
        {
            object wantedNode = FindName(node);
            if (wantedNode is FrameworkElement)
            {
                // Following executed if Text element was found.
                FrameworkElement wantedChild = wantedNode as FrameworkElement;
                return wantedChild;
            }else
            {
                return null;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new WindowBlureffect(this,AccentState.ACCENT_ENABLE_BLURBEHIND) { BlurOpacity = 0 };
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var user = Find("user");
            var pass = Find("pass");
            TextBox userbox = user as TextBox;
            TextBox passbox = pass as TextBox;

            logn = new MLogin();

            var result = logn.Authenticate(userbox.Text, passbox.Text, Guid.NewGuid().ToString());

            if (result.Result == MLoginResult.Success)
            {
                if (!Directory.Exists(Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf"))
                {
                    Directory.CreateDirectory(Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf");
                    MDownloader _downloader = new MDownloader(minecraft);
                    _downloader.DownloadAll(true);
                }  
                UpdateSession(result.Session);
                Launcher launcher = new Launcher(Session);
                launcher.Show();
                DataContext = new WindowBlureffect(launcher, AccentState.ACCENT_ENABLE_BLURBEHIND) { BlurOpacity = 0 };
                this.Close();
            }else
            {
                MessageBox.Show("Failed To Login.");
            }
        }

        public static DataTable Tabulate(string json)
        {
            var jsonLinq = JObject.Parse(json);

            // Find the first array using Linq
            var srcArray = jsonLinq.Descendants().Where(d => d is JArray).First();
            var trgArray = new JArray();
            foreach (JObject row in srcArray.Children<JObject>())
            {
                var cleanRow = new JObject();
                foreach (JProperty column in row.Properties())
                {
                    // Only include JValue types
                    if (column.Value is JValue)
                    {
                        cleanRow.Add(column.Name, column.Value);
                    }
                }

                trgArray.Add(cleanRow);
            }

            return JsonConvert.DeserializeObject<DataTable>(trgArray.ToString());
        }

        private void UpdateSession(MSession session)
        {
            this.Session = session;
        }

    }
}
