using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using SqlServer;
using SqlServer.Models;
using Newtonsoft.Json;
using Newtonsoft;

namespace EduSystem.Ashx
{
    /// <summary>
    /// GetVideoTableByPhoneHandler 的摘要说明
    /// </summary>
    public class GetVideoTableByPhoneHandler : IHttpHandler
    {
        DbBase db = new DbBase();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            try
            {
                db.Open();
                var data = context.Request;
                var stream = new StreamReader(data.InputStream).ReadToEnd();
                var msgData = new JavaScriptSerializer().Deserialize<tb_Session>(stream);
                string _phone = msgData.PhoneNumber;
                List<tb_UserVideo> userVideos = new List<tb_UserVideo>();
                if (db.SelectVideosByPhone(_phone, out userVideos))
                {
                    var data1 = new
                    {
                        data = userVideos
                    };
                    context.Response.Write(JsonConvert.SerializeObject(data1, new JsonSerializerSettings
                    {
                        DateFormatString = "yyyy-MM-dd HH:mm:ss"
                    }));
                }     
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                db.Close();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}