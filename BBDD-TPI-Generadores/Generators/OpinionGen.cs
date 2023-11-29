using Bogus;
using Dapper;
using MySqlConnector;

namespace BBDD_TPI_Generadores.Generators
{
    public static class OpinionGen
    {
        public static void Generate()
        {
            var random = new Random();
            var faker = new Faker();

            using (var connection = new MySqlConnection(ConnectionString.ConnString))
            {
                var reservationsIds = connection.Query<int>("SELECT id FROM reservations").ToArray();
                foreach (var reservationId in reservationsIds)
                {
                    string guestComment = faker.Lorem.Sentence(40);
                    double rating = Math.Round(random.NextDouble() * (10 - 1) + 1, 1, MidpointRounding.AwayFromZero);


                    try
                    {
                        var insertQuery = "INSERT INTO opinions (guest_comment, rating, reservation_id) " +
                                          "VALUES (@guestComment, @rating, @reservationId)";

                        int rowsAffected = connection.Execute(insertQuery, new
                        {
                            guestComment,
                            rating,
                            reservationId,
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
                            Console.WriteLine("Opinion for reservation already added.");
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
