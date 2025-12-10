using MySql.Data.MySqlClient;
using System.Data;
using MedRecordsApp.Models;

public static class DiagnosesHelper
{
  public static (DataTable? Table, string? Error) GetAll(MySqlConnection connection)
  {
    try
    {
      string sqlQuery = "SELECT * FROM Diagnoses";

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

  public static (int NewId, string? Error) Add(MySqlConnection connection, Diagnosis diagnosis)
  {
    try
    {
      string sqlQuery = @"INSERT INTO Diagnoses (PatientID, Conditions, DateDiagnosed, Notes)
                                VALUES (@patientId, @conditions, @dateDiagnosed, @notes);
                                SELECT LAST_INSERT_ID();";

      MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, connection);

      sqlCommand.Parameters.AddWithValue("@patientId", diagnosis.PatientID);
      sqlCommand.Parameters.AddWithValue("@conditions", diagnosis.Conditions);
      sqlCommand.Parameters.AddWithValue("@dateDiagnosed", diagnosis.DateDiagnosed);
      sqlCommand.Parameters.AddWithValue("@notes", diagnosis.Notes);

      int newId = Convert.ToInt32(sqlCommand.ExecuteScalar());
      return (newId, null);
    }
    catch (Exception exception)
    {
      return (0, exception.Message);
    }
  }

  public static (bool Success, string? Error) Update(MySqlConnection connection, Diagnosis diagnosis)
  {
    try
    {
      string sqlQuery = @"UPDATE Diagnoses SET
                                Conditions=@conditions,
                                DateDiagnosed=@dateDiagnosed,
                                Notes=@notes
                                WHERE DiagnosisID=@id";

      MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, connection);

      sqlCommand.Parameters.AddWithValue("@id", diagnosis.DiagnosisID);
      sqlCommand.Parameters.AddWithValue("@conditions", diagnosis.Conditions);
      sqlCommand.Parameters.AddWithValue("@dateDiagnosed", diagnosis.DateDiagnosed);
      sqlCommand.Parameters.AddWithValue("@notes", diagnosis.Notes);

      bool success = sqlCommand.ExecuteNonQuery() > 0;
      return (success, null);
    }
    catch (Exception exception)
    {
      return (false, exception.Message);
    }
  }

  public static (bool Success, string? Error) Delete(MySqlConnection connection, int diagnosisId)
  {
    try
    {
      string sqlQuery = "DELETE FROM Diagnoses WHERE DiagnosisID=@id";

      MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, connection);
      sqlCommand.Parameters.AddWithValue("@id", diagnosisId);

      bool success = sqlCommand.ExecuteNonQuery() > 0;
      return (success, null);
    }
    catch (Exception exception)
    {
      return (false, exception.Message);
    }
  }
}
