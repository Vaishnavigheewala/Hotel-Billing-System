using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HotelBillingMVC.Models
{
    public partial class Hotel_Billing_SystemContext : DbContext
    {
        public Hotel_Billing_SystemContext()
        {
        }

        public Hotel_Billing_SystemContext(DbContextOptions<Hotel_Billing_SystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Branch> Branches { get; set; } = null!;
        public virtual DbSet<DiningTable> DiningTables { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<Menu> Menus { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<OrderMaster> OrderMasters { get; set; } = null!;
        public virtual DbSet<Reservation> Reservations { get; set; } = null!;
        public virtual DbSet<Restaurent> Restaurents { get; set; } = null!;
        public virtual DbSet<RoleMaster> RoleMasters { get; set; } = null!;
        public virtual DbSet<Timeslot> Timeslots { get; set; } = null!;
        public virtual DbSet<Usermaster> Usermasters { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=LAPTOP-1J34JEV6;Initial Catalog=Hotel_Billing_System;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Branch>(entity =>
            {
                entity.HasIndex(e => e.RestBranchId, "IX_Branches_RestBranchId");

                entity.Property(e => e.ActiveFlag).HasColumnName("Active_flag");

                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.BranchName)
                    .HasMaxLength(50)
                    .HasColumnName("Branch_Name");

                entity.Property(e => e.RestaurentId).HasColumnName("Restaurent_Id");

                entity.HasOne(d => d.RestBranch)
                    .WithMany(p => p.Branches)
                    .HasForeignKey(d => d.RestBranchId);
            });

            modelBuilder.Entity<DiningTable>(entity =>
            {
                entity.HasIndex(e => e.TablesId, "IX_DiningTables_TablesId");

                entity.Property(e => e.ActiveFlag).HasColumnName("Active_flag");

                entity.Property(e => e.BranchId).HasColumnName("Branch_Id");

                entity.Property(e => e.SeatNo).HasColumnName("Seat_No");

                entity.HasOne(d => d.Tables)
                    .WithMany(p => p.DiningTables)
                    .HasForeignKey(d => d.TablesId);
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.HasIndex(e => e.FeedIdId, "IX_Feedbacks_FeedIdId");

                entity.Property(e => e.ActiveFlag).HasColumnName("Active_flag");

                entity.HasOne(d => d.FeedId)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.FeedIdId);
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.Property(e => e.ActiveFlag).HasColumnName("Active_flag");

                entity.Property(e => e.Description).HasMaxLength(150);

                entity.Property(e => e.ItemName).HasMaxLength(50);

                entity.Property(e => e.Price).HasColumnName("price");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("Order_Details");

                entity.HasIndex(e => e.MenuId, "IX_Order_Details_MenuId");

                entity.HasIndex(e => e.OrderId, "IX_Order_Details_OrderId");

                entity.Property(e => e.ActiveFlag).HasColumnName("Active_flag");

                entity.Property(e => e.UnitPrice).HasColumnName("unit_price");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.MenuId);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId);
            });

            modelBuilder.Entity<OrderMaster>(entity =>
            {
                entity.ToTable("Order_Masters");

                entity.HasIndex(e => e.OrderTablesId, "IX_Order_Masters_OrderTablesId");

                entity.HasIndex(e => e.OrderuserId, "IX_Order_Masters_OrderuserId");

                entity.Property(e => e.ActiveFlag).HasColumnName("Active_flag");

                entity.Property(e => e.CustName)
                    .HasMaxLength(100)
                    .HasColumnName("cust_name");

                entity.Property(e => e.OrderDate).HasColumnName("Order_date");

                entity.Property(e => e.OrderStatus).HasColumnName("Order_Status");

                entity.Property(e => e.PaymentType).HasColumnName("Payment_type");

                entity.Property(e => e.TableId).HasColumnName("Table_Id");

                entity.HasOne(d => d.OrderTables)
                    .WithMany(p => p.OrderMasters)
                    .HasForeignKey(d => d.OrderTablesId);

                entity.HasOne(d => d.Orderuser)
                    .WithMany(p => p.OrderMasters)
                    .HasForeignKey(d => d.OrderuserId);
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasIndex(e => e.ReservationBranchesId, "IX_Reservations_ReservationBranchesId");

                entity.HasIndex(e => e.ReservationTablesId, "IX_Reservations_ReservationTablesId");

                entity.HasIndex(e => e.ReservationTimeslotsId, "IX_Reservations_ReservationTimeslotsId");

                entity.HasIndex(e => e.ReservationuserId, "IX_Reservations_ReservationuserId");

                entity.HasIndex(e => e.ResevationRestId, "IX_Reservations_ResevationRestId");

                entity.Property(e => e.ActiveFlag).HasColumnName("Active_flag");

                entity.Property(e => e.BranchId).HasColumnName("Branch_Id");

                entity.Property(e => e.ReservationStatus).HasColumnName("Reservation_status");

                entity.Property(e => e.RestaurentId).HasColumnName("Restaurent_Id");

                entity.Property(e => e.TableId).HasColumnName("Table_Id");

                entity.Property(e => e.TimeslteId).HasColumnName("Timeslte_Id");

                entity.HasOne(d => d.ReservationBranches)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.ReservationBranchesId);

                entity.HasOne(d => d.ReservationTables)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.ReservationTablesId);

                entity.HasOne(d => d.ReservationTimeslots)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.ReservationTimeslotsId);

                entity.HasOne(d => d.Reservationuser)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.ReservationuserId);

                entity.HasOne(d => d.ResevationRest)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.ResevationRestId);
            });

            modelBuilder.Entity<Restaurent>(entity =>
            {
                entity.ToTable("Restaurent");

                entity.Property(e => e.ActiveFlag).HasColumnName("Active_flag");

                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.RestaurentName)
                    .HasMaxLength(150)
                    .HasColumnName("Restaurent_Name");
            });

            modelBuilder.Entity<RoleMaster>(entity =>
            {
                entity.ToTable("RoleMaster");

                entity.Property(e => e.ActiveFlag).HasColumnName("Active_flag");
            });

            modelBuilder.Entity<Timeslot>(entity =>
            {
                entity.ToTable("Timeslot");

                entity.HasIndex(e => e.TimeslotId, "IX_Timeslot_timeslotId");

                entity.Property(e => e.ActiveFlag).HasColumnName("Active_flag");

                entity.Property(e => e.BranchId).HasColumnName("Branch_Id");

                entity.Property(e => e.TimeslotId).HasColumnName("timeslotId");

                entity.HasOne(d => d.TimeslotNavigation)
                    .WithMany(p => p.Timeslots)
                    .HasForeignKey(d => d.TimeslotId);
            });

            modelBuilder.Entity<Usermaster>(entity =>
            {
                entity.ToTable("Usermaster");

                entity.HasIndex(e => e.RoleId, "IX_Usermaster_RoleId");

                entity.Property(e => e.ActiveFlag).HasColumnName("Active_flag");

                entity.Property(e => e.Password).HasMaxLength(10);

                entity.Property(e => e.Username).HasMaxLength(150);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Usermasters)
                    .HasForeignKey(d => d.RoleId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
