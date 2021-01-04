using System;
using System.Collections.Generic;
using System.Text;

namespace MessageCodes
{
    public enum SenderIDs : ulong
    {
        Sims3               =       0x92348DE77B2A9F34,
        TcpController       =       0xFFB435D1231AF018,
    }
    public enum MessageIDs : ulong
    {
        BufferFindID        =       0x1ABB00131F121AA5,
        HandShakeRequest    =       0x4BB30982E26F770C,
        HandShakeComfirm    =       0xCCDB3613A2188CE4,
        StartGame           =       0x23478723AAB123EF,
        GameFlowNormal      =       0x54387DF324Ab3042,
        GameFlowPause       =       0xFAE5892C23478EF1,
    }
}
