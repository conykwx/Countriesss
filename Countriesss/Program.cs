using System;
using Microsoft.Data.SqlClient;

namespace CountryReportApp
{
    class Program
    {
        private static string connectionString = "Server=DESKTOP-9K56BQI\\SQLEXPRESS;Database=Countries;Trusted_Connection=True;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Підключення успішне!");
                    ShowMainMenu(connection);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Помилка підключення: " + ex.Message);
                }
            }
        }

        static void ShowMainMenu(SqlConnection connection)
        {
            while (true)
            {
                Console.WriteLine("1. Відобразити повну інформацію про країни");
                Console.WriteLine("2. Відобразити часткову інформацію про країни");
                Console.WriteLine("3. Відобразити інформацію про конкретну країну");
                Console.WriteLine("4. Вивести Топ-3 столиці з найменшою кількістю жителів");
                Console.WriteLine("5. Додати дані");
                Console.WriteLine("6. Оновити дані");
                Console.WriteLine("7. Видалити дані");
                Console.WriteLine("8. Відобразити Топ-3 країни за найбільшою кількістю жителів");
                Console.WriteLine("9. Відобразити Топ-3 столиці за найбільшою кількістю жителів");
                Console.WriteLine("10. Відобразити країну з найбільшою кількістю мешканців");
                Console.WriteLine("11. Відобразити місто з найбільшою кількістю мешканців");
                Console.WriteLine("12. Вийти");
                Console.Write("Виберіть опцію: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ShowFullCountryInfo(connection);
                        break;
                    case "2":
                        ShowPartialCountryInfo(connection);
                        break;
                    case "3":
                        ShowSpecificCountryInfo(connection);
                        break;
                    case "4":
                        ShowTop3Capitals(connection);
                        break;
                    case "5":
                        AddData(connection);
                        break;
                    case "6":
                        UpdateData(connection);
                        break;
                    case "7":
                        DeleteData(connection);
                        break;
                    case "8":
                        ShowTop3CountriesByPopulation(connection);
                        break;
                    case "9":
                        ShowTop3CapitalsByPopulation(connection);
                        break;
                    case "10":
                        ShowCountryWithMaxPopulation(connection);
                        break;
                    case "11":
                        ShowCityWithMaxPopulation(connection);
                        break;
                    case "12":
                        Console.WriteLine("Вихід...");
                        return;
                    default:
                        Console.WriteLine("Невірний вибір.");
                        break;
                }
            }
        }

        // Відображення повної інформації про країни
        static void ShowFullCountryInfo(SqlConnection connection)
        {
            string query = "SELECT CountryName, Capital, Population, Region FROM CountryInfo";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("Інформація про країни:");
            while (reader.Read())
            {
                Console.WriteLine($"Країна: {reader["CountryName"]}, Столиця: {reader["Capital"]}, Населення: {reader["Population"]}, Регіон: {reader["Region"]}");
            }
            reader.Close();
        }

        // Відображення часткової інформації про країни
        static void ShowPartialCountryInfo(SqlConnection connection)
        {
            Console.Write("Введіть кількість записів для виведення: ");
            int count = int.Parse(Console.ReadLine());

            string query = $"SELECT TOP {count} CountryName, Capital FROM CountryInfo";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("Часткова інформація про країни:");
            while (reader.Read())
            {
                Console.WriteLine($"Країна: {reader["CountryName"]}, Столиця: {reader["Capital"]}");
            }
            reader.Close();
        }

        // Відображення інформації про конкретну країну
        static void ShowSpecificCountryInfo(SqlConnection connection)
        {
            Console.Write("Введіть назву країни: ");
            string countryName = Console.ReadLine();

            string query = "SELECT CountryName, Capital, Population, Region FROM CountryInfo WHERE CountryName = @CountryName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryName", countryName);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Console.WriteLine($"Країна: {reader["CountryName"]}, Столиця: {reader["Capital"]}, Населення: {reader["Population"]}, Регіон: {reader["Region"]}");
            }
            else
            {
                Console.WriteLine("Країна не знайдена.");
            }
            reader.Close();
        }

        // Топ-3 столиці з найменшою кількістю жителів
        static void ShowTop3Capitals(SqlConnection connection)
        {
            string query = "SELECT TOP 3 CapitalName, Population FROM Capitals ORDER BY Population ASC";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("Топ-3 столиці з найменшою кількістю жителів:");
            while (reader.Read())
            {
                Console.WriteLine($"Столиця: {reader["CapitalName"]}, Населення: {reader["Population"]}");
            }
            reader.Close();
        }

        // Топ-3 країни за найбільшою кількістю жителів
        static void ShowTop3CountriesByPopulation(SqlConnection connection)
        {
            string query = "SELECT TOP 3 CountryName, Population FROM CountryInfo ORDER BY Population DESC";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("Топ-3 країни за найбільшою кількістю жителів:");
            while (reader.Read())
            {
                Console.WriteLine($"Країна: {reader["CountryName"]}, Населення: {reader["Population"]}");
            }
            reader.Close();
        }

        // Топ-3 столиці за найбільшою кількістю жителів
        static void ShowTop3CapitalsByPopulation(SqlConnection connection)
        {
            string query = "SELECT TOP 3 CapitalName, Population FROM Capitals ORDER BY Population DESC";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("Топ-3 столиці за найбільшою кількістю жителів:");
            while (reader.Read())
            {
                Console.WriteLine($"Столиця: {reader["CapitalName"]}, Населення: {reader["Population"]}");
            }
            reader.Close();
        }

        // Країна з найбільшою кількістю мешканців
        static void ShowCountryWithMaxPopulation(SqlConnection connection)
        {
            string query = "SELECT TOP 1 CountryName, Population FROM CountryInfo ORDER BY Population DESC";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Console.WriteLine($"Країна з найбільшою кількістю мешканців: {reader["CountryName"]}, Населення: {reader["Population"]}");
            }
            reader.Close();
        }

        // Місто з найбільшою кількістю мешканців
        static void ShowCityWithMaxPopulation(SqlConnection connection)
        {
            string query = "SELECT TOP 1 CityName, Population FROM Cities ORDER BY Population DESC";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Console.WriteLine($"Місто з найбільшою кількістю мешканців: {reader["CityName"]}, Населення: {reader["Population"]}");
            }
            reader.Close();
        }

        // Додавання даних
        static void AddData(SqlConnection connection)
        {
            Console.WriteLine("1. Додати країну");
            Console.WriteLine("2. Додати столицю");
            Console.WriteLine("3. Додати велике місто");
            Console.WriteLine("4. Додати кількість жителів");
            Console.Write("Виберіть опцію: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    AddCountry(connection);
                    break;
                case "2":
                    AddCapital(connection);
                    break;
                case "3":
                    AddCity(connection);
                    break;
                case "4":
                    AddPopulation(connection);
                    break;
                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }
        }

        // Оновлення даних
        static void UpdateData(SqlConnection connection)
        {
            Console.WriteLine("1. Оновити країну");
            Console.WriteLine("2. Оновити столицю");
            Console.WriteLine("3. Оновити велике місто");
            Console.WriteLine("4. Оновити кількість жителів");
            Console.Write("Виберіть опцію: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    UpdateCountry(connection);
                    break;
                case "2":
                    UpdateCapital(connection);
                    break;
                case "3":
                    UpdateCity(connection);
                    break;
                case "4":
                    UpdatePopulation(connection);
                    break;
                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }
        }

        // Видалення даних
        static void DeleteData(SqlConnection connection)
        {
            Console.WriteLine("1. Видалити країну");
            Console.WriteLine("2. Видалити столицю");
            Console.WriteLine("3. Видалити велике місто");
            Console.WriteLine("4. Видалити кількість жителів");
            Console.Write("Виберіть опцію: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    DeleteCountry(connection);
                    break;
                case "2":
                    DeleteCapital(connection);
                    break;
                case "3":
                    DeleteCity(connection);
                    break;
                case "4":
                    DeletePopulation(connection);
                    break;
                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }
        }

        // Реалізація методів додавання, оновлення та видалення для країн, столиць, великих міст і кількості жителів
        static void AddCountry(SqlConnection connection)
        {
            Console.Write("Введіть назву країни: ");
            string countryName = Console.ReadLine();

            Console.Write("Введіть столицю країни: ");
            string capital = Console.ReadLine();

            Console.Write("Введіть кількість жителів: ");
            long population = long.Parse(Console.ReadLine());

            Console.Write("Введіть регіон: ");
            string region = Console.ReadLine();

            string query = "INSERT INTO CountryInfo (CountryName, Capital, Population, Region) VALUES (@CountryName, @Capital, @Population, @Region)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryName", countryName);
            command.Parameters.AddWithValue("@Capital", capital);
            command.Parameters.AddWithValue("@Population", population);
            command.Parameters.AddWithValue("@Region", region);
            command.ExecuteNonQuery();
            Console.WriteLine("Дані успішно додано.");
        }

        static void UpdateCountry(SqlConnection connection)
        {
            Console.Write("Введіть назву країни для оновлення: ");
            string countryName = Console.ReadLine();

            Console.Write("Введіть нову назву країни: ");
            string newCountryName = Console.ReadLine();

            string query = "UPDATE CountryInfo SET CountryName = @NewCountryName WHERE CountryName = @CountryName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryName", countryName);
            command.Parameters.AddWithValue("@NewCountryName", newCountryName);
            command.ExecuteNonQuery();
            Console.WriteLine("Дані успішно оновлено.");
        }

        static void DeleteCountry(SqlConnection connection)
        {
            Console.Write("Введіть назву країни для видалення: ");
            string countryName = Console.ReadLine();

            string query = "DELETE FROM CountryInfo WHERE CountryName = @CountryName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryName", countryName);
            command.ExecuteNonQuery();
            Console.WriteLine("Дані успішно видалено.");
        }

        static void AddCapital(SqlConnection connection)
        {
            Console.Write("Введіть назву столиці: ");
            string capitalName = Console.ReadLine();

            Console.Write("Введіть кількість жителів: ");
            long population = long.Parse(Console.ReadLine());

            string query = "INSERT INTO Capitals (CapitalName, Population) VALUES (@CapitalName, @Population)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CapitalName", capitalName);
            command.Parameters.AddWithValue("@Population", population);
            command.ExecuteNonQuery();
            Console.WriteLine("Столицю успішно додано.");
        }

        static void UpdateCapital(SqlConnection connection)
        {
            Console.Write("Введіть назву столиці для оновлення: ");
            string capitalName = Console.ReadLine();

            Console.Write("Введіть нову назву столиці: ");
            string newCapitalName = Console.ReadLine();

            Console.Write("Введіть нову кількість жителів: ");
            long newPopulation = long.Parse(Console.ReadLine());

            string query = "UPDATE Capitals SET CapitalName = @NewCapitalName, Population = @NewPopulation WHERE CapitalName = @CapitalName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CapitalName", capitalName);
            command.Parameters.AddWithValue("@NewCapitalName", newCapitalName);
            command.Parameters.AddWithValue("@NewPopulation", newPopulation);
            command.ExecuteNonQuery();
            Console.WriteLine("Дані столиці успішно оновлено.");
        }

        static void DeleteCapital(SqlConnection connection)
        {
            Console.Write("Введіть назву столиці для видалення: ");
            string capitalName = Console.ReadLine();

            string query = "DELETE FROM Capitals WHERE CapitalName = @CapitalName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CapitalName", capitalName);
            command.ExecuteNonQuery();
            Console.WriteLine("Столицю успішно видалено.");
        }

        static void AddCity(SqlConnection connection)
        {
            Console.Write("Введіть назву великого міста: ");
            string cityName = Console.ReadLine();

            Console.Write("Введіть кількість жителів: ");
            long population = long.Parse(Console.ReadLine());

            string query = "INSERT INTO Cities (CityName, Population) VALUES (@CityName, @Population)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CityName", cityName);
            command.Parameters.AddWithValue("@Population", population);
            command.ExecuteNonQuery();
            Console.WriteLine("Велике місто успішно додано.");
        }

        static void UpdateCity(SqlConnection connection)
        {
            Console.Write("Введіть назву великого міста для оновлення: ");
            string cityName = Console.ReadLine();

            Console.Write("Введіть нову назву великого міста: ");
            string newCityName = Console.ReadLine();

            Console.Write("Введіть нову кількість жителів: ");
            long newPopulation = long.Parse(Console.ReadLine());

            string query = "UPDATE Cities SET CityName = @NewCityName, Population = @NewPopulation WHERE CityName = @CityName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CityName", cityName);
            command.Parameters.AddWithValue("@NewCityName", newCityName);
            command.Parameters.AddWithValue("@NewPopulation", newPopulation);
            command.ExecuteNonQuery();
            Console.WriteLine("Дані великого міста успішно оновлено.");
        }

        static void DeleteCity(SqlConnection connection)
        {
            Console.Write("Введіть назву великого міста для видалення: ");
            string cityName = Console.ReadLine();

            string query = "DELETE FROM Cities WHERE CityName = @CityName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CityName", cityName);
            command.ExecuteNonQuery();
            Console.WriteLine("Велике місто успішно видалено.");
        }

        static void AddPopulation(SqlConnection connection)
        {
            Console.Write("Введіть назву країни або столиці для додавання кількості жителів: ");
            string locationName = Console.ReadLine();

            Console.Write("Введіть кількість жителів: ");
            long population = long.Parse(Console.ReadLine());

            string query = "UPDATE CountryInfo SET Population = @Population WHERE CountryName = @LocationName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocationName", locationName);
            command.Parameters.AddWithValue("@Population", population);
            command.ExecuteNonQuery();
            Console.WriteLine("Кількість жителів успішно додано.");
        }

        static void UpdatePopulation(SqlConnection connection)
        {
            Console.Write("Введіть назву країни або столиці для оновлення кількості жителів: ");
            string locationName = Console.ReadLine();

            Console.Write("Введіть нову кількість жителів: ");
            long newPopulation = long.Parse(Console.ReadLine());

            string query = "UPDATE CountryInfo SET Population = @NewPopulation WHERE CountryName = @LocationName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocationName", locationName);
            command.Parameters.AddWithValue("@NewPopulation", newPopulation);
            command.ExecuteNonQuery();
            Console.WriteLine("Кількість жителів успішно оновлено.");
        }

        static void DeletePopulation(SqlConnection connection)
        {
            Console.Write("Введіть назву країни або столиці для видалення кількості жителів: ");
            string locationName = Console.ReadLine();

            string query = "UPDATE CountryInfo SET Population = NULL WHERE CountryName = @LocationName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocationName", locationName);
            command.ExecuteNonQuery();
            Console.WriteLine("Кількість жителів успішно видалено.");
        }
    }
}
