using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class SuppliersDAO
    {
        private MyDBContext db = new MyDBContext();

        //Select * From...
        public List<Suppliers> getList()
        {
            return db.Suppliers.ToList();
        }
        // Select * from cho Index chỉ co status 1 và 2
        public List<Suppliers> getList(string status = "ALL") // stutus 0,1,2
        {//Tra ve danh sach cac nha cung cap so status =1 hoac 2 hoac 0 tat ca
            List<Suppliers> list = null;
            switch (status)
            {
                case "Index": // 1 , 2
                    {
                        list = db.Suppliers.Where(m => m.Status != 0).ToList();
                        break;
                    }
                case "Trash": // 0
                    {
                        list = db.Suppliers.Where(m => m.Status == 0).ToList();
                        break;
                    }
                default:
                    {
                        list = db.Suppliers.ToList();
                        break;
                    }
            }
            return list;
        }

        public Suppliers getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Suppliers.Find(id);
            }
        }
        //Tao moi mau tin
        public int Insert(Suppliers row)
        {
            db.Suppliers.Add(row);
            return db.SaveChanges();
        }
        // Cap nhap mau tin
        public int Update(Suppliers row)
        {

            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }
        // Xoa mau tin
        public int Delete(Suppliers row)
        {
            db.Suppliers.Remove(row);
            return db.SaveChanges();
        }
    }
}
