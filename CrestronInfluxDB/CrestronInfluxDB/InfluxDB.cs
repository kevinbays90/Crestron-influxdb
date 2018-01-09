/*
**        File | InfluxDB.cs 
**      Author | Kevin Bays
** Description | ...
*/

using System;
using System.Text;
using System.Collections.Generic;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronSockets;
using Crestron.SimplSharp.Net.Http;

namespace CrestronInfluxDB
{
    public class HttpAPI
    {
        internal static string InfluxDbIp;
        internal static string InfluxDbName;
        internal static int InfluxDbPort;

        // S+ Delegates //
        public static DelegateString CommandStatus { get; set; }

        //===================// Methods //===================//

        //-------------------------------------//
        //    Function | TCPClientSettings
        // Description | Called by S+ symbol to pass TCP client settings from SIMPL program, then 
        //               attempts to connect.
        //-------------------------------------//

        public static void TCPClientSettings(string _ip, ushort _prt, string _dbname)
        {
            InfluxDbIp = _ip;
            InfluxDbPort = _prt;
            InfluxDbName = _dbname;
        }

        //-------------------------------------//
        //    Function | InfluxDBPost
        // Description | Called by S+ symbol to pass HTTP post Content from SIMPL program, then 
        //               attempts to post to Influxdb.
        //-------------------------------------//
        public static void InfluxDBPost(String Content)
        {
            try
            {
                var httpInflux = new HttpClient();
                httpInflux.KeepAlive = false;
                httpInflux.Accept = "application/json";
                httpInflux.UserName = "admin";
                httpInflux.Password = "admin";
                HttpClientRequest sRequest = new HttpClientRequest();
                sRequest.RequestType = RequestType.Post;
                sRequest.Url.Parse("http://" + InfluxDbIp + ":" + InfluxDbPort + "/write?db=" + InfluxDbName);
                sRequest.ContentString = Content;

                httpInflux.DispatchAsync(sRequest, HTTPClientResponseCallback);

                CrestronConsole.PrintLine("Sent POST {0}", sRequest.ContentString);

            }
            catch (Exception e)
            {
                CrestronConsole.PrintLine("Error while sending POST {0}", e.Message);

            }
        }

        //-------------------------------------//
        //    Function | HTTPClientResponseCallback
        // Description | ...
        //-------------------------------------//
        public static void HTTPClientResponseCallback(HttpClientResponse userobj, HTTP_CALLBACK_ERROR error)
        {
            SendStatus(userobj.Code, userobj.ContentString);
            CrestronConsole.PrintLine("Received {0} from InfluxDB {0}", userobj.Code, userobj.ContentString);

        }

        //-------------------------------------//
        //    Function | SendStatus
        // Description | ...
        //-------------------------------------//

        internal static void SendStatus(int code, string datas)
        {
            string status;
            if (code > 200 && code < 300)
            {
                status = "OK";
            }
            else
            {
                status = "Error " + code + " - " + datas;
            }

            CommandStatus(status); //pass status to S+
        }
    }

    // S+ Plus DelegateString
    public delegate void DelegateString(SimplSharpString str);
}
