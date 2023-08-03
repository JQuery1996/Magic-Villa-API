namespace Learning.CoreApi.Logger {
    public class Logging : ILogging {
        public void Log(LogLevel level, string message) {
            switch (level) {
                case LogLevel.Debug:
                    Console.WriteLine($"[Debug]- {message}");
                    break;
                case LogLevel.Information:
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[Info]- {message}");
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case LogLevel.Warning:
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"[Warning]- {message}");
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case LogLevel.Error:
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[Error]- {message}");
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                default:
                    Console.WriteLine($"[Trace]- {message}");
                    break;
            }
        }
    }
}
