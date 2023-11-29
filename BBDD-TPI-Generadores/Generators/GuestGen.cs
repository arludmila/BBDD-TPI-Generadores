using Bogus;
using Dapper;
using Microsoft.AspNetCore.Identity;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBDD_TPI_Generadores.Generators
{
    public static class GuestGen
    {
        public static void Generate(int num)
        {
            var random = new Random();
            var faker = new Faker();
            var passwordHasher = new PasswordHasher<string>();
            string[] docTypes = { "DNI", "CUIT", "CUIL", "Passport Number" };

            using (var connection = new MySqlConnection(ConnectionString.ConnString))
            {
                connection.Open();

                for (int i = 0; i < num; i++)
                {
                    var phoneNum = faker.Phone.PhoneNumber();
                    var firstName = faker.Name.FirstName();
                    var lastName = faker.Name.LastName();
                    var docNum = random.Next(10000000, 90000000);
                    var docType = docTypes[random.Next(docTypes.Length)];

                    // Step 1: Insert into the 'people' table
                    var insertPersonQuery = $"INSERT INTO people (doc_type, doc_number, first_name, last_name, phone_number) VALUES (@doc_type, @doc_number, @first_name, @last_name, @phone_number)";

                    int personRowsAffected = connection.Execute(insertPersonQuery, new
                    {
                        doc_type = docType,
                        doc_number = docNum.ToString(),
                        first_name = firstName,
                        last_name = lastName,
                        phone_number = phoneNum
                    });

                    if (personRowsAffected > 0)
                    {
                        Console.WriteLine($"{personRowsAffected} person rows inserted successfully.");

                        var email = faker.Internet.ExampleEmail(firstName, lastName);
                        var passwordHash = passwordHasher.HashPassword(string.Empty, faker.Internet.Password());

                        var insertUserQuery = $"INSERT INTO users (email, password_hash, user_doc_type, user_doc_number) VALUES (@email, @password_hash, @user_doc_type, @user_doc_number)";

                        int userRowsAffected = connection.Execute(insertUserQuery, new
                        {
                            email,
                            password_hash = passwordHash,
                            user_doc_type = docType,
                            user_doc_number = docNum.ToString(),

                        });

                        if (userRowsAffected > 0)
                        {
                            Console.WriteLine($"{userRowsAffected} user rows inserted successfully.");

                            int userId = connection.Query<int>("SELECT id FROM users WHERE user_doc_type = @user_doc_type AND user_doc_number = @user_doc_number", new
                            {
                                user_doc_type = docType,
                                user_doc_number = docNum.ToString()
                            }).FirstOrDefault();

                            var insertGuestQuery = $"INSERT INTO guests (id) VALUES (@id)";
                            int guestRowsAffected = connection.Execute(insertGuestQuery, new { id = userId });

                            if (guestRowsAffected > 0)
                            {
                                Console.WriteLine($"{guestRowsAffected} guest rows inserted successfully.");
                            }
                            else
                            {
                                Console.WriteLine("Guest insertion failed or no rows were inserted.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Employee insertion failed or no rows were inserted.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Person insertion failed or no rows were inserted.");
                        continue; // Skip to the next iteration if person insertion fails
                    }



                }
            }
        }
    }
}
