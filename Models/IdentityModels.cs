using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SensenbrennerHospital.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {//Fields here
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int StreetNumber { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Faq> Faqs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FaqCategory> FaqCategories { get; set; }
        public DbSet<Career> Careers { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<HospitalTour> HospitalTours { get; set; }
        public DbSet<ReportsTable> ReportsTables { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<NewsBanner> NewsBanners { get; set; }
        public DbSet<Practice> Practices { get; set; }
        public DbSet<DoctorInfo> DoctorInfos { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<AppointmentBooking> AppointmentBookings { get; set; }
        public DbSet<DonorInfo> DonorInfos { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<VolunteerPosition> VolunteerPositions { get; set; }
    }
}