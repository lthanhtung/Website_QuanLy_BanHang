﻿using System;
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
        //Index
        // GET: Admin/Category
        public ActionResult Index()
        {
            return View(categoriesDAO.getList("Index"));
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        //Details
        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tồn tại loại sản phẩm");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Không tồn tại loại sản phẩm");
                return RedirectToAction("Index");
            }
            return View(categories);
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////
        //Create
        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"),"Id","Name");
            ViewBag.ListOrder = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //Xu Ly tu dong CreateAt
                categories.CreateAt = DateTime.Now;
                //Xu Ly tu dong Update
                categories.UpdateAt = DateTime.Now;
                //Xu Ly tu dong ParentID
                if (categories.ParentId == null)
                {
                    categories.ParentId = 0;
                }
                //Xu Ly tu dong Order
                if (categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order += 1;
                }
                //Xu Ly tu dong Slug
                categories.Slug = XString.Str_Slug(categories.Name);
                //thong bao them thanh cong
                TempData["message"] = new XMessage("success", "Tạo mới loại sản phẩm thành công");
                //Chèn thêm dòng cho DB
                categoriesDAO.Insert(categories);
               return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.ListOrder = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////
        //Edit
        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tìm thấy loại sản phẩm");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tìm thấy loại sản phẩm");
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.ListOrder = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //Xu ly tu dong Slug
                categories.Slug = XString.Str_Slug(categories.Name);
                //xu ly tu dong: ParentId
                if (categories.ParentId == null)
                {
                    categories.ParentId = 0;
                }
                //Xu ly tu dong Order
                if (categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order += 1;
                }
                //Xu ly tu dong UpdateAt
                categories.UpdateAt = DateTime.Now;

                //Cap nhap mau tin
                categoriesDAO.Update(categories);
                
                //thong bao thanh cong
                TempData["message"] = new XMessage("success", "Cập nhập loại sản phẩm thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.ListOrder = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        //Delete
        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Xóa loại sản phẩm thất bại");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Xóa loại sản phẩm thất bại");
                return RedirectToAction("Index");
            }
            return View(categories);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categories categories =categoriesDAO.getRow(id);
            categoriesDAO.Delete(categories);
            //Thong bao thành công
            TempData["message"] = new XMessage("success", "Xóa loại sản phẩm thành công");
            return RedirectToAction("Trash");
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Status/5
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger","Cập nhập trạng thái thất bại");
                return RedirectToAction("Index");
            }
            // Truy van id
            Categories categories = categoriesDAO.getRow(id);
            if(categories == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhập trạng thái thất bại");
                return RedirectToAction("Index");
            }
            else 
            {         
                // Chuyen Doi Trang Thai cua Status tu 1 <-> 2
                categories.Status = (categories.Status == 1) ? 2 : 1;

                //Cap nhap gia tri UpdateAt
                categories.UpdateAt = DateTime.Now;

                //Cap nhap lai DB
                categoriesDAO.Update(categories);

                //thong bao cap nhap trang thai thanh cong
                TempData["message"] = new XMessage("success", "Cập nhập trạng thái thành công");

                return RedirectToAction("Index");
            }
        }
        // GET: Admin/Category/DelTrash/5
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tìm thấy loại sản phẩm");
                return RedirectToAction("Index");
            }
            // Truy van id
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tìm thấy loại sản phẩm");
                return RedirectToAction("Index");
            }
            else
            {
                // Chuyen Doi Trang Thai cua Status tu 1 2 <-> 0 : Khong hien thi tren index
                categories.Status = 0;

                //Cap nhap gia tri UpdateAt
                categories.UpdateAt = DateTime.Now;

                //Cap nhap lai DB
                categoriesDAO.Update(categories);

                //thong bao thanh cong
                TempData["message"] = new XMessage("success", "Xóa thành công loại sản phẩm");

                return RedirectToAction("Index");
            }
        }

        //Trash
        // GET: Admin/Category/Trash
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
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Phục hồi thất bại");
                return RedirectToAction("Index");
            }
            // Truy van id
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Phục hồi thất bại");
                return RedirectToAction("Index");
            }
            else
            {
                // Chuyen Doi Trang Thai cua Status tu 0 <-> 2: khong xuat ban
                categories.Status = 2;

                //Cap nhap gia tri UpdateAt
                categories.UpdateAt = DateTime.Now;

                //Cap nhap lai DB
                categoriesDAO.Update(categories);

                //thong bao phuc hoi thanh cong
                TempData["message"] = new XMessage("success", "Phục hồi thành công");

                return RedirectToAction("Trash");
            }
        }
    }
}
