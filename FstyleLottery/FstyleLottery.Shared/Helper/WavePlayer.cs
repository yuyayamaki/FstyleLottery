using SharpDX.IO;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace FstyleLottery.Helper
{
  // original VB source
  // http://blogs.msdn.com/b/vbteam/archive/2014/06/15/vb-universal-app-part-4-using-sharpdx-for-sound-effects-with-ioc-and-linkedfiles.aspx
  // copyrights
  // http://blogs.msdn.com/b/vbteam/archive/2014/06/21/vb-universal-app-downloads.aspx
  // License: I the author of this source code hereby place it into the Public Domain. You can do whatever you like with it, including using it in your own apps.

  public class WavePlayer : IDisposable
  {
    private readonly XAudio2 xAudio = new XAudio2();
    private readonly Dictionary<string, BufferWithMetadata> sounds
      = new Dictionary<string, BufferWithMetadata>();
    private readonly Dictionary<BufferWithMetadata, Queue<SourceVoice>> voices
      = new Dictionary<BufferWithMetadata, Queue<SourceVoice>>();

    public void Dispose()
    {
      xAudio.Dispose();
    }

    public WavePlayer()
    {
      xAudio.StartEngine();
      (new MasteringVoice(xAudio)).SetVolume(1.0f);
    }


    public void StartPlay(StorageFile file)
    {
      Task.Run(() => StartPlayInternal(file));
    }

    private void StartPlayInternal(StorageFile file)
    {
      var filepath = file.Path;
      BufferWithMetadata wav;
      Queue<SourceVoice> freelist;

      // CONCURRENCY...
      // Synclock on "sounds" is for adding/retrieving a wav from "sounds", and for adding/retrieving
      // the corresponding freelist.
      // Synclock on "freelist" is for adding/removing elements from it

      lock (sounds)
      {
        if (sounds.ContainsKey(filepath))
        {
          wav = sounds[filepath];
          freelist = voices[wav];
        }
        else
        {
          using (var nfs = new NativeFileStream(filepath, NativeFileMode.Open,
            NativeFileAccess.Read))
          using (var ss = new SoundStream(nfs))
          {
            var buf = new AudioBuffer()
            {
              Stream = ss,
              AudioBytes = (int)(ss.Length),
              Flags = BufferFlags.EndOfStream,
            };
            wav = new BufferWithMetadata()
            {
              Buffer = buf,
              DecodedPacketsInfo = ss.DecodedPacketsInfo,
              WaveFormat = ss.Format,
            };
          }
          freelist = new Queue<SourceVoice>();
          voices.Add(wav, freelist);
          sounds.Add(filepath, wav);
        }
      }

      SourceVoice voice;
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
      voice.Start();
    }

  }
}
