namespace EB.Services
{
    // 📁 File: Services/ExcelWatcherService.cs
    using System.IO;
    using EB.Services;

    public class ExcelWatcherService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private FileSystemWatcher? _watcher;
        private readonly string _watchPath;

        public ExcelWatcherService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _watchPath = configuration["HotFolderSettings:Path"] ?? throw new ArgumentNullException("HotFolderSettings:Path");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!Directory.Exists(_watchPath))
                Directory.CreateDirectory(_watchPath);

            _watcher = new FileSystemWatcher(_watchPath, "*.xlsx");
            _watcher.Created += OnFileChanged;
            _watcher.Changed += OnFileChanged;
            _watcher.EnableRaisingEvents = true;

            Log($"Watching hot folder: {_watchPath}");
            return Task.CompletedTask;
        }

        private async void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                await Task.Delay(1000); // wait for copy to finish
                using var scope = _serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IBankHoursService>();

                await service.ImportFromExcelFromDiskAsync(e.FullPath);
                Log($"✅ Imported Excel: {Path.GetFileName(e.FullPath)}");
            }
            catch (Exception ex)
            {
                Log($"❌ Error importing {e.FullPath}: {ex.Message}");
            }
        }

        private void Log(string message)
        {
            var logFile = Path.Combine(_watchPath, "import-log.txt");
            File.AppendAllText(logFile, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}");
        }

        public override void Dispose()
        {
            base.Dispose();
            _watcher?.Dispose();
        }
    }

}
