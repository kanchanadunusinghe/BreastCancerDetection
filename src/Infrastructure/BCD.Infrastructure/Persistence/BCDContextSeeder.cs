using BCD.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BCD.Infrastructure.Persistence
{
    public class BCDContextSeeder(ILogger<BCDContextSeeder> logger, BCDContext context)
    {
        public async Task InitializeAsync()
        {
            try
            {
                if (context.Database.IsSqlServer())
                {
                    await context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initializing the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            await SeedRoleAsync();
            await SeedUsersAsync();
            await SeedPatientsAsync();
        }

        public async Task SeedRoleAsync()
        {
            try
            {
                if (!context.Roles.Any())
                {
                    var roles = Enum.GetValues(typeof(SystemRole))
                                    .Cast<SystemRole>()
                                    .Select(r => new BCD.Domain.Entities.Role
                                    {

                                        Name = r.ToString(),
                                        Description = r.ToString(),
                                    })
                                    .ToList();

                    await context.Roles.AddRangeAsync(roles);
                    await context.SaveChangesAsync();

                    logger.LogInformation("Roles seeded successfully.");
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding roles.");
                throw;
            }
        }

        public async Task SeedUsersAsync()
        {
            try
            {
                if (!context.Users.Any())
                {
                    var passwordHash = BCrypt.Net.BCrypt.HashPassword("pass@123");

                    var roles = await context.Roles.ToListAsync();

                    var adminRole = roles.First(r => r.Name == SystemRole.Admin.ToString());
                    var doctorRole = roles.First(r => r.Name == SystemRole.Doctor.ToString());
                    var nurseRole = roles.First(r => r.Name == SystemRole.Nurse.ToString());

                    var users = new List<BCD.Domain.Entities.User>
                    {
                        new()
                        {
                            FirstName = "Admin",
                            LastName = "Admin",
                            Email = "admin@bcd.com",
                            PasswordHash = passwordHash,
                            GMCNumber = "GMC1234567",
                            IsActive = true,
                            UserRoles = new List<BCD.Domain.Entities.UserRole>
                            {
                                new() { RoleId = adminRole.Id }
                            }
                        },
                        new()
                        {
                            FirstName = "Emma",
                            LastName = "Roberts",
                            Email = "emma.roberts@bcdhealth.co.uk",
                            PasswordHash = passwordHash,
                            GMCNumber = "GMC2345678",
                            IsActive = true,
                            UserRoles = new List<BCD.Domain.Entities.UserRole>
                            {
                                new() { RoleId = doctorRole.Id }
                            }
                        },
                        new()
                        {
                            FirstName = "Daniel",
                            LastName = "Smith",
                            Email = "daniel.smith@bcdhealth.co.uk",
                            PasswordHash = passwordHash,
                            GMCNumber = "GMC3456789",
                            IsActive = true,
                            UserRoles = new List<BCD.Domain.Entities.UserRole>
                            {
                                new() { RoleId = doctorRole.Id }
                            }
                        },
                        new()
                        {
                            FirstName = "Sophia",
                            LastName = "Taylor",
                            Email = "sophia.taylor@bcdhealth.co.uk",
                            PasswordHash = passwordHash,
                            GMCNumber = "",
                            IsActive = true,
                            UserRoles = new List<BCD.Domain.Entities.UserRole>
                            {
                                new() { RoleId = nurseRole.Id }
                            }
                        },
                        new()
                        {
                            FirstName = "Michael",
                            LastName = "Brown",
                            Email = "michael.brown@bcdhealth.co.uk",
                            PasswordHash = passwordHash,
                            GMCNumber = "",
                            IsActive = false,
                            UserRoles = new List<BCD.Domain.Entities.UserRole>
                            {
                                new() { RoleId = nurseRole.Id }
                            }
                        }
                    };

                    await context.Users.AddRangeAsync(users);
                    await context.SaveChangesAsync();

                    logger.LogInformation("Users and UserRoles seeded successfully.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding users.");
                throw;
            }
        }

        public async Task SeedPatientsAsync()
        {
            try
            {
                if (!await context.Patients.AnyAsync())
                {
                    var patients = new List<BCD.Domain.Entities.Patient>
                    {
                        new()
                        {
                            NHSNumber = "9434765919",
                            FirstName = "Oliver",
                            LastName = "Johnson",
                            Email = "oliver.johnson@nhs.uk",
                            MobileNumber = "07123456789",
                            DateOfBirth = new DateTime(1985, 3, 14),
                            Gender = Gender.Male,
                            PostCode = "SW1A 1AA",
                            CreatedAt = DateTime.Now
                        },
                        new()
                        {
                            NHSNumber = "8473625194",
                            FirstName = "Amelia",
                            LastName = "Williams",
                            Email = "amelia.williams@nhs.uk",
                            MobileNumber = "07234567890",
                            DateOfBirth = new DateTime(1990, 7, 22),
                            Gender = Gender.Female,
                            PostCode = "M1 1AE",
                            CreatedAt = DateTime.Now
                        },
                        new()
                        {
                            NHSNumber = "7564839201",
                            FirstName = "George",
                            LastName = "Brown",
                            Email = "george.brown@nhs.uk",
                            MobileNumber = "07345678901",
                            DateOfBirth = new DateTime(1978, 11, 5),
                             Gender = Gender.Male,
                            PostCode = "B1 1BB",
                            CreatedAt = DateTime.Now
                        },
                        new()
                        {
                            NHSNumber = "6657382910",
                            FirstName = "Isla",
                            LastName = "Taylor",
                            Email = "isla.taylor@nhs.uk",
                            MobileNumber = "07456789012",
                            DateOfBirth = new DateTime(1995, 1, 30),
                            Gender = Gender.Female,
                            PostCode = "LS1 4DY",
                            CreatedAt = DateTime.Now
                        },
                        new()
                        {
                            NHSNumber = "5548291736",
                            FirstName = "Harry",
                            LastName = "Davies",
                            Email = "harry.davies@nhs.uk",
                            MobileNumber = "07567890123",
                            DateOfBirth = new DateTime(1982, 9, 18),
                            Gender = Gender.Male,
                            PostCode = "CF10 1EP",
                            CreatedAt = DateTime.Now
                        }
                    };

                    await context.Patients.AddRangeAsync(patients);
                    await context.SaveChangesAsync();

                    logger.LogInformation("Patients seeded successfully.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding patients.");
                throw;
            }
        }
    }
}
