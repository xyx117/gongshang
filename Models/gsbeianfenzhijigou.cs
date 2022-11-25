using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gongshangchaxun.Models
{
    public class gsbeianfenzhijigou
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int xuhao { get; set; }

        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string zhuceID { get; set; }

        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string fenzhizhuceID { get; set; }


        [StringLength(20, ErrorMessage = "不能超过10个汉字。")]
        public string fenzhimingcheng { get; set; }


        [StringLength(50, ErrorMessage = "不能超过25个汉字。")]
        public string fenzhidengjijigou { get; set; }


        public virtual gsjiben gsjiben { get; set; }


    }

}