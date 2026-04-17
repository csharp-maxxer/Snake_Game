using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace Snake
{
   public static class SnakeLogger
    {
        public static Serilog.Core.Logger logger {  get; private set; }

        public static bool Initialized { get; private set; } = false; 

        public static void init(string logfilename)
        {
            logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File(logfilename, 
                    rollingInterval: RollingInterval.Day, 
                    retainedFileCountLimit: 7)
                .CreateLogger();

            logger.Debug("Logger initialisiert");
            Initialized = true;

        }
    }
}
