using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpLogger
{
    public interface Logger
    {
        /// <summary>
        /// Message prints always, regardless of filtering rules.
        /// Not recommended.
        /// </summary>
        /// <param name="message"></param>
        void Always(string message);

        /// <summary>
        /// Method for unrecoverable errors.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Fatal(string message, Exception ex = null);

        /// <summary>
        /// Method for sustainable errors.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Error(string message, Exception ex = null);

        /// <summary>
        /// Method for external errors, such as user-input or file access.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="id"></param>
        /// <param name="ex"></param>
        void Warning(string message, int id = 0, Exception ex = null);

        /// <summary>
        /// Method for external errors, such as user-input or file access.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="id"></param>
        /// <param name="ex"></param>
        void Warning(string message, int[] id, Exception ex = null);

        /// <summary>
        /// Method for external errors, such as user-input or file access.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Warning(string message, Exception ex);

        /// <summary>
        /// Method to log global stages of program execution.
        /// Default level.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="id"></param>      
        void Info(string message, int id = 0);

        /// <summary>
        /// Method to log global stages of program execution.
        /// Default level.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="id"></param>      
        void Info(string message, int[] id);

        /// <summary>
        /// Method for logging messages or events.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="id"></param>
        void Event(string message, int id = 0);

        /// <summary>
        /// Method for logging messages or events.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="id"></param>
        void Event(string message, int[] id);

        /// <summary>
        /// Method for regular debug logging.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="id"></param>
        void Debug(string message, int id = 0);

        /// <summary>
        /// Method for regular debug logging.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="id"></param>
        void Debug(string message, int[] id);

        /// <summary>
        /// Method for detailed debug logging.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="id"></param>
        void Detailed(string message, int id = 0);

        /// <summary>
        /// Method for detailed debug logging.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="id"></param>
        void Detailed(string message, int[] id);
    }
}
