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
        private readonly IConfiguration _config;
        public ReportService(IConfiguration config) 
        {
            _config = config;
        }

        public async Task<List<dynamic>> NurseAssignedConditions(int NurseId)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {

                var sql = "NurseAssignedConditions";
                DynamicParameters param = new DynamicParameters();
                param.Add("NurseId", NurseId);
                var result = await conn.QueryAsync(sql, param, commandType: CommandType.StoredProcedure);
                return result.AsList();
            }
        }
        public async Task<List<dynamic>> NurseAssignedContracts(int NurseId)
        {
            using(var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "NurseAssignedContracts";
                DynamicParameters param = new DynamicParameters();
                param.Add("NurseId", NurseId);
                var result = await conn.QueryAsync(sql, param, commandType: CommandType.StoredProcedure);
                return result.AsList();
            }
        }
        public async Task<List<dynamic>> NurseContractType(int NurseId, string status)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "NurseContractType";
                DynamicParameters param = new DynamicParameters();
                param.Add("NurseId", NurseId);
                param.Add("Status", status);
                var result = await conn.QueryAsync(sql, param, commandType: CommandType.StoredProcedure);
                return result.AsList();
            }
        }
        public async Task<List<dynamic>> NurseContractVisits(int ContractId)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "NurseContractVisits";
                DynamicParameters param = new DynamicParameters();
                param.Add("ContractId", ContractId);
                var result = await conn.QueryAsync(sql, param, commandType: CommandType.StoredProcedure);
                return result.AsList();
            }
        }
        public async Task<List<dynamic>> NurseVisitRange(int NurseId, DateTime startDate, DateTime endDate)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "NurseVisitRange";
                DynamicParameters param = new DynamicParameters();
                param.Add("NurseId", NurseId);
                param.Add("StartDate", startDate);
                param.Add("EndDate", endDate);
                var result = await conn.QueryAsync(sql, param, commandType: CommandType.StoredProcedure);
                return result.AsList();
            }
        }
        public async Task<List<dynamic>> AvailableNurses(int suburbId)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "ManagerAvailableNurses";
                DynamicParameters param = new DynamicParameters();
                param.Add("SuburbId", suburbId);
                var result = await conn.QueryAsync(sql, param, commandType: CommandType.StoredProcedure);
                return result.AsList();
            }
        }
        public async Task<List<dynamic>> CareVisits(DateTime startDate, DateTime endDate)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "ManagerCareVisits";
                DynamicParameters param = new DynamicParameters();
                param.Add("StartDate", startDate);
                param.Add("EndDate", endDate);
                var result = await conn.QueryAsync(sql, param, commandType: CommandType.StoredProcedure);
                return result.AsList();
            }
        }
        public async Task<List<dynamic>> ContractStatus(string status)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "ManagerContractStatus";
                DynamicParameters param = new DynamicParameters();
                param.Add("Status", status);
                var result = await conn.QueryAsync(sql, param, commandType: CommandType.StoredProcedure);
                return result.AsList();
            }
        }
        public async Task<List<dynamic>> ContractVisits(int contractId)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "ManagerContractVisits";
                DynamicParameters param = new DynamicParameters();
                param.Add("ContractId", contractId);
                var result = await conn.QueryAsync(sql, param, commandType: CommandType.StoredProcedure);
                return result.AsList();
            }
        }
        public async Task<List<dynamic>> PatientContract(int patientId)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "ManagerPatientContract";
                DynamicParameters param = new DynamicParameters();
                param.Add("PatientId", patientId);
                var result = await conn.QueryAsync(sql, param, commandType: CommandType.StoredProcedure);
                return result.AsList();
            }
        }
    }
}
