using System;
using System.Threading;

namespace GameServerCsharp
{
    class Program
    {
        private static bool isRunning = true;
        static void Main(string[] args)
        {
            Console.Title = "GameServer";
            
            Thread mainThread = new Thread(new ThreadStart(MainThread));
            mainThread.Start();

            Server.Start(50, 26950);

        }

        private static void MainThread()
        {
            Console.WriteLine($"Main Thread started. {Constants.TICKS_PER_SEC} ticks per second");
            DateTime nextLoop = DateTime.Now;

            while (isRunning)
            {
                while (nextLoop < DateTime.Now)
                {
                    GameLogic.Update();

                    nextLoop = nextLoop.AddMilliseconds(Constants.MS_PER_TICK);

                    if (nextLoop > DateTime.Now)
                    {
                        Thread.Sleep(nextLoop - DateTime.Now);
                    }
                }
            }
        }
    }
}
