using Dapper;
using HelpingHands_V2.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HelpingHands_V2.Services
{
    public class BusinessService: IBusiness
    {
        private readonly IConfiguration _config;
        string sql = "CRUDBusinessInformation";
        public BusinessService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<object> GetBusinessInfo()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "GetOne");
                param.Add("BusinessId", "1");
                var result = await conn.QueryFirstOrDefaultAsync(sql, param, commandType: CommandType.StoredProcedure);

                return result;
            }
        }
    }
}
