using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TradingJournal.core.Models;

namespace TradingJournal.core.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Trade> Trades { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure the Trade entity to use TPH (Table Per Hierarchy) inheritance
            //long/short trades will be stored in the same table with a discriminator column
            modelBuilder.Entity<Trade>()
                .HasDiscriminator<string>("TradeType")
                .HasValue<LongTrade>("Long")
                .HasValue<ShortTrade>("Short");

            modelBuilder.Entity<Trade>(entity =>
            {
                entity.Property(t => t.Entry).HasColumnType("decimal(18, 8)");
                entity.Property(t => t.StopLoss).HasColumnType("decimal(18, 8)");
                entity.Property(t => t.TakeProfit).HasColumnType("decimal(18, 8)");
                entity.Property(t => t.ExitPrice).HasColumnType("decimal(18, 8)");
                entity.Property(t => t.Amount).HasColumnType("decimal(18, 8)");
                entity.Property(t => t.Deposit).HasColumnType("decimal(18, 2)");
                entity.Property(t => t.Withdraw).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(a => a.StartingCapital).HasColumnType("decimal(18, 2)");
                entity.Property(a => a.CurrentBalance).HasColumnType("decimal(18, 2)");
            });
        }
    }
}
