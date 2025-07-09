using BudgetTracker.Web.Services;
using Microsoft.AspNetCore.Identity;
using BudgetTracker.Models;

namespace BudgetTracker.Web.Services
{
    public class DemoResetBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DemoResetBackgroundService> _logger;
        private readonly TimeSpan _resetInterval = TimeSpan.FromMinutes(30);

        public DemoResetBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<DemoResetBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(_resetInterval, stoppingToken);
                    
                    if (!stoppingToken.IsCancellationRequested)
                    {
                        await ResetDemoDataAsync();
                    }
                }
                catch (OperationCanceledException)
                {
                    // Expected when cancellation is requested
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in demo reset background service");
                }
            }
        }

        private async Task ResetDemoDataAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var demoService = scope.ServiceProvider.GetRequiredService<DemoService>();
            var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            try
            {
                const string demoEmail = "demo@williamarice.com";
                var demoUser = await userManager.FindByEmailAsync(demoEmail);
                
                if (demoUser != null)
                {
                    _logger.LogInformation("Starting automatic demo reset");
                    
                    await demoService.ResetDemoDataAsync();
                    
                    _logger.LogInformation("Automatic demo reset completed successfully");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to reset demo data automatically");
            }
        }
    }
}
