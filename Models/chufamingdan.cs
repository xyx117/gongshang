using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gongshangchaxun.Models
{
    public class chufamingdan
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int chufaID { get; set; }


        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string zhuceID { get; set; }

        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string mingcheng { get; set; }

        [StringLength(20, ErrorMessage = "不能超过10个汉字。")]
        public string fadingdaibiaoren { get; set; }

        [StringLength(600, ErrorMessage = "不能超过300个汉字。")]
        public string jizaishiyou { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime jizairiqi{ get; set; }

        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string jizaibumen { get; set; }

        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string chufatongzhishu{ get; set; }
    }
}