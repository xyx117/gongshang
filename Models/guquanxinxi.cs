using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace gongshangchaxun.Models
{
    public class guquanxinxi
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int guquanxinxiID { get; set; }


        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string mingcheng { get; set; }


        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string chuzhiren { get; set; }



        [StringLength(50, ErrorMessage = "不能超过10个汉字。")]
        public string zhiquanren { get; set; }


        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public decimal jine { get; set; }

        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public decimal beidanbaojine { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime dengjiriqi { get; set; }

        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string zhuangtai { get; set; }

        
    }
}