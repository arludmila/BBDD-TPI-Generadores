using Bogus;
using Dapper;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBDD_TPI_Generadores.Generators
{
    public static class CleaningStaffGen
    {
        public static void Generate(int num)
        {
            var random = new Random();
            var faker = new Faker();
            string[] docTypes = { "DNI", "CUIT", "CUIL", "Passport Number" };
            string[] shifts = ["Day", "Night"];

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
                        // Step 3: Retrieve the IDs of all addresses
                        var addressIds = connection.Query<int>("SELECT id FROM addresses").ToList();

                        // Step 4: Pick a random address ID
                        var randomAddressId = addressIds[random.Next(addressIds.Count)];

                        // Step 5: Insert into the 'employees' table
                        var shift = shifts[random.Next(shifts.Length)];
                        double salary = random.NextDouble() * (100000 - 5000) + 5000;

                        var insertEmployeeQuery = $"INSERT INTO employees (shift, salary, emp_doc_type, emp_doc_number, address_id) VALUES (@shift, @salary, @emp_doc_type, @emp_doc_number, @address_id)";

                        int employeeRowsAffected = connection.Execute(insertEmployeeQuery, new
                        {
                            shift,
                            salary,
                            emp_doc_type = docType,
                            emp_doc_number = docNum.ToString(),
                            address_id = randomAddressId
                        });

                        if (employeeRowsAffected > 0)
                        {
                            Console.WriteLine($"{employeeRowsAffected} employee rows inserted successfully.");

                            int employeeId = connection.Query<int>("SELECT id FROM employees WHERE emp_doc_type = @emp_doc_type AND emp_doc_number = @emp_doc_number", new
                            {
                                emp_doc_type = docType,
                                emp_doc_number = docNum.ToString()
                            }).FirstOrDefault();

                            var insertCleaningStaffQuery = $"INSERT INTO cleaning_staff (id) VALUES (@id)";
                            int cleaningStaffRowsAffected = connection.Execute(insertCleaningStaffQuery, new { id = employeeId });

                            if (cleaningStaffRowsAffected > 0)
                            {
                                Console.WriteLine($"{cleaningStaffRowsAffected} cleaning staff rows inserted successfully.");
                            }
                            else
                            {
                                Console.WriteLine("Cleaning staff insertion failed or no rows were inserted.");
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
