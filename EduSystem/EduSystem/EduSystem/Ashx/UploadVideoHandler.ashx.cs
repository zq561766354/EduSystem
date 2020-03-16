using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using SqlServer;
using SqlServer.Models;


namespace EduSystem.Ashx
{
    /// <summary>
    /// UploadVideoHandler 的摘要说明
    /// </summary>
    public class UploadVideoHandler : IHttpHandler
    {
        string fileNameNo = "";
        string DirectoryName = "";
        string Extension = "";
        string fileName = "";
        string fullPath = "";
        string uploadDown = "";
        string savePath = "";
        string netPath = "";
        string parm = "";
        StringBuilder msg = new StringBuilder();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            DbBase db = new DbBase();
            try
            {
                db.Open();
                string physicalPath = System.Web.HttpContext.Current.Server.MapPath("/VideoPath/");
                if (!Directory.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }
                HttpFileCollection hfc = context.Request.Files;

                HttpPostedFile hpf = context.Request.Files[0];

                fileNameNo = Path.GetFileName(hpf.FileName);
                //获取文件所在目录 
                DirectoryName = Path.GetDirectoryName(hpf.FileName);
                //获取扩展名 
                Extension = Path.GetExtension(hpf.FileName);

                fileName = Path.GetFileNameWithoutExtension(hpf.FileName);
                string newFileName = hpf.FileName;
                uploadDown = physicalPath + newFileName;
                netPath = "../VideoPath/" + newFileName;
                savePath = Path.Combine(physicalPath, newFileName);
                hpf.SaveAs(savePath);
                string _truePath = "http://106.13.37.89/VideoPath/" + hpf.FileName;
                //string videoNo = DateTime.Now.ToString("yyyyMMddhhmm");
                tb_VideoInfo videoInfo = new tb_VideoInfo();
                videoInfo.VideoName = newFileName;
                videoInfo.VideoPath = _truePath;
                videoInfo.CreateTime = DateTime.Now;
                //保存
                msg.Append(
        "{\"isok\":\"true\",\"username\":\"\",\"createtime\":\"\",\"message\":\"上传成功\",\"sourcefilename\":\"" +
        context.Request.RawUrl + "\",\"netfilename\":\"" + _truePath + "\",\"fileid\":\"" +
        newFileName + "\"}");
                context.Response.Write(msg.ToString());
            }
            catch (Exception)
            {
                msg.Append(
       "{\"isok\":\"true\",\"username\":\"\",\"createtime\":\"\",\"message\":\"上传失败\",\"sourcefilename\":\"" +
       context.Request.RawUrl + "\",\"netfilename\":\"" + "nothing" + "\",\"fileid\":\"" +
       "nothing" + "\"}");
                context.Response.Write(msg.ToString());
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