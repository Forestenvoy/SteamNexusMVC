using Microsoft.EntityFrameworkCore;
using SteamNexus.Models;

namespace SteamNexus.Data
{
    public class SteamNexusDbContext : DbContext
    {

        // 建構函式
        public SteamNexusDbContext(DbContextOptions<SteamNexusDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<Advertisement> Advertisements { get; set; }

        public virtual DbSet<CommonQuestion> CommonQuestions { get; set; }

        // 資料表 CPUs 是 CPU 中的每一行的資料實體的型別
        public virtual DbSet<CPU> CPUs { get; set; }
        // 資料表 GPUs 是 GPU 中的每一行的資料實體的型別
        public virtual DbSet<GPU> GPUs { get; set; }

        public virtual DbSet<Language> Languages { get; set; }

        public virtual DbSet<MinimumRequirement> MinimumRequirements { get; set; }

        public virtual DbSet<RecommendedRequirement> RecommendedRequirements { get; set; }

        public virtual DbSet<Game> Games { get; set; }

        public virtual DbSet<GameLanguage> GameLanguages { get; set; }

        public virtual DbSet<Tag> Tags { get; set; }

        public virtual DbSet<TagGroup> TagGroups { get; set; }

        public virtual DbSet<PriceHistory> PriceHistories { get; set; }

        public virtual DbSet<PlayersHistory> PlayersHistories { get; set; }

        public virtual DbSet<ComputerPartCategory> ComputerPartCategories { get; set; }

        public virtual DbSet<ComponentClassification> ComponentClassifications { get; set; }

        public virtual DbSet<ProductInformation> ProductInformations { get; set; }

        public virtual DbSet<ProductRAM> ProductRAMs { get; set; }

        public virtual DbSet<ProductGPU> ProductGPUs { get; set; }

        public virtual DbSet<ProductCPU> ProductCPUs { get; set; }

        // Model 屬性設定
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                // 主鍵設定
                // ValueGeneratedOnAdd 插入新的實體時，值自動生成
                // UseIdentityColumn(10000, 1) 識別碼起始值和增量
                entity.Property(e => e.UserId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(10000, 1);

                entity.HasOne(d => d.Role).WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Roles");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                // 主鍵設定
                // ValueGeneratedOnAdd 插入新的實體時，值自動生成
                // UseIdentityColumn(10000, 1) 識別碼起始值和增量
                entity.Property(e => e.RoleId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(10000, 1);
            });

            modelBuilder.Entity<Advertisement>(entity =>
            {
                // 主鍵設定
                // ValueGeneratedOnAdd 插入新的實體時，值自動生成
                // UseIdentityColumn(10000, 1) 識別碼起始值和增量
                entity.Property(e => e.AdvertisementId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(10000, 1);
            });

            modelBuilder.Entity<CommonQuestion>(entity =>
            {
                // 主鍵設定
                // ValueGeneratedOnAdd 插入新的實體時，值自動生成
                // UseIdentityColumn(10000, 1) 識別碼起始值和增量
                entity.Property(e => e.CommonQuestionId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(10000, 1);
            });

            modelBuilder.Entity<CPU>(entity =>
            {
                // 主鍵設定
                // ValueGeneratedOnAdd 插入新的實體時，值自動生成
                // UseIdentityColumn(10000, 1) 識別碼起始值和增量
                entity.Property(e => e.CPUId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(10000, 1);
            });

            modelBuilder.Entity<GPU>(entity =>
            {
                // 主鍵設定
                // ValueGeneratedOnAdd 插入新的實體時，值自動生成
                // UseIdentityColumn(10000, 1) 識別碼起始值和增量
                entity.Property(e => e.GPUId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(10000, 1);
            });

            modelBuilder.Entity<Language>(entity =>
            {
                // 主鍵設定
                // ValueGeneratedOnAdd 插入新的實體時，值自動生成
                // UseIdentityColumn(10000, 1) 識別碼起始值和增量
                entity.Property(e => e.LanguageId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(10000, 1);
            });

            modelBuilder.Entity<MinimumRequirement>(entity =>
            {
                // 主鍵設定
                // ValueGeneratedOnAdd 插入新的實體時，值自動生成
                // UseIdentityColumn(10000, 1) 識別碼起始值和增量
                entity.Property(e => e.MinReqId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(10000, 1);

                entity.HasOne(d => d.Game).WithMany(p => p.MinimumRequirements)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MinimumRequirements_Games");

                entity.HasOne(d => d.CPU).WithMany(p => p.MinimumRequirements)
                    .HasForeignKey(d => d.CPUId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MinimumRequirements_CPUs");

                entity.HasOne(d => d.GPU).WithMany(p => p.MinimumRequirements)
                    .HasForeignKey(d => d.GPUId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MinimumRequirements_GPUs");
            });

            modelBuilder.Entity<RecommendedRequirement>(entity =>
            {
                // 主鍵設定
                // ValueGeneratedOnAdd 插入新的實體時，值自動生成
                // UseIdentityColumn(10000, 1) 識別碼起始值和增量
                entity.Property(e => e.RecReqId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(10000, 1);

                entity.HasOne(d => d.Game).WithMany(p => p.RecommendedRequirements)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecommendedRequirements_Games");

                entity.HasOne(d => d.CPU).WithMany(p => p.RecommendedRequirements)
                    .HasForeignKey(d => d.CPUId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecommendedRequirements_CPUs");

                entity.HasOne(d => d.GPU).WithMany(p => p.RecommendedRequirements)
                    .HasForeignKey(d => d.GPUId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecommendedRequirements_GPUs");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                // 主鍵設定
                // ValueGeneratedOnAdd 插入新的實體時，值自動生成
                // UseIdentityColumn(10000, 1) 識別碼起始值和增量
                entity.Property(e => e.GameId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(10000, 1);
            });

            modelBuilder.Entity<GameLanguage>(entity =>
            {
                // 主鍵設定
                // ValueGeneratedOnAdd 插入新的實體時，值自動生成
                // UseIdentityColumn(10000, 1) 識別碼起始值和增量
                entity.Property(e => e.GameLanguageId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(10000, 1);

                entity.HasOne(d => d.Game).WithMany(p => p.GameLanguages)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GameLanguages_Games");

                entity.HasOne(d => d.Language).WithMany(p => p.GameLanguages)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GameLanguages_Languages");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                // 主鍵設定
                // ValueGeneratedOnAdd 插入新的實體時，值自動生成
                // UseIdentityColumn(10000, 1) 識別碼起始值和增量
                entity.Property(e => e.TagId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(10000, 1);
            });

            modelBuilder.Entity<PriceHistory>(entity =>
            {
                // 主鍵設定
                // ValueGeneratedOnAdd 插入新的實體時，值自動生成
                // UseIdentityColumn(10000, 1) 識別碼起始值和增量
                entity.Property(e => e.PriceHistoryId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(10000, 1);

                entity.HasOne(d => d.Game).WithMany(p => p.PriceHistories)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PriceHistories_Games");
            });

            modelBuilder.Entity<PlayersHistory>(entity =>
            {
                // 主鍵設定
                // ValueGeneratedOnAdd 插入新的實體時，值自動生成
                // UseIdentityColumn(10000, 1) 識別碼起始值和增量
                entity.Property(e => e.PlayersHistoryId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(10000, 1);

                entity.HasOne(d => d.Game).WithMany(p => p.PlayersHistories)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PlayersHistories_Games");
            });

            modelBuilder.Entity<TagGroup>(entity =>
            {
                // 主鍵設定
                // ValueGeneratedOnAdd 插入新的實體時，值自動生成
                // UseIdentityColumn(10000, 1) 識別碼起始值和增量
                entity.Property(e => e.TagGroupId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(10000, 1);

                entity.HasOne(d => d.Game).WithMany(p => p.TagGroups)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TagGroups_Games");

                entity.HasOne(d => d.Tag).WithMany(p => p.TagGroups)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TagGroups_Tags");
            });

            modelBuilder.Entity<ComputerPartCategory>(entity =>
            {
                // 主鍵設定
                // ValueGeneratedOnAdd 插入新的實體時，值自動生成
                // UseIdentityColumn(10000, 1) 識別碼起始值和增量
                entity.Property(e => e.ComputerPartCategoryId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(10000, 1);
            });

            modelBuilder.Entity<ComponentClassification>(entity =>
            {
                // 主鍵設定
                // ValueGeneratedOnAdd 插入新的實體時，值自動生成
                // UseIdentityColumn(10000, 1) 識別碼起始值和增量
                entity.Property(e => e.ComponentClassificationId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(10000, 1);

                entity.HasOne(d => d.ComputerPartCategory).WithMany(p => p.ComponentClassifications)
                    .HasForeignKey(d => d.ComputerPartCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ComponentClassifications_ComputerPartCategories");
            });

            modelBuilder.Entity<ProductInformation>(entity =>
            {
                // 主鍵設定
                // ValueGeneratedOnAdd 插入新的實體時，值自動生成
                // UseIdentityColumn(10000, 1) 識別碼起始值和增量
                entity.Property(e => e.ProductInformationId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(10000, 1);

                entity.HasOne(d => d.ComponentClassification).WithMany(p => p.ProductInformations)
                    .HasForeignKey(d => d.ComponentClassificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductInformations_ComponentClassifications");
            });

            modelBuilder.Entity<ProductRAM>(entity =>
            {
                // 主鍵設定
                // ValueGeneratedOnAdd 插入新的實體時，值自動生成
                // UseIdentityColumn(10000, 1) 識別碼起始值和增量
                entity.Property(e => e.ProductRAMId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(10000, 1);

                entity.HasOne(d => d.ProductInformation).WithMany(p => p.ProductRAMs)
                    .HasForeignKey(d => d.ProductInformationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductRAMs_ProductInformations");
            });

            modelBuilder.Entity<ProductGPU>(entity =>
            {
                // 主鍵設定
                // ValueGeneratedOnAdd 插入新的實體時，值自動生成
                // UseIdentityColumn(10000, 1) 識別碼起始值和增量
                entity.Property(e => e.ProductGPUId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(10000, 1);

                entity.HasOne(d => d.GPU).WithMany(p => p.ProductGPUs)
                    .HasForeignKey(d => d.GPUId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductGPUs_GPUs");

                entity.HasOne(d => d.ProductInformation).WithMany(p => p.ProductGPUs)
                    .HasForeignKey(d => d.ProductInformationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductGPUs_ProductInformations");
            });

            modelBuilder.Entity<ProductCPU>(entity =>
            {
                // 主鍵設定
                // ValueGeneratedOnAdd 插入新的實體時，值自動生成
                // UseIdentityColumn(10000, 1) 識別碼起始值和增量
                entity.Property(e => e.ProductCPUId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(10000, 1);


                entity.HasOne(d => d.CPU).WithMany(p => p.ProductCPUs)
                    .HasForeignKey(d => d.CPUId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductCPUs_CPUs");

                entity.HasOne(d => d.ProductInformation).WithMany(p => p.ProductCPUs)
                    .HasForeignKey(d => d.ProductInformationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductCPUs_ProductInformations");
            });
        }

        // OnConfiguring
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // 建立 IConfiguration 物件，讀取應用程式的設定資訊。
                IConfiguration Config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();

                // 獲取 資料庫連線字串
                optionsBuilder.UseSqlServer(Config.GetConnectionString("SteamNexus"));
            }
        }
    }
}
