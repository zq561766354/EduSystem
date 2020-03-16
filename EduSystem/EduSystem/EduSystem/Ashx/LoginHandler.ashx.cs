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

namespace EduSystem.Ashx
{
    /// <summary>
    /// LoginHandler 的摘要说明
    /// </summary>
    public class LoginHandler : IHttpHandler
    {
        DbBase db = new DbBase();
        List<string> ls1 = new List<string>() { "yes" };
        List<string> ls2 = new List<string>() { "no" };
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            try
            {

                if (db.Open())
                {
                    var data = context.Request;
                    var stream = new StreamReader(data.InputStream).ReadToEnd();
                    var msgData = new JavaScriptSerializer().Deserialize<tb_UserInfo>(stream);
                    msgData.Password = GenerateMD5(msgData.Password);
                    List<tb_UserInfo> userInfos = new List<tb_UserInfo>();
                    tb_Session session = new tb_Session();
                    if (db.SelectUserInfoByPhone(msgData.PhoneNumber, out userInfos))
                    {
                        if (msgData.Password==userInfos[0].Password)
                        {
                            session.PhoneNumber = userInfos[0].PhoneNumber;
                            session.Rank = userInfos[0].Rank;
                            session.UserName = userInfos[0].UserName;
                            session.Session = GenerateMD5(DateTime.Now.ToString("yyyy-MM-dd") + session.PhoneNumber);
                            session.CreateTime = DateTime.Now;
                            tb_Session newSession = new tb_Session();
                            db.SelectSessionBySession(session.Session, out newSession);
                            if (newSession.Session != null)
                            {
                                context.Response.Write(new JavaScriptSerializer().Serialize(ls1));
                            }
                            else
                            {
                                if (db.InsertNewSession(session))
                                {
                                    ls1.Add(session.Session);
                                    context.Response.Write(new JavaScriptSerializer().Serialize(ls1));
                                }
                                else
                                {
                                    context.Response.Write(new JavaScriptSerializer().Serialize(ls2));
                                }
                            }
                        }
                        else
                        {
                            context.Response.Write(new JavaScriptSerializer().Serialize(ls2));
                        }

                    }

                }
                else
                {
                    ls2.Add("数据库连接出错");
                    context.Response.Write(new JavaScriptSerializer().Serialize(ls2));
                }

            }
            catch (Exception)
            {
                context.Response.Write(new JavaScriptSerializer().Serialize(ls2));
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

        public string GenerateMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}