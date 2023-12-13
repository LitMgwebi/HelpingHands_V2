using Dapper;
using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HelpingHands_V2.Services
{
    public class OperationService: IOperation
    {
        private readonly IConfiguration _config;
        string sql = "CRUDOperationHours";
        public OperationService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<OperationHour>> GetOperationHours()
        {
            using(var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "GetAll");
                var result = await conn.QueryAsync<OperationHour>(sql, param, commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public async Task<OperationHour> GetOperation(int? id)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("OperationHoursId", id);
                param.Add("Command", "GetOne");

                var result = await conn.QuerySingleOrDefaultAsync<OperationHour>(sql, param, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<dynamic> AddOperationHours(OperationHour operation)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("OperationDay", operation.OperationDay);
                    param.Add("OpenTime", operation.OpenTime);
                    param.Add("CloseTime", operation.CloseTime);
                    param.Add("BusinessId", operation.BusinessId);
                    param.Add("Active", operation.Active);
                    param.Add("Command", "Insert");

                    var result = await conn.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            } catch (Exception)
            {
                throw;
            }
        }

        public async Task<dynamic> UpdateOperationHour(OperationHour operation)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("OperationHoursId", operation.OperationHoursId);
                    param.Add("OperationDay", operation.OperationDay);
                    param.Add("OpenTime", operation.OpenTime);
                    param.Add("CloseTime", operation.CloseTime);
                    param.Add("BusinessId", 1);
                    param.Add("Active", operation.Active);
                    param.Add("Command", "Update");

                    var result = await conn.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<dynamic> DeleteOperationHour(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("OperationHoursId", id);
                    param.Add("Command", "Delete");

                    var result = await conn.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception) { throw; }
        }
    }
}
