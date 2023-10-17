namespace HelpingHands_V2.Interfaces
{
    public interface INurse
    {
        public List<dynamic> GetNurses();

        public dynamic GetNurse(int? id);
    }
}
