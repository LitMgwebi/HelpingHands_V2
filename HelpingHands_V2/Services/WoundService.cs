using Dapper;
using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HelpingHands_V2.Services
{
    public class WoundService: IWound
    {
        private readonly IConfiguration _config;
        string sql = "CRUDWound";
        public WoundService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<Wound>> GetWounds()
        {
            using(var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "GetAll");
                var result = await conn.QueryAsync<Wound>(sql, param, commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public async Task<Wound> GetWound(int? id)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("WoundId", id);
                param.Add("Command", "GetOne");

                var result = await conn.QuerySingleOrDefaultAsync<Wound>(sql, param, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<dynamic> AddWound(Wound wound)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("WoundName", wound.WoundName);
                    param.Add("Grade", wound.Grade);
                    param.Add("WoundDescription", wound.WoundDescription);
                    param.Add("Active", wound.Active);
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

        public async Task<dynamic> UpdateWound(Wound wound)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("WoundId", wound.WoundId);
                    param.Add("WoundName", wound.WoundName);
                    param.Add("Grade", wound.Grade);
                    param.Add("WoundDescription", wound.WoundDescription);
                    param.Add("Active", wound.Active);
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

        public async Task<dynamic> DeleteWound(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("WoundId", id);
                    param.Add("Command", "Delete");

                    var result = await conn.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception) { throw; }
        }
    }
}
