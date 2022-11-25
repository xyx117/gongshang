using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace gongshangchaxun.Models
{
    public class xingzhengchufaxinxi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int xuhao { get; set; }

        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string zhuceID { get; set; }


        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string wenhao { get; set; }

        [StringLength(600, ErrorMessage = "不能超过300个汉字。")]
        public string weifaxingwei { get; set; }

        [StringLength(600, ErrorMessage = "不能超过300个汉字。")]
        public string chufayiju { get; set; }

        [StringLength(600, ErrorMessage = "不能超过300个汉字。")]
        public string chufajieguo { get; set; }

        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string chufajiguan { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime chufariqi { get; set; }

        public virtual gsjiben gsjiben { get; set; }



    }
}