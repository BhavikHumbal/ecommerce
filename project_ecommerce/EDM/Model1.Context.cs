//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace project_ecommerce.EDM
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EcommerceEntities1 : DbContext
    {
        public EcommerceEntities1()
            : base("name=EcommerceEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<admin> admins { get; set; }
        public virtual DbSet<card> cards { get; set; }
        public virtual DbSet<city> cities { get; set; }
        public virtual DbSet<customer> customers { get; set; }
        public virtual DbSet<Eorder> Eorders { get; set; }
        public virtual DbSet<order_detail> order_detail { get; set; }
        public virtual DbSet<product> products { get; set; }
        public virtual DbSet<state> states { get; set; }
    }
}
