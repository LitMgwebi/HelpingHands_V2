using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;
using System.Configuration;

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

        public List<dynamic> NurseAssignedConditions(int id)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {

                var sql = "NurseAssignedConditions";
                DynamicParameters param = new DynamicParameters();
                param.Add("NurseId", id);
                var result =  conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }

        public List<dynamic> NurseAssignedContracts(int id)
        {
            using(var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "NurseAssignedContracts";
                DynamicParameters param = new DynamicParameters();
                param.Add("NurseId", id);
                var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }

        public List<dynamic> NurseContractType(int id, string status)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "NurseContractType";
                DynamicParameters param = new DynamicParameters();
                param.Add("NurseId", id);
                param.Add("Status", status);
                var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }

        public List<dynamic> NurseContractVisits(int id)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "NurseContractVisits";
                DynamicParameters param = new DynamicParameters();
                param.Add("ContractId", id);
                var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }

        public List<dynamic> NurseVisitRange(int id, DateTime startDate, DateTime endDate)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "NurseVisitRange";
                DynamicParameters param = new DynamicParameters();
                param.Add("NurseId", id);
                param.Add("StartDate", startDate);
                param.Add("EndDate", endDate);
                var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
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
