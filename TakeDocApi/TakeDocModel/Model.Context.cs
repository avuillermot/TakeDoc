﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TakeDocModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class TakeDocEntities1 : DbContext
    {
        public TakeDocEntities1()
            : base("name=TakeDocEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Counter> Counter { get; set; }
        public virtual DbSet<Document> Document { get; set; }
        public virtual DbSet<Page> Page { get; set; }
        public virtual DbSet<StatutDocument> Statut_Document { get; set; }
        public virtual DbSet<StatutVersion> Statut_Version { get; set; }
        public virtual DbSet<Version> Version { get; set; }
        public virtual DbSet<View_PageStoreLocator> View_PageStoreLocator { get; set; }
        public virtual DbSet<View_VersionStoreLocator> View_VersionStoreLocator { get; set; }
        public virtual DbSet<TypeDocument> Type_Document { get; set; }
        public virtual DbSet<DataField> DataField { get; set; }
        public virtual DbSet<MetaData> MetaData { get; set; }
    
        public virtual ObjectResult<string> GetNewReference(string table, ObjectParameter reference)
        {
            var tableParameter = table != null ?
                new ObjectParameter("table", table) :
                new ObjectParameter("table", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("GetNewReference", tableParameter, reference);
        }
    }
}
