using Dapper;
using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;

namespace HelpingHands_V2.Services
{
    public class ContractService: IContract
    {
        private readonly IConfiguration _config;
        string sql = "CRUDCareContract";
        public ContractService(IConfiguration config) => _config = config;

        public async Task<IEnumerable<CareContract>> GetContracts()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "GetAll");
                var result = await conn.QueryAsync<CareContract, EndUser, EndUser, Suburb, Wound, CareContract>(
                    sql,
                    (contract, nurse, patient, suburb, wound)=>
                    {
                        contract.Nurse = nurse;
                        contract.Patient = patient;
                        contract.Suburb = suburb;
                        contract.Wound = wound;
                        return contract;
                    },
                    splitOn: "UserId, SuburbId, WoundId",
                    param: param,
                    commandType:  CommandType.StoredProcedure);

                return result;
            }
        }

        public async Task<CareContract> GetContract(int? id)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("ContractId", id);
                param.Add("Command", "GetOne");

                var results = await conn.QueryAsync<CareContract, EndUser, EndUser, Suburb, Wound, CareContract>(
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

                var contract = results.SingleOrDefault();

                if(contract != null)
                    return contract;
                else
                    throw new SqlNullValueException("There are no Contract in the system with the corresponding ID");
            }
        }

        public async Task<dynamic> AddContract(CareContract contract)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("ContractStatus", contract.ContractStatus);
                    param.Add("ContractDate", contract.ContractDate);
                    param.Add("PatientId", contract.PatientId);
                    param.Add("NurseId", contract.NurseId);
                    param.Add("WoundId", contract.WoundId);
                    param.Add("AddressLineOne", contract.AddressLineOne);
                    param.Add("AddressLineTwo", contract.AddressLineTwo);
                    param.Add("SuburbId", contract.SuburbId);
                    param.Add("StartDate", contract.StartDate);
                    param.Add("EndDate", contract.EndDate);
                    param.Add("ContractComment", contract.ContractComment);
                    param.Add("Active", contract.Active);
                    param.Add("Command", "Insert");

                    var result = await conn.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<dynamic> UpdateContract(CareContract contract)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("ContractId", contract.ContractId);
                    param.Add("ContractStatus", contract.ContractStatus);
                    param.Add("ContractDate", contract.ContractDate);
                    param.Add("PatientId", contract.PatientId);
                    param.Add("NurseId", contract.NurseId);
                    param.Add("WoundId", contract.WoundId);
                    param.Add("AddressLineOne", contract.AddressLineOne);
                    param.Add("AddressLineTwo", contract.AddressLineTwo);
                    param.Add("SuburbId", contract.SuburbId);
                    param.Add("StartDate", contract.StartDate);
                    param.Add("EndDate", contract.EndDate);
                    param.Add("ContractComment", contract.ContractComment);
                    param.Add("Active", contract.Active);
                    param.Add("Command", "Update");

                    var result = await conn.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<dynamic> DeleteContract(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("ContractId", id);
                    param.Add("Command", "Delete");

                    var result = await conn.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception) { throw; }
        }
    }
}
