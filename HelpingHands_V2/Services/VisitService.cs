using Dapper;
using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;

namespace HelpingHands_V2.Services
{
    public class VisitService: IVisit
    {
        private readonly IConfiguration _config;
        string sql = "CRUDVisit";
        public VisitService(IConfiguration config) => _config = config;

        public async Task<List<Visit>> GetVisits()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "GetAll");
                var result = await conn.QueryAsync<Visit>(sql, param: param, commandType: CommandType.StoredProcedure);

                return result.AsList();
            }
        }

        public async Task<Visit> GetVisit(int? id)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("VisitId", id);
                param.Add("Command", "GetOne");

                var visit = await conn.QuerySingleOrDefaultAsync<Visit>(sql, param: param, commandType: CommandType.StoredProcedure);

                if (visit != null)
                    return visit;
                else
                    throw new SqlNullValueException("There is no Visit information with the corresponding ID");
            }
        }

        public async Task<dynamic> AddVisit(Visit visit)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("ContractId", visit.ContractId);
                    param.Add("VisitDate", visit.VisitDate);
                    param.Add("ApproxTime", visit.ApproxTime);
                    param.Add("Arrival", visit.Arrival);
                    param.Add("Departure", visit.Departure);
                    param.Add("WoundCondition", visit.WoundCondition);
                    param.Add("Note", visit.Note);
                    param.Add("Active", visit.Active);
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

        public async Task<dynamic> UpdateVisit(Visit visit)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("VisitId", visit.VisitId);
                    param.Add("ContractId", visit.ContractId);
                    param.Add("VisitDate", visit.VisitDate);
                    param.Add("ApproxTime", visit.ApproxTime);
                    param.Add("Arrival", visit.Arrival);
                    param.Add("Departure", visit.Departure);
                    param.Add("WoundCondition", visit.WoundCondition);
                    param.Add("Note", visit.Note);
                    param.Add("Active", visit.Active);
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

        public async Task<dynamic> DeleteVisit(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("VisitId", id);
                    param.Add("Command", "Delete");

                    var result = await conn.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception) { throw; }
        }
    }
}
