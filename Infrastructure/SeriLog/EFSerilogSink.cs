using Domain.Entities;
using Infrastructure.DatabaseContext;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SeriLog
{
    public class EFSerilogSink : ILogEventSink
    {
        private readonly IServiceProvider _serviceProvider;

        public EFSerilogSink(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Emit(LogEvent logEvent)
        {
            if (logEvent.Level < LogEventLevel.Error)
                return;

            // Only process if it has our custom property
            if (!logEvent.Properties.TryGetValue("SourceContext", out var sourceProp) ||
                sourceProp.ToString() != "\"API.Handler.ExceptionHandler\"") 
                return;
                

            var exception = logEvent.Exception;

            string? exceptionMessage = exception?.InnerException?.Message ?? exception?.Message;

            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<LoggingDbContext>();

            var log = new LogException
            {
                Timestamp = logEvent.Timestamp.UtcDateTime,
                Level = logEvent.Level.ToString(),
                Message = logEvent.RenderMessage(),
                Exception = exceptionMessage
            };

            db.Exceptions.Add(log);
            db.SaveChanges();
        }
    }
}
