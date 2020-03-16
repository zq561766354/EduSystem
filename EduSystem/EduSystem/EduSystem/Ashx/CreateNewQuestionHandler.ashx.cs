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
    /// CreateNewQuestionHandler 的摘要说明
    /// </summary>
    public class CreateNewQuestionHandler : IHttpHandler
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
                var msgData = new JavaScriptSerializer().Deserialize<tb_Question>(stream);
                //保存到服务器
                db.InsertNewSession
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