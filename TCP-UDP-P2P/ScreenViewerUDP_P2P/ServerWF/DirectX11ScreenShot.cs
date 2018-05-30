using System;
using System.Threading;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace ServerWF {

    public class DirectX11ScreenShot : ScreenShot {

        public override void Start() {

            this.Run = true;

            var factory = new Factory1();

            //Get first adapter

            var adapter = factory.GetAdapter1(0);

            //Get device from adapter

            var device = new SharpDX.Direct3D11.Device(adapter);

            //Get front buffer of the adapter

            var output = adapter.GetOutput(0);

            var output1 = output.QueryInterface<Output1>();

            // Width/Height of desktop to capture
            int width = output.Description.DesktopBounds.Right;

            int height = output.Description.DesktopBounds.Bottom;

            // Create Staging texture CPU-accessible

            var textureDesc = new Texture2DDescription {
                CpuAccessFlags = CpuAccessFlags.Read,
                BindFlags = BindFlags.None,
                Format = Format.B8G8R8A8_UNorm,
                Width = width,
                Height = height,
                OptionFlags = ResourceOptionFlags.None,
                MipLevels = 1,
                ArraySize = 1,
                SampleDescription = { Count = 1, Quality = 0 },
                Usage = ResourceUsage.Staging
            };

            var screenTexture = new Texture2D(device, textureDesc);

            var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            var boundsRect = new Rectangle(0, 0, width, height);

            var bmpGraphics = Graphics.FromImage(bitmap);

            this.screenThread = new Thread(() => {

                // Duplicate the output

                Start:

                try {

                    using (var duplicatedOutput = output1.DuplicateOutput(device)) {

                        while (this.Run) {

                            try {

                                SharpDX.DXGI.Resource screenResource;

                                OutputDuplicateFrameInformation duplicateFrameInformation;

                                // Try to get duplicated frame within given time is ms

                                duplicatedOutput.AcquireNextFrame(5, out duplicateFrameInformation, out screenResource);

                                // copy resource into memory that can be accessed by the CPU

                                using (var screenTexture2D = screenResource.QueryInterface<Texture2D>())
                                    device.ImmediateContext.CopyResource(screenTexture2D, screenTexture);

                                // Get the desktop capture texture

                                var mapSource = device.ImmediateContext.MapSubresource(screenTexture, 0, MapMode.Read, SharpDX.Direct3D11.MapFlags.None);

                                // Create Drawing.Bitmap

                                // Copy pixels from screen capture Texture to GDI bitmap

                                var mapDest = bitmap.LockBits(boundsRect, ImageLockMode.WriteOnly, bitmap.PixelFormat);
                                var sourcePtr = mapSource.DataPointer;
                                var destPtr = mapDest.Scan0;

                                for (int y = 0; y < height; y++) {

                                    // Copy a single line 
                                    Utilities.CopyMemory(destPtr, sourcePtr, width * 4);

                                    // Advance pointers
                                    sourcePtr = IntPtr.Add(sourcePtr, mapSource.RowPitch);
                                    destPtr = IntPtr.Add(destPtr, mapDest.Stride);

                                }//for

                                // Release source and dest locks
                                bitmap.UnlockBits(mapDest);
                                device.ImmediateContext.UnmapSubresource(screenTexture, 0);

                                try {

                                    ////////////////////Рисование курсора/////////////////////////////

                                    WIN32.CURSORINFO ci = new WIN32.CURSORINFO();
                                    ci.cbSize = Marshal.SizeOf(typeof(WIN32.CURSORINFO));
                                    WIN32.GetCursorInfo(ref ci);

                                    if (System.Windows.Forms.Cursor.Current != null) {

                                        using (Icon cursorIcon = System.Drawing.Icon.FromHandle(ci.hCursor))
                                            bmpGraphics.DrawIcon(cursorIcon, new System.Drawing.Rectangle(System.Windows.Forms.Cursor.Position, cursorIcon.Size));

                                    }//if

                                    //////////////////////////////////////////////////////////////////

                                }//try
                                catch {


                                }//catch

                                using (var ms = new MemoryStream()) {

                                    bitmap.Save(ms, ImageFormat.Jpeg);

                                    ScreenRefreshed?.Invoke(this, this.MSToByteArrays(ms));

                                }//using

                                screenResource.Dispose();

                                duplicatedOutput.ReleaseFrame();

                            }//try
                            catch (SharpDXException sdx) {

                                if (sdx.ResultCode.Code != SharpDX.DXGI.ResultCode.WaitTimeout.Result.Code && sdx.ResultCode.Code != SharpDX.DXGI.ResultCode.AccessLost.Code && sdx.ResultCode.Code != SharpDX.DXGI.ResultCode.InvalidCall.Code) {

                                    Program.ConsoleWriteLine(sdx.Message, Program.Red, Color.White);

                                    using (var ms = new MemoryStream()) {

                                        var b = new Bitmap(width, height, PixelFormat.Format32bppArgb);

                                        b.Save(ms, ImageFormat.Jpeg);

                                        ScreenRefreshed?.Invoke(this, this.MSToByteArrays(ms));

                                        b.Dispose();

                                    }//using

                                    goto Start;

                                }//if

                            }//catch

                        }//while

                    }//using

                }//try
                catch (SharpDXException ex) {

                    if (ex.ResultCode.Code == SharpDX.DXGI.ResultCode.Unsupported.Result.Code) {

                        Thread.Sleep(200);

                        Program.ConsoleWriteLine("DirectX11 UNSUPPORTED! Please Stop Server And Use GDI!", Program.Red, Color.White);

                        return;

                    }//if

                    Program.ConsoleWriteLine(ex.Message, Program.Red, Color.White);

                    goto Start;

                }//catch

            });

            screenThread.IsBackground = true;

            screenThread.Priority = ThreadPriority.Highest;

            screenThread.Start();

        }//Start

    }//DirectX11ScreenShot

}//ServerWF
