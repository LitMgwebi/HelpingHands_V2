using Dapper;
using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HelpingHands_V2.Services
{
    public class PatientConditionService: IPatientCondition
    {
        private readonly IConfiguration _config;
        string sql = "CRUDPatientCondition";
        public PatientConditionService(IConfiguration config) => _config = config;

        public List<dynamic> GetPatientConditions()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "GetAll");
                var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();

                return result;
            }
        }

        public List<dynamic> GetPatientConditionsByPatient(int? id)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("PatientId", id);
                param.Add("Command", "ByPatients");

                var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }

        public dynamic GetOnePatientConditionByPatient(int? id)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("PatientId", id);
                param.Add("Command", "ByPatients");

                var result = conn.QuerySingleOrDefault(sql, param, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public dynamic GetPatientConditionByCondition(int? id)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("ConditionId", id);
                param.Add("Command", "GetOne");

                var result = conn.QuerySingleOrDefault(sql, param, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async void AddPatientCondition(PatientCondition patientCondition)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("PatientId", patientCondition.PatientId);
                    param.Add("ConditionId", patientCondition.ConditionId);
                    param.Add("Active", patientCondition.Active);
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
