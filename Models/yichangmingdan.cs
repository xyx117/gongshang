using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gongshangchaxun.Models
{
    public class yichangmingdan{ 
    //  [Key]
    //    [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
    //    [Display(Name="注册号")]
    //    public string zhuceID { get; set; }

    //    [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
    //    public string mingcheng { get; set; }

    //    [StringLength(20, ErrorMessage = "不能超过10个汉字。")]
    //    public string fadingdaibiaoren { get; set; }

    //    [StringLength(600, ErrorMessage = "不能超过300个汉字。")]
    //    public string jizaishiyou { get; set; }
        
    //    [DataType(DataType.Date)]
    //    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    //    public DateTime jizairiqi { get; set; }

    //    [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
    //    public string jizaibumen { get; set; }

        [Key ,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int yichangID { get; set; }
       

       
        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        [Display(Name = "注册号")]
        public string zhuceID { get; set; }

        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string mingcheng { get; set; }

        [StringLength(20, ErrorMessage = "不能超过10个汉字。")]
        public string fadingdaibiaoren { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime lieruriqi { get; set; }
       // public string  lieruriqi { get; set; }

        [StringLength(600, ErrorMessage = "不能超过300个汉字。")]
        public string lierushiyou { get; set; }

        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string zuochujuedingjiguan1 { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime yichuriqi { get; set; }
        //public string yichuriqi { get; set; }

        [StringLength(600, ErrorMessage = "不能超过300个汉字。")]
        public string yichushiyou { get; set; }

        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string zuochujuedingjiguan { get; set; }
    }
}