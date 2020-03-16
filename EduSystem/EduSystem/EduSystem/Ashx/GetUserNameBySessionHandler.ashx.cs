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
    /// GetUserNameBySessionHandler 的摘要说明
    /// </summary>
    public class GetUserNameBySessionHandler : IHttpHandler
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
                string _session = msgData.Session;
                tb_Session session = new tb_Session();
                if (db.SelectSessionBySession(_session,out session))
                {
                    var data1 = new
                    {
                        username = session.UserName,
                        rank = session.Rank,
                        phone = session.PhoneNumber
                    };
                    context.Response.Write(JsonConvert.SerializeObject(data1));
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