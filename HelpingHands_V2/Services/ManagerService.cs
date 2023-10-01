using Dapper;
using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HelpingHands_V2.Services
{
    public class ManagerService: IManager
    {
        private readonly IConfiguration _config;

        public ManagerService(IConfiguration config) => _config = config;

        public List<dynamic> GetManagers()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "CRUDUser";
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "Managers");
                var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }
    }
}
