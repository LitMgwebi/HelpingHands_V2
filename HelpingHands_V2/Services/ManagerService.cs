using Dapper;
using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HelpingHands_V2.Services
{
    public class ManagerService: IManager
    {
        private readonly Grp0444HelpingHandsContext _db;
        private readonly IConfiguration _config;

        public ManagerService(Grp0444HelpingHandsContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public List<dynamic> AvailableNurses(int suburbId)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "ManagerAvailableNurses";
                DynamicParameters param = new DynamicParameters();
                param.Add("SuburbId", suburbId);
                var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }

        public List<dynamic> CareVisits(DateTime startDate, DateTime endDate)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "ManagerCareVisits";
                DynamicParameters param = new DynamicParameters();
                param.Add("StartDate", startDate);
                param.Add("EndDate", endDate);
                var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }

        public List<dynamic> ContractStatus(string status)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "ManagerContractStatus";
                DynamicParameters param = new DynamicParameters();
                param.Add("Status", status);
                var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }

        public List<dynamic> ContractVisits(int contractId)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "ManagerContractVisits";
                DynamicParameters param = new DynamicParameters();
                param.Add("ContractId", contractId);
                var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }

        public List<dynamic> PatientContract(int patientId)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "ManagerPatientContract";
                DynamicParameters param = new DynamicParameters();
                param.Add("PatientId", patientId);
                var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }
    }
}
