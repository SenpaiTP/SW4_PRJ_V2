using AutoMapper;
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
        private readonly IMapper _mapper;

        public LogQueryService(IMongoDatabase database, IMapper mapper)
        {
            _logs = database.GetCollection<Log>("logs"); // Replace with your actual collection name
            _mapper = mapper;
        }

        // Get all logs
        public async Task<IEnumerable<LogDto>> GetAllLogsAsync()
        {
            
            var logs = await _logs.Find(_ => true).ToListAsync(); // Get all logs from MongoDB
            return _mapper.Map<IEnumerable<LogDto>>(logs); // Use AutoMapper for mapping
        }

        // Get logs by level (Info, Warning, Error, etc.)
        public async Task<IEnumerable<LogDto>> GetLogsByLevelAsync(string level)
        {
            var filter = Builders<Log>.Filter.Eq(log => log.Level, level);
            var logs = await _logs.Find(filter).ToListAsync();
            return _mapper.Map<IEnumerable<LogDto>>(logs); // Use AutoMapper for mapping
        }

        // Get logs by date range
        public async Task<IEnumerable<LogDto>> GetLogsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var filter = Builders<Log>.Filter.And(
                Builders<Log>.Filter.Gte(log => log.Timestamp, startDate),
                Builders<Log>.Filter.Lte(log => log.Timestamp, endDate)
            );
            var logs = await _logs.Find(filter).ToListAsync();
            return _mapper.Map<IEnumerable<LogDto>>(logs); // Use AutoMapper for mapping
        }

        public async Task<IEnumerable<LogDto>> GetLogsByRequest(string request)
        {
            var methodFilter = Builders<Log>.Filter.In("Properties.Method", new[] { "DELETE", "PUT", "POST", "GET" });

            if (!string.IsNullOrEmpty(request))
            {
                methodFilter &= Builders<Log>.Filter.Eq("Properties.Method", request.ToUpper());
            }

            try
            {
                var logs = await _logs.Find(methodFilter).ToListAsync();
                return _mapper.Map<IEnumerable<LogDto>>(logs); // Use AutoMapper for mapping
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching logs", ex);
            }
        }
    }
}
