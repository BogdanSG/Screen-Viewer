using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;


namespace ServerWF {
    class GDIScreenShot : ScreenShot {

        public GDIScreenShot(bool OnlyWindowScreen) : base(OnlyWindowScreen) { }

        public GDIScreenShot() : base() {


        }//GDIScreenShot


        public override void Start() {

            this.Run = true;

            int width = SystemInformation.VirtualScreen.Width;

            int height = SystemInformation.VirtualScreen.Height;

            Bitmap screenBmp = null;

            Graphics bmpGraphics = null;

            if (this.OnlyWindowScreen == false) {

                screenBmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                bmpGraphics = Graphics.FromImage(screenBmp);

            }//if

            IntPtr activeWindow = IntPtr.Zero;
            //IntPtr hdcTo = IntPtr.Zero;
            //IntPtr hdcFrom = IntPtr.Zero;
            IntPtr hBitmap = IntPtr.Zero;

            this.screenThread = new Thread(() => {

                Start:

                try {

                    while (this.Run) {

                        try {

                            if (this.OnlyWindowScreen) {

                                activeWindow = WIN32.GetForegroundWindow();

                                //hdcFrom = WIN32.GetDC(activeWindow);

                                //hdcFrom = WIN32.GetWindowDC(activeWindow);

                                var r = new WIN32.Rect();

                                if (WIN32.GetWindowRect(activeWindow, ref r)) {

                                    int w = r.right - r.left;

                                    int h = r.bottom - r.top;

                                    uint processID = 0;

                                    WIN32.GetWindowThreadProcessId(activeWindow, out processID);

                                    var p = Process.GetProcessById((int)processID);

                                    //Program.ConsoleWriteLine($"{p.ProcessName}", Program.Red, Color.White);

                                    screenBmp = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                                    bmpGraphics = Graphics.FromImage(screenBmp);

                                    if (p.ProcessName == "chrome") {

                                        bmpGraphics.CopyFromScreen(r.left, r.top, 0, 0, new System.Drawing.Size(w, h));

                                    }//if
                                    else {

                                        hBitmap = bmpGraphics.GetHdc();

                                        WIN32.PrintWindow(activeWindow, hBitmap, 0);

                                        bmpGraphics.ReleaseHdc();

                                    }//else

                                    bmpGraphics.Dispose();

                                }//if
                                else continue;

                                //if (hdcFrom != IntPtr.Zero)
                                //    WIN32.ReleaseDC(activeWindow, hdcFrom);
                                //if (hdcTo != IntPtr.Zero)
                                //    WIN32.DeleteDC(hdcTo);
                                if (hBitmap != IntPtr.Zero)
                                    WIN32.DeleteObject(hBitmap);

                            }//if
                            else {

                                //Рисование скриншота экрана в screenBmp (баз мыши)

                                IntPtr dc1 = WIN32.GetDC(IntPtr.Zero);
                                IntPtr dc2 = bmpGraphics.GetHdc();

                                WIN32.BitBlt(dc2, 0, 0, width, height, dc1, 0, 0, 13369376);

                                WIN32.ReleaseDC(IntPtr.Zero, dc1);
                                bmpGraphics.ReleaseHdc(dc2);

                                WIN32.CURSORINFO ci = new WIN32.CURSORINFO();
                                ci.cbSize = Marshal.SizeOf(typeof(WIN32.CURSORINFO));
                                WIN32.GetCursorInfo(ref ci);

                                if (System.Windows.Forms.Cursor.Current != null) {

                                    using (Icon cursorIcon = System.Drawing.Icon.FromHandle(ci.hCursor))
                                        bmpGraphics.DrawIcon(cursorIcon, new System.Drawing.Rectangle(System.Windows.Forms.Cursor.Position, cursorIcon.Size));

                                }//if

                            }//else

                        }//try
                        catch (ThreadAbortException tax) {

                            return;

                        }//catch
                        catch {

                            //if (hdcFrom != IntPtr.Zero)
                            //    WIN32.ReleaseDC(activeWindow, hdcFrom);
                            //if (hdcTo != IntPtr.Zero)
                            //    WIN32.DeleteDC(hdcTo);
                            if (hBitmap != IntPtr.Zero)
                                WIN32.DeleteObject(hBitmap);

                        }//catch

                        //Конвертация Bitmap изображение в байты (FullHD 120000-300000 байт обычно) (Jpeg)

                        using (var ms = new MemoryStream()) {

                            byte[] temp = new byte[MaxLength];

                            screenBmp.Save(ms, ImageFormat.Jpeg);

                            ScreenRefreshed?.Invoke(this, this.MSToByteArrays(ms));

                        }//using

                    }//while

                }//try
                catch (ThreadAbortException tax) {

                    return;

                }//catch
                catch (Exception ex) {

                    Program.ConsoleWriteLine(ex.Message, Program.Red, Color.White);

                    using (var ms = new MemoryStream()) {

                        var b = new Bitmap(width, height, PixelFormat.Format32bppArgb);

                        b.Save(ms, ImageFormat.Jpeg);

                        ScreenRefreshed?.Invoke(this, this.MSToByteArrays(ms));

                        b.Dispose();

                    }//using

                    goto Start;

                }//catch

            });

            screenThread.IsBackground = true;

            screenThread.Priority = ThreadPriority.Highest;

            screenThread.Start();

        }//Start

    }//GDIScreenShot

}//ServerWF
