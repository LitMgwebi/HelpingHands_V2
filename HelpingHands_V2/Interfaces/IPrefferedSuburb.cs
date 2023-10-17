namespace HelpingHands_V2.Interfaces
{
    public interface IPrefferedSuburb
    {
        public List<dynamic> GetPrefferedSuburbs();

        public dynamic GetPrefferedSuburbByNurse(int? id);

        public dynamic GetPrefferedSuburbBySuburb(int? id);
    }
}
