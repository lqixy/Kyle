using Kyle.Encrypt.Application.Constructs;
using Kyle.Infrastructure.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Encrypt.Application
{
    public class EncryptAppService : IEncryptAppService
    {
        public string CreatePasswordHash(string password)
        {
            return PasswordHash.CreateHash(password);
        }

        public bool ValidatePasswordHash(string password, string correctHash)
        {
            return PasswordHash.ValidatePassword(password, correctHash);
        }
    }
}
