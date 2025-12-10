using MySql.Data.MySqlClient;
using System.Data;

public static class DiagnosesDoctorsHelper
{
  public static (DataTable? Table, string? Error) GetAll(MySqlConnection connection)
  {
    try
    {
      string sqlQuery = "SELECT * FROM DiagnosesDoctors";

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

  public static (bool Success, string? Error) Add(MySqlConnection connection, int diagnosisId, int doctorId)
  {
    try
    {
      string sqlQuery = @"INSERT INTO DiagnosesDoctors (DiagnosisID, DoctorID)
                                VALUES (@diagnosisId, @doctorId)";

      MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, connection);

      sqlCommand.Parameters.AddWithValue("@diagnosisId", diagnosisId);
      sqlCommand.Parameters.AddWithValue("@doctorId", doctorId);

      bool success = sqlCommand.ExecuteNonQuery() > 0;
      return (success, null);
    }
    catch (Exception exception)
    {
      return (false, exception.Message);
    }
  }

  public static (bool Success, string? Error) Delete(MySqlConnection connection, int diagnosisId, int doctorId)
  {
    try
    {
      string sqlQuery = @"DELETE FROM DiagnosesDoctors
                                WHERE DiagnosisID=@diagnosisId AND DoctorID=@doctorId";

      MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, connection);

      sqlCommand.Parameters.AddWithValue("@diagnosisId", diagnosisId);
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
