using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;
using System.Configuration;
using System.Diagnostics.Contracts;

namespace HelpingHands_V2.Services
{
    public class ReportService: IReport
    {
        private readonly Grp0444HelpingHandsContext _db;
        private readonly IConfiguration _config;
        public ReportService(Grp0444HelpingHandsContext db, IConfiguration config) 
        {
            _db = db;
            _config = config;
        }

        public List<dynamic> NurseAssignedConditions(int NurseId)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {

                var sql = "NurseAssignedConditions";
                DynamicParameters param = new DynamicParameters();
                param.Add("NurseId", NurseId);
                var result =  conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }

        public List<dynamic> NurseAssignedContracts(int NurseId)
        {
            using(var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "NurseAssignedContracts";
                DynamicParameters param = new DynamicParameters();
                param.Add("NurseId", NurseId);
                var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }

        public List<dynamic> NurseContractType(int NurseId, string status)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "NurseContractType";
                DynamicParameters param = new DynamicParameters();
                param.Add("NurseId", NurseId);
                param.Add("Status", status);
                var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }

        public List<dynamic> NurseContractVisits(int ContractId)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "NurseContractVisits";
                DynamicParameters param = new DynamicParameters();
                param.Add("ContractId", ContractId);
                var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }

        public List<dynamic> NurseVisitRange(int NurseId, DateTime startDate, DateTime endDate)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "NurseVisitRange";
                DynamicParameters param = new DynamicParameters();
                param.Add("NurseId", NurseId);
                param.Add("StartDate", startDate);
                param.Add("EndDate", endDate);
                var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
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
