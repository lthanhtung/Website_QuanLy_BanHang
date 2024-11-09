﻿using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class ProductsDAO
    {
        //Copy noi dung cua class CATEGORIES, thay the Categories bang Suppliers
        private MyDBContext db = new MyDBContext();

        //SELECT * FROM ...
        public List<Products> getList()
        {
            return db.Products.ToList();
        }

        //Hien thi danh sach toan bo san pham theo tung Categories (catid)
        public List<Products> getListByCatId(int catid, int limit)
        {
            List<Products> list = db.Products
                .Where(m => m.CatId == catid && m.Status == 1)
                .Take(limit)
                .OrderByDescending(m => m.CreateBy)
                .ToList();
            return list;
        }

        /////////////////////////////////////////////////////////////////////////////////////
        //Hien thi danh sach toan bo san pham theo tung Categories (catid)
        //danh cho trang HOME
        public List<ProductInfo> getListByListCatId(List<int> listcatid, int limit)
        {
            List<ProductInfo> list = db.Products
                .Join(
                                db.Categories, // Bảng Categories
                                p => p.CatId, // Khóa ngoại của Products liên kết với Categories
                                c => c.Id, // Khóa chính của Categories
                                (p, c) => new { Product = p, Category = c } // Kết hợp Products và Categories
                            )
                            .Join(
                                db.Suppliers, // Bảng Suppliers
                                pc => pc.Product.Supplier, // Khóa ngoại của Product/Category liên kết với Suppliers
                                s => s.Id, // Khóa chính của Suppliers
                                (pc, s) => new ProductInfo
                                {
                                    Id = pc.Product.Id,
                                    CatID = pc.Product.CatId,
                                    Name = pc.Product.Name,
                                    CatName = pc.Category.Name, // Lấy tên danh mục từ bảng Categories
                                    SupplierId = pc.Product.Supplier,
                                    SupplierName = s.Name, // Lấy tên nhà cung cấp từ bảng Suppliers
                                    Slug = pc.Product.Slug,
                                    Image = pc.Product.Img,
                                    Price = pc.Product.Price,
                                    SalePrice = pc.Product.SalePrice,
                                    Amount = pc.Product.Qty,
                                    MetaDesc = pc.Product.MetaDesc,
                                    MetaKey = pc.Product.MetaKey,
                                    CreateBy = pc.Product.CreateBy,
                                    CreateAt = pc.Product.CreateAt,
                                    UpdateBy = pc.Product.UpdateBy,
                                    UpdateAt = pc.Product.UpdateAt,
                                    Status = pc.Product.Status
                                }
                            )
                .Where(m => m.Status == 1 && listcatid.Contains(m.CatID))
                .Take(limit)
                .OrderByDescending(m => m.CreateBy)
                .ToList();
            return list;
        }


        //Hien thi danh sach toan bo san pham dua vao limit so mau tin
        //danh cho trang HOME
        public List<ProductInfo> getListBylimit(int limit)
        {
            List<ProductInfo> list = db.Products
                .Join(
                                db.Categories, //Bang Categories
                                p => p.CatId, //Khoa ngoai cua Products lien ket voi Categories
                                c => c.Id, //Khoa chinh cua Categories
                                (p, c) => new { Product = p, Category = c } //Join Products va Categories
                            )
                            .Join(
                                db.Suppliers, //Bang Suppliers
                                pc => pc.Product.Supplier, //Khoa ngoai cua Product lien ket voi Suppliers
                                s => s.Id, //Khoa chinh cua Suppliers
                                (pc, s) => new ProductInfo
                                {
                                    Id = pc.Product.Id,
                                    CatID = pc.Product.CatId,
                                    Name = pc.Product.Name,
                                    CatName = pc.Category.Name, //Lay Name tu Categories
                                    SupplierId = pc.Product.Supplier,
                                    SupplierName = s.Name, //Lay ten NCC tu bang Suppliers
                                    Slug = pc.Product.Slug,
                                    Image = pc.Product.Img,
                                    Price = pc.Product.Price,
                                    SalePrice = pc.Product.SalePrice,
                                    Amount = pc.Product.Qty,
                                    MetaDesc = pc.Product.MetaDesc,
                                    MetaKey = pc.Product.MetaKey,
                                    CreateBy = pc.Product.CreateBy,
                                    CreateAt = pc.Product.CreateAt,
                                    UpdateBy = pc.Product.UpdateBy,
                                    UpdateAt = pc.Product.UpdateAt,
                                    Status = pc.Product.Status
                                }
                            )
                .Where(m => m.Status == 1)
                .Take(limit)
                .OrderByDescending(m => m.CreateBy)
                .ToList();
            return list;
        }



        //Index chi voi staus 1,2        
        public List<Products> getList(string status = "ALL")//status 0,1,2
        {
            List<Products> list = null;
            switch (status)
            {
                case "Index"://1,2
                    {
                        list = db.Products.Where(m => m.Status != 0).ToList();
                        break;
                    }
                case "Trash"://0
                    {
                        list = db.Products.Where(m => m.Status == 0).ToList();
                        break;
                    }
                default:
                    {
                        list = db.Products.ToList();
                        break;
                    }
            }
            return list;
        }
        //details
        public Products getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Products.Find(id);
            }
        }

        //Hien thi danh sach 1 mau tin (ban ghi)
        public Products getRow(string slug)
        {

            return db.Products
                .Where(m => m.Slug == slug && m.Status == 1)
                .FirstOrDefault();

        }

        //tao moi mau tin
        public int Insert(Products row)
        {
            db.Products.Add(row);
            return db.SaveChanges();
        }

        //cap nhat mau tin
        public int Update(Products row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //Xoa mau tin
        public int Delete(Products row)
        {
            db.Products.Remove(row);
            return db.SaveChanges();//thanh cong => return 1
        }

        ///Chi tiet san pham
        ///Trang giao dien chi tiet san pham
        public List<ProductInfo> GetProductDetailBySlug(string slug)
        {
            List<ProductInfo> list = null;
            list = db.Products
                .Where(p => p.Slug == slug && p.Status == 1)
                .Join(
                    db.Categories,
                    p => p.CatId,
                    c => c.Id,
                    (p, c) => new { Product = p, Category = c }
                )
                .Join(
                    db.Suppliers,
                    pc => pc.Product.Supplier,
                    s => s.Id,
                    (pc, s) => new ProductInfo
                    {
                        Id = pc.Product.Id,
                        CatID = pc.Product.CatId,
                        Name = pc.Product.Name,
                        CatName = pc.Category.Name,
                        SupplierId = pc.Product.Supplier,
                        SupplierName = s.Name,
                        Slug = pc.Product.Slug,
                        Image = pc.Product.Img,
                        Price = pc.Product.Price,
                        SalePrice = pc.Product.SalePrice,
                        Amount = pc.Product.Qty,
                        MetaDesc = pc.Product.MetaDesc,
                        MetaKey = pc.Product.MetaKey,
                        CreateBy = pc.Product.CreateBy,
                        CreateAt = pc.Product.CreateAt,
                        UpdateBy = pc.Product.UpdateBy,
                        UpdateAt = pc.Product.UpdateAt,
                        Status = pc.Product.Status,
                        //bo sung truong Slug cua Categories
                        CategorySlug = pc.Category.Slug
                    }
                )
                .ToList();
            return list;
        }

        /////////////////////////////////////////////////////////////////////////////////////
        ///Hien thi danh sach cac san pham cung loai
        ///Trang giao dien chi tiet san pham
        public List<ProductInfo> GetProductDetailByCategoryId(int catid)
        {
            var relatedProducts = db.Products
                .Where(p => p.CatId == catid && p.Status == 1)
                .Join(
                    db.Categories,
                    p => p.CatId,
                    c => c.Id,
                    (p, c) => new { Product = p, Category = c }
                )
                .Join(
                    db.Suppliers,
                    pc => pc.Product.Supplier,
                    s => s.Id,
                    (pc, s) => new ProductInfo
                    {
                        Id = pc.Product.Id,
                        CatID = pc.Product.CatId,
                        Name = pc.Product.Name,
                        CatName = pc.Category.Name,
                        SupplierId = pc.Product.Supplier,
                        SupplierName = s.Name,
                        Slug = pc.Product.Slug,
                        Image = pc.Product.Img,
                        Price = pc.Product.Price,
                        SalePrice = pc.Product.SalePrice,
                        Amount = pc.Product.Qty,
                        MetaDesc = pc.Product.MetaDesc,
                        MetaKey = pc.Product.MetaKey,
                        CreateBy = pc.Product.CreateBy,
                        CreateAt = pc.Product.CreateAt,
                        UpdateBy = pc.Product.UpdateBy,
                        UpdateAt = pc.Product.UpdateAt,
                        Status = pc.Product.Status
                    }
                )
                .ToList();

            return relatedProducts;
        }

    }
}
