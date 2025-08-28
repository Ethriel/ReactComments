using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ReactComments.Services.Model;
using System.Text.Json;

namespace ReactComments.Services.Validation.Validators
{
    public class CaptchaValidator
    {
        private readonly HttpClient _httpClient;
        private readonly string _secretKey;

        public CaptchaValidator(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClient = httpClientFactory.CreateClient();
            _secretKey = config["GoogleReCaptcha:SecretKey"]; // store in appsettings.json or env
        }

        public async Task<bool> ValidateAsync(string token, string? userIp = null)
        {
            var url = $"https://www.google.com/recaptcha/api/siteverify?secret={_secretKey}&response={token}";

            if (!string.IsNullOrEmpty(userIp))
                url += $"&remoteip={userIp}";

            var response = await _httpClient.PostAsync(url, null);
            var json = await response.Content.ReadAsStringAsync();

            //var captchaResult = JsonSerializer.Deserialize<ReCaptchaResponse>(json);
            var captchaResult = JsonConvert.DeserializeObject<ReCaptchaResponse>(json);

            return captchaResult?.Success ?? false;
        }
    }
}
