using System;
using System.Collections.Generic;
using System.Text;

namespace MessageCodes
{
    public enum SenderIDs : ulong
    {
        Sims3               =       0x92348DE77B2A9F34,
        TcpController       =       0xFFB435D1231AF018,
        BufferFindID        =       0x1ABB00131F121AA5,
    }
    public enum MessageIDs : uint
    {
        HandShakeRequest    =       0x4BB30982,
        HandShakeComfirm    =       0xCCDB3613,
        GameLoaded          =       0x3BBF918A,
        StartGame           =       0x23478723,
        GameFlowNormal      =       0x54387DF3,
        GameFlowPause       =       0xFAE5892C,
        InteractionTerrain  =       0xEB3191FA,
        InteractionShared   =       0xAAB837FE,
    }
    public enum InteractionTerrainTypes : uint
    {
        GoHereSameLot,
    }
    public enum InteractionSharedTypes : uint
    {
        JumpOnObject,
        JumpOffObject,
        SleepAndNapOnObject,
        StretchOnObject,
        ShooOff,
        ShooNeighborPet,
        ShooFromFood,
        ScratchObject,
        PetSingAlong,
        ReactToDisturbance,
        Sit,
        Stand,
        ViewObjects,
        Reminisce,
        CatchFlies,
    }
}
