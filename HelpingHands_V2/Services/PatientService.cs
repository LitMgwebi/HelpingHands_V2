using Dapper;
using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HelpingHands_V2.Services
{
    public class PatientService: IPatient
    {
        private readonly IConfiguration _config;
        string sql = "CRUDPatient";
        public PatientService(IConfiguration config) => _config = config;

        public async Task<IEnumerable<dynamic>> GetPatients()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "GetAll");
                var result = await conn.QueryAsync(sql, param, commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public async Task<object> GetPatient(int? id)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("PatientId", id);
                param.Add("Command", "GetOne");

                var result = await conn.QuerySingleOrDefaultAsync(sql, param, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<dynamic> AddPateint(Patient patient)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("PatientId", patient.PatientId);
                    param.Add("AddressLineOne", patient.AddressLineOne);
                    param.Add("AddressLineTwo", patient.AddressLineTwo);
                    param.Add("SuburbId", patient.SuburbId);
                    param.Add("ICEName", patient.Icename);
                    param.Add("ICENumber", patient.Icenumber);
                    param.Add("AdditionalInfo", patient.AdditionalInfo);
                    param.Add("Active", patient.Active);
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
    }
}
