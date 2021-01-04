using MessageCodes;
using Sims3;
using Sims3.Gameplay;
using Sims3.Gameplay.Utilities;
using Sims3.Gameplay.EventSystem;
using Sims3.UI;
using Sims3.SimIFace;
using Sims3.Gameplay.Interactions;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Sims3.Gameplay.Core;
using Sims3.Gameplay.InteractionsShared;

namespace SimsOnlineMod
{
    public class Main
    {
        private static bool isInitialized = false;
        public static byte[] messageBuffer = new byte[1024];
        private static AlarmHandle messageAlarmHandle;
        const int longSize = sizeof(ulong);
        const int intSize = sizeof(uint);
        private static List<byte[]> sendBuffer = new List<byte[]>();
        private static bool bufferConnected = false;
        private static bool gameStarted = false;
        private static bool netIntercactionAdded = false;

        public static void Initialise()
        {
            if (!isInitialized)
            {
                byte[] bufferIDBytes = BitConverter.GetBytes((ulong)SenderIDs.BufferFindID);
                for (int i = 0; i < bufferIDBytes.Length; i++)
                {
                    messageBuffer[i] = bufferIDBytes[7 - i];
                }
                Sims3.SimIFace.World.OnWorldLoadFinishedEventHandler += new System.EventHandler(OnWorldLoadFinished);
                Sims3.SimIFace.World.OnStartupAppEventHandler += new System.EventHandler(OnStartupApp);

                isInitialized = true;
            }
        }
      
        public static void OnStartupApp(object obj, System.EventArgs e)
        {
            while(!bufferConnected)
            {
                ProcessMessages();
            }
            string saveFile = "SimsOnlineModTempWorld.sims3";
            Sims3.UI.GameEntry.SaveGameMetadata meta = new Sims3.UI.GameEntry.SaveGameMetadata();
            meta.mSaveFile = saveFile;
            Sims3.Gameplay.MainMenuState ms = new Sims3.Gameplay.MainMenuState();
            ms.Startup();
            ms.Shutdown();
            Sims3.Gameplay.GameStates.TransitionToGameStateToInWorld(saveFile, meta, Sims3.Gameplay.InWorldState.SubState.LiveMode, true);
        }
        public static void OnWorldLoadFinished(object obj ,System.EventArgs e)
        {
            EventTracker.AddListener(EventTypeId.kLoadingScreenDisposed, new ProcessEventDelegate(LoadingScreenDisposed));
        }
        private static void SetEvents()
        {
            messageAlarmHandle = AlarmManager.Global.AddAlarmRepeating(0.0f, TimeUnit.Seconds, new AlarmTimerCallback(ProcessMessages),
                10.0f, TimeUnit.Seconds, "MessageReader",
                Sims3.Gameplay.Utilities.AlarmType.AlwaysPersisted, null);
            Sims3.Gameplay.Actors.Sim.ActiveActor.InteractionQueue.QueueChanged += new Sims3.Gameplay.ActorSystems.InteractionQueue.QueueChangedCallback(QueueChanged);
        }
        private static ListenerAction LoadingScreenDisposed(Event e)
        {
            SetEvents(); 
            byte[] gameLoadedBytes = BitConverter.GetBytes((uint)MessageIDs.GameLoaded);
            sendBuffer.Add(gameLoadedBytes);
            while (!gameStarted)
            {
                ProcessMessages();
            }     
            return ListenerAction.Remove;
        }

        private static void QueueChanged()
        {
            foreach (InteractionInstance interaction in Sims3.Gameplay.Actors.Sim.ActiveActor.InteractionQueue.InteractionList)
            {
                if (!netIntercactionAdded)
                {
                    string interactionDefinition = interaction.InteractionDefinition.ToString();
                    if ( interactionDefinition== Terrain.GoHere.SameLotSingleton.ToString())
                    {
                        List<byte> message = new List<byte>();
                        message.AddRange(BitConverter.GetBytes((uint)MessageIDs.InteractionTerrain));
                        message.AddRange(BitConverter.GetBytes((uint)InteractionTerrainTypes.GoHereSameLot));
                        message.AddRange(BitConverter.GetBytes(interaction.Target.ObjectId.Value));
                        message.AddRange(BitConverter.GetBytes(interaction.GetTargetPosition().x));
                        message.AddRange(BitConverter.GetBytes(interaction.GetTargetPosition().y));
                        message.AddRange(BitConverter.GetBytes(interaction.GetTargetPosition().z));
                        sendBuffer.Add(message.ToArray());
                    }
                    string sharedNameSpace = "Sims3.Gameplay.InteractionsShared.";
                    if (interactionDefinition.Contains(sharedNameSpace))
                    {
                        List<byte> message = new List<byte>();
                        message.AddRange(BitConverter.GetBytes((uint)MessageIDs.InteractionShared));
                        int nameStartIndex = sharedNameSpace.Length;
                        int nameLenght = interactionDefinition.IndexOf("+") - nameStartIndex;
                        string name = interactionDefinition.Substring(nameStartIndex, nameLenght);
                        message.AddRange(BitConverter.GetBytes((uint)Enum.Parse(typeof(InteractionSharedTypes),name)));
                        message.AddRange(BitConverter.GetBytes(interaction.Target.ObjectId.Value));
                        sendBuffer.Add(message.ToArray());
                    }
                }
                netIntercactionAdded = false;
            }
        }

        private static void ProcessMessages()
        {
            Sims3.SimIFace.StopWatch stopWatch = Sims3.SimIFace.StopWatch.Create(Sims3.SimIFace.StopWatch.TickStyles.Milliseconds);
            stopWatch.Start();

            ulong sender = 12345;
            int numMessages = -1;

            stopWatch.SetElapsedTime(10);
            while (sender == 12345)
            {
                try { sender = System.BitConverter.ToUInt64(messageBuffer, 0); }
                catch { }
                while (stopWatch.GetElapsedTime() < 10)
                {
                }
                stopWatch.Restart();
            }
            if (sender == (ulong)SenderIDs.Sims3)
                return;

            stopWatch.SetElapsedTime(10);
            while (numMessages == -1)
            {
                try { numMessages = (int)messageBuffer[longSize]; }
                catch { }
                while (stopWatch.GetElapsedTime() < 10)
                {
                }
                stopWatch.Restart();
            }
            int messageIndex = longSize +1;
            for(int i = 0; i<numMessages;i++)
            {
                messageIndex = ProcessMessage(messageIndex);
                if (messageIndex == -1)
                    return;
            }
            for (int i = 0; i < messageBuffer.Length; i++)
            {
                messageBuffer[i] = 0;
            }
            SendMessages();
        }
        private static int ProcessMessage(int messageIndex)
        {
            Sims3.SimIFace.StopWatch stopWatch = Sims3.SimIFace.StopWatch.Create(Sims3.SimIFace.StopWatch.TickStyles.Milliseconds);
            stopWatch.Start();

            uint message = 0;
            stopWatch.SetElapsedTime(10);
            int readIndex = messageIndex;
            while (message == 0)
            {
                try { message = BitConverter.ToUInt32(messageBuffer, readIndex); readIndex += intSize; }
                catch { }
                while (stopWatch.GetElapsedTime() < 10)
                {
                }
                stopWatch.Restart();
            }

            if(message == (uint)MessageIDs.HandShakeRequest)
            {
                byte[] comfirmBytes = BitConverter.GetBytes((uint)MessageIDs.HandShakeComfirm);
                sendBuffer.Add(comfirmBytes);
                bufferConnected = true;
            }
            else if (message == (uint)MessageIDs.StartGame)
            {
                Sims3.Gameplay.Gameflow.SetGameSpeed(Sims3.SimIFace.Gameflow.GameSpeed.Normal, Sims3.Gameplay.Gameflow.SetGameSpeedContext.GameStates);
                gameStarted = true;
            }
            else if (message == (uint)MessageIDs.GameFlowNormal)
            {
                Sims3.Gameplay.Gameflow.SetGameSpeed(Sims3.SimIFace.Gameflow.GameSpeed.Normal, Sims3.Gameplay.Gameflow.SetGameSpeedContext.GameStates);
            }
            else if (message == (uint)MessageIDs.GameFlowPause)
            {
                Sims3.Gameplay.Gameflow.SetGameSpeed(Sims3.SimIFace.Gameflow.GameSpeed.Pause, Sims3.Gameplay.Gameflow.SetGameSpeedContext.GameStates);
            }
            else if (message == (uint)MessageIDs.InteractionTerrain)
            {
                uint type = BitConverter.ToUInt32(messageBuffer, readIndex); readIndex += intSize;
                if (type == (uint)InteractionTerrainTypes.GoHereSameLot)
                {
                    ulong targetID = BitConverter.ToUInt64(messageBuffer, readIndex); readIndex += longSize;
                    float x = BitConverter.ToSingle(messageBuffer, readIndex); readIndex += sizeof(float);
                    float y = BitConverter.ToSingle(messageBuffer, readIndex); readIndex += sizeof(float);
                    float z = BitConverter.ToSingle(messageBuffer, readIndex); readIndex += sizeof(float);

                    InteractionPriority priority = new InteractionPriority();
                    priority.Level = InteractionPriorityLevel.UserDirected;
                    ObjectGuid guid = new ObjectGuid();
                    guid.Value = targetID;
                    guid.mValue = targetID;
                    Sims3.Gameplay.Autonomy.InteractionObjectPair iop = new Sims3.Gameplay.Autonomy.InteractionObjectPair(Terrain.GoHere.SameLotSingleton, Terrain.GetObject(guid));
                    InteractionInstanceParameters paras = new InteractionInstanceParameters(iop,Sims3.Gameplay.Actors.Sim.ActiveActor,priority,false,true);
                    Terrain.GoHere gohere = new Terrain.GoHere();
                    gohere.Init(ref paras);
                    gohere.SetTargetPosition(new Vector3(x, y, z));
                    netIntercactionAdded = true;
                    Sims3.Gameplay.Actors.Sim.ActiveActor.InteractionQueue.Add(gohere);
                }
            }
            else if (message == (uint)MessageIDs.InteractionShared)
            {
                uint type = BitConverter.ToUInt32(messageBuffer, readIndex); readIndex += intSize;
                ulong targetID = BitConverter.ToUInt64(messageBuffer, readIndex); readIndex += longSize;
                InteractionPriority priority = new InteractionPriority();
                priority.Level = InteractionPriorityLevel.UserDirected;
                ObjectGuid guid = new ObjectGuid();
                guid.Value = targetID;
                guid.mValue = targetID;

                if (type == (uint)InteractionSharedTypes.JumpOnObject)
                {
                    Sims3.Gameplay.Autonomy.InteractionObjectPair iop = new Sims3.Gameplay.Autonomy.InteractionObjectPair(JumpOnObject.Singleton, Terrain.GetObject(guid));
                    InteractionInstanceParameters paras = new InteractionInstanceParameters(iop, Sims3.Gameplay.Actors.Sim.ActiveActor, priority, false, true);
                    JumpOnObject interaction = new JumpOnObject();
                    interaction.Init(ref paras);
                    netIntercactionAdded = true;
                    Sims3.Gameplay.Actors.Sim.ActiveActor.InteractionQueue.Add(interaction);
                }
                else if (type == (uint)InteractionSharedTypes.JumpOffObject)
                {
                    Sims3.Gameplay.Autonomy.InteractionObjectPair iop = new Sims3.Gameplay.Autonomy.InteractionObjectPair(JumpOffObject.Singleton, Terrain.GetObject(guid));
                    InteractionInstanceParameters paras = new InteractionInstanceParameters(iop, Sims3.Gameplay.Actors.Sim.ActiveActor, priority, false, true);
                    JumpOffObject interaction = new JumpOffObject();
                    interaction.Init(ref paras);
                    netIntercactionAdded = true;
                    Sims3.Gameplay.Actors.Sim.ActiveActor.InteractionQueue.Add(interaction);
                }
                else if (type == (uint)InteractionSharedTypes.SleepAndNapOnObject)
                {
                    Sims3.Gameplay.Autonomy.InteractionObjectPair iop = new Sims3.Gameplay.Autonomy.InteractionObjectPair(SleepAndNapOnObject.Singleton, Terrain.GetObject(guid));
                    InteractionInstanceParameters paras = new InteractionInstanceParameters(iop, Sims3.Gameplay.Actors.Sim.ActiveActor, priority, false, true);
                    SleepAndNapOnObject interaction = new SleepAndNapOnObject();
                    interaction.Init(ref paras);
                    netIntercactionAdded = true;
                    Sims3.Gameplay.Actors.Sim.ActiveActor.InteractionQueue.Add(interaction);
                }
                else if (type == (uint)InteractionSharedTypes.StretchOnObject)
                {
                    Sims3.Gameplay.Autonomy.InteractionObjectPair iop = new Sims3.Gameplay.Autonomy.InteractionObjectPair(StretchOnObject.Singleton, Terrain.GetObject(guid));
                    InteractionInstanceParameters paras = new InteractionInstanceParameters(iop, Sims3.Gameplay.Actors.Sim.ActiveActor, priority, false, true);
                    StretchOnObject interaction = new StretchOnObject();
                    interaction.Init(ref paras);
                    netIntercactionAdded = true;
                    Sims3.Gameplay.Actors.Sim.ActiveActor.InteractionQueue.Add(interaction);
                }
                else if (type == (uint)InteractionSharedTypes.ShooOff)
                {
                    Sims3.Gameplay.Autonomy.InteractionObjectPair iop = new Sims3.Gameplay.Autonomy.InteractionObjectPair(ShooOff.Singleton, Terrain.GetObject(guid));
                    InteractionInstanceParameters paras = new InteractionInstanceParameters(iop, Sims3.Gameplay.Actors.Sim.ActiveActor, priority, false, true);
                    ShooOff interaction = new ShooOff();
                    interaction.Init(ref paras);
                    netIntercactionAdded = true;
                    Sims3.Gameplay.Actors.Sim.ActiveActor.InteractionQueue.Add(interaction);
                }
                else if (type == (uint)InteractionSharedTypes.ShooNeighborPet)
                {
                    Sims3.Gameplay.Autonomy.InteractionObjectPair iop = new Sims3.Gameplay.Autonomy.InteractionObjectPair(ShooNeighborPet.Singleton, Terrain.GetObject(guid));
                    InteractionInstanceParameters paras = new InteractionInstanceParameters(iop, Sims3.Gameplay.Actors.Sim.ActiveActor, priority, false, true);
                    ShooNeighborPet interaction = new ShooNeighborPet();
                    interaction.Init(ref paras);
                    netIntercactionAdded = true;
                    Sims3.Gameplay.Actors.Sim.ActiveActor.InteractionQueue.Add(interaction);
                }
                else if (type == (uint)InteractionSharedTypes.ShooFromFood)
                {
                    Sims3.Gameplay.Autonomy.InteractionObjectPair iop = new Sims3.Gameplay.Autonomy.InteractionObjectPair(ShooFromFood.Singleton, Terrain.GetObject(guid));
                    InteractionInstanceParameters paras = new InteractionInstanceParameters(iop, Sims3.Gameplay.Actors.Sim.ActiveActor, priority, false, true);
                    ShooFromFood interaction = new ShooFromFood();
                    interaction.Init(ref paras);
                    netIntercactionAdded = true;
                    Sims3.Gameplay.Actors.Sim.ActiveActor.InteractionQueue.Add(interaction);
                }
                else if (type == (uint)InteractionSharedTypes.ScratchObject)
                {
                    Sims3.Gameplay.Autonomy.InteractionObjectPair iop = new Sims3.Gameplay.Autonomy.InteractionObjectPair(ScratchObject.Singleton, Terrain.GetObject(guid));
                    InteractionInstanceParameters paras = new InteractionInstanceParameters(iop, Sims3.Gameplay.Actors.Sim.ActiveActor, priority, false, true);
                    ScratchObject interaction = new ScratchObject();
                    interaction.Init(ref paras);
                    netIntercactionAdded = true;
                    Sims3.Gameplay.Actors.Sim.ActiveActor.InteractionQueue.Add(interaction);
                }
                else if (type == (uint)InteractionSharedTypes.PetSingAlong)
                {
                    Sims3.Gameplay.Autonomy.InteractionObjectPair iop = new Sims3.Gameplay.Autonomy.InteractionObjectPair(PetSingAlong.Singleton, Terrain.GetObject(guid));
                    InteractionInstanceParameters paras = new InteractionInstanceParameters(iop, Sims3.Gameplay.Actors.Sim.ActiveActor, priority, false, true);
                    PetSingAlong interaction = new PetSingAlong();
                    interaction.Init(ref paras);
                    netIntercactionAdded = true;
                    Sims3.Gameplay.Actors.Sim.ActiveActor.InteractionQueue.Add(interaction);
                }
                else if (type == (uint)InteractionSharedTypes.ReactToDisturbance)
                {
                    Sims3.Gameplay.Autonomy.InteractionObjectPair iop = new Sims3.Gameplay.Autonomy.InteractionObjectPair(ReactToDisturbance.Singleton, Terrain.GetObject(guid));
                    InteractionInstanceParameters paras = new InteractionInstanceParameters(iop, Sims3.Gameplay.Actors.Sim.ActiveActor, priority, false, true);
                    ReactToDisturbance interaction = new ReactToDisturbance();
                    interaction.Init(ref paras);
                    netIntercactionAdded = true;
                    Sims3.Gameplay.Actors.Sim.ActiveActor.InteractionQueue.Add(interaction);
                }
                else if (type == (uint)InteractionSharedTypes.Sit)
                {
                    Sims3.Gameplay.Autonomy.InteractionObjectPair iop = new Sims3.Gameplay.Autonomy.InteractionObjectPair(Sit.Singleton, Terrain.GetObject(guid));
                    InteractionInstanceParameters paras = new InteractionInstanceParameters(iop, Sims3.Gameplay.Actors.Sim.ActiveActor, priority, false, true);
                    Sit interaction = new Sit();
                    interaction.Init(ref paras);
                    netIntercactionAdded = true;
                    Sims3.Gameplay.Actors.Sim.ActiveActor.InteractionQueue.Add(interaction);
                }
                else if (type == (uint)InteractionSharedTypes.Stand)
                {
                    Sims3.Gameplay.Autonomy.InteractionObjectPair iop = new Sims3.Gameplay.Autonomy.InteractionObjectPair(Stand.Singleton, Terrain.GetObject(guid));
                    InteractionInstanceParameters paras = new InteractionInstanceParameters(iop, Sims3.Gameplay.Actors.Sim.ActiveActor, priority, false, true);
                    Stand interaction = new Stand();
                    interaction.Init(ref paras);
                    netIntercactionAdded = true;
                    Sims3.Gameplay.Actors.Sim.ActiveActor.InteractionQueue.Add(interaction);
                }
                else if (type == (uint)InteractionSharedTypes.ViewObjects)
                {
                    Sims3.Gameplay.Autonomy.InteractionObjectPair iop = new Sims3.Gameplay.Autonomy.InteractionObjectPair(ViewObjects.Singleton, Terrain.GetObject(guid));
                    InteractionInstanceParameters paras = new InteractionInstanceParameters(iop, Sims3.Gameplay.Actors.Sim.ActiveActor, priority, false, true);
                    ViewObjects interaction = new ViewObjects();
                    interaction.Init(ref paras);
                    netIntercactionAdded = true;
                    Sims3.Gameplay.Actors.Sim.ActiveActor.InteractionQueue.Add(interaction);
                }
                else if (type == (uint)InteractionSharedTypes.Reminisce)
                {
                    Sims3.Gameplay.Autonomy.InteractionObjectPair iop = new Sims3.Gameplay.Autonomy.InteractionObjectPair(Reminisce.Singleton, Terrain.GetObject(guid));
                    InteractionInstanceParameters paras = new InteractionInstanceParameters(iop, Sims3.Gameplay.Actors.Sim.ActiveActor, priority, false, true);
                    Reminisce interaction = new Reminisce();
                    interaction.Init(ref paras);
                    netIntercactionAdded = true;
                    Sims3.Gameplay.Actors.Sim.ActiveActor.InteractionQueue.Add(interaction);
                }
                else if (type == (uint)InteractionSharedTypes.CatchFlies)
                {
                    Sims3.Gameplay.Autonomy.InteractionObjectPair iop = new Sims3.Gameplay.Autonomy.InteractionObjectPair(CatchFlies.Singleton, Terrain.GetObject(guid));
                    InteractionInstanceParameters paras = new InteractionInstanceParameters(iop, Sims3.Gameplay.Actors.Sim.ActiveActor, priority, false, true);
                    CatchFlies interaction = new CatchFlies();
                    interaction.Init(ref paras);
                    netIntercactionAdded = true;
                    Sims3.Gameplay.Actors.Sim.ActiveActor.InteractionQueue.Add(interaction);
                }
            }
            else
            {
                return -1;
            }
            return readIndex;
        }
        private static void SendMessages()
        {
            int messageIndex = longSize +1;
            for (int i = 0; i < sendBuffer.Count;i++ )
            {
                for (int b = 0; b < sendBuffer[i].Length; b++)
                {
                    messageBuffer[messageIndex + b] = sendBuffer[i][b];
                }
                messageIndex += sendBuffer[i].Length;
            }
            messageBuffer[longSize] = (byte)sendBuffer.Count;
            byte[] senderBytes = BitConverter.GetBytes((ulong)SenderIDs.Sims3);
            for (int i = 0; i < senderBytes.Length; i++)
            {
                messageBuffer[i] = senderBytes[i];
            }
            sendBuffer.Clear();
        }
    }
    
}
