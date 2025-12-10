using MySql.Data.MySqlClient;
using System.Data;

public static class TreatmentDoctorsHelper
{
  public static (DataTable? Table, string? Error) GetAll(MySqlConnection connection)
  {
    try
    {
      string sqlQuery = "SELECT * FROM TreatmentDoctors";

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

  public static (bool Success, string? Error) Add(MySqlConnection connection, int treatmentId, int doctorId)
  {
    try
    {
      string sqlQuery = @"INSERT INTO TreatmentDoctors (TreatmentID, DoctorID)
                                VALUES (@treatmentId, @doctorId)";

      MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, connection);

      sqlCommand.Parameters.AddWithValue("@treatmentId", treatmentId);
      sqlCommand.Parameters.AddWithValue("@doctorId", doctorId);

      bool success = sqlCommand.ExecuteNonQuery() > 0;
      return (success, null);
    }
    catch (Exception exception)
    {
      return (false, exception.Message);
    }
  }

  public static (bool Success, string? Error) Delete(MySqlConnection connection, int treatmentId, int doctorId)
  {
    try
    {
      string sqlQuery = @"DELETE FROM TreatmentDoctors
                                WHERE TreatmentID=@treatmentId AND DoctorID=@doctorId";

      MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, connection);

      sqlCommand.Parameters.AddWithValue("@treatmentId", treatmentId);
      sqlCommand.Parameters.AddWithValue("@doctorId", doctorId);

      bool success = sqlCommand.ExecuteNonQuery() > 0;
      return (success, null);
    }
    catch (Exception exception)
    {
      return (false, exception.Message);
    }
  }
}
