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


namespace SimsOnlineMod
{
    public class Main
    {
        private static bool isInitialized = false;
        public static byte[] messageBuffer = new byte[1024];
        private static AlarmHandle messageAlarmHandle;
        const int longSize = sizeof(ulong);
        private static List<byte[]> sendBuffer = new List<byte[]>();
        private static bool bufferConnected = false;
        private static bool gameStarted = false;

        public static void Initialise()
        {
            if (!isInitialized)
            {
                byte[] bufferIDBytes = BitConverter.GetBytes((ulong)MessageIDs.BufferFindID);
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
            SetEvents();
            while (!gameStarted)
            {
                ProcessMessages();
            }          
        }
        private static void SetEvents()
        {
            messageAlarmHandle = AlarmManager.Global.AddAlarmRepeating(0.0f, TimeUnit.Seconds,new AlarmTimerCallback(ProcessMessages), 
                10.0f, TimeUnit.Seconds, "MessageReader",
                Sims3.Gameplay.Utilities.AlarmType.AlwaysPersisted, null);
           EventTracker.AddListener(EventTypeId.kAdoptedChild, new ProcessEventDelegate(OnAddoptChild));
           Sims3.Gameplay.Actors.Sim.ActiveActor.InteractionQueue.QueueChanged += new Sims3.Gameplay.ActorSystems.InteractionQueue.QueueChangedCallback(QueueChanged);
        }

        private static void QueueChanged()
        {
            foreach (InteractionInstance interaction in Sims3.Gameplay.Actors.Sim.ActiveActor.InteractionQueue.InteractionList)
            {
                    SimpleMessageDialog.Show("QueueChanged", interaction.InteractionDefinition.ToString() + " Actor:" + interaction.InstanceActor.Name + " Target:"+ interaction.Target.GetLocalizedName());
                    SimpleMessageDialog.Show("TEst", "Singleton.tostring :" +Sims3.Gameplay.Objects.Electronics.TV.WatchTV.Singleton.ToString());
            } 
        }
        private static Sims3.Gameplay.EventSystem.ListenerAction OnWatchedTv(Sims3.Gameplay.EventSystem.Event ev)
        {
            SimpleMessageDialog.Show("QueueChanged","Watched tv event system");
            return Sims3.Gameplay.EventSystem.ListenerAction.Keep;
        }

        private static Sims3.Gameplay.EventSystem.ListenerAction OnAddoptChild(Sims3.Gameplay.EventSystem.Event ev)
        {
            string messtr = "";
            foreach (byte b in messageBuffer)
                messtr += b.ToString() + ",";
           SimpleMessageDialogCustom.ShowDialog("Title", messtr, Sims3.UI.ModalDialog.PauseMode.NoPause);
            return Sims3.Gameplay.EventSystem.ListenerAction.Keep;
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
            if (sender != (ulong)SenderIDs.TcpController)
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

            ulong message = 0;
            stopWatch.SetElapsedTime(10);
            while (message == 0)
            {
                try { message = BitConverter.ToUInt64(messageBuffer, messageIndex); } catch { }
                while (stopWatch.GetElapsedTime() < 10)
                {
                }
                stopWatch.Restart();
            }

            if(message == (ulong)MessageIDs.HandShakeRequest)
            {
                byte[] comfirmBytes = BitConverter.GetBytes((ulong)MessageIDs.HandShakeComfirm);
                sendBuffer.Add(comfirmBytes);
                bufferConnected = true;
                return messageIndex + longSize;
            }
            else if (message == (ulong)MessageIDs.StartGame)
            {
                Sims3.Gameplay.Gameflow.SetGameSpeed(Sims3.SimIFace.Gameflow.GameSpeed.Normal, Sims3.Gameplay.Gameflow.SetGameSpeedContext.GameStates);
                gameStarted = true;
                return messageIndex + longSize;
            }
            else if (message == (ulong)MessageIDs.GameFlowNormal)
            {
                Sims3.Gameplay.Gameflow.SetGameSpeed(Sims3.SimIFace.Gameflow.GameSpeed.Normal, Sims3.Gameplay.Gameflow.SetGameSpeedContext.GameStates);
                return messageIndex + longSize;
            }
            else if (message == (ulong)MessageIDs.GameFlowPause)
            {
                Sims3.Gameplay.Actors.Sim.ActiveActor.InteractionQueue.CancelAllInteractions();
                //Sims3.Gameplay.Gameflow.SetGameSpeed(Sims3.SimIFace.Gameflow.GameSpeed.Pause, Sims3.Gameplay.Gameflow.SetGameSpeedContext.GameStates);
                return messageIndex + longSize;
            }
            return -1;
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
