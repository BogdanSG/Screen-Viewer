using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;

namespace ServerWF {
    class Audio {

        WaveIn voice;

        public EventHandler<byte[]> VoiceRefreshed { get; set; }

        public Audio(int DeviceNumber, WaveFormat waveFormat = null) {


            this.voice = new WaveIn();

            this.voice.DeviceNumber = DeviceNumber;

            if (waveFormat == null) {

                this.voice.WaveFormat = new WaveFormat(8000, 16, 1);

            }//if
            else {

                this.voice.WaveFormat = waveFormat;

            }//else

            this.voice.DataAvailable += (s, e) => {

                ThreadPool.QueueUserWorkItem((x) => {

                    this.VoiceRefreshed?.Invoke(this, e.Buffer);

                });

            };

        }//Audio

        public void Start() {

            this.voice.StartRecording();

        }//Start

        public void Stop() {

            this.voice.StopRecording();

            this.voice.Dispose();

        }//Stop

    }//Audio

}//ServerWF
