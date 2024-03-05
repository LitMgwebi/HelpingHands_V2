using Dapper;
using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;

namespace HelpingHands_V2.Services
{
    public class PatientService: IPatient
    {
        private readonly IConfiguration _config;
        private readonly IEndUser _endUser;
        string sql = "CRUDPatient";
        public PatientService(IConfiguration config, IEndUser endUser) 
        { 
            _config = config;
            _endUser = endUser;
        }

        public async Task<List<Patient>> GetPatients()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "GetAll");
                var result = await conn.QueryAsync<Patient, EndUser, Suburb, Patient>(
                    sql,
                    (patient, endUser, suburb) =>
                    {
                        patient.EndUser = endUser;
                        patient.Suburb = suburb;
                        return patient;
                    },
                    splitOn: "UserId, SuburbId",
                    param: param, 
                    commandType: CommandType.StoredProcedure);

                if (result != null)
                    return result.ToList();
                else
                    throw new SqlNullValueException("There are no Patients in the system");
            }
        }

        public async Task<List<EndUser>> GetUsersByIDs(List<Patient> Patients)
        {
            List<EndUser> users = new List<EndUser> { };
            foreach (Patient patient in Patients)
            {
                var user = await _endUser.GetUserById(patient.PatientId);
                users.Add(user);
            }
            return users;
        }

        public async Task<Patient> GetPatient(int? id)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("PatientId", id);
                param.Add("Command", "GetOne");

                var results = await conn.QueryAsync<Patient, EndUser, Suburb, Patient>(
                    sql, 
                    (patient, endUser, suburb) =>
                    {
                        patient.EndUser = endUser;
                        patient.Suburb = suburb;
                        return patient;
                    },
                    splitOn: "UserId, SuburbId",
                    param: param, 
                    commandType: CommandType.StoredProcedure);

                var patient = results.SingleOrDefault();

                if (patient != null)
                    return patient;
                else
                    throw new SqlNullValueException("There are no Nurses in the system with the corresponding ID");
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

        public async Task<dynamic> UpdatePatient(Patient patient)
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

        public async Task<dynamic> DeletePatient(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("PatientId", id);
                    param.Add("Command", "Delete");

                    var result = await conn.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception) { throw; }
        }
    }
}
