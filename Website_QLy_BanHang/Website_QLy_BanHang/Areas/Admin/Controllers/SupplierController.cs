using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Model;
using Website_QLy_BanHang.Library;

namespace Website_QLy_BanHang.Areas.Admin.Controllers
{
    public class SupplierController : Controller
    {
        SuppliersDAO suppliersDAO = new SuppliersDAO();

        //////////////////////////////////////////////////////////////     
        // GET: Admin/Supplier/Index
        public ActionResult Index()
        {
            // hien thi cac mau tin o trang index ( status = 1,2 hay DAO = Index)
            return View(suppliersDAO.getList("Index"));
        }

        //////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tồn tại nhà cung cấp");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tồn tại nhà cung cấp");
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        //////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Create
        public ActionResult Create()
        {
            ViewBag.ListOrder = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {   //xu ly tu dong cho cac truong: CreateAt/By, UpdateAt/By, Slug, OrderBy
                //Xu Ly tu dong CreateAt
                suppliers.CreateAt = DateTime.Now;
                //Xu Ly tu dong Update
                suppliers.UpdateAt = DateTime.Now;             
                //Xu Ly tu dong Order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order += 1;
                }
                //Xu Ly tu dong Slug
                suppliers.Slug = XString.Str_Slug(suppliers.Name);

                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = suppliers.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        suppliers.Image = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/supplier/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh

                //Them mau tin vao db
                suppliersDAO.Insert(suppliers);
                //Thong bao thanh cong
                TempData["message"] = new XMessage("success", "Thêm nhà cung cấp thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListOrder = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliers);
        }

        //////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tồn tại nhà cung cấp");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);// tim kiem 1 mau tin co Id = Id
            if (suppliers == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tồn tại nhà cung cấp");
            }
            ViewBag.ListOrder = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Suppliers suppliers)
        {
            if (ModelState.IsValid)
            {
                //xu ly tu dong cho cac truong: CreateAt/By, UpdateAt/By, Slug, OrderBy
                //Xu Ly tu dong Update
                suppliers.UpdateAt = DateTime.Now;

                //Xu Ly tu dong Order
                if (suppliers.Order == null)
                {
                    suppliers.Order = 1;
                }
                else
                {
                    suppliers.Order += 1;
                }
                //Xu Ly tu dong Slug
                suppliers.Slug = XString.Str_Slug(suppliers.Name);

                var img = Request.Files["img"];//lay thong tin file
                string PathDir = "~/Public/img/supplier/";
                if (img.ContentLength != 0 && suppliers.Image != null) // ton tai mot logo cua Nha cung cap
                {
                    //xoa anh cu
                    string DelPath = Path.Combine(Server.MapPath(PathDir), suppliers.Image);
                    System.IO.File.Delete(DelPath);
                }
                //Upload logo moi cua nha cung ca
                //Truoc khi cap nhap lai anh moi thi xoa anh cu
               

                //xu ly cho phan upload hình ảnh
                
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = suppliers.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        suppliers.Image = imgName;
                        //upload hinh
                       
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh

                //Cap nhap mau tin
                suppliersDAO.Update(suppliers);
                //Thong bao thanh cong
                TempData["message"] = new XMessage("success", "Cập nhập nhà cung cấp thành công");
                return RedirectToAction("Index");
            }
            return View(suppliers);
        }

        //////////////////////////////////////////////////////////////
        // GET: Admin/Supplier/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tồn tại nhà cung cấp");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tồn tại nhà cung cấp");
            }
            return View(suppliers);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Suppliers suppliers =suppliersDAO.getRow(id); // tim kiem 1 mau tin co Id = Id

            var img = Request.Files["img"];//lay thong tin file
            string PathDir = "~/Public/img/supplier/";
            //Xoa au tin ra khoi db
            if (suppliersDAO.Delete(suppliers) == 1)
            {
                //Xu ly cho muc xoa hinh anh
                if (suppliers.Image != null)
                {
                    string DelPath = Path.Combine(Server.MapPath(PathDir), suppliers.Image);
                    System.IO.File.Delete(DelPath);
                }
            }
            //Thong bao xoa thanh cong
            TempData["message"] = new XMessage("success", "Xóa nhà cung cấp thành công");
            return RedirectToAction("Index");
        }

        // GET: Admin/Supplier/Status/5
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhập trạng thái thất bại");
                return RedirectToAction("Index");
            }
            // Truy van id
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhập trạng thái thất bại");
                return RedirectToAction("Index");
            }
            else
            {
                // Chuyen Doi Trang Thai cua Status tu 1 <-> 2
                suppliers.Status = (suppliers.Status == 1) ? 2 : 1;

                //Cap nhap gia tri UpdateAt
                suppliers.UpdateAt = DateTime.Now;

                //Cap nhap lai DB
                suppliersDAO.Update(suppliers);

                //thong bao cap nhap trang thai thanh cong
                TempData["message"] = new XMessage("success", "Cập nhập trạng thái thành công");

                return RedirectToAction("Index");
            }
        }
        // GET: Admin/Supplier/DelTrash/5
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Index");
            }
            // Truy van id
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tìm thấy nhà cung cấp");
                return RedirectToAction("Index");
            }
            else
            {
                // Chuyen Doi Trang Thai cua Status tu 1 2 <-> 0 : Khong hien thi tren index
                suppliers.Status = 0;

                //Cap nhap gia tri UpdateAt
                suppliers.UpdateAt = DateTime.Now;

                //Cap nhap lai DB
                suppliersDAO.Update(suppliers);

                //thong bao thanh cong
                TempData["message"] = new XMessage("success", "Xóa thành công loại sản phẩm");

                return RedirectToAction("Index");
            }
        }
        //Trash
        // GET: Admin/Category/Trash
        public ActionResult Trash()
        {
            return View(suppliersDAO.getList("Trash"));
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
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //Thong bao that bai
                TempData["message"] = new XMessage("danger", "Phục hồi thất bại");
                return RedirectToAction("Index");
            }
            else
            {
                // Chuyen Doi Trang Thai cua Status tu 0 <-> 2: khong xuat ban
                suppliers.Status = 2;

                //Cap nhap gia tri UpdateAt
                suppliers.UpdateAt = DateTime.Now;

                //Cap nhap lai DB
                suppliersDAO.Update(suppliers);

                //thong bao phuc hoi thanh cong
                TempData["message"] = new XMessage("success", "Phục hồi thành công");

                return RedirectToAction("Index");
            }
        }
    }
}
