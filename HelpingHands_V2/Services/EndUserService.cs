using Dapper;
using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using BC = BCrypt.Net.BCrypt;
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


        public async Task<IEnumerable<dynamic>> GetManagers()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "CRUDUser";
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "Managers");
                var result = await conn.QueryAsync(sql, param, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<IEnumerable<dynamic>> GetUsers()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "CRUDUser";
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "GetAll");
                var result = await conn.QueryAsync(sql, param, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<dynamic> GetUserById(int? id)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "CRUDUser";
                DynamicParameters param = new DynamicParameters();
                param.Add("UserId", id);
                param.Add("Command", "GetOne");

                var result = await conn.QuerySingleOrDefaultAsync(sql, param, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<dynamic> GetUserByUsername(string username)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var sql = "GetUserByUsername";
                DynamicParameters param = new DynamicParameters();
                param.Add("Username", username);

                var user = await conn.QueryFirstOrDefaultAsync(sql, param, commandType: CommandType.StoredProcedure);

                return user;
            }
        }

        public async Task<dynamic> AddUser(EndUser user)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    var sql = "CRUDUser";
                    DynamicParameters param = new DynamicParameters();
                    param.Add("Username", user.Username);
                    param.Add("Firstname", user.Firstname);
                    param.Add("Lastname", user.Lastname);
                    param.Add("DateOfBirth", user.DateOfBirth);
                    param.Add("Email", user.Email);
                    param.Add("Password", user.Password);
                    //param.Add("Password", BC.HashPassword(user.Password));
                    param.Add("Gender", user.Gender);
                    param.Add("ContactNumber", user.ContactNumber);
                    param.Add("IDNumber", user.Idnumber);
                    param.Add("UserType", user.UserType);
                    param.Add("ApplicationType", user.ApplicationType);
                    //param.Add("ProfilePicture", user.ProfilePicture);
                    //param.Add("ProfilePictureName", user.ProfilePictureName);
                    param.Add("Active", user.Active);
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

        public async Task<dynamic> UpdateUser(dynamic user)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    var sql = "CRUDUser";
                    DynamicParameters param = new DynamicParameters();
                    param.Add("UserId", user.UserId);
                    param.Add("Username", user.Username);
                    param.Add("Firstname", user.Firstname);
                    param.Add("Lastname", user.Lastname);
                    param.Add("DateOfBirth", user.DateOfBirth);
                    param.Add("Email", user.Email);
                    param.Add("Password", user.Password);
                    param.Add("Gender", user.Gender);
                    param.Add("ContactNumber", user.ContactNumber);
                    param.Add("IDNumber", user.Idnumber);
                    param.Add("UserType", user.UserType);
                    param.Add("ApplicationType", user.ApplicationType);
                    //param.Add("ProfilePicture", user.ProfilePicture);
                    //param.Add("ProfilePictureName", user.ProfilePictureName);
                    param.Add("Active", user.Active);
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

        public async Task<dynamic> DeleteUser(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    var sql = "CRUDUser";
                    DynamicParameters param = new DynamicParameters();
                    param.Add("UserId", id);
                    param.Add("Command", "Delete");

                    var result = await conn.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception) { throw; }
        }
    }
}