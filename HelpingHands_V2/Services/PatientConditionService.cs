using Dapper;
using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
    
namespace HelpingHands_V2.Services
{
    public class PatientConditionService: IPatientCondition
    {
        private readonly IConfiguration _config;
        string sql = "CRUDPatientCondition";
        public PatientConditionService(IConfiguration config) => _config = config;

        public async Task<IEnumerable<PatientCondition>> GetPatientConditions()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "GetAll");
                var result = await conn.QueryAsync<PatientCondition, EndUser, Condition, PatientCondition>(
                    sql,
                    (pc, endUser, condition) =>
                    {
                        pc.Patient = endUser;
                        pc.Condition = condition;
                        return pc;
                    },
                    splitOn: "UserId, ConditionId",
                    param: param, 
                    commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public async Task<IEnumerable<PatientCondition>> GetPatientConditionsByPatient(int? id)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("PatientId", id);
                param.Add("Command", "ByPatients");

                var result = await conn.QueryAsync<PatientCondition, EndUser, Condition, PatientCondition>(
                    sql,
                    (pc, endUser, condition) =>
                    {
                        pc.Patient = endUser;
                        pc.Condition = condition;
                        return pc;
                    },
                    splitOn: "UserId, ConditionId",
                    param: param,
                    commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public async Task<PatientCondition> GetOnePatientCondition(int? patientId, int? conditionId)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("PatientId", patientId);
                param.Add("ConditionId", conditionId);
                param.Add("Command", "GetOne");

                var results = await conn.QueryAsync<PatientCondition, EndUser, Condition, PatientCondition>(
                     sql,
                     (pc, endUser, condition) =>
                     {
                         pc.Patient = endUser;
                         pc.Condition = condition;
                         return pc;
                     },
                     splitOn: "UserId, ConditionId",
                     param: param,
                     commandType: CommandType.StoredProcedure);

                var pc = results.SingleOrDefault();

                if(pc != null)
                    return pc;
                else
                    throw new SqlNullValueException("There are no Patient Condition in the system with the corresponding ID");
            }
        }

        public async Task<dynamic> AddPatientCondition(PatientCondition patientCondition)
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

                    var result = await conn.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<dynamic> UpdatePatientCondition(PatientCondition patientCondition)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("PatientId", patientCondition.PatientId);
                    param.Add("ConditionId", patientCondition.ConditionId);
                    param.Add("Active", patientCondition.Active);
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

        public async Task<dynamic> DeletePatientCondition(PatientCondition patientCondition)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("PatientId", patientCondition.PatientId);
                    param.Add("ConditionId", patientCondition.ConditionId);
                    param.Add("Command", "Delete");

                    var result = await conn.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception) { throw; }
        }
    }
}
