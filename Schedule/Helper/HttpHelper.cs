using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Schedule.Helper
{
    public class HttpHelper
    {
        public static string LoginPost(string Url, byte[] postData, CookieContainer bCookie, String encodingFormat)
        {
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url.ToString());
                myRequest.CookieContainer = bCookie;
                myRequest.Method = "POST";
                myRequest.Headers["Cache-control"] = "no-cache";
                myRequest.Headers["Accept-Language"] = "zh-cn";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.Accept = "*/*";

                Stream newStream = myRequest.GetRequestStream();
                newStream.Write(postData, 0, postData.Length);

                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.GetEncoding(encodingFormat));
                string outdata = reader.ReadToEnd();
                if (!outdata.Contains("基础连接已经关闭: 连接被意外关闭") && !outdata.Contains("无法连接到远程服务器") && !outdata.Contains("基础连接已经关闭: 接收时发生错误。"))
                    return outdata;
                else
                    return "基础连接已经关闭: 连接被意外关闭";
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("基础连接已经关闭: 连接被意外关闭") && !ex.Message.Contains("无法连接到远程服务器") && !ex.Message.Contains("基础连接已经关闭: 接收时发生错误。"))
                    return ex.Message;
                else
                    return "基础连接已经关闭: 连接被意外关闭";
            }

        }
        public static byte[] GetCheckCode(string Url, CookieContainer bCookie)
        {
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url.ToString());
                myRequest.CookieContainer = bCookie;
                myRequest.Method = "GET";
                myRequest.KeepAlive = true;
                myRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0)";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                return ReadToEnd(myResponse.GetResponseStream());
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public static byte[] ReadToEnd(Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
    }
}
