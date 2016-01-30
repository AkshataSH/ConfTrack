using System;
using System.Configuration;
using System.IO;
using ConferenceManagement.Utilities;

namespace ConferenceManagement.Log
{
    public class Logger
    {
        /// <summary>
        ///     Function which writes error message to text file
        /// </summary>
        /// <param name="message">The ErrorMessage.</param>
        /// <returns>Write error message to text file</returns>
        public static void LogMessage(string message)
        {
            try
            {
                using (var fileWriter = File.AppendText(ConfigurationSettings.AppSettings["LogFileName"]))
                    WriteToTextFile(message, fileWriter);
            }
            catch (Exception ex)
            {
                throw new Exception(Constants.Error + ClassMemeberDetails.GetCallerFunctionName() + ex.Message);
            }
        }

        #region WritingtoTextFile

        /// <summary>
        ///     Function which writes error message to text file
        /// </summary>
        /// <param name="message">The ErrorMessage.</param>
        /// <param name="fileWriter">The FileWriter.</param>
        /// <returns>Write error message to text file</returns>
        private static void WriteToTextFile(string message, StreamWriter fileWriter)
        {
            fileWriter.WriteLine("{0} {1} {2}", message, DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
        }

        #endregion
    }
}