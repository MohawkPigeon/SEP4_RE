﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace sep4
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class sep4_dbEntities1 : DbContext
    {
        public sep4_dbEntities1()
            : base("name=sep4_dbEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Datapoint> Datapoint { get; set; }
        public virtual DbSet<Establishment> Establishment { get; set; }
        public virtual DbSet<NotificationHistory> NotificationHistory { get; set; }
        public virtual DbSet<Reservation> Reservation { get; set; }
        public virtual DbSet<Sauna> Sauna { get; set; }
        public virtual DbSet<ServoSetting> ServoSetting { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<DateDim> DateDim { get; set; }
        public virtual DbSet<EstablishmentDim> EstablishmentDim { get; set; }
        public virtual DbSet<ReservationDim> ReservationDim { get; set; }
        public virtual DbSet<SaunaDim> SaunaDim { get; set; }
        public virtual DbSet<SaunaFact> SaunaFact { get; set; }
        public virtual DbSet<StageDatapoint> StageDatapoint { get; set; }
        public virtual DbSet<StageDateDim> StageDateDim { get; set; }
        public virtual DbSet<StageEstablishmentDIM> StageEstablishmentDIM { get; set; }
        public virtual DbSet<StageReservationDim> StageReservationDim { get; set; }
        public virtual DbSet<StageSaunaDim> StageSaunaDim { get; set; }
        public virtual DbSet<StageSupervisorDim> StageSupervisorDim { get; set; }
        public virtual DbSet<StageUserDim> StageUserDim { get; set; }
        public virtual DbSet<SupervisorDim> SupervisorDim { get; set; }
        public virtual DbSet<UserDim> UserDim { get; set; }
        public virtual DbSet<database_firewall_rules> database_firewall_rules { get; set; }
    }
}
