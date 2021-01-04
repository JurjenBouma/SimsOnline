using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SimsTcpClient
{
    public static class MessageBuffer
    {
        [DllImport("MessageBuffer.dll",CallingConvention = CallingConvention.StdCall)]
        public static extern void Initialize(int bufferSize);

        [DllImport("MessageBuffer.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool Connect(int pID, ulong bufferID);

        [DllImport("MessageBuffer.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void ReadMessage(byte[] receiveBuffer, ulong receiveID, bool keepMessage);

        [DllImport("MessageBuffer.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool WriteMessage(byte[] sendBuffer);

        [DllImport("MessageBuffer.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int GetAddress();

        [DllImport("MessageBuffer.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int GetPageNum();
    }
}
