using Dapper;
using HelpingHands_V2.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HelpingHands_V2.Services
{
    public class CityService: ICity
    {
        private readonly IConfiguration _config;
        public CityService(IConfiguration config ) => _config = config;

        public List<dynamic> GetCities()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "CRUDCity";
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "GetAll");
                var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }
    }
}
