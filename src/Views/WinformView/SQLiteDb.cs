using Microsoft.EntityFrameworkCore;
using System.IO;

namespace SimpleForm
{

    public class SQLiteDb : DbContext
    {
        public SQLiteDb()
        {
            Database.EnsureCreated();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>().HasKey(p => new
            {
                p.KeeperKey,
                p.KeeperInitKey
            });
            modelBuilder.Entity<User>().Property(p => p.KeeperInitKey).HasMaxLength(200000);
            modelBuilder.Entity<User>().Ignore(p => p.Status).Ignore(p => p.LastRefreshTime).Ignore(p => p.IsLogin)
                .Ignore(p => p.KeeperModel).Ignore(p => p.Cookies);

            //Database.SetInitializer(new Createtable(modelBuilder));
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder)
        {
            optionbuilder.UseSqlite($"Data Source={Directory.GetCurrentDirectory()}\\user.db");
        }

        //private Type StupidMethodForCopySqliteEF6()
        //{
        //    return SQLiteProviderFactory.Instance.GetType();
        //}
    }

    //public class Createtable : SqliteCreateDatabaseIfNotExists<SQLiteDb>
    //{
    //    public Createtable(DbModelBuilder modelBuilder) : base(modelBuilder)
    //    { }
    //    protected override void Seed(SQLiteDb context)
    //    {
    //    }
    //}

    //public class SQLiteConfiguration : DbConfiguration
    //{
    //    public SQLiteConfiguration()
    //    {
    //        SetDefaultConnectionFactory(new SQLiteConnectionFactory());
    //        SetProviderFactory("System.Data.SQLite.EF6", new SQLiteFactory());
    //        SetProviderFactory("System.Data.SQLite.EF6", new SQLiteProviderFactory());

    //        var EF6ProviderServicesType = typeof(SQLiteProviderFactory).Assembly.DefinedTypes.First(x => x.Name == "SQLiteProviderServices");
    //        var EF6ProviderServices = (DbProviderServices)Activator.CreateInstance(EF6ProviderServicesType);
    //        SetProviderServices("System.Data.SQLite.EF6", EF6ProviderServices);
    //    }
    //}
    //public class SQLiteConnectionFactory : IDbConnectionFactory
    //{
    //    public DbConnection CreateConnection(string connectionString)
    //    {
    //        return new SQLiteConnection(connectionString);
    //    }
    //}
}
