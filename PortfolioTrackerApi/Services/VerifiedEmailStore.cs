using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;

namespace PortfolioTrackerApi.Services
{
    public static class VerifiedEmailStore
    {
        private static readonly string FilePath =
    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "verified_emails.json");
       

        public static HashSet<string> LoadVerifiedEmails()
        {
            Lock.EnterReadLock();
            try
            {
                if (!File.Exists(FilePath))
                    return new HashSet<string>();

                var json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<HashSet<string>>(json) ?? new HashSet<string>();
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        public static void SaveVerifiedEmail(string email)
        {
            Lock.EnterWriteLock();
            try
            {
                HashSet<string> emails;

                if (File.Exists(FilePath))
                {
                    var json = File.ReadAllText(FilePath);
                    emails = JsonSerializer.Deserialize<HashSet<string>>(json) ?? new HashSet<string>();
                }
                else
                {
                    emails = new HashSet<string>();
                }

                if (emails.Add(email.ToLowerInvariant()))
                {
                    var json = JsonSerializer.Serialize(emails, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(FilePath, json);
                }
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        public static bool IsEmailVerified(string email)
        {
            Lock.EnterReadLock();
            try
            {
                if (!File.Exists(FilePath))
                    return false;

                var json = File.ReadAllText(FilePath);
                var emails = JsonSerializer.Deserialize<HashSet<string>>(json) ?? new HashSet<string>();
                return emails.Contains(email.ToLowerInvariant());
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }
    }
}
