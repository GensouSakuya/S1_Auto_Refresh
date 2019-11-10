namespace Core
{
    using SQLite.CodeFirst;
    using System;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Core.Common;
    using System.Data.Entity.Infrastructure;
    using System.Data.SQLite;
    using System.Data.SQLite.EF6;
    using System.IO;
    using System.Linq;

    [DbConfigurationType(typeof(SQLiteConfiguration))]
    public class SQLiteDb : DbContext
    {
        public SQLiteDb():base($"Data Source={Directory.GetCurrentDirectory()}\\user.db;Version=3;")
        {
        }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>().ToTable("UserInfo");
            modelBuilder.Entity<UserInfo>().HasKey(p => new
            {
                p.UserName, p.FromForum
            });
            modelBuilder.Entity<UserInfo>().Ignore(p => p.Status).Ignore(p => p.LastRefreshTime);
            Database.SetInitializer<SQLiteDb>(new Createtable(modelBuilder));
        }

        private Type StupidMethodForCopySqliteEF6()
        {
            return SQLiteProviderFactory.Instance.GetType();
        }
    }

    public class Createtable : SqliteCreateDatabaseIfNotExists<SQLiteDb>
    {
        public Createtable(DbModelBuilder modelBuilder):base(modelBuilder)
        { }
        protected override void Seed(SQLiteDb context)
        {
        }
    }

    public class SQLiteConfiguration : DbConfiguration
    {
        public SQLiteConfiguration()
        {
            SetDefaultConnectionFactory(new SQLiteConnectionFactory());
            SetProviderFactory("System.Data.SQLite.EF6", new SQLiteFactory());
            SetProviderFactory("System.Data.SQLite.EF6", new SQLiteProviderFactory());

            var EF6ProviderServicesType = typeof(System.Data.SQLite.EF6.SQLiteProviderFactory).Assembly.DefinedTypes.First(x => x.Name == "SQLiteProviderServices");
            var EF6ProviderServices = (DbProviderServices)Activator.CreateInstance(EF6ProviderServicesType);
            SetProviderServices("System.Data.SQLite.EF6", EF6ProviderServices);
        }
    }
    public class SQLiteConnectionFactory : IDbConnectionFactory
    {
        public DbConnection CreateConnection(string connectionString)
        {
            return new SQLiteConnection(connectionString);
        }
    }
}