using Avaliacao2BimLP3.Database;
using Avaliacao2BimLP3.Models;
using Microsoft.Data.Sqlite;
using Dapper;

namespace Avaliacao2BimLP3.Repositories;

class StudentRepository
{
    private readonly DatabaseConfig _databaseConfig;

    public StudentRepository(DatabaseConfig databaseConfig)
    {
        _databaseConfig = databaseConfig;
    }

    public Student Save(Student student)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();

        connection.Execute("INSERT INTO Students VALUES(@Registration, @Name, @City, @Former)",
student);

        return student;
    }

    public void Delete(string id)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();

        connection.Execute("DELETE FROM Students WHERE registration == @Id", new {Id = id});
    }

    public void MarkAsFormed(string id)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();

        connection.Execute("UPDATE Students SET former = True  WHERE registration == @Id", new {Id = id});
    }

    public List<Student> GetAll()
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();

        var students =  connection.Query<Student>("SELECT * FROM Students").ToList();
        
        
        return students;
    }

    public List<Student> GetAllFormed()
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();

        var students =  connection.Query<Student>("SELECT * FROM Students WHERE former == True").ToList();
        
        
        return students;
    }

    public List<Student> GetAllStudentsByCity(string city)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();

        var students =  connection.Query<Student>("SELECT * FROM Students WHERE city LIKE @City", new {City = city + "%"}).ToList();
        
        return students;
    }

    public List<Student> GetAllByCities(string[] cities)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();
        var students = new List<Student>();

        foreach (var city in cities)
        {
            students.AddRange(connection.Query<Student>("SELECT * FROM Students WHERE city == @City", new {City = city}).ToList());
        }

        return students;
    }


    public List<CountStudentGroupByAttribute> CountByCities()
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();

        var countByCities =  connection.Query<CountStudentGroupByAttribute>("SELECT city AS attributeName, count(registration) AS studentNumber FROM Students GROUP BY city").ToList();
        
        
        return countByCities;
    }

    public List<CountStudentGroupByAttribute> CountByFormed()
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();

        var countByFormed =  connection.Query<CountStudentGroupByAttribute>("SELECT former AS attributeName, count(registration) AS studentNumber FROM Students GROUP BY former").ToList();
        
        
        return countByFormed;
    }

    public bool ExistsById(string id)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();
   
        bool result = connection.ExecuteScalar<bool>("SELECT count(registration) FROM Students WHERE registration = @Id", new {Id = id});

        return result;
    }

    public bool Exists()
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();
   
        bool result = connection.ExecuteScalar<bool>("SELECT count(registration) FROM Students");

        return result;
    }

}
