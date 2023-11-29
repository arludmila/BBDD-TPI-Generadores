using Bogus;
using Dapper;
using MySqlConnector;

namespace BBDD_TPI_Generadores.Generators
{
    public class PropertyGen
    {
        public static void Generate(int num)
        {
            var faker = new Faker();
            var random = new Random();
            string[] propertyTypes = ["House", "Apartment", "Room"];

            using (var connection = new MySqlConnection(ConnectionString.ConnString))
            {
                connection.Open();
                var ownersIds = connection.Query<int>("SELECT id FROM owners").ToArray();
                var addressesIds = connection.Query<int>("SELECT id FROM addresses").ToArray();
            

                for (int i = 0; i < num; i++)
                {
                    var images = faker.Internet.Url();
                    int? parking = random.NextDouble() > 0.5 ? (int?)random.Next(1, 5) : null;
                    var propertyType = propertyTypes[random.Next(propertyTypes.Length)];
                    var wifi = random.Next(0, 2);
                    int bathrooms = random.Next(1, 5);
                    int guestCapacity = random.Next(1, 10);
                    double dailyPrice = (15000 - 1000) * random.NextDouble() + 1000;
                    var title = faker.Lorem.Sentence(20);
                    var description = faker.Lorem.Paragraph();

                    var ownerId = ownersIds[random.Next(ownersIds.Length)];
                    var addressId = addressesIds[random.Next(addressesIds.Length)];

                    try
                    {
                        var insertQuery = "INSERT INTO properties (owner_id, address_id, images, parking, property_type, wifi, bathrooms, guest_capacity, daily_price, title, description) " +
                                           "VALUES (@ownerId, @addressId, @images, @parking, @propertyType, @wifi, @bathrooms, @guestCapacity, @dailyPrice, @title, @description)";

                        int rowsAffected = connection.Execute(insertQuery, new
                        {
                            ownerId,
                            addressId,
                            images,
                            parking,
                            propertyType,
                            wifi,
                            bathrooms,
                            guestCapacity,
                            dailyPrice,
                            title,
                            description
                        });

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"{rowsAffected} rows inserted successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Insertion failed or no rows were inserted.");
                        }
                    }
                    catch (MySqlException ex)
                    {
                        if (ex.Number == 1062) // MySQL error code for duplicate entry
                        {
                            Console.WriteLine("Address already added.");
                        }
                        else
                        {
                            Console.WriteLine($"An error occurred: {ex.Message}");
                        }
                    }

                }
            }
        }
    }
}
