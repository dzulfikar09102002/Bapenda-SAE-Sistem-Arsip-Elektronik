namespace Sistem_Pemberkasan.Models.Lib
{
    public class errorAPI
    {
        public static async Task LogErrorAsync(string apiUrl, string applicationName, string errorMessage, string stackTrace)
        {
            using (var client = new HttpClient())
            {
                var errorLog = new
                {
                    ApplicationName = applicationName,
                    ErrorMessage = errorMessage,
                    StackTrace = stackTrace,
                    OccuredAt = DateTime.Now,
                    status = 1
                    
                };

                var response = await client.PostAsJsonAsync(apiUrl, errorLog);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
