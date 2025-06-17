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

            var exception = logEvent.Exception;

            var method = exception?.TargetSite;

            string? exceptionMessage = exception?.InnerException?.Message ?? exception?.Message;
            string? className = null;
            string? methodName = null;

            if (exception != null)
            {
                var st = new StackTrace(exception, true);
                var frames = st.GetFrames().Select(f=> f.GetMethod()?.DeclaringType?.Namespace);
                var frame = st.GetFrames()?.FirstOrDefault(f =>
                    f.GetMethod()?.DeclaringType?.Namespace?.StartsWith("API") == true);

                if (frame != null)
                {
                    className = frame.GetMethod()?.DeclaringType?.FullName;
                    methodName = frame.GetMethod()?.Name;
                }
            }

            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<LoggingDbContext>();

            var log = new LogException
            {
                Timestamp = logEvent.Timestamp.UtcDateTime,
                Level = logEvent.Level.ToString(),
                Message = logEvent.RenderMessage(),
                Exception = exceptionMessage,
                SourceClass = className,
                SourceMethod = methodName
            };

            db.Exceptions.Add(log);
            db.SaveChanges();
        }
    }
}
