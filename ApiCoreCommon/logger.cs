
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ApiCoreCommon
{
    public class logger
    {

        private static log4net.ILog _Logger;
        public static log4net.ILog Logger
        {
            get
            {

                if (_Logger == null)
                {
                    log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo("log4net.config"));

                    _Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

                }
                return _Logger;
            }
        }

        public static void LogInfo(string message)
        {
            Logger.Info(message);
        }

        public static void LogWarn(string message)
        {
            Logger.Warn(message);


        }
        public static void LogError(string message, Exception ex)
        {
            Logger.Error(message, ex);

        }
    }
}
