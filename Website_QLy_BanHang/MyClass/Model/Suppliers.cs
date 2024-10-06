using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Suppliers")]
    public class Suppliers
    {
        [Key] //Khoa
        public int Id { get; set; }

        [Required(ErrorMessage ="Tên nhà cung cấp không được để trống")] // Bat buoc
        [Display(Name = "Tên NCC")]
        public string Name { get; set; }
        
        [Display(Name = "Logo NCC")]
        public string Image { get; set; }// Neu muon doi ten truong (1) Sua o Model , 2 Sua o SQL, 3 Cau hinh sql cho phep lưu db
        
        [Display(Name = "Tên rút gọn")]
        public string Slug { get; set; }

        [Display(Name = "Sắp xếp")]
        public int? Order { get; set; }

        [Display(Name = "Tên đầy đủ")]
        public string Fullname { get; set; }

        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Liên kết")]
        public string URL { get; set; }

        [Required(ErrorMessage ="Mô tả không được để trống")]
        [Display(Name = "Mô tả")]
        public string MetaDesc { get; set; }

        [Required(ErrorMessage ="Từ khóa không được để trống")]
        [Display(Name = "Từ khóa")]
        public string MetaKey { get; set; }

        [Display(Name = "Người tạo")]
        public int CreateBy { get; set; }

        [Display(Name = "Ngày  tạo")]
        public DateTime CreateAt { get; set; }

        [Display(Name = "Người cập nhập")]
        public int UpdateBy { get; set; }

        [Display(Name = "Ngày cập nhập")]
        public DateTime UpdateAt { get; set; }

        [Display(Name = "Trạng thái")]
        public int? Status { get; set; }//Neu Model # Sql ->Create he thong khong bao loi Nhung SQL ko luu duoc 
    }
}
