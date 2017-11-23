using Newtonsoft.Json.Linq;
using System.Xml;

namespace MyWebService
{
    class DataParser
    {
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
