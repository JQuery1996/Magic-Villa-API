namespace Learning.CoreApi.Logger {
    public interface ILogging {
        public void Log(LogLevel level, string message);
    }
}
