using Dapper;
using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;

namespace HelpingHands_V2.Services
{
    public class SuburbService: ISuburb
    {
        private readonly IConfiguration _config;
        string sql = "CRUDSuburb";
        public SuburbService(IConfiguration config) => _config = config;

        public async Task<IEnumerable<Suburb>> GetSuburbs()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "GetAll");

                var suburbs = await conn.QueryAsync<Suburb, City, Suburb>(sql, (suburb, city) =>
                {
                    suburb.City = city;
                    suburb.CityId = city.CityId;
                    return suburb;
                }, splitOn: "CityId", param: param, commandType: CommandType.StoredProcedure);

                if (suburbs != null)
                    return suburbs;
                else
                    throw new SqlNullValueException("There are no suburbs available");
            }
        }

        public async Task<Suburb> GetSuburb(int? id)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("SuburbId", id);
                param.Add("Command", "GetOne");

                var result = await conn.QueryAsync<Suburb, City, Suburb>(sql, (suburb, city) =>
                {
                    suburb.City = city;
                    suburb.CityId = city.CityId;
                    return suburb;
                }, splitOn: "CityId", param: param, commandType: CommandType.StoredProcedure);

                var suburb = result.FirstOrDefault();

                if (suburb != null)
                    return suburb;
                else
                    throw new SqlNullValueException("There is no Suburb information with the corresponding ID");
            }
        }

        public async Task<dynamic> AddSuburb(Suburb suburb)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("SuburbName", suburb.SuburbName);
                    param.Add("PostalCode", suburb.PostalCode);
                    param.Add("CityId", suburb.CityId);
                    param.Add("Active", suburb.Active);
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

        public async Task<dynamic> UpdateSuburb(Suburb suburb)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("SuburbId", suburb.SuburbId);
                    param.Add("SuburbName", suburb.SuburbName);
                    param.Add("PostalCode", suburb.PostalCode);
                    param.Add("CityId", suburb.CityId);
                    param.Add("Active", suburb.Active);
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

        public async Task<dynamic> DeleteSuburb(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("SuburbId", id);
                    param.Add("Command", "Delete");

                    var result = await conn.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception) { throw; }
        }
    }
}
