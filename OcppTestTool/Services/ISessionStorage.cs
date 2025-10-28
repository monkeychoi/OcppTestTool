using OcppTestTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcppTestTool.Services
{
    public interface ISessionStorage
    {
        Task SaveAsync(AuthUser user);
        Task<AuthUser?> LoadAsync();
        Task ClearAsync();
    }
}
