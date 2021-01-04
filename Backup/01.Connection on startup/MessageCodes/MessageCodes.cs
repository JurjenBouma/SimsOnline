using System;
using System.Collections.Generic;
using System.Text;

namespace MessageCodes
{
    public enum MessageIDs : ulong
    {
        BufferFindID        =       0x1ABB00131F121AA5,
        HandShakeRequest    =       0x4BB30982E26F770C,
        HandShakeComfirm    =       0xCCDB3613A2188CE4,
        StartGame           =       0x23478723AAB123EF,
    }
}
