using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRJ4.DTOs;
using PRJ4.Models;


namespace PRJ4.Services
{
    public class LogQueryService : ILogQueryService
    {
        private readonly IMongoCollection<Log> _logs;

        public LogQueryService(IMongoDatabase database)
        {
            _logs = database.GetCollection<Log>("Logs"); // Replace with your actual collection name
        }

        // Get all logs
        public async Task<IEnumerable<LogDto>> GetAllLogsAsync()
        {
            var logs = await _logs.Find(_ => true).ToListAsync(); // Get all logs from MongoDB
            return logs.Select(log => new LogDto
            {
                Id = log.Id,
                Level = log.Level,
                Timestamp = log.Timestamp,
                MessageTemplate = log.MessageTemplate,
                Properties = log.Properties,
                Exception = log.Exception
                
            });
        }

        // Get logs by level (Info, Warning, Error, etc.)
        public async Task<IEnumerable<LogDto>> GetLogsByLevelAsync(string level)
        {
            var filter = Builders<Log>.Filter.Eq(log => log.Level, level);
            var logs = await _logs.Find(filter).ToListAsync();
            return logs.Select(log => new LogDto
            {
                Id = log.Id,
                Level = log.Level,
                Timestamp = log.Timestamp,
                MessageTemplate = log.MessageTemplate,
                Properties = log.Properties,
                Exception = log.Exception
            });
        }

        // Get logs by date range
        public async Task<IEnumerable<LogDto>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var filter = Builders<Log>.Filter.And(
                Builders<Log>.Filter.Gte(log => log.Timestamp, startDate),
                Builders<Log>.Filter.Lte(log => log.Timestamp, endDate)
            );
            var logs = await _logs.Find(filter).ToListAsync();
            return logs.Select(log => new LogDto
            {
                Id = log.Id,
                Level = log.Level,
                Timestamp = log.Timestamp,
                MessageTemplate = log.MessageTemplate,
                Properties = log.Properties,
                Exception = log.Exception
            });
        }
        public async Task<IEnumerable<LogDto>> GetLogsByRequest(string request)
        {
            // Define filter to match HTTP methods in Properties.Method
            var methodFilter = Builders<Log>.Filter.In("Properties.Method", new[] { "DELETE", "PUT", "POST", "GET" });

            // Additional filter if specific request method is provided
            if (!string.IsNullOrEmpty(request))
            {
                methodFilter &= Builders<Log>.Filter.Eq("Properties.Method", request.ToUpper());
            }

            try
            {
                // Fetch logs from the collection
                var logs = await _logs.Find(methodFilter).ToListAsync();

                // Map logs to LogDto, accessing Properties safely
                var logDtos = logs.Select(log =>
                {
                    var method = log.Properties.ContainsKey("Method") ? log.Properties["Method"]?.ToString() : null;

                    return new LogDto
                    {
                        Id = log.Id,
                        Method = method,
                        Timestamp = log.Timestamp,
                        MessageTemplate = log.MessageTemplate
                    };
                });

                return logDtos;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching logs", ex);
            }
        }


    }
}
