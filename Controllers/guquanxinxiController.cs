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
    public class guquanxinxiController : Controller
    {
        private GongshangContent db = new GongshangContent();

        //
        // GET: /guquanxinxi/

        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            ViewBag.biaoti = "县企业股权出质登记信息";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var guquanxinxis = from s in db.guquanxinxis
                                  select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                guquanxinxis = guquanxinxis.Where(s => s.mingcheng.ToUpper().Contains(searchString.ToUpper())
                                       || s.chuzhiren.ToUpper().Contains(searchString.ToUpper()) || s.zhiquanren.ToUpper().Contains(searchString.ToUpper()));


            }

            guquanxinxis = guquanxinxis.OrderByDescending(s => s.dengjiriqi);

            int pageSize = 10;

            int pageNumber = (page ?? 1);

            return View(guquanxinxis.ToPagedList(pageNumber, pageSize));
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


            var guquanxinxis = from s in db.guquanxinxis
                                  select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                guquanxinxis = guquanxinxis.Where(s => s.mingcheng.ToUpper().Contains(searchString.ToUpper())
                                        || s.chuzhiren.ToUpper().Contains(searchString.ToUpper()) || s.zhiquanren.ToUpper().Contains(searchString.ToUpper()));

            }

            guquanxinxis = guquanxinxis.OrderByDescending(s => s.dengjiriqi);

            int pageSize = 10;

            int pageNumber = (page ?? 1);

            return View(guquanxinxis.ToPagedList(pageNumber, pageSize));
        }




        public FileResult GetFile()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "content/uploads/moban/";
            string fileName = "股权出质登记信息公示内容.xlsx";
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
                    string sqldel = "delete from guquanxinxi";
                    try
                    {
                        con.Open();

                        SqlCommand sqlcmddel = new SqlCommand(sqldel, con);

                        sqlcmddel.ExecuteNonQuery();   //删除了现有的名单


                        int j;
                        for (int i = 0; i < dr.Length; i++)
                        {
                            j = i + 1;
                            string mingcheng = dr[i]["股权所在公司名称"].ToString();
                           
                            string chuzhiren = dr[i]["出质人"].ToString();
                            string zhiquanren = dr[i]["质权人"].ToString();
                            string chuzhijine = dr[i]["出质股权数额（万元）"].ToString();

                            
                            string danbaojine = dr[i]["被担保债权数额（万元）"].ToString();
                            string dengjiriqi = dr[i]["登记日期"].ToString();
                            string zhuangtai = dr[i]["状态"].ToString();


                            string insertstr = "insert into guquanxinxi (mingcheng, chuzhiren, zhiquanren, jine, beidanbaojine, dengjiriqi, zhuangtai) values ('" + mingcheng + "','" + chuzhiren + "','" + zhiquanren + "','" + chuzhijine + "','" + danbaojine + "','" + dengjiriqi + "','" + zhuangtai + "')";
                           
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
                            string mingcheng = dr[i]["股权所在公司名称"].ToString();

                            string chuzhiren = dr[i]["出质人"].ToString();
                            string zhiquanren = dr[i]["质权人"].ToString();
                            string chuzhijine = dr[i]["出质股权数额（万元）"].ToString();


                            string danbaojine = dr[i]["被担保债权数额（万元）"].ToString();
                            string dengjiriqi = dr[i]["登记日期"].ToString();
                            string zhuangtai = dr[i]["状态"].ToString();


                            string insertstr = "insert into guquanxinxi (mingcheng, chuzhiren, zhiquanren, jine, beidanbaojine, dengjiriqi, zhuangtai) values ('" + mingcheng + "','" + chuzhiren + "','" + zhiquanren + "','" + chuzhijine + "','" + danbaojine + "','" + dengjiriqi + "','" + zhuangtai + "')";
                           
                            
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

        // GET: /guquanxinxi/Details/5

        public ActionResult Details(int id = 0)
        {
            guquanxinxi guquanxinxi = db.guquanxinxis.Find(id);
            if (guquanxinxi == null)
            {
                return HttpNotFound();
            }
            return View(guquanxinxi);
        }

        //
        // GET: /guquanxinxi/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /guquanxinxi/Create

        [HttpPost]
        public ActionResult Create(guquanxinxi guquanxinxi)
        {
            if (ModelState.IsValid)
            {
                db.guquanxinxis.Add(guquanxinxi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(guquanxinxi);
        }

        //
        // GET: /guquanxinxi/Edit/5

        public ActionResult Edit(int id = 0)
        {
            guquanxinxi guquanxinxi = db.guquanxinxis.Find(id);
            if (guquanxinxi == null)
            {
                return HttpNotFound();
            }
            return View(guquanxinxi);
        }

        //
        // POST: /guquanxinxi/Edit/5

        [HttpPost]
        public ActionResult Edit(guquanxinxi guquanxinxi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(guquanxinxi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(guquanxinxi);
        }

        //
        // GET: /guquanxinxi/Delete/5

        public ActionResult Delete(int id = 0)
        {
            guquanxinxi guquanxinxi = db.guquanxinxis.Find(id);
            if (guquanxinxi == null)
            {
                return HttpNotFound();
            }
            return View(guquanxinxi);
        }

        //
        // POST: /guquanxinxi/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            guquanxinxi guquanxinxi = db.guquanxinxis.Find(id);
            db.guquanxinxis.Remove(guquanxinxi);
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