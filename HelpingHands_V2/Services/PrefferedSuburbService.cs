namespace HelpingHands_V2.Services
{
    using Dapper;
    using HelpingHands_V2.Interfaces;
    using Microsoft.Data.SqlClient;
    using System.Data;
    public class PrefferedSuburbService:IPrefferedSuburb
    {
        private readonly IConfiguration _config;
        string sql = "CRUDPrefferedSuburb";
        public PrefferedSuburbService(IConfiguration config) => _config = config;

        public List<dynamic> GetPrefferedSuburbs()
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
