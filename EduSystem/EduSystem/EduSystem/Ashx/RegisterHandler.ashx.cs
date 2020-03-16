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
    /// RegisterHandler 的摘要说明
    /// </summary>
    public class RegisterHandler : IHttpHandler
    {
        DbBase db = new DbBase();
        List<string> ls1 = new List<string>() { "yes" };
        List<string> ls2 = new List<string>() { "no" };
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            try
            {
                db.Open();
                var data = context.Request;
                var stream = new StreamReader(data.InputStream).ReadToEnd();
                var msgData = new JavaScriptSerializer().Deserialize<tb_UserInfo>(stream);
                //检测手机号是否注册
                if (CheckPhone(msgData.PhoneNumber))
                {
                    msgData.Rank = "5";
                    msgData.Password = GenerateMD5(msgData.Password);
                    if (db.InsertNewUser(msgData))
                    {
                        context.Response.Write(new JavaScriptSerializer().Serialize(ls1));
                    }
                    else
                    {
                        context.Response.Write(new JavaScriptSerializer().Serialize(ls2));
                    }
                }
                else
                {
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

        public bool CheckPhone(string phoneNo)
        {
            try
            {
                List<tb_UserInfo> userInfos = new List<tb_UserInfo>();
                if (db.SelectUserInfoByPhone(phoneNo, out userInfos))
                {
                    if (userInfos.Count==0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }           
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}