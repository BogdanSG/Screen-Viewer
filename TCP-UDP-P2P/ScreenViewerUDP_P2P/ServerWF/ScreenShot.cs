using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerWF {

    public abstract class ScreenShot {

        public EventHandler<byte[][]> ScreenRefreshed { get; set; }

        protected static int MaxLength = 65500;

        protected bool Run;

        protected Thread screenThread;

        protected bool OnlyWindowScreen;
        public ScreenShot() {

            this.OnlyWindowScreen = false;

        }//ScreenShot

        public ScreenShot(bool OnlyWindowScreen) {

            this.OnlyWindowScreen = OnlyWindowScreen;

        }//ScreenShot

        public abstract void Start();

        public void Stop() {

            this.Run = false;

            if (this.screenThread != null)
                screenThread.Abort();

        }//Stop

        protected byte[][] MSToByteArrays(MemoryStream ms) {

            ms.Seek(0, SeekOrigin.Begin);

            byte[] temp = new byte[MaxLength];

            var Length = ms.Length;

            var Count = (ms.Length / MaxLength);

            byte[][] arrays = new byte[Count + 1][];

            for (int i = 0; i < Count; i++) {

                ms.Read(temp, 0, temp.Length);

                arrays[i] = temp;

                temp = new byte[MaxLength];

            }//for

            int lastRead = (int)(Length - (Count * MaxLength));

            temp = new byte[lastRead];

            ms.Read(temp, 0, temp.Length);

            arrays[Count] = temp;

            return arrays;

        }//MSToByteArrays

    }//ScreenShot

}//ServerWF
