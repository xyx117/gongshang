using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using gongshangchaxun.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace gongshangchaxun.DAL
{
    public class GongshangContent :DbContext 
    {

        public DbSet<yichangmingdan> yichangmingdans{ get; set; }

        public DbSet<heimingdan> heimingdans { get; set; }

        public DbSet<chufamingdan> chufamingdans { get; set; }

        public DbSet<shichangzhutixinxi> shichangzhutixinxis { get; set; }

        public DbSet<guquanxinxi > guquanxinxis { get; set; }

        public DbSet<dongchandiyaxinxi> dongchandiyaxinxis { get; set; }

        public DbSet<setupdb> setupdbs { get; set; }

        public DbSet<UserProfile> userprofiles { get; set; }


        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

     


    }

   
}