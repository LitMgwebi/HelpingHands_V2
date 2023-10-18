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

        public List<dynamic> GetOperationHours()
        {
            using(var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "GetAll");
                var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();

                return result;
            }
        }

        public dynamic GetOperation(int? id)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("OperationHoursId", id);
                param.Add("Command", "GetOne");

                var result = conn.QuerySingleOrDefault(sql, param, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async void AddOperationHours(OperationHour operation)
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

                    await conn.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
                }
            } catch (Exception)
            {
                throw;
            }
        }
    }
}
