using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gongshangchaxun.Models
{
     public enum zhuangtaileixing
    {
        yichang, heimingdan, chufa
    }


    public class gsjiben
    {
        [StringLength(50,ErrorMessage = "不能超过25个汉字。")]
        public string zhuceID { get; set; }
       
        [StringLength(50,ErrorMessage = "不能超过25个汉字。")]
        public string mingcheng { get; set; }

        [StringLength(50,ErrorMessage = "不能超过25个汉字。")]
        public string leixing { get; set; }

        [StringLength(20,ErrorMessage = "不能超过10个汉字。")]
        public string fadingdaibiaoren { get; set; }
    
        public decimal zhuceziben { get; set; }
    
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString="{0:yyyy-MM-dd}",ApplyFormatInEditMode=true)]
        public DateTime chengliriqi { get; set; }

        [StringLength(100,ErrorMessage = "不能超过50个汉字。")]
        public string zhusuo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString="{0:yyyy-MM-dd}",ApplyFormatInEditMode=true)]
        public DateTime yingyeqixianqi { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString="{0:yyyy-MM-dd}",ApplyFormatInEditMode=true)]
        public DateTime yingyeqixianzhi { get; set; }

         [StringLength(100,ErrorMessage = "不能超过50个汉字。")]
        public string jingyingfanwei { get; set; }

         [StringLength(50,ErrorMessage = "不能超过25个汉字。")]
        public string dengjijiguan { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString="{0:yyyy-MM-dd}",ApplyFormatInEditMode=true)]
        public DateTime fazhaoriqi { get; set; }


        public zhuangtaileixing? zhuangtai { get; set; }

        
        [StringLength(600,ErrorMessage = "不能超过300个汉字。")]
        public string jizaishiyou { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString="{0:yyyy-MM-dd}",ApplyFormatInEditMode=true)]
        public DateTime jizairiqi { get; set; }

         [StringLength(50,ErrorMessage = "不能超过25个汉字。")]
        public string jizaijigou{ get; set; }

        public virtual ICollection<gstouziren> gstouzirens{ get; set; }
        public virtual ICollection<gsbiangeng> gsbiangengs { get; set; }
        public virtual ICollection<gsbeianrenyuan> gsbeianrenyuans { get; set; }
        public virtual ICollection<gsbeianfenzhijigou> gsbeianfenzhijigous { get; set; }
        public virtual ICollection<gsbeianqingsuanxinxi> gsbeianqingsuanxinxis { get; set; }
        public virtual ICollection<gszigeshenpixinxi> gszigeshenpixinxis { get; set; }
        public virtual ICollection<gsrongyuxinxi> gsrongyuxinxis{ get; set; }
        public virtual ICollection<xingzhengchufaxinxi> xingzhengchufaxinxis{ get; set; }


    }


    }
