using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;

namespace Evp.Ds.Data.Core
{
    public class EvpContext : DbContext, IDbContext
    {
        public EvpContext() : base("name=EvpContext")
        {
            Database.Log = s => Debug.WriteLine(s);
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}