using HelpingHands_V2.Interfaces;
using HelpingHands_V2.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HelpingHands_V2.Services
{
    public class AccountService: IAccount
    {
        private readonly Grp0444HelpingHandsContext _context;
        public AccountService(Grp0444HelpingHandsContext context) => _context = context;

        public async Task<IEnumerable<EndUsers>> GetUser(string username)
        {
            var param = new SqlParameter("@Username", username);

            var user = await Task.Run(() => _context.EndUsers.FromSqlRaw(@"exec GetUserByUsername @Username", param).ToListAsync());

            return user;
            
        }
    }
}
