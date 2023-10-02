using Dapper;
using HelpingHands_V2.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HelpingHands_V2.Services
{
    public class VisitService: IVisit
    {
        private readonly IConfiguration _config;
        string sql = "CRUDVisit";
        public VisitService(IConfiguration config) => _config = config;

        public List<dynamic> GetVisits()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "GetAll");
                var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();

                return result;
            }
        }
    }
}
