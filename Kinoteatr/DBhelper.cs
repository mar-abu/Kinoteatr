using System.Data.OleDb;

public static class DatabaseHelper
{
    // Строка подключения берется из App.config
    private static readonly string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CinemaDB"].ConnectionString;

    // Метод для получения подключения к базе данных
    public static OleDbConnection GetConnection()
    {
        return new OleDbConnection(ConnectionString);
    }
}
