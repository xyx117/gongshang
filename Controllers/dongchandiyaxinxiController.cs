//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using gongshangchaxun.Models;
//using gongshangchaxun.DAL;
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
    public class dongchandiyaxinxiController : Controller
    {
        private GongshangContent db = new GongshangContent();

        //
        // GET: /dongchandiyaxinxi/

        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            ViewBag.biaoti = "定安县动产抵押登记信息";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var dongchandiyaxinxis = from s in db.dongchandiyaxinxis
                               select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                dongchandiyaxinxis = dongchandiyaxinxis.Where(s => s.diyaren.ToUpper().Contains(searchString.ToUpper())
                                       || s.dengjiID.ToUpper().Contains(searchString.ToUpper()));


            }

            dongchandiyaxinxis = dongchandiyaxinxis.OrderByDescending(s => s.dengjiriqi);

            int pageSize = 10;

            int pageNumber = (page ?? 1);

            return View(dongchandiyaxinxis.ToPagedList(pageNumber, pageSize));
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


            var dongchandiyaxinxis = from s in db.dongchandiyaxinxis
                               select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                dongchandiyaxinxis = dongchandiyaxinxis.Where(s => s.diyaren.ToUpper().Contains(searchString.ToUpper())
                                       || s.dengjiID.ToUpper().Contains(searchString.ToUpper()));

            }

            dongchandiyaxinxis = dongchandiyaxinxis.OrderByDescending(s => s.dengjiriqi);

            int pageSize = 10;

            int pageNumber = (page ?? 1);

            return View(dongchandiyaxinxis.ToPagedList(pageNumber, pageSize));
        }



        public FileResult GetFile()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "content/uploads/moban/";
            string fileName = "动产抵押登记信息.xlsx";
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

                string path = AppDomain.CurrentDomain.BaseDirectory + "content/uploads/excel/";
                savePath = Path.Combine(path, FileName);
                file.SaveAs(savePath);
            }

            //string result = string.Empty;
            string strConn;



            strConn = "Provider=Microsoft.Ace.OleDb.12.0;Data Source=" + savePath + ";" + "Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'";

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
                    string sqldel = "delete from dongchandiyaxinxi";
                    try
                    {
                        con.Open();

                        SqlCommand sqlcmddel = new SqlCommand(sqldel, con);

                        sqlcmddel.ExecuteNonQuery();   //删除了现有的名单


                        int j;
                        for (int i = 0; i < dr.Length; i++)
                        {
                            j = i + 1;
                            string diyaren = dr[i]["抵押人名称（姓名）"].ToString();

                            string dengjiID = dr[i]["登记编号"].ToString();
                            string dengjiriqi = dr[i]["登记日期"].ToString();
                            string dengjijiguan = dr[i]["登记机关"].ToString();


                            string beidanbaozhaiquanshue = dr[i]["被担保债权数额"].ToString();
                            string danbaofanwei = dr[i]["担保的范围"].ToString();
                            string danbaoqixian = dr[i]["担保期限"].ToString();


                            string insertstr = "insert into dongchandiyaxinxi (diyaren, dengjiID, dengjiriqi, dengjijiguan, beidanbaozhaiquanshue, danbaofanwei, danbaoqixian) values ('" + diyaren + "','" + dengjiID + "','" + dengjiriqi + "','" + dengjijiguan + "','" + beidanbaozhaiquanshue + "','" + danbaofanwei + "','" + danbaoqixian + "')";

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
                            string diyaren = dr[i]["抵押人名称（姓名）"].ToString();

                            string dengjiID = dr[i]["登记编号"].ToString();
                            string dengjiriqi = dr[i]["登记日期"].ToString();
                            string dengjijiguan = dr[i]["登记机关"].ToString();


                            string beidanbaozhaiquanshue = dr[i]["被担保债权数额"].ToString();
                            string danbaofanwei = dr[i]["担保的范围"].ToString();
                            string danbaoqixian = dr[i]["担保期限"].ToString();


                            string insertstr = "insert into dongchandiyaxinxi (diyaren, dengjiID, dengjiriqi, dengjijiguan, beidanbaozhaiquanshue, danbaofanwei, danbaoqixian) values ('" + diyaren + "','" + dengjiID + "','" + dengjiriqi + "','" + dengjijiguan + "','" + beidanbaozhaiquanshue + "','" + danbaofanwei + "','" + danbaoqixian + "')";


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
        // GET: /dongchandiyaxinxi/Details/5

        public ActionResult Details(int id = 0)
        {
            dongchandiyaxinxi dongchandiyaxinxi = db.dongchandiyaxinxis.Find(id);
            if (dongchandiyaxinxi == null)
            {
                return HttpNotFound();
            }
            return View(dongchandiyaxinxi);
        }

        //
        // GET: /dongchandiyaxinxi/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /dongchandiyaxinxi/Create

        [HttpPost]
        public ActionResult Create(dongchandiyaxinxi dongchandiyaxinxi)
        {
            if (ModelState.IsValid)
            {
                db.dongchandiyaxinxis.Add(dongchandiyaxinxi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dongchandiyaxinxi);
        }

        //
        // GET: /dongchandiyaxinxi/Edit/5

        public ActionResult Edit(int id = 0)
        {
            dongchandiyaxinxi dongchandiyaxinxi = db.dongchandiyaxinxis.Find(id);
            if (dongchandiyaxinxi == null)
            {
                return HttpNotFound();
            }
            return View(dongchandiyaxinxi);
        }

        //
        // POST: /dongchandiyaxinxi/Edit/5

        [HttpPost]
        public ActionResult Edit(dongchandiyaxinxi dongchandiyaxinxi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dongchandiyaxinxi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dongchandiyaxinxi);
        }

        //
        // GET: /dongchandiyaxinxi/Delete/5

        public ActionResult Delete(int id = 0)
        {
            dongchandiyaxinxi dongchandiyaxinxi = db.dongchandiyaxinxis.Find(id);
            if (dongchandiyaxinxi == null)
            {
                return HttpNotFound();
            }
            return View(dongchandiyaxinxi);
        }

        //
        // POST: /dongchandiyaxinxi/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            dongchandiyaxinxi dongchandiyaxinxi = db.dongchandiyaxinxis.Find(id);
            db.dongchandiyaxinxis.Remove(dongchandiyaxinxi);
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