using HelpingHands_V2.Models;

namespace HelpingHands_V2.Interfaces
{
    public interface IBusiness
    {
        public Task<BusinessInformation> GetBusinessInfo();

        public Task<dynamic> UpdateBusinessInfo(BusinessInformation business);
    }
}
