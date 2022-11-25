using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace gongshangchaxun.Models
{
    public class setupdb
    {

        [Key]
        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string canshu{ get; set; }

        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string zhi{ get; set; }
    }
}