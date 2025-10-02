namespace Yume.Models.Session
{
    public class SessionDbSettingsModel
    {
        public required string ConnectionString { get; set; }
        public required string Name { get; set; }
        public required string Collection { get; set; }
    }
}
