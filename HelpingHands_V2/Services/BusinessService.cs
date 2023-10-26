using Dapper;
using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HelpingHands_V2.Services
{
    public class BusinessService: IBusiness
    {
        private readonly IConfiguration _config;
        string sql = "CRUDBusinessInformation";
        public BusinessService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<object> GetBusinessInfo()
        {
            using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("Command", "GetOne");
                param.Add("BusinessId", "1");
                var result = await conn.QueryFirstOrDefaultAsync(sql, param, commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public async Task<dynamic> UpdateBusinessInfo(BusinessInformation business)
        {
            try
            {
                using (var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("BusinessId", business.BusinessId);
                    param.Add("OrganizationName", business.OrganizationName);
                    param.Add("Nponumber", business.Nponumber);
                    param.Add("AddressLineOne", business.AddressLineOne);
                    param.Add("AddressLineTwo", business.AddressLineTwo);
                    param.Add("SuburbId", business.SuburbId);
                    param.Add("ContactNumber", business.ContactNumber);
                    param.Add("Email", business.Email);
                    param.Add("Active", business.Active);
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
    }
}
