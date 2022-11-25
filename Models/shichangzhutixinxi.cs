using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace gongshangchaxun.Models
{
    public class shichangzhutixinxi
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int shichangzhutiID { get; set; }


        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string mingcheng { get; set; }


        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        [Display(Name = "注册号")]
        public string zhuceID { get; set; }

        

        [StringLength(20, ErrorMessage = "不能超过10个汉字。")]
        public string fadingdaibiaoren { get; set; }


        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string beianxinxi { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime chengliriqi { get; set; }

        

        [StringLength(60, ErrorMessage = "不能超过10个汉字。")]
        public string dengjibumen { get; set; }


    }
}