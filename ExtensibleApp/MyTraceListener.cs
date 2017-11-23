using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibleApp
{
    /// <summary>
    /// Write mode
    /// </summary>
    enum Mode
    {
        Debug,
        Trace
    }

    internal class MyTraceListener : TraceListener
    {
        private Mode mode;
        private string debugLogFile = "debugLog.log"; 
        private string traceLogFile = "traceLog.log";

        public MyTraceListener(Mode mode)
        {
            this.mode = mode;
        }

        /// <summary>
        /// Write message to log file. Log file is based on the Mode set at construction time.
        /// </summary>
        /// <param name="message">Message to write.</param>
        public override void Write(string message)
        {
            using (StreamWriter writer = new StreamWriter((mode == Mode.Debug ? debugLogFile : traceLogFile), true))
            {
                writer.Write(message);
            }
        }

        /// <summary>
        /// Write message with line terminator to log file. Log file is based on the Mode set at construction time.
        /// </summary>
        /// <param name="message">Messege to write.</param>
        public override void WriteLine(string message)
        {
            using (StreamWriter writer = new StreamWriter((mode == Mode.Debug ? debugLogFile : traceLogFile), true))
            {
                writer.WriteLine(message);
            }
        }
    }
}
