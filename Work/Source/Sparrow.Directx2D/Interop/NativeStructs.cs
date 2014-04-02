﻿using System;
using System.Runtime.InteropServices;

namespace Sparrow.Directx2D
{
    // These structs are taken from the DirectX SDK
    // In the SDK unsigned int (uint in c#) is used, however, we're only going
    // to use normal ints so we are CLR compliant.
    internal static class NativeStructs
    {
        [StructLayout(LayoutKind.Sequential)]
        public sealed class D3DDISPLAYMODEEX
        {
            public int Size;
            public int Width;
            public int Height;
            public int RefreshRate;
            public int Format;
            public int ScanLineOrdering;
        }

        [StructLayout(LayoutKind.Sequential)]
        public sealed class D3DPRESENT_PARAMETERS
        {
            public int BackBufferWidth;
            public int BackBufferHeight;
            public int BackBufferFormat;
            public int BackBufferCount;
            public int MultiSampleType;
            public int MultiSampleQuality;
            public int SwapEffect;
            public IntPtr hDeviceWindow;
            public int Windowed;
            public int EnableAutoDepthStencil;
            public int AutoDepthStencilFormat;
            public int Flags;
            public int FullScreen_RefreshRateInHz;
            public int PresentationInterval;
        }
    }
}
