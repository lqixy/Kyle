using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Encrypt.Application.Constructs
{
    public interface IEncryptAppService
    {
        string CreatePasswordHash(string password);
        bool ValidatePasswordHash(string password, string correctHash);
    }
}
