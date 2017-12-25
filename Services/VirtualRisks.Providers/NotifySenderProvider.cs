using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CastleGo.Application.Users;
using CastleGo.Shared.Common;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;

namespace CastleGo.Providers
{
    public class NotifySenderProvider : INotifySenderProvider
    {
        private readonly NotifySettings _notifySetting;
        public NotifySenderProvider(NotifySettings notifySetting)
        {
            _notifySetting = notifySetting;
        }
        public async Task SendNotify(string device, string token, string id, string type, string icon, string title, string content)
        {
            MobileDevice dv;
            var canParse = Enum.TryParse(device, true, out dv);
            if (!canParse)
                return;
            switch (dv)
            {
                case MobileDevice.Android:
                    await SendNotifyToAndroidApp(token, id, type, icon, title, content);
                    break;
                case MobileDevice.iOS:
                    await SendNotifyToiOSApp(token, id, type, icon, title, content);
                    break;
                    //case MobileDevice.Browser:
                    //    NotifyType nType;
                    //    Enum.TryParse(type, out nType);
                    //    NotifyHub.Send(token, new NotificationDataDto()
                    //    {
                    //        Header = title,
                    //        Content = content,
                    //        Type = nType
                    //    });
                    //    break;
            }
        }

        public async Task SendNotify(List<NotifyTokenDto> tokens, string id, string type, string icon, string title, string message)
        {
            if (tokens == null)
                return;
            await
                Task.WhenAll(
                    tokens.Select(
                        token => SendNotify(token.Device.ToString(), token.Token, id, type, icon, title, message)));
        }

        private async Task SendNotifyToiOSApp(string token, string id, string type, string icon, string title, string content)
        {
            // Configuration (NOTE: .pfx can also be used here)
            //var notifySettingsSection = ConfigurationManager.GetSection("NotificationSettings") as NameValueCollection;
            var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Production, _notifySetting.iOSCertificatePath, _notifySetting.iOSCertificatePassword);
            // Create a new broker
            var apnsBroker = new ApnsServiceBroker(config);

            // Wire up events
            apnsBroker.OnNotificationFailed += (notification, aggregateEx) =>
            {

                aggregateEx.Handle(ex =>
                {

                    // See what kind of exception it was to further diagnose
                    if (ex is ApnsNotificationException)
                    {
                        var notificationException = (ApnsNotificationException)ex;

                        // Deal with the failed notification
                        var apnsNotification = notificationException.Notification;
                        var statusCode = notificationException.ErrorStatusCode;

                        Console.WriteLine($"Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}");

                    }
                    else
                    {
                        // Inner exception might hold more useful information like an ApnsConnectionException           
                        Console.WriteLine($"Apple Notification Failed for some unknown reason : {ex.InnerException}");
                    }

                    // Mark it as handled
                    return true;
                });
            };

            apnsBroker.OnNotificationSucceeded += (notification) =>
            {
                Console.WriteLine("Apple Notification Sent!");
            };

            // Start the broker
            apnsBroker.Start();

            // Queue a notification to send
            var data = new JObject(new JProperty("aps",
                        new JObject(
                            new JProperty("alert",
                                new JObject(new JProperty("title", title),
                                            new JProperty("body", content))
                ))), new JProperty("data", new JObject(new JProperty("objectId", id),
                                                        new JProperty("type", type),
                                                        new JProperty("time", DateTime.UtcNow.ToString()))));
            apnsBroker.QueueNotification(new ApnsNotification
            {
                DeviceToken = token,
                Payload = data
            });

            // Stop the broker, wait for it to finish   
            // This isn't done after every message, but after you're
            // done with the broker
            apnsBroker.Stop();
        }

        async Task SendNotifyToAndroidApp(string token, string id, string type, string icon, string title, string content)
        {
            var tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = " application/x-www-form-urlencoded;charset=UTF-8";
            tRequest.Headers.Add($"Authorization: key={_notifySetting.AndroidApplicationId}");

            tRequest.Headers.Add($"Sender: id={_notifySetting.AndroidSendId}");
            string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&"
                + "data.key=" + id
                + "&data.type=" + type
                + "&data.icon=" + icon
                + "&data.title=" + title
                + "&data.message=" + content
                + "&data.time=" + DateTime.UtcNow
                + "&registration_id=" + token + "";
            Console.WriteLine(postData);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            tRequest.ContentLength = byteArray.Length;

            Stream dataStream = await tRequest.GetRequestStreamAsync();
            await dataStream.WriteAsync(byteArray, 0, byteArray.Length);
            dataStream.Close();

            using (WebResponse tResponse = await tRequest.GetResponseAsync())
            {
                dataStream = tResponse.GetResponseStream();
                if (dataStream == null)
                    return;
                using (StreamReader tReader = new StreamReader(dataStream))
                {
                    string sResponseFromServer = await tReader.ReadToEndAsync();
                    tReader.Close();
                    dataStream.Close();
                    tResponse.Close();
                }
            }
        }
    }
}
