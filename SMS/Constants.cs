using System;
using System.Collections.Generic;
using System.Text;

namespace SMS
{
    class Constants
    {
        private static int timeOut;
        private static int commPort;
        private static int baudRate;
        private static string server;
        private static string phoneToNotify;
        private static string key;

        public static void setTimeOut(int pTimeOut)
        {
            timeOut = pTimeOut;
        }
        public static int getTimeOut()
        {
            return timeOut;
        }
        public static void setCommPort(int pCommPort)
        {
            commPort = pCommPort;
        }
        public static int getCommPort()
        {
            return commPort;
        }
        public static void setBaudRate(int pBaudRate)
        {
            baudRate = pBaudRate;
        }
        public static int getBaudRate()
        {
            return baudRate;
        }
        public static void setServer(string pServer)
        {
            server = pServer;
        }
        public static string getServer()
        {
            return server;
        }
        public static void setPhoneToNotify(string pPhoneToNotify)
        {
            phoneToNotify = pPhoneToNotify;
        }
        public static string getPhoneToNotify()
        {
            return phoneToNotify;
        }
        public static void setKey(string pKey)
        {
            key = pKey;
        }
        public static string getKey()
        {
            return key;
        }
    }
}
