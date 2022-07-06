﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ICTS.com.Models;

namespace ICTS.com.Areas.ICTS.Controllers
{
    [HandleError]
    public class SolutionsController : BaseController
    {
        private Entities db = new Entities();

        // GET: ICTS/Solutions
        public ActionResult Index()
        {
            return View();
        }
        public string UploadImage(HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var now = DateTime.Now.ToString().Trim();
                    var index1 = now.IndexOf(" ");
                    var sub1 = now.Substring(0, index1);
                    var sub11 = sub1.Replace("/", "");
                    var index2 = now.IndexOf(" ", index1 + 1);
                    var sub2 = now.Substring(index1 + 1);
                    var sub21 = sub2.Replace(":", "");
                    string _FileName = "";
                    int index = file.FileName.IndexOf('.');
                    _FileName = sub11 + sub21 + "Solution" + file.FileName;
                    file.SaveAs(Server.MapPath("/Images/" + _FileName));

                    return "/Images/" + _FileName;
                }

            }
            return "";

        }
        [HttpGet]
        public JsonResult Solution(string seach, int page)
        {
            try
            {
                var pageSize = 10;
                var dmspp = (from s in db.Solutions.Where(x => x.Id > 0)
                             select new
                             {
                                 id = s.Id,
                                 name = s.Name,
                                 title = s.Title,
                                 meta = s.Meta,
                                 dec = s.Description,
                                 content = s.Content,
                                 image = s.Image,
                                 createdate = s.CreateDate.Value.Day + "/" + s.CreateDate.Value.Month + "/" + s.CreateDate.Value.Year,
                                 createby = s.CreateBy,
                                 modifiledate = s.ModifileDate.Value.Day + "/" + s.ModifileDate.Value.Month + "/" + s.ModifileDate.Value.Year,
                                 modifileby = s.ModifileBy,
                                 status = s.Status == true ? "Hiển thị" : "Không hiển thị",
                                 statusimage = s.StatusImage == true ? "Hiển thị" : "Không hiển thị"
                             }).ToList().Where(x => x.status.ToLower().Contains(seach) || x.name.ToLower().Contains(seach) ||
                                            x.createby.ToLower().Contains(seach) || x.createdate.ToLower().Contains(seach) ||
                                            x.modifileby.ToLower().Contains(seach) || x.modifiledate.ToLower().Contains(seach) ||
                                            x.statusimage.ToLower().Contains(seach)||x.content.ToLower().Contains(seach));
                var pages = dmspp.Count() % pageSize == 0 ? dmspp.Count() / pageSize : dmspp.Count() / pageSize + 1;
                var dmsp = dmspp.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                return Json(new { code = 200, pages = pages, dmsp = dmsp, msg = "Hiển Thị Dữ liệu thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Hiểm thị dữ liệu thất bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult Add(string name, string url, string title, bool status, bool statusimage, string image,string content)
        {
            try
            {
                var session = (Admin)Session["admin"];
                var nameAdmin = session.Name;
                var d = new Solution();
                var images = image.Substring(8);
                d.Image = images;
                d.Name = name;
                d.Meta = url;
                d.Title = title;
                d.CreateBy = nameAdmin;
                d.CreateDate = DateTime.Now;
                d.ModifileBy = nameAdmin;
                d.ModifileDate = DateTime.Now;
                d.Content = content;
                d.Status = status;
                d.StatusImage = statusimage;
                db.Solutions.Add(d);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Thêm danh sách thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, mmsg = "Thêm danh sách không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult Edit(int id, string name, string url, string title, bool status, bool statusimage, string image,string content)
        {
            try
            {
                var images = image.Substring(8);
                var session = (Admin)Session["admin"];
                var nameAdmin = session.Name;
                var dmsp = db.Solutions.SingleOrDefault(x => x.Id == id);
                dmsp.Name = name;
                dmsp.Meta = url;
                dmsp.Title = title;
                dmsp.Status = status;
                dmsp.StatusImage = statusimage;
                dmsp.Image = images;
                dmsp.Content = content;
                dmsp.ModifileBy = nameAdmin;
                dmsp.ModifileDate = DateTime.Now;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Sửa danh sách thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, mmsg = "Sửa danh sách không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Detail(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var tv = db.Solutions.SingleOrDefault(x => x.Id == id);
                return Json(new { code = 200, tv = tv, msg = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost,ValidateInput(false)]
        public JsonResult Delete(int id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var l = db.Solutions.Find(id);
                var session = (Admin)Session["admin"];
                var role = session.Role;
                if (role == true)
                {
                    db.Solutions.Remove(l);
                    db.SaveChanges();
                    return Json(new { code = 200, msg = "Xoa Dữ Liệu thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 300, msg = "bạn không có quyền xóa dữ liệu" }, JsonRequestBehavior.AllowGet);
                }
               
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Xoa nhật dữ liệu thất bại" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
