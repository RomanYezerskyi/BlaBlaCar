using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.Logger
{
    public class NLogger:ILog
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public void Information(string message)
        {
            Logger.Info(message);
        }

        public void Warning(string message)
        {
            Logger.Warn(message);
        }

        public void Debug(string message)
        {
            Logger.Debug(message);
        }

        public void Error(string message)
        {
            Logger.Debug(message);
        }
    }
}
