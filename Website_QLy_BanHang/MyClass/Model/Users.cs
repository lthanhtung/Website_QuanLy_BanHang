using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Users")]
    public class Users
    {
        [Key] //Khoa
        public int Id { get; set; }

        [Required] // Bat buoc
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required] // Bat buoc
        public string FullName { get; set; }
        [Required] // Bat buoc
        public string Email { get; set; }

        [Required] // Bat buoc
        public string Phone { get; set; }

        public string img { get; set; }

        public string Role { get; set; }
        public int Gender { get; set; }
        public string Address { get; set; }

        public int CreateBy { get; set; }

        public DateTime CreateAt { get; set; }

        public int? UpdateBy { get; set; }

        public DateTime? UpdateAt { get; set; }

        public int Status { get; set; }


    }
}
