using System;
using System.IO;
using System.Net;

namespace MyWebService
{
    class DataDownloader
    {
        private Format format;
        private int gameId;
        private string logFile;

        /// <summary>
        /// Constructor for the data downloader.
        /// </summary>
        /// <param name="gameId">Id of the game which player count we want to know.</param>
        /// <param name="format">Format returned by the steam api.</param>
        /// <param name="logFile">File where to store retrieved informations.</param>
        public DataDownloader(int gameId, Format format, string logFile)
        {
            this.format = format;
            this.gameId = gameId;
            this.logFile = logFile;
        }

        /// <summary>
        /// Will try to get data from the steam api.
        /// When exception occurs we dont do anything sice we can tolerate it. 
        /// </summary>
        public void GetData()
        {
            string adress = string.Format(
                "https://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v1/?appid={0}&format={1}",
                gameId, format.ToString());

            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadStringCompleted += StringDownloaded;
                try
                {
                    webClient.DownloadStringAsync(new Uri(adress));
                }
                catch (WebException)
                { }
                catch (ArgumentNullException)
                { }
            }
        }

        /// <summary>
        /// Log game info when string is downloaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StringDownloaded(object sender, DownloadStringCompletedEventArgs e)
        {
            GameInfo actualGameInfo = DataParser.ParseData(e.Result, format);
            LogGameInfo(actualGameInfo);
        }

        /// <summary>
        /// Try to log game info. If the game info is null it won't do anything.
        /// </summary>
        /// <param name="gameInfo">Info about the game.</param>
        private void LogGameInfo(GameInfo gameInfo)
        {
            if (gameInfo == null)
                return;

            using (StreamWriter writer = new StreamWriter(logFile, true))
            {
                writer.WriteLine(string.Format("{0};{1}", DateTime.Now, gameInfo.PlayerCount));
            }
        }
    }
}
