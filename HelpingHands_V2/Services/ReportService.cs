using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using HelpingHands_V2.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;

namespace HelpingHands_V2.Services
{
    public class ReportService : IReport
    {
        private readonly IConfiguration _config;
        public ReportService(IConfiguration config) => _config = config;

        public async Task<List<AssignedConditionsModel>> NurseAssignedConditions(int? NurseId)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {

                var sql = "NurseAssignedConditions";
                DynamicParameters param = new DynamicParameters();
                param.Add("NurseId", NurseId);
                var result = await conn.QueryAsync<AssignedConditionsModel, CareContract, EndUser, Suburb, PatientCondition, Condition, Wound, AssignedConditionsModel>(
                    sql,
                    (viewModel, contract, patient, suburb, pc, condition, wound) =>
                    {
                        viewModel.Contract = contract;
                        viewModel.Patient = patient;
                        viewModel.Suburb = suburb;
                        viewModel.Condition = condition;
                        viewModel.PatientCondition = pc;
                        viewModel.Wound = wound;
                        return viewModel;
                    },
                    splitOn: "",
                    param: param,
                    commandType: CommandType.StoredProcedure);
                return result.AsList();
            }
        }
        public async Task<List<CareContract>> NurseAssignedContracts(int? NurseId)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "NurseAssignedContracts";
                DynamicParameters param = new DynamicParameters();
                param.Add("NurseId", NurseId);
                var result = await conn.QueryAsync<CareContract, EndUser, Suburb, Wound, CareContract>(
                    sql,
                    (contract, endUser, suburb, wound) =>
                    {
                        contract.Patient = endUser;
                        contract.Suburb = suburb;
                        contract.Wound = wound;
                        return contract;
                    },
                    splitOn: "UserId, SuburbId, WoundId",
                    param: param,
                    commandType: CommandType.StoredProcedure);
                return result.AsList();
            }
        }
        public async Task<List<CareContract>> NurseContractType(int? NurseId, string? status)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "NurseContractType";
                DynamicParameters param = new DynamicParameters();
                param.Add("NurseId", NurseId);
                param.Add("Status", status);
                var result = await conn.QueryAsync<CareContract, EndUser, Suburb, Wound, CareContract>(
                   sql,
                   (contract, endUser, suburb, wound) =>
                   {
                       contract.Patient = endUser;
                       contract.Suburb = suburb;
                       contract.Wound = wound;
                       return contract;
                   },
                   splitOn: "UserId, SuburbId, WoundId",
                   param: param,
                   commandType: CommandType.StoredProcedure);
                return result.AsList();
            }
        }
        public async Task<List<VisitRange>> NurseVisitRange(int? NurseId, DateTime? startDate, DateTime? endDate)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "NurseVisitRange";
                DynamicParameters param = new DynamicParameters();
                param.Add("NurseId", NurseId);
                param.Add("StartDate", startDate);
                param.Add("EndDate", endDate);
                var result = await conn.QueryAsync<VisitRange, EndUser, EndUser, Suburb, Wound, Visit, VisitRange>(
                    sql,
                    (range, nurse, patient, suburb, wound, visit) =>
                    {
                        range.Nurse = nurse;
                        range.Patient = patient;
                        range.Suburb = suburb;
                        range.Wound = wound;
                        range.Visit = visit;
                        return range;
                    },
                    splitOn: "UserId, SuburbId, WoundId, VisitId",
                    param: param,
                    commandType: CommandType.StoredProcedure);
                return result.AsList();
            }
        }
        public async Task<List<CareContract>> NurseContractsByGrades(int? NurseId)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "NurseContractsByGrades";
                DynamicParameters param = new DynamicParameters();
                param.Add("NurseId", NurseId);
                var result = await conn.QueryAsync<CareContract, EndUser, Suburb, Wound, CareContract>(
                   sql,
                   (contract, endUser, suburb, wound) =>
                   {
                       contract.Patient = endUser;
                       contract.Suburb = suburb;
                       contract.Wound = wound;
                       return contract;
                   },
                   splitOn: "UserId, SuburbId, WoundId",
                   param: param,
                   commandType: CommandType.StoredProcedure);
                return result.AsList();
            }
        }
        public async Task<List<dynamic>> AvailableNurses(int? suburbId)
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
        public async Task<List<CareContract>> CareVisits(int? PatientId, DateTime? startDate, DateTime? endDate)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "ManagerCareVisits";
                DynamicParameters param = new DynamicParameters();
                param.Add("PatientId", PatientId);
                param.Add("StartDate", startDate);
                param.Add("EndDate", endDate);
                var result = await conn.QueryAsync<CareContract, EndUser, EndUser, Suburb, Wound, CareContract>(
                   sql,
                   (contract, nurse, patient, suburb, wound) =>
                   {
                       contract.Nurse = nurse;
                       contract.Patient = patient;
                       contract.Suburb = suburb;
                       contract.Wound = wound;
                       return contract;
                   },
                   splitOn: "UserId, SuburbId, WoundId",
                   param: param,
                   commandType: CommandType.StoredProcedure);
                return result.AsList();
            }
        }
        public async Task<List<CareContract>> ContractStatus(string? status)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "ManagerContractStatus";
                DynamicParameters param = new DynamicParameters();
                param.Add("Status", status);
                var result = await conn.QueryAsync<CareContract, EndUser, EndUser, Suburb, Wound, CareContract>(
                   sql,
                   (contract, nurse, patient, suburb, wound) =>
                   {
                       contract.Nurse = nurse;
                       contract.Patient = patient;
                       contract.Suburb = suburb;
                       contract.Wound = wound;
                       return contract;
                   },
                   splitOn: "UserId, SuburbId, WoundId",
                   param: param,
                   commandType: CommandType.StoredProcedure);
                return result.AsList();
            }
        }
        public async Task<List<Visit>> ContractVisits(int? contractId)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "ManagerContractVisits";
                DynamicParameters param = new DynamicParameters();
                param.Add("ContractId", contractId);
                var result = await conn.QueryAsync<Visit, CareContract, Visit>(
                    sql,
                    (visit, contract) =>
                {
                    visit.Contract = contract;
                    return visit;
                },
                    splitOn: "ContractId",
                    param: param,
                    commandType: CommandType.StoredProcedure);

                return result.AsList();
            }
        }
        public async Task<List<CareContract>> PatientContract(int? patientId)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "ManagerPatientContract";
                DynamicParameters param = new DynamicParameters();
                param.Add("PatientId", patientId);
                var result = await conn.QueryAsync<CareContract, EndUser, EndUser, Suburb, Wound, CareContract>(
                    sql,
                    (contract, nurse, patient, suburb, wound) =>
                    {
                        contract.Nurse = nurse;
                        contract.Patient = patient;
                        contract.Suburb = suburb;
                        contract.Wound = wound;
                        return contract;
                    },
                    splitOn: "UserId, SuburbId, WoundId",
                    param: param,
                    commandType: CommandType.StoredProcedure);

                return result.AsList();
            }
        }
    }
}
