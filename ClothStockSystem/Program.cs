using ClothStock.Business.Services;
using ClothStock.Data.Connection;
using ClothStock.Data.Interfaces;
using ClothStock.Data.Repositories;
using ClothStockSystem.Forms.Auth;
using ClothStockSystem.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClothStockSystem;

static class Program
{
    // TESTING: true = login nahi aayega, seedha Owner. Release se pehle false karein.
    private const bool SkipLoginForTesting = true;
    // Testing mein seller ka view dekhna ho to Seller likhein
    private const string TestRole = "Owner";   // "Owner" ya "Seller"

    public static IServiceProvider Services { get; private set; } = null!;

    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connString = config.GetConnectionString("Default")!;
        var sc = new ServiceCollection();

        sc.AddSingleton(new DbConnectionFactory(connString));

        sc.AddTransient<IArticleRepository, ArticleRepository>();
        sc.AddTransient<IColorRepository, ColorRepository>();
        sc.AddTransient<IStockRepository, StockRepository>();
        sc.AddTransient<ISettingsRepository, SettingsRepository>();
        sc.AddTransient<ICourierRepository, CourierRepository>();
        sc.AddTransient<IOrderRepository, OrderRepository>();
        sc.AddTransient<IShipmentRepository, ShipmentRepository>();
        sc.AddTransient<IExpenseRepository, ExpenseRepository>();
        sc.AddTransient<IWorkerRepository, WorkerRepository>();
        sc.AddTransient<IExpenseSetupRepository, ExpenseSetupRepository>();
        sc.AddTransient<IUserRepository, UserRepository>();
        sc.AddTransient<IFinanceRepository, FinanceRepository>();

        sc.AddTransient<StockService>();
        sc.AddTransient<SettingsService>();
        sc.AddTransient<ArticleService>();
        sc.AddTransient<CourierService>();
        sc.AddTransient<OrderService>();
        sc.AddTransient<ShipmentService>();
        sc.AddTransient<ExpenseService>();
        sc.AddTransient<WorkerService>();
        sc.AddTransient<ExpenseSetupService>();
        sc.AddTransient<AuthService>();
        sc.AddTransient<FinanceService>();

        Services = sc.BuildServiceProvider();

        try
        {
            Services.GetRequiredService<AuthService>().EnsureSeedAsync().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Could not connect to database or Users table was not created:\n\n" + ex.Message,
                "Startup", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (SkipLoginForTesting)
        {
            var repo = Services.GetRequiredService<IUserRepository>();
            var uname = TestRole == "Seller" ? "seller" : "owner";
            var u = repo.GetByUsernameAsync(uname).GetAwaiter().GetResult();
            if (u == null) { MessageBox.Show("Test user not found."); return; }

            SessionManager.Set(u);
            SessionManager.SellerSeesCost = Services.GetRequiredService<SettingsService>()
                .GetBoolAsync("SellerSeesCost", false).GetAwaiter().GetResult();

            Application.Run(new MainForm());   // Logout button is mode mein form band kar dega
            return;
        }

        while (true)
        {
            using (var login = new LoginForm())
            {
                if (login.ShowDialog() != DialogResult.OK) break;
            }

            SessionManager.LogoutRequested = false;
            Application.Run(new MainForm());

            if (!SessionManager.LogoutRequested) break;
            SessionManager.Clear();
        }
    }
}