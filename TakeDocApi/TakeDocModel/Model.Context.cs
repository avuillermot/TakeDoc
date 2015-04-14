﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
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
        public virtual DbSet<Version> Version { get; set; }
        public virtual DbSet<View_PageStoreLocator> View_PageStoreLocator { get; set; }
        public virtual DbSet<View_VersionStoreLocator> View_VersionStoreLocator { get; set; }
        public virtual DbSet<TypeDocument> Type_Document { get; set; }
        public virtual DbSet<DataField> DataField { get; set; }
        public virtual DbSet<MetaData> MetaData { get; set; }
        public virtual DbSet<UserTk> UserTk { get; set; }
        public virtual DbSet<View_UserEntity> View_UserEntity { get; set; }
        public virtual DbSet<Entity> Entity { get; set; }
        public virtual DbSet<DataFieldType> DataFieldType { get; set; }
        public virtual DbSet<View_TypeDocumentDataField> View_TypeDocumentDataField { get; set; }
        public virtual DbSet<DataFieldValue> DataFieldValue { get; set; }
        public virtual DbSet<Status_Document> Status_Document { get; set; }
        public virtual DbSet<Status_Version> Status_Version { get; set; }
        public virtual DbSet<DataFieldAutoComplete> DataFieldAutoComplete { get; set; }
        public virtual DbSet<View_DocumentExtended> View_DocumentExtended { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Parameter> Parameter { get; set; }
        public virtual DbSet<GroupTk> GroupTk { get; set; }
    
        public virtual ObjectResult<string> GetNewReference(string table, ObjectParameter reference)
        {
            var tableParameter = table != null ?
                new ObjectParameter("table", table) :
                new ObjectParameter("table", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("GetNewReference", tableParameter, reference);
        }
    
        public virtual int AddUserToEntity(string userRef, string entityRef)
        {
            var userRefParameter = userRef != null ?
                new ObjectParameter("userRef", userRef) :
                new ObjectParameter("userRef", typeof(string));
    
            var entityRefParameter = entityRef != null ?
                new ObjectParameter("entityRef", entityRef) :
                new ObjectParameter("entityRef", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddUserToEntity", userRefParameter, entityRefParameter);
        }
    
        public virtual int DeleteEntityUser(string userRef)
        {
            var userRefParameter = userRef != null ?
                new ObjectParameter("userRef", userRef) :
                new ObjectParameter("userRef", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeleteEntityUser", userRefParameter);
        }
    
        public virtual ObjectResult<SearchUserTk> SearchUserTk(string firstName, string lastName, string email, Nullable<System.Guid> entityId)
        {
            var firstNameParameter = firstName != null ?
                new ObjectParameter("firstName", firstName) :
                new ObjectParameter("firstName", typeof(string));
    
            var lastNameParameter = lastName != null ?
                new ObjectParameter("lastName", lastName) :
                new ObjectParameter("lastName", typeof(string));
    
            var emailParameter = email != null ?
                new ObjectParameter("email", email) :
                new ObjectParameter("email", typeof(string));
    
            var entityIdParameter = entityId.HasValue ?
                new ObjectParameter("entityId", entityId) :
                new ObjectParameter("entityId", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SearchUserTk>("SearchUserTk", firstNameParameter, lastNameParameter, emailParameter, entityIdParameter);
        }
    }
}
