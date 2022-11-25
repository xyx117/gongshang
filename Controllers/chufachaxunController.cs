using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gongshangchaxun.Models;
using gongshangchaxun.DAL;
using PagedList;
using System.Data.OleDb;
using System.Transactions;
using System.Data.SqlClient;

namespace gongshangchaxun.Controllers
{
    public class chufachaxunController : Controller
    {       

        private GongshangContent db = new GongshangContent();

        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            ViewBag.biaoti = "县企业处罚名单公示与查询";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

           

            var chufamingdans = from s in db.chufamingdans
                                  select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                chufamingdans = chufamingdans.Where(s => s.zhuceID.ToUpper().Contains(searchString.ToUpper())
                                       || s.mingcheng.Contains(searchString) || s.fadingdaibiaoren.Contains(searchString) || s.jizaibumen.Contains(searchString)
                                       || s.chufatongzhishu.ToUpper().Contains(searchString.ToUpper()));
                                    
            }

            chufamingdans = chufamingdans.OrderByDescending(s => s.jizairiqi);

            int pageSize =10;

            int pageNumber = (page ?? 1);

            return View(chufamingdans.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult caozuoindex(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var chufamingdans = from s in db.chufamingdans
                                select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                
                chufamingdans = chufamingdans.Where(s => s.zhuceID.ToUpper().Contains(searchString.ToUpper())
                                       || s.mingcheng.Contains(searchString) || s.fadingdaibiaoren.Contains(searchString) || s.jizaibumen.Contains(searchString)
                                       || s.chufatongzhishu.ToUpper().Contains(searchString.ToUpper()));
            }

            chufamingdans = chufamingdans.OrderByDescending(s => s.jizairiqi);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(chufamingdans.ToPagedList(pageNumber, pageSize));
        }


        public FileResult GetFile()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "content/uploads/moban/";
            string fileName = "处罚企业名录.xls";
            return File(path + fileName, "text/plain", fileName);
        }


         [Authorize]
        public ActionResult excelImport()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult excelImport(FormCollection form)
        {
            ViewBag.error = "";
            HttpPostedFileBase file = Request.Files["files"];
            string FileName;
            string savePath;
            if (file == null || file.ContentLength <= 0)
            {
                ViewBag.error = "文件不能为空";
                return View();
            }
            else
            {
                string filename = Path.GetFileName(file.FileName);
                int filesize = file.ContentLength;//获取上传文件的大小单位为字节byte
                string fileEx = System.IO.Path.GetExtension(filename);//获取上传文件的扩展名
                string NoFileName = System.IO.Path.GetFileNameWithoutExtension(filename);//获取无扩展名的文件名

                //int Maxsize = 4000 * 1024;//定义上传文件的最大空间大小为4M

                string FileType = ".xls,.xlsx";//定义上传文件的类型字符串

                FileName = NoFileName + DateTime.Now.ToString("yyyyMMddhhmmss") + fileEx;
                if (!FileType.Contains(fileEx))
                {
                    ViewBag.error = "文件类型不对，只能导入xls和xlsx格式的文件";
                    return View();
                }
                //if (filesize >= Maxsize)
                //{
                //    ViewBag.error = "上传文件超过4M，不能上传";
                //    return View();
                //}
                string path = AppDomain.CurrentDomain.BaseDirectory + "content/uploads/excel/";
                savePath = Path.Combine(path, FileName);
                file.SaveAs(savePath);
            }

            //string result = string.Empty;
            string strConn;
            //office 2007 可用 导入xls 不用于.xlsx
           //strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + savePath + ";" + "Extended Properties=Excel 8.0";

            strConn="Provider=Microsoft.Ace.OleDb.12.0;Data Source="+ savePath +";"+"Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'";

            OleDbConnection conn = new OleDbConnection(strConn);
            OleDbDataAdapter myCommand = new OleDbDataAdapter("select * from [Sheet1$]", strConn);
            DataSet myDataSet = new DataSet();
            try
            {
                conn.Open();
                myCommand.Fill(myDataSet, "ExcelInfo");

            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;
                return View(); //返回错误描述
            }
            finally
            {
                conn.Close(); //无论如何都要执行的语句。
            }

            DataTable ds = myDataSet.Tables["ExcelInfo"].DefaultView.ToTable();
            DataRow[] dr = ds.Select();

            //引用事务机制，出错时，事物回滚
            using (TransactionScope transaction = new TransactionScope())
            {
                if (form["gengxinmoshi"].ToString() == "fugai")
                {
                    string ConString = System.Configuration.ConfigurationManager.ConnectionStrings["GongshangContent"].ConnectionString;
                    SqlConnection con = new SqlConnection(ConString);
                    string sqldel = "delete from chufamingdan";
                    try
                    {
                        con.Open();

                        SqlCommand sqlcmddel = new SqlCommand(sqldel, con);

                        sqlcmddel.ExecuteNonQuery();   //删除了现有的名单

                        int j;
                        for (int i = 0; i < dr.Length; i++)
                        {
                            j = i+1;
                            string zhuceID = dr[i]["注册号"].ToString();
                            string mingcheng = dr[i]["企业名称"].ToString();
                            string fadingdaibiaoren = dr[i]["法定代表人"].ToString();
                            string jizaishiyou = dr[i]["处罚事由"].ToString();
                            string chufatongzhishu = dr[i]["详情"].ToString();
                            DateTime tempriqi = DateTime.Now;
                            if (!DateTime.TryParse(dr[i]["处罚日期"].ToString(), out tempriqi))
                            {
                                ViewBag.error = "第" + j + "条记录的记载日期格式可能有误，请认真检查后再导入！";
                                return View();
                            }

                            string jizaibumen = dr[i]["作出决定机关"].ToString();

                            string insertstr = "insert into chufamingdan (zhuceID, mingcheng, fadingdaibiaoren, jizaishiyou, jizairiqi, jizaibumen ,chufatongzhishu) values ('" + zhuceID + "','" + mingcheng + "','" + fadingdaibiaoren + "','" + jizaishiyou + "','" + tempriqi + "','" + jizaibumen + "','" + chufatongzhishu + "' )";
                            // string insertstr = "insert into yichangmingdan (zhuceID, mingcheng, fadingdaibiaoren, jizaishiyou, jizairiqi, jizaibumen) values ('2000220022','dkfjgf','dfsfsdfs','dfsdfs','2009-10-10','fdsfsfdfd ')";
                            SqlCommand cmd = new SqlCommand(insertstr, con);

                            try
                            {
                                cmd.ExecuteNonQuery();
                            }

                            catch (Exception ex)
                            {
                                ViewBag.error = "第" + j + "条记录插入有误，请认真检查格式后再导入！";

                                return View();
                            }
                        }
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


                   // ViewBag.fugai = "离开覆盖了";
                }
                else  //追加模式
                {
                    
                    string ConString = System.Configuration.ConfigurationManager.ConnectionStrings["GongshangContent"].ConnectionString;
                    SqlConnection con = new SqlConnection(ConString);

                    try
                    {
                        con.Open();
                        int j;
                        for (int i = 0; i < dr.Length; i++)
                        {
                            j = i+1;
                            string zhuceID = dr[i]["注册号"].ToString();
                            string mingcheng = dr[i]["企业名称"].ToString();
                            string fadingdaibiaoren = dr[i]["法定代表人"].ToString();
                            string jizaishiyou = dr[i]["处罚事由"].ToString();
                            string chufatongzhishu = dr[i]["详情"].ToString();

                            // DateTime jizairiqi = Convert.ToDateTime(dr[i]["记载日期"].ToString());
                            //DateTime jizairiqi = DateTime.Now;
                            // DateTime jizairiqi = Convert.ToDateTime(getDateStr(dr[i]["记载日期"].ToString()));
                            DateTime tempriqi = DateTime.Now;
                            if (!DateTime.TryParse(dr[i]["处罚日期"].ToString(), out tempriqi))
                            {
                                ViewBag.error = "第" + j + "条记录的记载日期格式可能有误，请认真检查后再导入！";
                                return View();
                            }

                            string jizaibumen = dr[i]["作出决定机关"].ToString();

                            string insertstr = "insert into chufamingdan (zhuceID, mingcheng, fadingdaibiaoren, jizaishiyou, jizairiqi, jizaibumen ,chufatongzhishu) values ('" + zhuceID + "','" + mingcheng + "','" + fadingdaibiaoren + "','" + jizaishiyou + "','" + tempriqi + "','" + jizaibumen + "','" + chufatongzhishu + "' )";
                            // string insertstr = "insert into yichangmingdan (zhuceID, mingcheng, fadingdaibiaoren, jizaishiyou, jizairiqi, jizaibumen) values ('2000220022','dkfjgf','dfsfsdfs','dfsdfs','2009-10-10','fdsfsfdfd ')";
                            SqlCommand cmd = new SqlCommand(insertstr, con);

                            try
                            {
                                cmd.ExecuteNonQuery();
                            }

                            catch (Exception ex)
                            {
                                ViewBag.error = "第" + j + "条记录插入有误，请认真检查格式后再导入！";

                                return View();
                            }
                        }
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
                }
                transaction.Complete();
            }
            ViewBag.error = "祝贺您，本次企业信息导入成功！";
            System.Threading.Thread.Sleep(2000);
            return RedirectToAction("./caozuoindex");
        }


        public ActionResult opentongzhishu(string filename)
        {
          
            filename=filename+ ".pdf";
            string path = "~/Content/uploads/chufatongzhishu/"+filename;
            string s = Server.MapPath(path);
            
            string filenames = path + filename+s ;
        
            if (System.IO.File.Exists(s))
            {
              //存在文件                
               return Redirect(path); 
             }else
           {
                //不存在文件
               return View();
             }        
        }

       

        public ActionResult Details(string id = null)
        {
            chufamingdan chufamingdan = db.chufamingdans.Find(id);
            if (chufamingdan == null)
            {
                return HttpNotFound();
            }
            return View(chufamingdan);
        }

        //
        // GET: /chufachaxun/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /chufachaxun/Create

        [HttpPost]
        public ActionResult Create(chufamingdan chufamingdan)
        {
            if (ModelState.IsValid)
            {
                db.chufamingdans.Add(chufamingdan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chufamingdan);
        }

        //
        // GET: /chufachaxun/Edit/5

        public ActionResult Edit(string id = null)
        {
            chufamingdan chufamingdan = db.chufamingdans.Find(id);
            if (chufamingdan == null)
            {
                return HttpNotFound();
            }
            return View(chufamingdan);
        }

        //
        // POST: /chufachaxun/Edit/5

        [HttpPost]
        public ActionResult Edit(chufamingdan chufamingdan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chufamingdan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chufamingdan);
        }

        //
        // GET: /chufachaxun/Delete/5

        public ActionResult Delete(string id = null)
        {
            chufamingdan chufamingdan = db.chufamingdans.Find(id);
            if (chufamingdan == null)
            {
                return HttpNotFound();
            }
            return View(chufamingdan);
        }

        //
        // POST: /chufachaxun/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            chufamingdan chufamingdan = db.chufamingdans.Find(id);
            db.chufamingdans.Remove(chufamingdan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}