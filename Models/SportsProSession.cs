namespace SportsPro.Models
{
    public class SportsProSession
    {
        private const string NameKey = "name";

        private ISession session { get; set; }
        public SportsProSession(ISession session) => this.session = session;

        public void SetName(string Name)
        {
            if (string.IsNullOrEmpty(Name))
            {
                session.Remove(NameKey);
            }
            else
            {
                session.SetString(NameKey, Name);
            }
        }
        public string GetName() => session.GetString(NameKey) ?? string.Empty;

    }
}
