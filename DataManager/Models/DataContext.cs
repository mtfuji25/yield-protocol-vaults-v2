using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DataManager.Models
{
    public class DataContext : IdentityDbContext<AspNetUser>
    {
        #region Methods
        public DataContext() :
            base("DataContext", throwIfV1Schema: false)
        {
        }

        public static DataContext Create()
        {
            return new DataContext();
        }

        private object GetPrimaryKeyValue(DbEntityEntry entry)
        {
            ObjectStateEntry objectStateEntry = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            return objectStateEntry.EntityKey.EntityKeyValues[0].Value;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<TouchScreen>()
                .Property(e => e.TouchScreenName)
                .IsUnicode(false);

            base.OnModelCreating(modelBuilder);
        }
        #endregion

        #region Properties
        public virtual DbSet<TaskLog> TaskLogs { get; set; }
        public virtual DbSet<TouchScreen> TouchScreens { get; set; }
        public virtual DbSet<ViewSetting> ViewSettings { get; set; }
        #endregion
    }
}
