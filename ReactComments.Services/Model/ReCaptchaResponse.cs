namespace ReactComments.Services.Model
{
    public class ReCaptchaResponse
    {
        public bool Success { get; set; }
        public DateTime Challenge_ts { get; set; }
        public string Hostname { get; set; }
        public string[] ErrorCodes { get; set; }
    }
}
