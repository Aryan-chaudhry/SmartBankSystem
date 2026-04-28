using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SmartBank.Data.Entities;

public partial class SmartBankContext : DbContext
{
    public SmartBankContext()
    {
    }

    public SmartBankContext(DbContextOptions<SmartBankContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<KycDocument> KycDocuments { get; set; }

    public virtual DbSet<Loan> Loans { get; set; }

    public virtual DbSet<LoanDocument> LoanDocuments { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SupportTicket> SupportTickets { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Transfer> Transfers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VwCustomerAccountSummary> VwCustomerAccountSummaries { get; set; }

    public virtual DbSet<VwDailyTransactionSummary> VwDailyTransactionSummaries { get; set; }

    public virtual DbSet<VwLoanPipeline> VwLoanPipelines { get; set; }

    public virtual DbSet<VwLowBalanceAccount> VwLowBalanceAccounts { get; set; }

    public virtual DbSet<VwUnreadNotification> VwUnreadNotifications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=SmartOnlineBankingDb;User Id=sa;Password=YourStrongPassw0rd@123;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Accounts__349DA5A62F9D7B26");

            entity.HasIndex(e => e.AccountNumber, "IX_Accounts_AccountNumber");

            entity.HasIndex(e => e.Status, "IX_Accounts_Status");

            entity.HasIndex(e => e.UserId, "IX_Accounts_UserId");

            entity.HasIndex(e => e.AccountNumber, "UQ__Accounts__BE2ACD6FBA436A11").IsUnique();

            entity.Property(e => e.AccountNumber).HasMaxLength(20);
            entity.Property(e => e.AccountType).HasMaxLength(20);
            entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BranchCode).HasMaxLength(20);
            entity.Property(e => e.Currency)
                .HasMaxLength(5)
                .HasDefaultValue("INR");
            entity.Property(e => e.Ifsccode)
                .HasMaxLength(15)
                .HasColumnName("IFSCCode");
            entity.Property(e => e.InterestRate).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.MinimumBalance)
                .HasDefaultValue(500m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OpenedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Active");

            entity.HasOne(d => d.User).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Accounts_Users");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.AuditLogId).HasName("PK__AuditLog__EB5F6CBD96432931");

            entity.HasIndex(e => new { e.EntityType, e.EntityId }, "IX_AuditLogs_EntityType_EntityId").HasFilter("([EntityType] IS NOT NULL)");

            entity.HasIndex(e => new { e.UserId, e.CreatedAt }, "IX_AuditLogs_UserId_CreatedAt")
                .IsDescending(false, true)
                .HasFilter("([UserId] IS NOT NULL)");

            entity.Property(e => e.Action).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.EntityType).HasMaxLength(50);
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(50)
                .HasColumnName("IPAddress");
            entity.Property(e => e.UserAgent).HasMaxLength(500);

            entity.HasOne(d => d.User).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__AuditLogs__UserI__7D439ABD");
        });

        modelBuilder.Entity<KycDocument>(entity =>
        {
            entity.HasKey(e => e.KycDocumentId).HasName("PK__KycDocum__7D919522B0F77BD9");

            entity.HasIndex(e => new { e.UserId, e.Status }, "IX_KycDocs_UserId_Status");

            entity.Property(e => e.DocumentNumber).HasMaxLength(100);
            entity.Property(e => e.DocumentType).HasMaxLength(50);
            entity.Property(e => e.FilePath).HasMaxLength(500);
            entity.Property(e => e.RejectionReason).HasMaxLength(500);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.UploadedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.KycDocumentUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KycDocume__UserI__49C3F6B7");

            entity.HasOne(d => d.VerifiedByUser).WithMany(p => p.KycDocumentVerifiedByUsers)
                .HasForeignKey(d => d.VerifiedByUserId)
                .HasConstraintName("FK__KycDocume__Verif__4AB81AF0");
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(e => e.LoanId).HasName("PK__Loans__4F5AD457A23C5725");

            entity.HasIndex(e => e.AccountId, "IX_Loans_AccountId").HasFilter("([AccountId] IS NOT NULL)");

            entity.HasIndex(e => e.UserId, "IX_Loans_UserId");

            entity.HasIndex(e => new { e.UserId, e.Status }, "IX_Loans_UserId_Status");

            entity.Property(e => e.ApprovedAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Emiamount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("EMIAmount");
            entity.Property(e => e.InterestRate).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.LoanType).HasMaxLength(50);
            entity.Property(e => e.Purpose).HasMaxLength(500);
            entity.Property(e => e.RejectionReason).HasMaxLength(500);
            entity.Property(e => e.RequestedAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Account).WithMany(p => p.Loans)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Loans__AccountId__693CA210");

            entity.HasOne(d => d.ReviewedByUser).WithMany(p => p.LoanReviewedByUsers)
                .HasForeignKey(d => d.ReviewedByUserId)
                .HasConstraintName("FK__Loans__ReviewedB__6A30C649");

            entity.HasOne(d => d.User).WithMany(p => p.LoanUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Loans__UserId__68487DD7");
        });

        modelBuilder.Entity<LoanDocument>(entity =>
        {
            entity.HasKey(e => e.LoanDocumentId).HasName("PK__LoanDocu__86EF3B70F27A45F4");

            entity.Property(e => e.DocumentType).HasMaxLength(100);
            entity.Property(e => e.FilePath).HasMaxLength(500);
            entity.Property(e => e.UploadedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Loan).WithMany(p => p.LoanDocuments)
                .HasForeignKey(d => d.LoanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LoanDocum__LoanI__6FE99F9F");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E1257C79779");

            entity.HasIndex(e => new { e.UserId, e.IsRead }, "IX_Notif_UserId_IsRead");

            entity.HasIndex(e => e.UserId, "IX_Notifications_UserId");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.Message).HasMaxLength(2000);
            entity.Property(e => e.RelatedEntityType).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__UserI__797309D9");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1AA25F458A");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B61602C60CFA2").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<SupportTicket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__SupportT__712CC607464870CC");

            entity.HasIndex(e => e.AssignedToUserId, "IX_Tickets_AssignedToUserId").HasFilter("([AssignedToUserId] IS NOT NULL)");

            entity.HasIndex(e => new { e.CreatedByUserId, e.Status }, "IX_Tickets_CreatedByUserId_Status");

            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Priority).HasMaxLength(20);
            entity.Property(e => e.Resolution).HasMaxLength(2000);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.Subject).HasMaxLength(200);

            entity.HasOne(d => d.AssignedToUser).WithMany(p => p.SupportTicketAssignedToUsers)
                .HasForeignKey(d => d.AssignedToUserId)
                .HasConstraintName("FK__SupportTi__Assig__74AE54BC");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.SupportTicketCreatedByUsers)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SupportTi__Creat__73BA3083");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A6B048AD53B");

            entity.HasIndex(e => e.AccountId, "IX_Transactions_AccountId");

            entity.HasIndex(e => new { e.AccountId, e.CreatedAt }, "IX_Transactions_AccountId_CreatedAt").IsDescending(false, true);

            entity.HasIndex(e => e.ReferenceNumber, "IX_Transactions_ReferenceNumber");

            entity.HasIndex(e => e.TransferId, "IX_Transactions_TransferId").HasFilter("([TransferId] IS NOT NULL)");

            entity.HasIndex(e => e.ReferenceNumber, "UQ__Transact__C5ADBE4DF32C03D4").IsUnique();

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BalanceAfter).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Channel).HasMaxLength(20);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ReferenceNumber).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.TransactionType).HasMaxLength(20);

            entity.HasOne(d => d.Account).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Accou__619B8048");

            entity.HasOne(d => d.PerformedByUser).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.PerformedByUserId)
                .HasConstraintName("FK__Transacti__Perfo__6383C8BA");

            entity.HasOne(d => d.Transfer).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.TransferId)
                .HasConstraintName("FK__Transacti__Trans__628FA481");
        });

        modelBuilder.Entity<Transfer>(entity =>
        {
            entity.HasKey(e => e.TransferId).HasName("PK__Transfer__95490091CF858930");

            entity.HasIndex(e => e.FromAccountId, "IX_Transfers_FromAccountId");

            entity.HasIndex(e => e.ReferenceNumber, "IX_Transfers_ReferenceNumber");

            entity.HasIndex(e => e.ToAccountId, "IX_Transfers_ToAccountId");

            entity.HasIndex(e => e.ReferenceNumber, "UQ__Transfer__C5ADBE4DFDD6485C").IsUnique();

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ReferenceNumber).HasMaxLength(50);
            entity.Property(e => e.Remarks).HasMaxLength(500);
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.FromAccount).WithMany(p => p.TransferFromAccounts)
                .HasForeignKey(d => d.FromAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transfers__FromA__59063A47");

            entity.HasOne(d => d.InitiatedByUser).WithMany(p => p.Transfers)
                .HasForeignKey(d => d.InitiatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transfers__Initi__5AEE82B9");

            entity.HasOne(d => d.ToAccount).WithMany(p => p.TransferToAccounts)
                .HasForeignKey(d => d.ToAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transfers__ToAcc__59FA5E80");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CFE7331F8");

            entity.HasIndex(e => e.Email, "IX_Users_Email");

            entity.HasIndex(e => e.NationalId, "IX_Users_NationalId").HasFilter("([NationalId] IS NOT NULL)");

            entity.HasIndex(e => e.PhoneNumber, "IX_Users_PhoneNumber").HasFilter("([PhoneNumber] IS NOT NULL)");

            entity.HasIndex(e => e.RoleId, "IX_Users_RoleId");

            entity.HasIndex(e => e.PhoneNumber, "UQ__Users__85FB4E3839BD0AB6").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105346A861823").IsUnique();

            entity.HasIndex(e => e.NationalId, "UQ__Users__E9AA32FA62280954").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasDefaultValue("India");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FailedLoginAttempts).HasDefaultValue(0);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsEmailVerified).HasDefaultValue(false);
            entity.Property(e => e.IsFrozen).HasDefaultValue(false);
            entity.Property(e => e.KycStatus)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.NationalId).HasMaxLength(30);
            entity.Property(e => e.PasswordHash).HasMaxLength(512);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        modelBuilder.Entity<VwCustomerAccountSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_CustomerAccountSummary");

            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FullName).HasMaxLength(201);
            entity.Property(e => e.KycStatus).HasMaxLength(20);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.TotalBalance).HasColumnType("decimal(38, 2)");
        });

        modelBuilder.Entity<VwDailyTransactionSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_DailyTransactionSummary");

            entity.Property(e => e.AverageAmount).HasColumnType("decimal(38, 6)");
            entity.Property(e => e.MaxAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MinAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.TransactionType).HasMaxLength(20);
        });

        modelBuilder.Entity<VwLoanPipeline>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_LoanPipeline");

            entity.Property(e => e.ApprovedAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CustomerEmail).HasMaxLength(150);
            entity.Property(e => e.CustomerName).HasMaxLength(201);
            entity.Property(e => e.Emiamount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("EMIAmount");
            entity.Property(e => e.InterestRate).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.LoanType).HasMaxLength(50);
            entity.Property(e => e.Purpose).HasMaxLength(500);
            entity.Property(e => e.RequestedAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ReviewedBy).HasMaxLength(201);
            entity.Property(e => e.Status).HasMaxLength(20);
        });

        modelBuilder.Entity<VwLowBalanceAccount>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_LowBalanceAccounts");

            entity.Property(e => e.AccountNumber).HasMaxLength(20);
            entity.Property(e => e.AccountType).HasMaxLength(20);
            entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BufferAmount).HasColumnType("decimal(19, 2)");
            entity.Property(e => e.CustomerName).HasMaxLength(201);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.MinimumBalance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
        });

        modelBuilder.Entity<VwUnreadNotification>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_UnreadNotifications");
        });
        modelBuilder.HasSequence<int>("AccountNumberSeq");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
