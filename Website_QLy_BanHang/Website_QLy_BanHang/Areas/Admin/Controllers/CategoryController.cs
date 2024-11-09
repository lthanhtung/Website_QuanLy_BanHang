using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Model;
using Website_QLy_BanHang.Library;

namespace Website_QLy_BanHang.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        /// <summary>
        /// ///////////////////////////////////////////////////
        /// Index
        // GET: Admin/Category
        public ActionResult Index()
        {
            return View(categoriesDAO.getList("Index"));
        }

        /// ///////////////////////////////////////////////////
        /// Details
        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //Thông báo cập nhập trạng thái thất bại

                TempData["message"] = new XMessage("danger", "Không có tồn tại loại sản phẩm");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories catelogies = categoriesDAO.getRow(id);

            if (catelogies == null)
            {
                //Thông báo cập nhập trạng thái thất bại

                TempData["message"] = new XMessage("danger", "Không tồn tại loại sản phẩm");
                return HttpNotFound();
            }
            return View(catelogies);
        }

        /// ///////////////////////////////////////////////////
        /// Create
        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categories catelogies)
        {
            if (ModelState.IsValid)
            {

                //Xử lý tự động: CreateAt
                catelogies.CreateAt = DateTime.Now;
                //Xử lý tự động: UpdateAt
                catelogies.UpdateAt = DateTime.Now;
                //Xử lý tự động: ParentID
                if (catelogies.ParentId == null)
                {
                    catelogies.ParentId = 0;
                }
                //Xử lý tự động: Order
                if (catelogies.Order == null)
                {
                    catelogies.Order = 1;
                }
                else
                {
                    catelogies.Order += 1;
                }
                //Xử lý tự động: Slug
                catelogies.Slug = XString.Str_Slug(catelogies.Name);



                //Chèn thêm dòng cho database
                categoriesDAO.Insert(catelogies);
                //Thong báo thành công
                TempData["message"] = TempData["message"] = new XMessage("success", "Cập nhập trạng thái thành công");
                //Trở về trang index
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(catelogies);
        }

        /// ///////////////////////////////////////////////////
        /// Edit
        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["message"] = TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin ");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories catelogies = categoriesDAO.getRow(id);
            if (catelogies == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(catelogies);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Categories catelogies)
        {
            if (ModelState.IsValid)
            {

                //xử lý tự động : slug
                catelogies.Slug = XString.Str_Slug(catelogies.Name);
                //Xử lý tự động: ParentID
                if (catelogies.ParentId == null)
                {
                    catelogies.ParentId = 0;

                }
                //xử lý tự động : Order
                if (catelogies.Order == null)
                {
                    catelogies.Order = 1;
                }
                else
                {
                    catelogies.Order += 1;
                }
                //xử lý tự động: UpdateAt
                catelogies.CreateAt = DateTime.Now;
                //Cập nhập mẫu tin
                categoriesDAO.Update(catelogies);

                //Thong báo thành công
                TempData["message"] = TempData["message"] = new XMessage("success", "Cập nhập mẫu tin thành công");

                //Trở về trang index
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(catelogies);

        }

        /// ///////////////////////////////////////////////////
        /// Delete
        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //thông báo thất bại
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories catelogies = categoriesDAO.getRow(id);
            if (catelogies == null)
            {
                TempData["message"] = new XMessage("danger", "Xóa mẫu tin thất bại");
                return HttpNotFound();
            }
            return View(catelogies);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categories catelogies = categoriesDAO.getRow(id);
            categoriesDAO.Delete(catelogies);
            //thông báo thành công
            TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công");
            return RedirectToAction("Trash");
        }

        /// ///////////////////////////////////////////////////
        /// Thùng rác - Trash
        // GET: Admin/Category/Status/5
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //Thông báo cập nhập trạng thái thất bại

                TempData["message"] = new XMessage("danger", "Cập nhập trạng thái thất bại");
                return RedirectToAction("Index");
            }
            //Truy vấn id
            Categories catelogies = categoriesDAO.getRow(id);
            if (catelogies == null)
            {
                //Thông báo cập nhập trạng thái thất bại

                TempData["message"] = new XMessage("danger", "Cập nhập trạng thái thất bại");
                return RedirectToAction("Index");
            }
            else
            {


                //chuyển đổi trạng thái của Status 1<->2
                catelogies.Status = (catelogies.Status == 1) ? 2 : 1;
                //Cập nhập trạng thái
                catelogies.UpdateAt = DateTime.Now;
                //cập nhập lại database
                categoriesDAO.Update(catelogies);

                //Thông báo cập nhập trạng thái thành công
                TempData["message"] = TempData["message"] = new XMessage("success", "Cập nhập trạng thái thành công");
                return RedirectToAction("Index");
            }
        }


        // GET: Admin/Category/DelTrash/5
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //Thông báo cập nhập trạng thái thất bại

                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }
            //Truy vấn id
            Categories catelogies = categoriesDAO.getRow(id);
            if (catelogies == null)
            {
                //Thông báo cập nhập trạng thái thất bại

                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }
            else
            {


                //chuyển đổi trạng thái của Status 1,2<->0 : khong hieu thi o index
                catelogies.Status = 0;
                //Cập nhập trạng thái
                catelogies.UpdateAt = DateTime.Now;
                //cập nhập lại database
                categoriesDAO.Update(catelogies);

                //Thông báo cập nhập trạng thái thành công
                TempData["message"] = TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công");
                return RedirectToAction("Index");
            }
        }

        /// Trash
        // GET: Admin/Category/Recover
        public ActionResult Trash()
        {
            return View(categoriesDAO.getList("Trash"));
        }
        //Recover
        // GET: Admin/Category/Recover/5

        public ActionResult Recover(int? id)
        {
            if (id == null)
            {
                //Thông báo cập nhập trạng thái thất bại

                TempData["message"] = new XMessage("danger", "Phục hồi mẫu tin thất bại");
                return RedirectToAction("Index");
            }
            //Truy vấn id
            Categories catelogies = categoriesDAO.getRow(id);
            if (catelogies == null)
            {
                //Thông báo cập nhập trạng thái thất bại

                TempData["message"] = new XMessage("danger", "Phục hồi mẫu tin thất bại");
                return RedirectToAction("Index");
            }
            else
            {


                //chuyển đổi trạng thái của Status 0-> 2: không xuất bản
                catelogies.Status = 2;
                //Cập nhập trạng thái
                catelogies.UpdateAt = DateTime.Now;
                //cập nhập lại database
                categoriesDAO.Update(catelogies);

                //Thông báo phục hồi mẫu tin thành công
                TempData["message"] = TempData["message"] = new XMessage("success", "Phục hồi mẫu tin thành công");
                return RedirectToAction("Index");
            }
        }

    }
}
