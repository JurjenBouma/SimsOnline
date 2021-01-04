namespace SimsInitializer
{
    public class Initializer
    {
        private static bool isInitialized = false;
     
        public static void Initialise()
        {
            if (!isInitialized)
            {
                SimsOnlineMod.Main.Initialise();
                isInitialized = true;
            }
        }
    }
}
