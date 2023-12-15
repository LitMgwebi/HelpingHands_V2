namespace HelpingHands_V2.Services
{
    using Dapper;
    using HelpingHands_V2.Interfaces;
    using HelpingHands_V2.Models;
    using Microsoft.Data.SqlClient;
    using System.Data;
    using System.Data.SqlTypes;

    public class PrefferedSuburbService:IPrefferedSuburb
    {
        private readonly IConfiguration _config;
        string sql = "CRUDPrefferedSuburb";
        public PrefferedSuburbService(IConfiguration config) => _config = config;

        public async Task<IEnumerable<PrefferedSuburb>> GetPrefferedSuburbs()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "GetAll");
                var result = await conn.QueryAsync<PrefferedSuburb, EndUser, Suburb, PrefferedSuburb>(
                    sql, 
                    (ps, endUser, suburb) =>
                    {
                        ps.Nurse = endUser;
                        ps.Suburb = suburb;
                        return ps;
                    },
                    splitOn: "UserId, SuburbId",
                    param: param, 
                    commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public async Task<IEnumerable<PrefferedSuburb>> GetPrefferedSuburbsByNurse(int? id)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("NurseId", id);
                    param.Add("Command", "ByNurses");

                    var result = await conn.QueryAsync<PrefferedSuburb, EndUser, Suburb, PrefferedSuburb>(
                     sql,
                     (ps, endUser, suburb) =>
                     {
                         ps.Nurse = endUser;
                         ps.Suburb = suburb;
                         return ps;
                     },
                     splitOn: "UserId, SuburbId",
                     param: param,
                     commandType: CommandType.StoredProcedure);

                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PrefferedSuburb> GetPrefferedSuburb(int? nurseId, int? suburbId)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("NurseId", nurseId);
                param.Add("SuburbId", suburbId);
                param.Add("Command", "GetOne");

                var result = await conn.QueryAsync<PrefferedSuburb, EndUser, Suburb, PrefferedSuburb>(
                    sql,
                    (ps, endUser, suburb) =>
                    {
                        ps.Nurse = endUser;
                        ps.Suburb = suburb;
                        return ps;
                    },
                    splitOn: "UserId, SuburbId",
                    param: param,
                    commandType: CommandType.StoredProcedure);

                var ps = result.SingleOrDefault();

                if(ps != null)
                    return ps;
                else
                    throw new SqlNullValueException("There are no Preferred Suburb in the system with the corresponding ID");
            }
        }

        public async Task<dynamic> AddPrefferedSuburb(PrefferedSuburb prefferedSuburb)
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

                    var result = await conn.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<dynamic> DeletePrefferedSuburb(PrefferedSuburb prefferedSuburb)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("NurseId", prefferedSuburb.NurseId);
                    param.Add("SuburbId", prefferedSuburb.SuburbId);
                    param.Add("Command", "Delete");

                    var result = await conn.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception) { throw; }
        }
    }
}
