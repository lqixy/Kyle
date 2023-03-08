// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");


using Kyle.Infrastructure.Secrets;
var pwd = "123456";
var pwdHash = PasswordHash.CreateHash(pwd);
Console.WriteLine(pwdHash);


Console.WriteLine(PasswordHash.ValidatePassword(pwd, pwdHash));



