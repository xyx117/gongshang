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
//using System.Data;

namespace gongshangchaxun.Controllers
{
    public class yichangchaxunController : Controller
    {
        private GongshangContent db = new GongshangContent();

        //
        // GET: /yichangchaxun/

        public ActionResult Index(string currentFilter, string searchString, int? page)
        {   
            ViewBag.biaoti = "县企业异常名录公示与查询";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var yichangmingdans = from s in db.yichangmingdans
                                  select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                yichangmingdans = yichangmingdans.Where(s => s.zhuceID.ToUpper().Contains(searchString.ToUpper())
                                       || s.mingcheng.ToUpper().Contains(searchString.ToUpper()) || s.fadingdaibiaoren.ToUpper().Contains(searchString.ToUpper()) || s.zuochujuedingjiguan.Contains(searchString));
                            }

            yichangmingdans = yichangmingdans.OrderByDescending(s => s.lieruriqi);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
          
            return View(yichangmingdans.ToPagedList(pageNumber, pageSize));
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


            var yichangmingdans = from s in db.yichangmingdans
                                  select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                yichangmingdans = yichangmingdans.Where(s => s.zhuceID.ToUpper().Contains(searchString.ToUpper())
                                       || s.mingcheng.ToUpper().Contains(searchString.ToUpper()) || s.fadingdaibiaoren.ToUpper().Contains(searchString.ToUpper()) || s.zuochujuedingjiguan.Contains(searchString));

            }

            yichangmingdans = yichangmingdans.OrderByDescending(s => s.lieruriqi);

            int pageSize = 10;

            int pageNumber = (page ?? 1);

            return View(yichangmingdans.ToPagedList(pageNumber, pageSize));
        
        
        }

        public FileResult GetFile()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "content/uploads/moban/";
            string fileName = "异常企业名录.xls";
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
      // public ActionResult excelImport(HttpPostedFileBase filebase)
        {
            ViewBag.error = "";
           // ViewBag.fugai = form["gengxinmoshi"].ToString();
            HttpPostedFileBase file = Request.Files["files"];
            //string file =form["files"];

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
                    string sqldel = "delete from yichangmingdan";
                    try
                    {
                        con.Open();                                                     

                        SqlCommand sqlcmddel = new SqlCommand(sqldel, con);

                        sqlcmddel.ExecuteNonQuery();   //删除了现有的名单


                        int j;
                        for (int i = 0; i < dr.Length; i++)
                        {
                            j = i + 1;
                            string zhuceID = dr[i]["注册号"].ToString();
                            string mingcheng = dr[i]["企业名称"].ToString();
                            string fadingdaibiaoren = dr[i]["法定代表人"].ToString();
                            string lieruriqi = dr[i]["列入日期"].ToString();
                            string lierushiyou = dr[i]["列入事由"].ToString();

                            string zuochujuedingjiguan1 = dr[i][6].ToString(); //"作出决定机关"

                            string yichuriqi = dr[i]["移出日期"].ToString();
                            string yichushiyou = dr[i]["移出事由"].ToString();

                            string zuochujuedingjiguan = dr[i][9].ToString(); //"作出决定机关"


                            //string insertstr = "insert into yichangmingdan (zhuceID, mingcheng, fadingdaibiaoren, lieruriqi,lierushiyou, yichuriqi, yichushiyou, zuochujuedingjiguan) values ('" + zhuceID + "','" + mingcheng + "','" + fadingdaibiaoren + "','" + lieruriqi + "','" + lierushiyou + "','" + yichuriqi + "','" + tempriqi + "','" + zuochujuedingjiguan + "' )";   "tempriqi"是什么
                                string insertstr = "insert into yichangmingdan (zhuceID, mingcheng, fadingdaibiaoren, lieruriqi,lierushiyou,zuochujuedingjiguan1, yichuriqi, yichushiyou, zuochujuedingjiguan) values ('" + zhuceID + "','" + mingcheng + "','" + fadingdaibiaoren + "','" + lieruriqi + "','" + lierushiyou + "','" + zuochujuedingjiguan1 + "','" + yichuriqi + "','" + yichushiyou + "','" + zuochujuedingjiguan + "' )";
                                //  string insertstr = "insert into yichangmingdan (zhuceID, mingcheng, fadingdaibiaoren, jizaishiyou, jizairiqi, jizaibumen) values ('2000220022','dkfjgf','dfsfsdfs','dfsdfs','2009-10-10','fdsfsfdfd ')";
                                SqlCommand cmd = new SqlCommand(insertstr, con);
                                try
                                {
                                    cmd.ExecuteNonQuery();
                                }

                                catch (Exception ex )
                                {
                                    ViewBag.error = ex.ToString() + "第" + j + "条记录插入有误，请认真检查格式后再导入！";

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

                       
                   //ViewBag.fugai = "离开覆盖了";
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
                            j = i + 1;
                            string zhuceID = dr[i]["注册号"].ToString();
                            string mingcheng = dr[i]["企业名称"].ToString();
                            string fadingdaibiaoren = dr[i]["法定代表人"].ToString();
                            string lieruriqi = dr[i]["列入日期"].ToString();
                            string lierushiyou = dr[i]["列入事由"].ToString();
                            string zuochujuedingjiguan1 = dr[i][6].ToString();//"作出决定机关1"
                            string yichuriqi = dr[i]["移出日期"].ToString();
                            string yichushiyou = dr[i]["移出事由"].ToString();

                             //string insertstr = "insert into yichangmingdan (zhuceID, mingcheng, fadingdaibiaoren, jizaishiyou, jizairiqi, jizaibumen) values ('" + zhuceID + "','" + mingcheng + "','" + fadingdaibiaoren + "','" + jizaishiyou + "','" + tempriqi + "','" + zuochujuedingjiguan + "' )";
                            //     string insertstr = "insert into yichangmingdan (zhuceID, mingcheng, fadingdaibiaoren, jizaishiyou, jizairiqi, jizaibumen) values ('2000220022','dkfjgf','dfsfsdfs','dfsdfs','2009-10-10','fdsfsfdfd ')";
                            string insertstr = "insert into yichangmingdan (zhuceID, mingcheng, fadingdaibiaoren, lieruriqi,lierushiyou, zuochujuedingjiguan1 ,yichuriqi, yichushiyou, zuochujuedingjiguan) values ('" + zhuceID + "','" + mingcheng + "','" + fadingdaibiaoren + "','" + lieruriqi + "','" + lierushiyou + "','" + zuochujuedingjiguan1 + "','" + yichuriqi + "','" + yichushiyou + "','" + zuochujuedingjiguan + "' )";
                             SqlCommand cmd = new SqlCommand(insertstr, con);

                            try
                            {
                                cmd.ExecuteNonQuery();
                            }

                            catch (Exception ex)
                            {
                                ViewBag.error = ex.ToString() + "第" + j + "条记录插入有误，请认真检查格式后再导入！";

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
       
        //
        // GET: /yichangchaxun/Details/5

        public ActionResult Details(string id = null)
        {
            yichangmingdan yichangmingdan = db.yichangmingdans.Find(id);
            if (yichangmingdan == null)
            {
                return HttpNotFound();
            }
            return View(yichangmingdan);
        }

        //
        // GET: /yichangchaxun/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /yichangchaxun/Create

        [HttpPost]
        public ActionResult Create(yichangmingdan yichangmingdan)
        {
            if (ModelState.IsValid)
            {
                db.yichangmingdans.Add(yichangmingdan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(yichangmingdan);
        }

        //
        // GET: /yichangchaxun/Edit/5

        public ActionResult Edit(string id = null)
        {
            yichangmingdan yichangmingdan = db.yichangmingdans.Find(id);
            if (yichangmingdan == null)
            {
                return HttpNotFound();
            }
            return View(yichangmingdan);
        }

        //
        // POST: /yichangchaxun/Edit/5

        [HttpPost]
        public ActionResult Edit(yichangmingdan yichangmingdan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(yichangmingdan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(yichangmingdan);
        }

        //
        // GET: /yichangchaxun/Delete/5

        public ActionResult Delete(string id = null)
        {
            yichangmingdan yichangmingdan = db.yichangmingdans.Find(id);
            if (yichangmingdan == null)
            {
                return HttpNotFound();
            }
            return View(yichangmingdan);
        }

        //
        // POST: /yichangchaxun/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            yichangmingdan yichangmingdan = db.yichangmingdans.Find(id);
            db.yichangmingdans.Remove(yichangmingdan);
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