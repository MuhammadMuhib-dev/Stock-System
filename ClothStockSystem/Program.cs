using ClothStock.Business.Services;
using ClothStock.Data.Connection;
using ClothStock.Data.Interfaces;
using ClothStock.Data.Repositories;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClothStockSystem;

static class Program
{
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

        // Database
        sc.AddSingleton(new DbConnectionFactory(connString));

        // Repositories
        sc.AddTransient<IArticleRepository, ArticleRepository>();
        sc.AddTransient<IColorRepository, ColorRepository>();
        sc.AddTransient<IStockRepository, StockRepository>();
        sc.AddTransient<ISettingsRepository, SettingsRepository>();

        // Services
        sc.AddTransient<StockService>();
        sc.AddTransient<SettingsService>();
        sc.AddTransient<ArticleService>();

        Services = sc.BuildServiceProvider();

        Application.Run(new MainForm());
    }
}