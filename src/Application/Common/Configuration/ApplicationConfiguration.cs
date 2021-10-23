namespace Application.Common.Configuration
{
    public class ApplicationConfiguration
    {
        public int DefaultPageSize { get; set; }
        public SerilogConfiguration SerilogConfiguration { get; set; }
        public JwtConfiguration JwtConfiguration { get; set; }
        public IdentityConfiguration IdentityConfiguration { get; set; }
    }
}