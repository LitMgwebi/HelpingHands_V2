using Dapper;
using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HelpingHands_V2.Services
{
    public class VisitService: IVisit
    {
        private readonly IConfiguration _config;
        string sql = "CRUDVisit";
        public VisitService(IConfiguration config) => _config = config;

        public List<dynamic> GetVisits()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "GetAll");
                var result = conn.Query(sql, param, commandType: CommandType.StoredProcedure).ToList();

                return result;
            }
        }

        public dynamic GetVisit(int? id)
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("VisitId", id);
                param.Add("Command", "GetOne");

                var result = conn.QuerySingleOrDefault(sql, param, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async void AddVisit(Visit visit)
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
