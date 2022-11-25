using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace gongshangchaxun.Models
{
    public class gstouziren
    {   [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int xuhao { get; set; }

        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string zhuceID { get; set; }

        [StringLength(20, ErrorMessage = "不能超过10个汉字。")]
        public string touzirenmingcheng { get; set; }
       
        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string touzirenleixing { get; set; }

        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string zhengzhaoleixing { get; set; }

        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string zhengzhaohaoma { get; set; }
       
        public decimal renjiaochuzie { get; set; }

        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string chuzifangshi { get; set; }
      
        // public DateTime chuziriqi { get; set; }
        
        public decimal shijiaochuzie { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime chuziriqi { get; set; }

        public virtual gsjiben gsjiben{ get; set; }



    }
}