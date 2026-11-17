using Microsoft.AspNetCore.Identity;

namespace CinemaTicketBooking.DBSeeding
{
    public class DBInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        public readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DBInitializer> _logger;

        public DBInitializer(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILogger<DBInitializer> logger)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }


        public async Task InitializeAsync()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Any())
                {
                    _context.Database.Migrate();
                }

                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole(CD.SUPER_ADMIN));
                    await _roleManager.CreateAsync(new IdentityRole(CD.ADMIN));
                    await _roleManager.CreateAsync(new IdentityRole(CD.EMPLOYEE));
                    await _roleManager.CreateAsync(new IdentityRole(CD.CUSTOMER));

                    await _userManager.CreateAsync(new ApplicationUser
                    {
                        UserName = "SuperAdmin",
                        Name = "Super Admin",
                        Email = "SuperAdmin@example.com",
                        EmailConfirmed = true,
                    }, "SuperAdmin@123");

                    var user = await _userManager.FindByEmailAsync("SuperAdmin@example.com");
                    await _userManager.AddToRoleAsync(user, CD.SUPER_ADMIN);

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }



        }
    }
}
