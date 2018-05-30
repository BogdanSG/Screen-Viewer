using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerWF {

    class MouseClick {//Palatino //Microsoft Sans Serif

        public static Font font = new Font("Palatino", 15);

        private static bool Run = false;

        private static Image image = ServerWF.Properties.Resources.arrow;

        private static Brush brush = new SolidBrush(Color.FromArgb(255, 66, 161, 255));

        public float X { get; set; }

        public float Y { get; set; }

        public string Message { get; set; }

        public int Sleep { get; private set; }

        public MouseClick(int Sleep = 1200) {

            this.Message = string.Empty;

            this.Sleep = Sleep;

        }//MouseClick

        public static void DrawMouseClick(MouseClick click) {

            if (Run)
                return;

            Task.Run(() => {

                Run = true;

                var imageClone = image.Clone() as Image;

                var imageGraphics = Graphics.FromImage(imageClone);

                imageGraphics.DrawString(click.Message, MouseClick.font, MouseClick.brush, 33, 25);

                var dc = WIN32.GetDC(IntPtr.Zero);

                var desktopScreen = Graphics.FromHdc(dc);

                desktopScreen.DrawImage(imageClone, click.X - 15, click.Y - 10);

                Thread.Sleep(click.Sleep);

                WIN32.InvalidateRect(IntPtr.Zero, IntPtr.Zero, true);

                WIN32.ReleaseDC(IntPtr.Zero, dc);

                Thread.Sleep(50);

                Run = false;

            });

        }//DrawMouseClick

    }//MouseClick

}
