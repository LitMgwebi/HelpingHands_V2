using Dapper;
using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HelpingHands_V2.Services
{
    public class NurseService: INurse
    {
        private readonly Grp0444HelpingHandsContext _db;
        private readonly IConfiguration _config;
        public NurseService(Grp0444HelpingHandsContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }
        public List<dynamic> GetNurses()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {

                var sql = "CRUDNurse";
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "GetAll");
                var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }
    }
}
