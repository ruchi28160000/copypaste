using System.Text.Json;

namespace PortfolioTrackerApi.Services
{
    public class EmailStoreService
    {
        private const string FilePath = "verifiedEmails.json";

        public List<string> LoadEmails()
        {
            if (!File.Exists(FilePath)) return new List<string>();
            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
        }

        public void SaveEmails(List<string> emails)
        {
            var json = JsonSerializer.Serialize(emails, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }

        public bool IsEmailVerified(string email)
        {
            var emails = LoadEmails();
            return emails.Any(e => e.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public void AddEmail(string email)
        {
            var emails = LoadEmails();
            if (!emails.Contains(email, StringComparer.OrdinalIgnoreCase))
            {
                emails.Add(email);
                SaveEmails(emails); 
            }
        }
    }
}
