using System;
using System.ServiceProcess;
using System.Timers;
using System.IO;
using System.Xml;
using System.Net.Mail;
using System.Net;

namespace MyWebService
{
    public partial class MySteamService : ServiceBase
    {
        public static readonly string logFile = "service_log.log";

        private int gameId = 244850;
        private Format format = Format.XML;
        private int interval = 1;

        private Timer timer;
        private DataDownloader dataDownloader;

        int counter = 0;
        private int sendAfter = 30;
        private string emailSource;
        private string emailDestination;
        private string emailPassword;
        private string smtpServer;
        private int smtpPort;

        public MySteamService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            LoadSettings();
            if (File.Exists(logFile))
                File.Delete(logFile);


            timer = new Timer(interval * 60 * 1000);
            timer.Elapsed += TimerElapsed;
            timer.AutoReset = true;
            timer.Start();

        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (dataDownloader == null)
            {
                dataDownloader = new DataDownloader(gameId, format, logFile);
            }
            ++counter;

            dataDownloader.GetData();

            if (counter % sendAfter == 0)
            {
                counter = 0;
                SendStatistics();
            }
        }

        private void LoadSettings()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load("SteamServiceSettings.xml");

            string sInterval = xml.SelectSingleNode("settings/interval").InnerText;
            string sGameId = xml.SelectSingleNode("settings/game_id").InnerText;
            string sFormat = xml.SelectSingleNode("settings/format").InnerText;
            string sSendAfter = xml.SelectSingleNode("settings/send_after").InnerText;

            //email settings
            string sSource = xml.SelectSingleNode("settings/email/source")?.InnerText;
            string sDestination = xml.SelectSingleNode("settings/email/destination")?.InnerText;
            string sPassword = xml.SelectSingleNode("settings/email/password")?.InnerText;
            string sSMTP = xml.SelectSingleNode("settings/email/smtp")?.InnerText;
            string sPort = xml.SelectSingleNode("settings/email/port")?.InnerText;

            if (!string.IsNullOrEmpty(sInterval) && int.TryParse(sInterval, out int i))
                this.interval = i;

            if (!string.IsNullOrEmpty(sGameId) && int.TryParse(sGameId, out int game))
                this.gameId = game;

            if (sFormat == "JSON")
                this.format = Format.JSON;

            if (!string.IsNullOrEmpty(sSendAfter) && int.TryParse(sSendAfter, out int outSendAfter))
                this.sendAfter = outSendAfter;

            if (!string.IsNullOrEmpty(sSource))
                this.emailSource = sSource;

            if (!string.IsNullOrEmpty(sDestination))
                this.emailDestination = sDestination;

            if (!string.IsNullOrEmpty(sPassword))
                this.emailPassword = sPassword;

            if (!string.IsNullOrEmpty(sSMTP))
                this.smtpServer = sSMTP;

            if (!string.IsNullOrEmpty(sPort) && int.TryParse(sPort, out int outPort))
                this.smtpPort = outPort;
        }




        private void SendStatistics()
        {
            string graphFile = ChartMaker.CreateChart(logFile);

            try
            {
                using (MailMessage email = new MailMessage(emailSource, emailDestination))
                {
                    email.Subject = "Statistic about game " + gameId.ToString();
                    email.Body = "Hello here is graph about how many players are playing this game at some time.";

                    Attachment attachment = new Attachment(graphFile, "image/png");
                    email.Attachments.Add(attachment);

                    using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                    {
                        smtpClient.Credentials = new NetworkCredential(emailSource, emailPassword);
                        //smtpClient.SendAsync(email, null);
                        smtpClient.Send(email);
                    }
                }
                
            }
            catch (Exception e)
            { 
            }
        }

        protected override void OnStop()
        {
            timer.Stop();
            SendStatistics();
        }


    }
}
