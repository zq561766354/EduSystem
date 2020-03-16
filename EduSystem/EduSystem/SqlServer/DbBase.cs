using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlSugar;
using SqlServer.Models;

namespace SqlServer
{
    public class DbBase
    {
        //string _connectionString = "Server=106.13.37.89; Database=EduSystem;User ID=TestSql; Password=Zq1993102;MultipleActiveResultSets=true";
        string _connectionString = "Server=192.168.2.184;Database=EduSystem;User ID=zqSql; Password=Zq1993102;MultipleActiveResultSets=true";

        SqlSugarClient db;

        #region "Open & Close"
        public bool Open()
        {
            try
            {
                db = new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = _connectionString,
                    DbType = SqlSugar.DbType.SqlServer,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute
                });
                db.Open();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Close()
        {
            try
            {
                db.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region "Insert"
        public bool InsertNewUser(tb_UserInfo userInfo)
        {
            try
            {
                int a = db.Insertable(userInfo).ExecuteReturnIdentity();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool InsertNewSession(tb_Session session)
        {
            try
            {
                int a = db.Insertable(session).ExecuteReturnIdentity();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool InsertNewQuestion(tb_Question question)
        {
            try
            {
                int a = db.Insertable(question).ExecuteReturnIdentity();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region "Delete"
        #endregion

        #region "Update"
        #endregion

        #region "Select"
        public bool SelectUserInfoByPhone(string phone, out List<tb_UserInfo> userInfos)
        {
            userInfos = new List<tb_UserInfo>();
            try
            {
                userInfos = db.Queryable<tb_UserInfo>().Where(it => it.PhoneNumber == phone).ToList();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SelectSessionBySession(string session, out tb_Session Session)
        {
            Session = new tb_Session();
            List<tb_Session> Sessions = new List<tb_Session>();
            try
            {
                Sessions = db.Queryable<tb_Session>().Where(it => it.Session == session).ToList();
                Session = Sessions[0];
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SelectVideosByPhone(string phone, out List<tb_UserVideo> userVideos)
        {
            userVideos = new List<tb_UserVideo>();
            try
            {
                userVideos = db.Queryable<tb_UserVideo>().Where(it => it.UserPhone==phone).ToList();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
