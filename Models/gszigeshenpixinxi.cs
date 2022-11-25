using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace gongshangchaxun.Models
{
    public class gszigeshenpixinxi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int xuhao { get; set; }

        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string zhuceID { get; set; }

        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string bianhao { get; set; }

        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string jiguanmingcheng{ get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime youxiaoriqi { get; set; }


        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string zhuangtai { get; set; }

        [StringLength(600, ErrorMessage = "不能超过300个汉字。")]
        public string biangengxinxi { get; set; }

        public virtual gsjiben gsjiben { get; set; }
    }
}