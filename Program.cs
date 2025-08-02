using SSXMultiTool.JsonFiles.Tricky;
using SSXMultiTool.Utilities;
using System;
using System.Diagnostics;
using System.IO;

namespace SSXMultiTool
{
    internal static class Program
    {
        public static string Version = "0.2.0";
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
                if (args[1].ToLower() == "trickybuild")
                {
                    if (Directory.Exists(args[2]))
                    {
                        ConsoleWindow.GenerateConsole();
                        try
                        {
                            SSXTrickyConfig config = new SSXTrickyConfig();
                            config = SSXTrickyConfig.Load(args[2]+ "/ConfigTricky.ssx");

                            if (args.Length < 4)
                            {
                                if (config.BuildPath != "")
                                {
                                    TrickyLevelInterface trickyLevelInterface = new TrickyLevelInterface();
                                    trickyLevelInterface.BuildTrickyLevelFiles(args[2], config.BuildPath);
                                    MessageBox.Show("Level Built");
                                }
                                else
                                {
                                    MessageBox.Show("Please Build Project Once to Set Build Path or provide a build path");
                                }
                            }
                            else
                            {
                                TrickyLevelInterface trickyLevelInterface = new TrickyLevelInterface();
                                trickyLevelInterface.BuildTrickyLevelFiles(args[2], args[3]);
                                MessageBox.Show("Level Built");
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Unknown Error Building");
                        }
                        ConsoleWindow.CloseConsole();
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