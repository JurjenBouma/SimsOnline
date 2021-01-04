using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SimsTcpClient
{
    public static class MessageBuffer
    {
        [DllImport("D:\\Users\\Jurjen\\Documents\\Visual Studio 2013\\Projects\\Sims3 mods\\SimsOnline\\SimsTcpClient\\Release\\MessageBuffer.dll")]
        public static extern void Initialize(int bufferSize);

        [DllImport("D:\\Users\\Jurjen\\Documents\\Visual Studio 2013\\Projects\\Sims3 mods\\SimsOnline\\SimsTcpClient\\Release\\MessageBuffer.dll")]
        public static extern bool Connect(int pID, ulong bufferID);

        [DllImport("D:\\Users\\Jurjen\\Documents\\Visual Studio 2013\\Projects\\Sims3 mods\\SimsOnline\\SimsTcpClient\\Release\\MessageBuffer.dll")]
        public static extern void ReadMessage(byte[] receiveBuffer, ulong receiveID, bool keepMessage);

        [DllImport("D:\\Users\\Jurjen\\Documents\\Visual Studio 2013\\Projects\\Sims3 mods\\SimsOnline\\SimsTcpClient\\Release\\MessageBuffer.dll")]
        public static extern bool WriteMessage(byte[] sendBuffer);

        [DllImport("D:\\Users\\Jurjen\\Documents\\Visual Studio 2013\\Projects\\Sims3 mods\\SimsOnline\\SimsTcpClient\\Release\\MessageBuffer.dll")]
        public static extern int GetAddress();

        [DllImport("D:\\Users\\Jurjen\\Documents\\Visual Studio 2013\\Projects\\Sims3 mods\\SimsOnline\\SimsTcpClient\\Release\\MessageBuffer.dll")]
        public static extern int GetPageNum();
    }
}
