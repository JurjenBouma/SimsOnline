using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TcpMessageCodes;

namespace SimsTcpClient
{
    public class FileInterface
    {
        public string Name;
        public List<byte> fileData = new List<byte>();
        public int PackagesReceived = 0;
        public int NumPackages = -1;
        public double id;
        public FileTypes type;

        public bool FileReady()
        { return (PackagesReceived == NumPackages); }

        public void SaveFile(string savePath)
        {
            if (FileReady())
            {
                Stream oStream = new FileStream(savePath, FileMode.Create);
                BinaryWriter writer = new BinaryWriter(oStream);
                writer.Write(fileData.ToArray());
                oStream.Close();
            }
        }
    }
}
