using MySql.Data.MySqlClient;
using System.Data;
using MedRecordsApp.Models;

public static class PatientsHelper
{
  public static (DataTable? Table, string? Error) GetAll(MySqlConnection connection)
  {
    try
    {
      string sqlQuery = "SELECT * FROM Patients";

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

  public static (int NewId, string? Error) Add(MySqlConnection connection, Patient patient)
  {
    try
    {
      string sqlQuery = @"INSERT INTO Patients (PatientName, Birthdate, Phone)
                                VALUES (@name, @birthdate, @phone);
                                SELECT LAST_INSERT_ID();";

      MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, connection);

      sqlCommand.Parameters.AddWithValue("@name", patient.PatientName);
      sqlCommand.Parameters.AddWithValue("@birthdate", patient.Birthdate);
      sqlCommand.Parameters.AddWithValue("@phone", patient.Phone);

      int newId = Convert.ToInt32(sqlCommand.ExecuteScalar());
      return (newId, null);
    }
    catch (Exception exception)
    {
      return (0, exception.Message);
    }
  }

  public static (bool Success, string? Error) Update(MySqlConnection connection, Patient patient)
  {
    try
    {
      string sqlQuery = @"UPDATE Patients SET
                                PatientName=@name,
                                Birthdate=@birthdate,
                                Phone=@phone
                                WHERE PatientID=@id";

      MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, connection);

      sqlCommand.Parameters.AddWithValue("@id", patient.PatientID);
      sqlCommand.Parameters.AddWithValue("@name", patient.PatientName);
      sqlCommand.Parameters.AddWithValue("@birthdate", patient.Birthdate);
      sqlCommand.Parameters.AddWithValue("@phone", patient.Phone);

      bool success = sqlCommand.ExecuteNonQuery() > 0;
      return (success, null);
    }
    catch (Exception exception)
    {
      return (false, exception.Message);
    }
  }

  public static (bool Success, string? Error) Delete(MySqlConnection connection, int patientId)
  {
    try
    {
      string sqlQuery = "DELETE FROM Patients WHERE PatientID=@id";

      MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, connection);
      sqlCommand.Parameters.AddWithValue("@id", patientId);

      bool success = sqlCommand.ExecuteNonQuery() > 0;
      return (success, null);
    }
    catch (Exception exception)
    {
      return (false, exception.Message);
    }
  }
}
