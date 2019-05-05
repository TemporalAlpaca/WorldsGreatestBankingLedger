using System;

namespace WorldsGreatestBankingLedger_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            StartBankLedger();
        }

        static void StartBankLedger()
        {
            WorldsGreatestBankLedger worldsGreatestBankLedger = new WorldsGreatestBankLedger();
            worldsGreatestBankLedger.Run();
        }
    }
}
