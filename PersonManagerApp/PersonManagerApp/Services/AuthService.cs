using PersonManagerApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PersonManagerApp.Services;

public class AuthService
{
    private string path = "Users.txt";

    public User Login()
    {
        int attempts = 0;

        while (attempts < 3)
        {
            Console.Clear();
            Console.Write("Usuario: ");
            string username = Console.ReadLine();

            Console.Write("Contraseña: ");
            string password = Console.ReadLine();

            var users = File.ReadAllLines(path)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line =>
                {
                    var parts = line.Split(',');
                    return new User
                    {
                        Username = parts[0].Trim(),
                        Password = parts[1].Trim(),
                        IsBlocked = bool.Parse(parts[2].Trim())
                    };
                }).ToList();

            var user = users.FirstOrDefault(x =>
                x.Username == username &&
                x.Password == password);

            if (user != null)
            {
                if (user.IsBlocked)
                {
                    Console.WriteLine("Usuario bloqueado");
                    Console.ReadKey();
                    return null;
                }

                return user;
            }

            attempts++;
            Console.WriteLine($"Credenciales inválidas. Intentos: {attempts}/3");
            Console.ReadKey();

            if (attempts == 3)
            {
                var blockedUser = users.FirstOrDefault(x => x.Username == username);

                if (blockedUser != null)
                {
                    blockedUser.IsBlocked = true;
                    SaveUsers(users);
                }

                Console.WriteLine("Usuario bloqueado");
                Console.ReadKey();
            }
        }

        return null;
    }

    private void SaveUsers(List<User> users)
    {
        List<string> lines = new List<string>();

        foreach (var user in users)
        {
            lines.Add($"{user.Username},{user.Password},{user.IsBlocked}");
        }

        File.WriteAllLines(path, lines);
    }
}