using MessageCodes;
using Sims3;
using Sims3.Gameplay;
using Sims3.Gameplay.Utilities;
using Sims3.Gameplay.EventSystem;
using Sims3.UI;

namespace SimsOnlineMod
{
    public class Main
    {
        private static bool isInitialized = false;
        public static byte[] messageBuffer = new byte[1024] { 26,187,0,19,31,18,26,165,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
        private static AlarmHandle messageAlarmHandle;

        public static void Initialise()
        {
            if (!isInitialized)
            {
                Sims3.SimIFace.World.OnWorldLoadFinishedEventHandler += new System.EventHandler(OnWorldLoadFinished);
                Sims3.SimIFace.World.OnStartupAppEventHandler += new System.EventHandler(OnStartupApp);
                
                isInitialized = true;
            }
        }
        public static void OnStartupApp(object obj, System.EventArgs e)
        {
            ulong messageCode = 0;
            Sims3.SimIFace.StopWatch stopWatch = Sims3.SimIFace.StopWatch.Create(Sims3.SimIFace.StopWatch.TickStyles.Milliseconds);
            stopWatch.Start();
            while (messageCode != (ulong)MessageIDs.HandShakeRequest)
            {
                stopWatch.Restart();
                try { messageCode = System.BitConverter.ToUInt64(messageBuffer, 0); }
                catch
                { }

                while (stopWatch.GetElapsedTime() < 100)
                {

                }
            }

            string saveFile = "Sunset Valley.sims3";
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
            Sims3.Gameplay.Gameflow.SetGameSpeed(Sims3.SimIFace.Gameflow.GameSpeed.Pause, Sims3.Gameplay.Gameflow.SetGameSpeedContext.GameStates);

           
        }
        private static void SetEvents()
        {
            messageAlarmHandle = AlarmManager.Global.AddAlarmRepeating(0.0f, TimeUnit.Seconds,new AlarmTimerCallback(ProcessMessages), 
                10.0f, TimeUnit.Seconds, "MessageReader",
                Sims3.Gameplay.Utilities.AlarmType.AlwaysPersisted, null);
           EventTracker.AddListener(EventTypeId.kCalledSim, new ProcessEventDelegate(OnCalledSims));
           EventTracker.AddListener(EventTypeId.kAdoptedChild, new ProcessEventDelegate(OnAddoptChild));
        }
        private static Sims3.Gameplay.EventSystem.ListenerAction OnCalledSims(Sims3.Gameplay.EventSystem.Event ev)
        { 
            Sims3.SimIFace.Resolution res = new Sims3.SimIFace.Resolution();
            res.mHeight = 800;
            res.mWidth =600;
            res.mRefresh = 144;
            Sims3.UI.UIManager.AcceptResolutionChange();
            Sims3.UI.UIManager.ChangeResolution(res,true);
            Sims3.UI.UIManager.AcceptResolutionChange();
            
            return Sims3.Gameplay.EventSystem.ListenerAction.Keep;
        }

        private static Sims3.Gameplay.EventSystem.ListenerAction OnAddoptChild(Sims3.Gameplay.EventSystem.Event ev)
        {
            string messtr = "";
            foreach (byte b in messageBuffer)
                messtr += b.ToString();
           SimpleMessageDialogCustom.ShowDialog("Title", messtr, Sims3.UI.ModalDialog.PauseMode.NoPause);
            return Sims3.Gameplay.EventSystem.ListenerAction.Keep;
        }

        private static void ProcessMessages()
        {
            Sims3.UI.StyledNotification.Format f = new Sims3.UI.StyledNotification.Format();
            f.mText = "reading...";
            f.mStyle = Sims3.UI.StyledNotification.NotificationStyle.kSystemMessage;
            f.mCloseable = true;
            Sims3.UI.StyledNotification.Show(f);

        }
    }
    
}
