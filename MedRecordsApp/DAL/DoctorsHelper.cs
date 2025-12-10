using MySql.Data.MySqlClient;
using System.Data;
using MedRecordsApp.Models;

public static class DoctorsHelper
{
  public static (DataTable? Table, string? Error) GetAll(MySqlConnection connection)
  {
    try
    {
      string sqlQuery = "SELECT * FROM Doctors";

      MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, connection);

      DataTable resultTable = new DataTable();
      using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlCommand))
      {
        dataAdapter.Fill(resultTable);
      }

      return (resultTable, null);
    }
    catch (Exception exception)
    {
      return (null, exception.Message);
    }
  }

  public static (int NewId, string? Error) Add(MySqlConnection connection, Doctor doctor)
  {
    try
    {
      string sqlQuery = @"INSERT INTO Doctors (DoctorName, Specialization)
                                VALUES (@name, @specialization);
                                SELECT LAST_INSERT_ID();";

      MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, connection);
      sqlCommand.Parameters.AddWithValue("@name", doctor.DoctorName);
      sqlCommand.Parameters.AddWithValue("@specialization", doctor.Specialization);

      int newId = Convert.ToInt32(sqlCommand.ExecuteScalar());
      return (newId, null);
    }
    catch (Exception exception)
    {
      return (0, exception.Message);
    }
  }

  public static (bool Success, string? Error) Update(MySqlConnection connection, Doctor doctor)
  {
    try
    {
      string sqlQuery = @"UPDATE Doctors SET
                                DoctorName=@name,
                                Specialization=@specialization
                                WHERE DoctorID=@id";

      MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, connection);
      sqlCommand.Parameters.AddWithValue("@id", doctor.DoctorID);
      sqlCommand.Parameters.AddWithValue("@name", doctor.DoctorName);
      sqlCommand.Parameters.AddWithValue("@specialization", doctor.Specialization);

      bool success = sqlCommand.ExecuteNonQuery() > 0;
      return (success, null);
    }
    catch (Exception exception)
    {
      return (false, exception.Message);
    }
  }

  public static (bool Success, string? Error) Delete(MySqlConnection connection, int doctorId)
  {
    try
    {
      string sqlQuery = "DELETE FROM Doctors WHERE DoctorID=@id";

      MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, connection);
      sqlCommand.Parameters.AddWithValue("@id", doctorId);

      bool success = sqlCommand.ExecuteNonQuery() > 0;
      return (success, null);
    }
    catch (Exception exception)
    {
      return (false, exception.Message);
    }
  }
}
