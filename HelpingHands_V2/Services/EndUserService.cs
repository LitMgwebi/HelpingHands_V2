﻿using Dapper;
using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Entity;

namespace HelpingHands_V2.Services
{
    public class EndUserService: IEndUser
    {
        private readonly Grp0444HelpingHandsContext _db;
        private readonly IConfiguration _config;
        public EndUserService(Grp0444HelpingHandsContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }


        public EndUser GetUser(string username)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "GetUserByUsername";
                DynamicParameters param = new DynamicParameters();
                param.Add("Username", username);

                var user = conn.QueryFirstOrDefault<EndUser>(sql, param, commandType: CommandType.StoredProcedure);

                return user;
            }
        }

        public List<dynamic> GetManagers()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "CRUDUser";
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "Managers");
                var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }
    }
}