using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Downloader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Game_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow(string Autologin = "")
        {
            InitializeComponent();
            this.autolog = Autologin;
        }

        public MainWindow()
        {
            InitializeComponent();
            this.autolog = "enable";
        }

        MinecraftPath minecraft = new MinecraftPath(Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf");
        public string userlogin;
        public string passlogin;
        public string autolog;
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
            }
            else
            {
                return null;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            DataContext = new WindowBlureffect(this, AccentState.ACCENT_ENABLE_BLURBEHIND) { BlurOpacity = 0 };
            // Log(, "74 - DataContext = new WindowBlureffect(this, AccentState.ACCENT_ENABLE_BLURBEHIND) { BlurOpacity = 0 };");
            var userl = Find("user");
            // Log(, "77 - var userl = Find('user');");
            var passl = Find("pass");
            // Log( "78 - var passl = Find('pass');");
            TextBox userboxl = userl as TextBox;
            userboxl.Text = Properties.Settings.Default.user;
            TextBox passboxl = passl as TextBox;
            passboxl.Text = Properties.Settings.Default.pass;
            if (autolog == "enable")
            {
                // Log(, "Attempting To Autologin.");
                var login = new MLogin();
                var response = login.TryAutoLogin();

                if (response.Result == MLoginResult.Success)
                {
                    // Log(, "Autologin Success!");
                    if (!Directory.Exists(Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf"))
                    {
                        Directory.CreateDirectory(Environment.GetEnvironmentVariable("APPDATA") + "\\.mclauncherwpf");
                        MDownloader _downloader = new MDownloader(minecraft);
                        _downloader.DownloadAll(true);
                    }
                    UpdateSession(response.Session);
                    Launcher launcher = new Launcher(Session);
                    launcher.Show();
                    DataContext = new WindowBlureffect(launcher, AccentState.ACCENT_ENABLE_BLURBEHIND) { BlurOpacity = 0 };
                    this.Close();
                }
            }
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
                UpdateCredentials(userbox.Text, passbox.Text);
                UpdateSession(result.Session);
                Launcher launcher = new Launcher(Session);
                launcher.Show();
                DataContext = new WindowBlureffect(launcher, AccentState.ACCENT_ENABLE_BLURBEHIND) { BlurOpacity = 0 };
                this.Close();
            }
            else
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

        private void UpdateCredentials(string username, string password)
        {
            Properties.Settings.Default.user = username;
            Properties.Settings.Default.pass = password;
            Properties.Settings.Default.Save();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBlock_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var build = Find("build");
            TextBlock buildl = build as TextBlock;
            build.Cursor = Cursors.None;
        }
    }
}
