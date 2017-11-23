using Newtonsoft.Json.Linq;
using System.Xml;

namespace MyWebService
{
    class DataParser
    {
        /// <summary>
        /// Parse data based on format.
        /// </summary>
        /// <param name="data">Data string.</param>
        /// <param name="format">Format of response.</param>
        /// <returns>Game info or null when received data are not valid.</returns>
        public static GameInfo ParseData(string data, Format format)
        {
            switch (format)
            {
                case Format.XML:
                    return ParseXML(data);
                case Format.JSON:
                    return ParseJSON(data);
            }
            return null;
        }

        /// <summary>
        /// Parse JSON string into game info.
        /// </summary>
        /// <param name="data">Json string.</param>
        /// <returns>Game info or null when data are wrong.</returns>
        private static GameInfo ParseJSON(string data)
        {
            JObject jsonData = JObject.Parse(data);
            JToken resultToken = jsonData.SelectToken("response.result");

            if (resultToken != null && resultToken.Value<int>() == 1)
            {
                int result = resultToken.Value<int>();
                if (result == 1)
                {
                    JToken playerCountToken = jsonData.SelectToken("response.player_count");
                    if (playerCountToken != null && playerCountToken.Value<int>() > 0)
                    {
                        return new GameInfo(playerCountToken.Value<int>());
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Parse XML string into game info.
        /// </summary>
        /// <param name="data">XML string.</param>
        /// <returns>Game info or null when data are wrong.</returns>
        private static GameInfo ParseXML(string data)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(data);

            if (xml.SelectSingleNode("//result").InnerText == "1")
            {
                string sPlayerCount = xml.SelectSingleNode("//player_count").InnerText;
                if (!string.IsNullOrEmpty(sPlayerCount) && int.TryParse(sPlayerCount, out int playerCount))
                {
                    return new GameInfo(playerCount);
                }
            }
            return null;
        }
    }
}
