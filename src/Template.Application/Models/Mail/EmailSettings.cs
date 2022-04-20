namespace Template.Application.Model.Mail
{
    public class EmailSettings
    {
        public string ApiKey { get; set; }

        public string FromAddress { get; set; }

        public string FromName { get; set; }
        public bool UseDevServer { get; set; }
    }
}
