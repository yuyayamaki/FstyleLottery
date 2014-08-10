using SharpDX.Multimedia;
using SharpDX.XAudio2;
using System;
using System.Collections.Generic;
using System.Text;

namespace FstyleLottery.Helper
{
    class BufferWithMetadata
    {
        public AudioBuffer Buffer { get; set; }
        public uint[] DecodedPacketsInfo { get; set; }
        public WaveFormat WaveFormat { get; set; }
    }
}
