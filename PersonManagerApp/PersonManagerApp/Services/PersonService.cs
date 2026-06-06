using PersonManagerApp.Models;
using System.Text.RegularExpressions;

namespace PersonManagerApp.Services
{
    public class PersonService
    {
        private string path = "Persons.txt";

        public List<Person> GetPersons()
        {
            List<Person> persons = new List<Person>();

            if (!File.Exists(path))
                return persons;

            var lines = File.ReadAllLines(path);

            foreach (var line in lines)
            {
                var parts = line.Split(',');

                persons.Add(new Person
                {
                    Id = int.Parse(parts[0]),
                    FirstName = parts[1],
                    LastName = parts[2],
                    Phone = parts[3],
                    City = parts[4],
                    Balance = decimal.Parse(parts[5])
                });
            }

            return persons;
        }

        public void ShowPersons()
        {
            var persons = GetPersons();

            Console.WriteLine("===================================");

            foreach (var person in persons)
            {
                Console.WriteLine($"{person.Id}");
                Console.WriteLine($"   {person.FirstName} {person.LastName}");
                Console.WriteLine($"   Teléfono: {person.Phone}");
                Console.WriteLine($"   Ciudad: {person.City}");
                Console.WriteLine($"   Saldo: ${person.Balance:N2}");
                Console.WriteLine();
            }
        }

        public void AddPerson()
        {
            var persons = GetPersons();

            Console.Write("ID: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("El ID no es válido");
                return;
            }

            if (persons.Any(x => x.Id == id))
            {
                Console.WriteLine("El ID ya existe");
                return;
            }

            Console.Write("Nombres: ");
            string firstName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(firstName))
            {
                Console.WriteLine("Debe ingresar los nombres");
                return;
            }

            Console.Write("Apellidos: ");
            string lastName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(lastName))
            {
                Console.WriteLine("Debe ingresar los apellidos");
                return;
            }

            Console.Write("Teléfono: ");
            string phone = Console.ReadLine();

            if (!Regex.IsMatch(phone, @"^\d{10}$"))
            {
                Console.WriteLine("El teléfono no es válido");
                return;
            }

            Console.Write("Ciudad: ");
            string city = Console.ReadLine();

            Console.Write("Saldo: ");

            if (!decimal.TryParse(Console.ReadLine(), out decimal balance))
            {
                Console.WriteLine("El saldo no es válido");
                return;
            }

            if (balance < 0)
            {
                Console.WriteLine("El saldo debe ser positivo");
                return;
            }

            persons.Add(new Person
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                City = city,
                Balance = balance
            });

            SavePersons(persons);

            Console.WriteLine("Persona agregada correctamente");
        }

        public void EditPerson()
        {
            var persons = GetPersons();

            Console.Write("Ingrese el ID: ");

            int id = int.Parse(Console.ReadLine());

            var person = persons.FirstOrDefault(x => x.Id == id);

            if (person == null)
            {
                Console.WriteLine("Persona no encontrada");
                return;
            }

            Console.WriteLine("Presione ENTER para mantener el valor actual");

            Console.Write($"Nombres ({person.FirstName}): ");
            string firstName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(firstName))
                person.FirstName = firstName;

            Console.Write($"Apellidos ({person.LastName}): ");
            string lastName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(lastName))
                person.LastName = lastName;

            Console.Write($"Teléfono ({person.Phone}): ");
            string phone = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(phone))
                person.Phone = phone;

            Console.Write($"Ciudad ({person.City}): ");
            string city = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(city))
                person.City = city;

            Console.Write($"Saldo ({person.Balance}): ");
            string balanceText = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(balanceText))
                person.Balance = decimal.Parse(balanceText);

            SavePersons(persons);

            Console.WriteLine("Persona actualizada correctamente");
        }

        public void DeletePerson()
        {
            var persons = GetPersons();

            Console.Write("Ingrese el ID: ");

            int id = int.Parse(Console.ReadLine());

            var person = persons.FirstOrDefault(x => x.Id == id);

            if (person == null)
            {
                Console.WriteLine("Persona no encontrada");
                return;
            }

            Console.WriteLine($"{person.FirstName} {person.LastName}");

            Console.Write("¿Desea eliminar esta persona? (S/N): ");

            string answer = Console.ReadLine();

            if (answer.ToUpper() == "S")
            {
                persons.Remove(person);

                SavePersons(persons);

                Console.WriteLine("Persona eliminada correctamente");
            }
        }

        public void CityReport()
        {
            var persons = GetPersons();

            var grouped = persons.GroupBy(x => x.City);

            decimal grandTotal = 0;

            foreach (var city in grouped)
            {
                Console.WriteLine();
                Console.WriteLine($"Ciudad: {city.Key}");
                Console.WriteLine();

                decimal total = 0;

                Console.WriteLine("ID   Nombres   Apellidos   Saldo");
                Console.WriteLine("====================================");

                foreach (var person in city)
                {
                    Console.WriteLine($"{person.Id}   {person.FirstName}   {person.LastName}   ${person.Balance:N2}");

                    total += person.Balance;
                }

                grandTotal += total;

                Console.WriteLine("====================================");
                Console.WriteLine($"Total {city.Key}: ${total:N2}");
            }

            Console.WriteLine();
            Console.WriteLine("====================================");
            Console.WriteLine($"Total General: ${grandTotal:N2}");
        }

        private void SavePersons(List<Person> persons)
        {
            List<string> lines = new List<string>();

            foreach (var person in persons)
            {
                lines.Add($"{person.Id},{person.FirstName},{person.LastName},{person.Phone},{person.City},{person.Balance}");
            }

            File.WriteAllLines(path, lines);
        }
    }
}