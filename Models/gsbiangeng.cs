using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gongshangchaxun.Models
{
    public class gsbiangeng
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int xuhao { get; set; }

        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string zhuceID { get; set; }

        [StringLength(100, ErrorMessage = "不能超过50个汉字。")]
        public string biangengshixiang { get; set; }

        [StringLength(200, ErrorMessage = "不能超过50个汉字。")]
        public string biangengqian { get; set; }

        [StringLength(200, ErrorMessage = "不能超过50个汉字。")]
        public string biangenghou { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime biangengriqi{ get; set; }

        public virtual gsjiben gsjiben { get; set; }
    }
}