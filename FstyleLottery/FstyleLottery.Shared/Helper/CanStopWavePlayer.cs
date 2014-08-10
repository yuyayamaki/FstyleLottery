using SharpDX.IO;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace FstyleLottery.Helper
{
    class CanStopWavePlayer : IDisposable
    {
        private readonly XAudio2 xAudio = new XAudio2();
        private SourceVoice voice;

        public void Dispose()
        {
            xAudio.Dispose();
        }

        public CanStopWavePlayer(string soundFileUrl)
        {
            xAudio.StartEngine();
            (new MasteringVoice(xAudio)).SetVolume(1.0f);

            initializeSourceVoice(soundFileUrl);
        }

        BufferWithMetadata wav;

        private async void initializeSourceVoice(string soundFileUrl)
        {
            var file = await Windows.Storage.StorageFile
                      .GetFileFromApplicationUriAsync(new Uri(soundFileUrl));
            var filepath = file.Path;
            Queue<SourceVoice> freelist;


            using (var nfs = new NativeFileStream(filepath, NativeFileMode.Open,
            NativeFileAccess.Read))
            using (var ss = new SoundStream(nfs))
            {
                var buf = new AudioBuffer()
                {
                    Stream = ss,
                    AudioBytes = (int)(ss.Length),
                    Flags = BufferFlags.EndOfStream,
                    LoopCount = AudioBuffer.LoopInfinite,
                };
                wav = new BufferWithMetadata()
                {
                    Buffer = buf,
                    DecodedPacketsInfo = ss.DecodedPacketsInfo,
                    WaveFormat = ss.Format,
                };
            }
            freelist = new Queue<SourceVoice>();


            lock (freelist)
            {
                if (freelist.Count > 0)
                    voice = freelist.Dequeue();
                else
                    voice = new SourceVoice(xAudio, wav.WaveFormat);
            }

            Action<IntPtr> lambda = null;
            lambda = (i) =>
                     {
                         voice.BufferEnd -= lambda;
                         lock (freelist)
                         {
                             freelist.Enqueue(voice);
                         }
                     };
            voice.BufferEnd += lambda;
            voice.SubmitSourceBuffer(wav.Buffer, wav.DecodedPacketsInfo);
        }

        public void Play()
        {
            Task.Run(() => voice.Start());
        }

        public void Stop()
        {
            Task.Run(() =>
                {
                    voice.Stop();

                    voice.FlushSourceBuffers();
                    wav.Buffer.Stream.Position = 0;
                    voice.SubmitSourceBuffer(wav.Buffer, wav.DecodedPacketsInfo);
                });
        }

    }
}
