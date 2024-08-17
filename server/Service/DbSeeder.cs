using DataAccess;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Service;

public class DbSeeder
{
    private readonly ILogger<DbSeeder> logger;
    private readonly AppDbContext context;
    private readonly UserManager<User> userManager;
    private readonly RoleManager<IdentityRole> roleManager;

    public DbSeeder(
        ILogger<DbSeeder> logger,
        AppDbContext context,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager
    )
    {
        this.logger = logger;
        this.context = context;
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        // context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        await CreateUser(username: "admin@example.com", password: "S3cret!", role: Role.Admin);
        await CreateUser(username: "editor@example.com", password: "S3cret!", role: Role.Editor);
        await CreateUser(username: "reader@example.com", password: "S3cret!", role: Role.Reader);

        var admin = await userManager.FindByNameAsync("admin@example.com");
        if (!context.Posts.Where(p => p.Title == "First post").Any())
        {
            context.Posts.Add(
                new Post
                {
                    Title = "First post",
                    Content = "This is the first post",
                    AuthorId = admin!.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    PublishedAt = DateTime.UtcNow
                }
            );
        }
        await context.SaveChangesAsync();
    }

    async Task CreateUser(string username, string password, string role)
    {
        await roleManager.CreateAsync(new IdentityRole(role));
        var user = new User
        {
            UserName = username,
            Email = username,
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                logger.LogWarning("{Code}: {Description}", error.Code, error.Description);
            }
        }
        user = await userManager.FindByNameAsync(username);
        if (user != null)
        {
            await userManager.AddToRoleAsync(user!, role!);
        }
    }
}

