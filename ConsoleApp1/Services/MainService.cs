using ConsoleApp1.Services.EntityServices;
using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsoleApp1.Services
{
    internal class MainService(
        ILogger<MainService> logger,
        IServiceScopeFactory scopeFactory) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Random rnd = await StartInit(logger, stoppingToken);

            GTIN gtin = GtinGenerator.GetGTIN(rnd);
            //int count = 0;
            //var start = Stopwatch.GetTimestamp();
            //await foreach (CodeValue code in CodeGenerator.GetCodesRange(13, gtin, rnd, 1000, 100))
            //    Console.WriteLine($"{++count,6}: {code.Code}");
            //TimeSpan elapsed = Stopwatch.GetElapsedTime(start);

            //Console.WriteLine(elapsed);


            //создать линию

            //using var scope = scopeFactory.CreateScope();
            //var productionLineRepository = scope.ServiceProvider.GetRequiredService<IProductionLineDAL>();
            //var productionRepository = scope.ServiceProvider.GetRequiredService<IProductDAL>();

            //var lineId = await EntityService<MainService>.CreateProductionLine("Линия-1", logger, productionLineRepository);
            //var line = await EntityService<MainService>.GetProductionLineById(logger, lineId, productionLineRepository);
            ////добавить продукт
            //var productId = await EntityService<MainService>.CreateProduct("Продукт-1", gtin, 13, logger, productionRepository, line);
            ////запустить сессию
            ////закрыть сессию
            char ch;
            do
            {
                string mainMenu = Menu.MainMenu();
                Console.WriteLine(mainMenu);
                ch = Menu.GetAnswear() switch
                {
                    ConsoleKey.P => 'p',
                    ConsoleKey.Q => 'q',
                    ConsoleKey.L => 'l',
                    ConsoleKey.Backspace => '←',
                    _ => '?'
                };
                Console.WriteLine(ch);
            } while (ch != 'q');
        }

       

        private static async Task<Random> StartInit(ILogger<MainService> logger, CancellationToken stoppingToken)
        {
            Random rnd = new Random();
            await Task.Delay(1000, stoppingToken);
            string str = "Server started...";
            logger.LogInformation(str);
            return rnd;
        }
    }
}
