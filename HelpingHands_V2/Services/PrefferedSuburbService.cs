namespace HelpingHands_V2.Services
{
    using Dapper;
    using HelpingHands_V2.Interfaces;
    using HelpingHands_V2.Models;
    using Microsoft.Data.SqlClient;
    using System.Data;
    public class PrefferedSuburbService:IPrefferedSuburb
    {
        private readonly IConfiguration _config;
        string sql = "CRUDPrefferedSuburb";
        public PrefferedSuburbService(IConfiguration config) => _config = config;

        public List<dynamic> GetPrefferedSuburbs()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "GetAll");
                var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();

                return result;
            }
        }

        public List<dynamic> GetPrefferedSuburbsByNurse(int? id)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("NurseId", id);
                    param.Add("Command", "ByNurses");

                    var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public dynamic GetPrefferedSuburb(int? nurseId, int? suburbId)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("NurseId", nurseId);
                param.Add("SuburbId", suburbId);
                param.Add("Command", "GetOne");

                var result = conn.QuerySingleOrDefault(sql, param, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public dynamic GetPrefferedSuburbBySuburb(int? id)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("SuburbId", id);
                param.Add("Command", "GetOne");

                var result = conn.QuerySingleOrDefault(sql, param, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async void AddPrefferedSuburb(PrefferedSuburb prefferedSuburb)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("NurseId", prefferedSuburb.NurseId);
                    param.Add("SuburbId", prefferedSuburb.SuburbId);
                    param.Add("Active", prefferedSuburb.Active);
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
