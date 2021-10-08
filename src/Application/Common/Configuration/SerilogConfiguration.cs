namespace Application.Common.Configuration
{
    public class SerilogConfiguration
    {
        public bool NeedWriteLogToConsole { get; set; }
        public bool NeedWriteLogToFile { get; set; }
        public bool NeedWriteLogToElasticSearch { get; set; }
        public ElasticConfiguration ElasticConfiguration { get; set; }
    }
    public class ElasticConfiguration
    {
        public string Uri { get; set; }
    }
}