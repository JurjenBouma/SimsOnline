using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SimsTcpClient
{
    public static class MessageBuffer
    {
        [DllImport("MessageBuffer.dll")]
        public static extern void Initialize(int bufferSize);

        [DllImport("MessageBuffer.dll")]
        public static extern bool Connect(int pID, ulong bufferID);

        [DllImport("MessageBuffer.dll")]
        public static extern void ReadMessage(byte[] receiveBuffer, ulong receiveID, bool keepMessage);

        [DllImport("MessageBuffer.dll")]
        public static extern bool WriteMessage(byte[] sendBuffer);

        [DllImport("MessageBuffer.dll")]
        public static extern int GetAddress();

        [DllImport("MessageBuffer.dll")]
        public static extern int GetPageNum();
    }
}
