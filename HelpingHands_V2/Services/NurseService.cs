﻿using Dapper;
using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;

namespace HelpingHands_V2.Services
{
    public class NurseService : INurse
    {
        private readonly IConfiguration _config;
        string sql = "CRUDNurse";
        public NurseService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<Nurse>> GetNurses()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "GetAll");
                var result = await conn.QueryAsync<Nurse, EndUser, Nurse>(
                    sql,
                    (nurse, endUser) =>
                    {
                        nurse.EndUser = endUser;
                        return nurse;
                    },
                    splitOn: "UserId",
                    param: param,
                    commandType: CommandType.StoredProcedure);

                if (result != null)
                    return result;
                else
                    throw new SqlNullValueException("There are no Nurses in the system");
            }
        }

        public async Task<Nurse> GetNurse(int? id)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("NurseId", id);
                param.Add("Command", "GetOne");

                var results = await conn.QueryAsync<Nurse, EndUser, Nurse>(
                    sql,
                    (nurse, endUser) =>
                    {
                        nurse.EndUser = endUser;
                        return nurse;
                    },
                    splitOn: "UserId", 
                    param: param, 
                    commandType: CommandType.StoredProcedure);

                var nurse = results.SingleOrDefault();

                if (nurse != null)
                    return nurse;
                else
                    throw new SqlNullValueException("There are no Nurses in the system with the corresponding ID");
            }
        }

        public async Task<dynamic> AddNurse(Nurse nurse)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("NurseId", nurse.NurseId);
                    param.Add("Grade", nurse.Grade);
                    param.Add("Active", nurse.Active);
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

        public async Task<dynamic> UpdateNurse(Nurse nurse)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("NurseId", nurse.NurseId);
                    param.Add("Grade", nurse.Grade);
                    param.Add("Active", nurse.Active);
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

        public async Task<dynamic> DeleteNurse(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("NurseId", id);
                    param.Add("Command", "Delete");

                    var result = await conn.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception) { throw; }
        }
    }
}
