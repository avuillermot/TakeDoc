﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeDocService.Security.Interface
{
    public interface ICryptoService
    {
        string Encrypt(string plainText);
        string Decrypt(string encryptedText);
    }
}
