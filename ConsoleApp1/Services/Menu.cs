

namespace ConsoleApp1.Services
{
    internal static class Menu
    {
        internal static string MainMenu()
        {
            string mainMenu = """
                q - выход
                l - создать линию
                2 - вторая страница
                --> 
            """;
            return mainMenu;
        }

        internal static ConsoleKey GetAnswear() => Console.ReadKey().Key;
        
        internal static string TwoPageMenu()
        {
            string page2 = """
                q  - выход
                p  - создать продукт
                <- - назад
                --> 
                """;

            return page2;
        }
    }
}
