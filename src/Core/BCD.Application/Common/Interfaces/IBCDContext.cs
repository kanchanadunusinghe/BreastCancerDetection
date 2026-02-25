using BCD.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BCD.Application.Common.Interfaces;

public interface IBCDContext
{
    DbSet<AppSetting> AppSettings { get; }
    DbSet<User> Users { get; }
    DbSet<Role> Roles { get; }
    DbSet<UserRole> UserRoles { get; }
    DbSet<Patient> Patients { get; }
    DbSet<MammographyScan> MammographyScans { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitTransactionAsync(CancellationToken cancellationToken);
    Task RollbackTransactionAsync(CancellationToken cancellationToken);
    Task RetryOnExceptionAsync(Func<Task> func);

}
