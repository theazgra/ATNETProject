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

        public DataDownloader(int gameId, Format format, string logFile)
        {
            this.format = format;
            this.gameId = gameId;
            this.logFile = logFile;
        }

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

        private void StringDownloaded(object sender, DownloadStringCompletedEventArgs e)
        {
            GameInfo actualGameInfo = DataParser.ParseData(e.Result, format);
            LogGameInfo(actualGameInfo);
        }

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
