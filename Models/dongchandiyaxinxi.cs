using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace gongshangchaxun.Models
{
    public class dongchandiyaxinxi
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int dongchandiyaxinxiID { get; set; }


        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string diyaren { get; set; }


        [StringLength(100, ErrorMessage = "不能超过25个汉字。")]
        public string dengjiID { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime dengjiriqi { get; set; }


        [StringLength(20, ErrorMessage = "不能超过10个汉字。")]
        public string dengjijiguan { get; set; }


        
        public decimal beidanbaozhaiquanshue { get; set; }


        



        [StringLength(300, ErrorMessage = "不能超过150个汉字。")]
        public string danbaofanwei { get; set; }


        [StringLength(100, ErrorMessage = "不能超过30个汉字。")]
        public string danbaoqixian { get; set; }


    }
}