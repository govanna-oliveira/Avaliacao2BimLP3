using Microsoft.Data.Sqlite;
using Avaliacao2BimLP3.Database;
using Avaliacao2BimLP3.Repositories;
using Avaliacao2BimLP3.Models;

var databaseConfig = new DatabaseConfig();
var databaseSetup = new DatabaseSetup(databaseConfig);
var studentRepository = new StudentRepository(databaseConfig);

var modelName = args[0];
var modelAction = args[1];

if(modelName == "Student")
{
    if(modelAction == "New")
    {
        var registration = args[2];
        var name = args[3];
        var city = args[4];

        if (!studentRepository.ExistsById(registration))
        {
            var student = new Student(registration, name, city);
            studentRepository.Save(student);

            Console.WriteLine("Estudante {0} cadastrado com sucesso", name);
        }
        else
        {
            Console.WriteLine("Estudante com registro {0} já cadastrado", registration);
        }
    }

    if (modelAction == "Delete")
    {
        var registration = args[2];

        if (studentRepository.ExistsById(registration))
        {
            studentRepository.Delete(registration);

            Console.WriteLine("Estudante {0} removido com sucesso", registration);
        }
        else
        {
            Console.WriteLine("Estudante {0} não encontrado", registration);
        }
    }

    if(modelAction == "List")
    {
        if (studentRepository.Exists())
        {
            foreach (var student in studentRepository.GetAll())
            {
                if (student.Former)
                {
                    Console.WriteLine("{0}, {1}, {2}, formado", student.Registration, student.Name, student.City);
                }
                else
                {
                    Console.WriteLine("{0}, {1}, {2}, não formado", student.Registration, student.Name, student.City);
                }
            }
        }
        else
        {
            Console.WriteLine("Nenhum estudante cadastrado");
        }
    }

    if (modelAction == "MarkAsFormed")
    {
        var registration = args[2];

        if (studentRepository.ExistsById(registration))
        {
            studentRepository.MarkAsFormed(registration);

            Console.WriteLine("Estudante {0} definido como formado", registration);
        }
        else
        {
            Console.WriteLine("Estudante {0} não encontrado", registration);
        }
    }

    if (modelAction == "ListFormed")
    {
        if (studentRepository.GetAllFormed().Count() > 0)
        {
            foreach (var student in studentRepository.GetAllFormed())
            {
                Console.WriteLine("{0}, {1}, {2}, formado", student.Registration, student.Name, student.City);
            }
        }
        else
        {
            Console.WriteLine("Nenhum estudante cadastrado");
        }
    }

    if(modelAction == "ListByCity")
    {
        var city = args[2];

        if (studentRepository.GetAllStudentsByCity(city).Count() > 0)
        {
            foreach (var student in studentRepository.GetAllStudentsByCity(city))
            {
                if (student.Former)
                {
                    Console.WriteLine("{0}, {1}, {2}, formado", student.Registration, student.Name, student.City);
                }
                else
                {
                    Console.WriteLine("{0}, {1}, {2}, não formado", student.Registration, student.Name, student.City);
                }
            }
        }
        else
        {
            Console.WriteLine("Nenhum estudante cadastrado");
        }
    }

    if(modelAction == "ListByCities")
    {
        string[] cities = new string[args.Count() - 2];

        for (int i = 0; i < args.Count() - 2; i++)
        {
            cities[i] = args[i+2];
        }

        if (studentRepository.GetAllByCities(cities).Count() > 0)
        {
            foreach (var student in studentRepository.GetAllByCities(cities))
            {
                if (student.Former)
                {
                    Console.WriteLine("{0}, {1}, {2}, formado", student.Registration, student.Name, student.City);
                }
                else
                {
                    Console.WriteLine("{0}, {1}, {2}, não formado", student.Registration, student.Name, student.City);
                }
            }
        }
        else
        {
            Console.WriteLine("Nenhum estudante cadastrado");
        }
    }

    if (modelAction == "Report")
    {
        var action = args[2];
        if(studentRepository.Exists())
        {
            if (action == "CountByCities")
            {
                foreach (var city in studentRepository.CountByCities())
                {
                    Console.WriteLine("{0}, {1}", city.AttributeName, city.StudentNumber);
                }
            }

            if (action == "CountByFormed")
            {
                foreach (var formed in studentRepository.CountByFormed())
                {
                    if (formed.AttributeName == "1")
                    {
                        Console.WriteLine("Formados, {0}", formed.StudentNumber);
                    }

                    if (formed.AttributeName == "0")
                    {
                        Console.WriteLine("Não formados, {0}", formed.StudentNumber);
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("Nenhum estudante cadastrado");
        }
    }
}
