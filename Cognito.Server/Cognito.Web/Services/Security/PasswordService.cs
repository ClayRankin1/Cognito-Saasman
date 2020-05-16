using Cognito.Web.Services.Security.Abstract;
using System;

namespace Cognito.Web.Services.Security
{
    public class PasswordService : IPasswordService
    {
        private const string ValidCharacters = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";

        public string GetRandomPassword(int length = 15)
        {
            var random = new Random(); 
            var chars = new char[length];

            for (int i = 0; i < length; i++)
            {
                chars[i] = ValidCharacters[random.Next(0, ValidCharacters.Length)];
            }

            return new string(chars);
        }
    }
}
