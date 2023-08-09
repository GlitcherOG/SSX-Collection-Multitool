using System;
using System.Diagnostics;
using System.IO;

namespace SSXMultiTool
{
    internal static class Program
    {
        public static string Version = "0.0.1";
        public static bool Start = true;
        public static SSXMultitoolHubpage hubpage = new SSXMultitoolHubpage();
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();
                Application.Run(hubpage);
            }
            else
            {
                if (args[0].ToLower().Contains(".big"))
                {
                    if(File.Exists(args[0]))
                    {
                        ApplicationConfiguration.Initialize();
                        Application.Run(new BigArchiveTool(args[0]));
                    }
                }
                if (args[0].ToLower().Contains(".ssh"))
                {
                    if (File.Exists(args[0]))
                    {
                        ApplicationConfiguration.Initialize();
                        Application.Run(new SSHImageTools(args[0]));
                    }
                }
                if (args[0].ToLower()=="big")
                {
                    BigLaunchOptions(args);
                }
                if(args[0].ToLower() == "trickylevel")
                {
                    SSXTrickyProjectBuild(args);
                }
            }
        }



        static void BigLaunchOptions(string[] args)
        {
            if(args.Length == 1)
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new BigArchiveTool());
            }
            else
            {
                MessageBox.Show("Unknown Arguments");
            }
        }

        static void SSXTrickyProjectBuild(string[] args)
        {
            if(args.Length==1)
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new TrickyProjectWindow());
            }
            else
            {
                if (args[1].ToLower() == "build")
                {
                    if (Directory.Exists(args[2]))
                    {
                        try
                        {
                            TrickyLevelInterface trickyLevelInterface = new TrickyLevelInterface();
                            trickyLevelInterface.BuildTrickyLevelFiles(args[2], args[3]);
                            if (args[4].ToLower() == "run")
                            {
                                if (File.Exists(args[5]))
                                {
                                    ProcessStartInfo startInfo = new ProcessStartInfo();
                                    startInfo.FileName = args[5];
                                    string path = args[6];

                                    if (File.Exists(path))
                                    {
                                        if (path.ToLower().Contains(".iso"))
                                        {
                                            startInfo.Arguments = "\"" + path + "\"";
                                        }
                                        else if (path.ToLower().Contains(".elf"))
                                        {
                                            startInfo.Arguments = "-elf \"" + path + "\"";
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("No .Elf or ISO Path set");
                                    }
                                    Process.Start(startInfo);
                                }
                                else
                                {
                                    MessageBox.Show("Unknown Emulator Path");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Level Built");
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Unknown Error Building");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unknown Input Path");
                    }
                }
                else
                {
                    MessageBox.Show("Unknown Arguments");
                }
            }
        }
    }
}