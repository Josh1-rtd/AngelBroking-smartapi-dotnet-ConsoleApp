using Newtonsoft.Json;
using OtpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AngelBroking
{
    internal class Loginlogic
    {
        public static void loginAngel(string username, string password)
        {
            byte[] array = Base32Encoding.ToBytes("527SUTY4DASTR6KTHXJUQLZWH4"); // convert string to byte array    
            var totp = new Totp(array, step: 30);
            var totpCode = totp.ComputeTotp(); Console.WriteLine("Logging in");
 
        }




    }
}
