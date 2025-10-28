using OcppTestTool.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OcppTestTool.Services
{
    public sealed class FileSessionStorage : ISessionStorage
    {
        private static string Dir =>
            Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData),
                "OcppTestTool");
        private static string PathFile => System.IO.Path.Combine(Dir, "session.json");

        public async Task SaveAsync(AuthUser user)
        {
            Directory.CreateDirectory(Dir);
            var json = JsonSerializer.Serialize(user);
            await File.WriteAllTextAsync(PathFile, json);
        }

        public async Task<AuthUser?> LoadAsync()
        {
            if (!File.Exists(PathFile)) return null;
            var json = await File.ReadAllTextAsync(PathFile);
            return JsonSerializer.Deserialize<AuthUser>(json);
        }

        public Task ClearAsync()
        {
            if (File.Exists(PathFile)) File.Delete(PathFile);
            return Task.CompletedTask;
        }
    }
}
