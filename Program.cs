using System;

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
                if (args[0].ToLower()=="-big")
                {
                    BigLaunchOptions(args);
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
    }
}