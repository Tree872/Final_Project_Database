using MySql.Data.MySqlClient;
using System.Data;
using MedRecordsApp.Models;

public static class TreatmentsHelper
{
  public static (DataTable? Table, string? Error) GetAll(MySqlConnection connection)
  {
    try
    {
      string sqlQuery = "SELECT * FROM Treatments";

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

  public static (int NewId, string? Error) Add(MySqlConnection connection, Treatment treatment)
  {
    try
    {
      string sqlQuery = @"INSERT INTO Treatments (PatientID, TreatmentDescription, StartDate, Status)
                                VALUES (@patientId, @description, @startDate, @status);
                                SELECT LAST_INSERT_ID();";

      MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, connection);

      sqlCommand.Parameters.AddWithValue("@patientId", treatment.PatientID);
      sqlCommand.Parameters.AddWithValue("@description", treatment.TreatmentDescription);
      sqlCommand.Parameters.AddWithValue("@startDate", treatment.StartDate);
      sqlCommand.Parameters.AddWithValue("@status", treatment.Status);

      int newId = Convert.ToInt32(sqlCommand.ExecuteScalar());
      return (newId, null);
    }
    catch (Exception exception)
    {
      return (0, exception.Message);
    }
  }

  public static (bool Success, string? Error) Update(MySqlConnection connection, Treatment treatment)
  {
    try
    {
      string sqlQuery = @"UPDATE Treatments SET
                                TreatmentDescription=@description,
                                StartDate=@startDate,
                                Status=@status
                                WHERE TreatmentID=@id";

      MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, connection);

      sqlCommand.Parameters.AddWithValue("@id", treatment.TreatmentID);
      sqlCommand.Parameters.AddWithValue("@description", treatment.TreatmentDescription);
      sqlCommand.Parameters.AddWithValue("@startDate", treatment.StartDate);
      sqlCommand.Parameters.AddWithValue("@status", treatment.Status);

      bool success = sqlCommand.ExecuteNonQuery() > 0;
      return (success, null);
    }
    catch (Exception exception)
    {
      return (false, exception.Message);
    }
  }

  public static (bool Success, string? Error) Delete(MySqlConnection connection, int treatmentId)
  {
    try
    {
      string sqlQuery = "DELETE FROM Treatments WHERE TreatmentID=@id";

      MySqlCommand sqlCommand = new MySqlCommand(sqlQuery, connection);
      sqlCommand.Parameters.AddWithValue("@id", treatmentId);

      bool success = sqlCommand.ExecuteNonQuery() > 0;
      return (success, null);
    }
    catch (Exception exception)
    {
      return (false, exception.Message);
    }
  }
}
