using Dapper;
using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HelpingHands_V2.Services
{
    public class CityService: ICity
    {
        private readonly IConfiguration _config;
        string sql = "CRUDCity";
        public CityService(IConfiguration config ) => _config = config;

        public async Task<IEnumerable<dynamic>> GetCities()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "GetAll");
                var result = await conn.QueryAsync(sql, param, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<dynamic> GetCity(int? id)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("CityId", id);
                param.Add("Command", "GetOne");

                var result = await conn.QuerySingleOrDefaultAsync(sql, param, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<dynamic> AddCity(City city)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("CityName", city.CityName);
                    param.Add("CityAbbreviation", city.CityAbbreviation);
                    param.Add("Active", city.Active);
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

        public async Task<dynamic> UpdateCity(dynamic city)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("CityId", city.CityId);
                    param.Add("CityName", city.CityName);
                    param.Add("CityAbbreviation", city.CityAbbreviation);
                    param.Add("Active", city.Active);
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

        public async Task<dynamic> DeleteCity(int id)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("CityId", id);
                param.Add("Command", "Delete");

                var result = await conn.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
                return result;
            }
        }
    }
}
