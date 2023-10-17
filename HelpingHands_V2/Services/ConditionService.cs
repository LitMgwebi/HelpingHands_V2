namespace HelpingHands_V2.Services
{
    using Dapper;
    using HelpingHands_V2.Interfaces;
    using Microsoft.Data.SqlClient;
    using System.Data;
    public class ConditionService: ICondition
    {
        private readonly IConfiguration _config;
        string sql = "CRUDCondition";
        public ConditionService(IConfiguration config) => _config = config;

        public List<dynamic> GetConditions()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "GetAll");
                var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();

                return result;
            }
        }

        public dynamic GetCondition(int? id)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("ConditionId", id);
                param.Add("Command", "GetOne");

                var result = conn.QuerySingleOrDefault(sql, param, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
    }
}
