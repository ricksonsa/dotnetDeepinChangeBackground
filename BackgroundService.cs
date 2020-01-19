using System;
using System.Diagnostics;
using System.IO;
using System.Timers;
using Newtonsoft.Json;

namespace cbservice
{
    public class BackgroundService
    {
        private readonly Timer _timer;
        private string _currentBackground;

        public BackgroundService()
        {
            _timer = new Timer();
            _timer.Elapsed += TimerElapsed;
            _timer.Interval = 1800000; //3600000
            var configFile = $"{AppDomain.CurrentDomain.BaseDirectory}config.json";

            if (File.Exists(configFile))
            {
                try
                {
                    var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configFile));
                    if (config.Interval != 0)
                    {
                        _timer.Interval = config.Interval;
                    }
                }
                catch (System.Exception)
                {
                    _timer.Interval = 1800000;
                }
            }
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            var bg = GetBackground();

            if (string.IsNullOrEmpty(_currentBackground) || _currentBackground != bg)
            {
                System.Console.WriteLine($"{AppDomain.CurrentDomain.BaseDirectory}backgrounds/{bg}.jpeg");
                _currentBackground = bg;
                var output = Bash($"gsettings set com.deepin.wrap.gnome.desktop.background picture-uri {AppDomain.CurrentDomain.BaseDirectory}backgrounds/{bg}.jpeg");
            }

        }

        private string GetBackground()
        {
            var now = Convert.ToInt32(DateTime.Now.ToString("HH"));
            var background = "mojave_dynamic_1";
            switch (now)
            {
                case 0:
                case 4:
                    background = "mojave_dynamic_16";
                    break;
                case 5:
                case 6:
                    background = "mojave_dynamic_1";
                    break;
                case 7:
                    background = "mojave_dynamic_2";
                    break;
                case 8:
                    background = "mojave_dynamic_3";
                    break;
                case 9:
                    background = "mojave_dynamic_4";
                    break;
                case 10:
                    background = "mojave_dynamic_5";
                    break;
                case 11:
                    background = "mojave_dynamic_6";
                    break;
                case 12:
                    background = "mojave_dynamic_7";
                    break;
                case 13:
                    background = "mojave_dynamic_7";
                    break;
                case 14:
                    background = "mojave_dynamic_8";
                    break;
                case 15:
                    background = "mojave_dynamic_9";
                    break;
                case 16:
                    background = "mojave_dynamic_10";
                    break;
                case 17:
                    background = "mojave_dynamic_11";
                    break;
                case 18:
                    background = "mojave_dynamic_12";
                    break;
                case 19:
                    background = "mojave_dynamic_13";
                    break;
                case 20:
                    background = "mojave_dynamic_14";
                    break;
                case 21:
                case 23:
                    background = "mojave_dynamic_15";
                    break;
            }

            return background;
        }

        private string Bash(string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                FileName = "/bin/bash",
                Arguments = $"-c \"{escapedArgs}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }

        public void Start()
        {
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}