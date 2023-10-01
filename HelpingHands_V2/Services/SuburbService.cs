using Dapper;
using HelpingHands_V2.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HelpingHands_V2.Services
{
    public class SuburbService: ISuburb
    {
        private readonly IConfiguration _config;
        public SuburbService(IConfiguration config) => _config = config;

        public List<dynamic> GetSuburbs()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "CRUDSuburb";
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "GetAll");
                var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }
    }
}
