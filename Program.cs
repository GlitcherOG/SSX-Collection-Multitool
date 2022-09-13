
namespace SSXMultiTool
{
    internal static class Program
    {
        public static string Version = "0.0.1";
        public static SSXMultitoolHubpage hubpage = new SSXMultitoolHubpage();
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(hubpage);
        }

        static void HideHubpage()
        {
            hubpage.Hide();
        }
    }
}