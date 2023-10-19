using Dapper;
using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HelpingHands_V2.Services
{
    public class ContractService: IContract
    {
        private readonly IConfiguration _config;
        string sql = "CRUDCareContract";
        public ContractService(IConfiguration config) => _config = config;

        public List<dynamic> GetContracts()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "GetAll");
                var result = conn.Query(sql, param, commandType:  CommandType.StoredProcedure).ToList();

                return result;
            }
        }

        public dynamic GetContract(int? id)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("ContractId", id);
                param.Add("Command", "GetOne");

                var result = conn.QuerySingleOrDefault(sql, param, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async void AddContract(CareContract contract)
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

                    await conn.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
