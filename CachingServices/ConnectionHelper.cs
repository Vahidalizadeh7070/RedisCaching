using StackExchange.Redis;
using System;


namespace RedisCaching.CachingServices {
    public class ConnectionHelper {
        static ConnectionHelper() {
            ConnectionHelper.lazyConnection = new Lazy <ConnectionMultiplexer> (() => {
                return ConnectionMultiplexer.Connect(ConfigurationManager.AppSetting["RedisCacheUrl"]);
            });
        }
        private static Lazy <ConnectionMultiplexer> lazyConnection;
        public static ConnectionMultiplexer Connection {
            get {
                return lazyConnection.Value;
            }
        }
    }
}