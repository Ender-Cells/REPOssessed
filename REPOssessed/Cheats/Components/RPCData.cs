using ExitGames.Client.Photon;
using Photon.Realtime;
using System;

namespace REPOssessed.Cheats.Components
{
    public class RPCData
    {
        private static TimeSpan MAX_LIFE_TIME = TimeSpan.FromSeconds(60);
        public Player sender;
        public string rpc;
        public Hashtable hashtable;
        public DateTime timestamp;
        public bool suspected = false;
        public object? data;
        public RPCData? parent;

        public RPCData(Player sender, string rpc, Hashtable hash)
        {
            this.sender = sender;
            this.rpc = rpc;
            this.hashtable = hash;
            this.timestamp = DateTime.Now;
        }

        public bool IsExpired() => (DateTime.Now - MAX_LIFE_TIME) > timestamp;
        public bool IsRecent(int seconds) => (DateTime.Now - TimeSpan.FromSeconds(seconds)) < timestamp;
        public int AgeInSeconds() => (int)(DateTime.Now - timestamp).TotalSeconds;
        public void SetSuspected(object data)
        {
            suspected = true;
            this.data = data;
        }
        public void SetSuspected() => suspected = true;
    }
}
