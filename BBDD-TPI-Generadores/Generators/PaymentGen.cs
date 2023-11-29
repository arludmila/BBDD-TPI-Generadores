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
    public static class PaymentGen
    {
        public static void Generate()
        {
            var random = new Random();
            var faker = new Faker();
            string[] paymentBackupDocumentTypes = {
        "Receipt",
        "Bank Transfer",
        "Credit Memo",
        "Deposit Receipt",
    };

            using (var connection = new MySqlConnection(ConnectionString.ConnString))
            {
                var reservationsIds = connection.Query<int>("SELECT id FROM reservations").ToArray();
                foreach (var reservationId in reservationsIds)
                {
                    // Check if a payment already exists for the reservation
                    var existingPaymentQuery = "SELECT COUNT(*) FROM payments WHERE reservation_id = @reservationId";
                    var existingPaymentCount = connection.QuerySingle<int>(existingPaymentQuery, new { reservationId });

                    if (existingPaymentCount > 0)
                    {
                        Console.WriteLine($"Payment already exists for reservation {reservationId}. Skipping...");
                        continue; // Skip to the next reservation
                    }

                    string docType = paymentBackupDocumentTypes[random.Next(paymentBackupDocumentTypes.Length)];
                    int docNum = random.Next(10000, 150000);
                    string paymentType;

                    // TODO: ??? random o na? service fee?
                    double debt = 0;
                    double serviceFee = 0.15;
                    //

                    switch (docType)
                    {
                        case "Receipt":
                            paymentType = "Cash Payment";
                            break;
                        case "Bank Transfer":
                            paymentType = "Bank Transfer";
                            break;
                        case "Credit Memo":
                            paymentType = "Credit Card Payment";
                            break;
                        case "Deposit Receipt":
                            paymentType = "Deposit";
                            break;

                        default:
                            paymentType = "Unknown";
                            break;
                    }

                    var reservationTotalAmountQuery = "SELECT total_amount FROM reservations WHERE id = @reservationId";

                    var reservationTotalAmount = connection.Query<double>(reservationTotalAmountQuery, new { reservationId }).FirstOrDefault();

                    try
                    {
                        var insertQuery = "INSERT INTO payments (doc_type , doc_id , payment_type, payed_amount , debt , service_fee , reservation_id ) " +
                                          "VALUES (@docType, @docNum, @paymentType, @reservationTotalAmount, @debt, @serviceFee, @reservationId)";

                        int rowsAffected = connection.Execute(insertQuery, new
                        {
                            docType,
                            docNum,
                            paymentType,
                            reservationTotalAmount,
                            debt,
                            serviceFee,
                            reservationId
                        });

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"{rowsAffected} rows inserted successfully for reservation {reservationId}.");
                        }
                        else
                        {
                            Console.WriteLine($"Insertion failed or no rows were inserted for reservation {reservationId}.");
                        }
                    }
                    catch (MySqlException ex)
                    {
                        if (ex.Number == 1062) // MySQL error code for duplicate entry
                        {
                            Console.WriteLine($"Duplicate entry for reservation {reservationId}.");
                        }
                        else
                        {
                            Console.WriteLine($"An error occurred for reservation {reservationId}: {ex.Message}");
                        }
                    }
                }
            }
        }

    }
}
