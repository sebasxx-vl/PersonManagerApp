using PersonManagerApp.Services;

AuthService authService = new AuthService();

var user = authService.Login();

if (user == null)
    return;

PersonService personService = new PersonService();

LogService logService = new LogService();

while (true)
{
    Console.Clear();

    Console.WriteLine("=================================");
    Console.WriteLine("1. Mostrar personas");
    Console.WriteLine("2. Agregar persona");
    Console.WriteLine("3. Editar persona");
    Console.WriteLine("4. Eliminar persona");
    Console.WriteLine("5. Reporte por ciudad");
    Console.WriteLine("0. Salir");

    Console.Write("Seleccione una opción: ");

    string option = Console.ReadLine();

    Console.Clear();

    switch (option)
    {
        case "1":
            personService.ShowPersons();
            logService.WriteLog(user.Username, "Show persons");
            break;

        case "2":
            personService.AddPerson();
            logService.WriteLog(user.Username, "Add person");
            break;

        case "3":
            personService.EditPerson();
            logService.WriteLog(user.Username, "Edit person");
            break;

        case "4":
            personService.DeletePerson();
            logService.WriteLog(user.Username, "Delete person");
            break;

        case "5":
            personService.CityReport();
            logService.WriteLog(user.Username, "City report");
            break;

        case "0":
            return;
    }

    Console.WriteLine();
    Console.WriteLine("Presione cualquier tecla...");
    Console.ReadKey();
}