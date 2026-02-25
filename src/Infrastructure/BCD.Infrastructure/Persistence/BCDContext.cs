using BCD.Application.Common.Interfaces;
using BCD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;
using System.Reflection;

namespace BCD.Infrastructure.Persistence;

public class BCDContext : DbContext, IBCDContext
{
    private IDbContextTransaction dbContextTransaction;
    public BCDContext(DbContextOptions<BCDContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(message => Debug.WriteLine(message))
            .EnableSensitiveDataLogging();
        optionsBuilder.UseLazyLoadingProxies();

    }

    public DbSet<AppSetting> AppSettings => Set<AppSetting>();

    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<UserRole> UserRoles => Set<UserRole>();

    public DbSet<Patient> Patients => Set<Patient>();

    public DbSet<MammographyScan> MammographyScans => Set<MammographyScan>();


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        dbContextTransaction ??= await Database.BeginTransactionAsync(cancellationToken);
    }
    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        try
        {
            await SaveChangesAsync(cancellationToken);
            dbContextTransaction?.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (dbContextTransaction != null)
            {
                DisposeTransaction();
            }
        }
    }
    public async Task RetryOnExceptionAsync(Func<Task> func)
    {
        await Database.CreateExecutionStrategy().ExecuteAsync(func);
    }
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        try
        {
            await dbContextTransaction?.RollbackAsync(cancellationToken);
        }
        finally
        {
            DisposeTransaction();
        }
    }
    private void DisposeTransaction()
    {
        try
        {
            if (dbContextTransaction != null)
            {
                dbContextTransaction.Dispose();
                dbContextTransaction = null;
            }
        }
        catch (Exception ex)
        {

        }
    }

}
