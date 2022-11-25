using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gongshangchaxun.DAL;
using gongshangchaxun.Models;
using System.Data.OleDb;
using System.Transactions;
using System.Data.SqlClient;

namespace gongshangchaxun.Controllers
{   
    public class adminController : Controller
    { 
       private GongshangContent db = new GongshangContent();
        //
        // GET: /admin/
        [Authorize(Roles="manage")]
        public ActionResult Index()
        {
            return View( );
        }

        [Authorize(Roles = "manage")]
        public ActionResult listuser()
        {
            return View(db.userprofiles.ToList());
        }


        // GET: /Default1/Delete/5
        [Authorize(Roles = "manage")]
        public ActionResult Delete(int id = 0)
        {
            UserProfile userprofile = db.userprofiles.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
           
            return View(userprofile);
        }

        
        // POST: /Default1/Delete/5

        [HttpPost, ActionName("Delete")]
        
        [Authorize(Roles = "manage")]
        public ActionResult DeleteConfirmed(int id)
        {
            UserProfile userprofile = db.userprofiles.Find(id);
            using (TransactionScope transaction = new TransactionScope())
            {               
                string ConString = System.Configuration.ConfigurationManager.ConnectionStrings["GongshangContent"].ConnectionString;
                SqlConnection con = new SqlConnection(ConString);
                string sqldel1 = "delete from UserProfile where userid=' " + id.ToString() + "'";
                string sqldel2 = "delete from webpages_Membership where userid=' " + id.ToString() + "'";
                string sqldel3 = "delete from member where Username='" + userprofile.UserName + "'";
                try
                {
                    con.Open();

                    SqlCommand sqlcmddel = new SqlCommand(sqldel1, con);

                    sqlcmddel.ExecuteNonQuery();   //删除了姓名的名单


                    sqlcmddel = new SqlCommand(sqldel2, con);

                    sqlcmddel.ExecuteNonQuery();   //删除了密码的名单

                    sqlcmddel = new SqlCommand(sqldel3, con);

                    sqlcmddel.ExecuteNonQuery();   //删除了ftp用户的名单
                  
                }
                catch (Exception ex)
                {
                    ViewBag.error = ex.Message;
                    return View(); //返回错误描述
                }
                finally
                {
                    con.Close(); //无论如何都要执行的语句。
                }
                transaction.Complete();
            }
            return RedirectToAction("listuser");
    
          }


        public ActionResult setupcanshu() 
        {         
            return View(db.setupdbs.ToList());                 
        }


        [HttpPost]
        //[Authorize(Roles = "manage")]
     public ActionResult setupcanshu(FormCollection form)
         //  public ActionResult setupcanshu( setupdb setupdb)
        {
            

           string canshu=form["canshu"].ToString();
           string zhi = form["zhi"].ToString();
           
            string ConString = System.Configuration.ConfigurationManager.ConnectionStrings["GongshangContent"].ConnectionString;
            SqlConnection con = new SqlConnection(ConString);

            string insertstr = "insert into setupdb (canshu, zhi) values ('" + canshu + "','" + zhi + "' )";
            //  string insertstr = "insert into yichangmingdan (zhuceID, mingcheng, fadingdaibiaoren, jizaishiyou, jizairiqi, jizaibumen) values ('2000220022','dkfjgf','dfsfsdfs','dfsdfs','2009-10-10','fdsfsfdfd ')";
            SqlCommand cmd = new SqlCommand(insertstr, con);

            try
            {
                cmd.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                ViewBag.error = "第" +  "条名单插入有误，请认真检查格式后再导入！";

                return View();
            }

            finally
            {
                con.Close();
            }
            
            return View();        
            
        }

        
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        
    }
}
