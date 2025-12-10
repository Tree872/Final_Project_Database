using MedRecordsApp.Models;
using MySql.Data.MySqlClient;
using System.Data;
using System.Xml.Schema;

namespace MedRecordsApp.DAL
{
  public class DataManager
  {
    
    private string connectionString;

    // Normalized in-memory tables
    public DataTable PatientsTable { get; private set; }
    public DataTable DoctorsTable { get; private set; }
    public DataTable DiagnosesTable { get; private set; }
    public DataTable TreatmentsTable { get; private set; }
    public DataTable DiagnosesDoctorsTable { get; private set; }
    public DataTable TreatmentDoctorsTable { get; private set; }

    // Denormalized UI tables
    public DataTable DiagnosesViewTable { get; private set; }
    public DataTable TreatmentsViewTable { get; private set; }

    public DataManager(string connectionString)
    {
      try

      {
        this.connectionString = connectionString;
        using (var connection = new MySqlConnection(this.connectionString))
        {
          connection.Open();
          // Init database
          var createDbCommand = new MySqlCommand("CREATE DATABASE IF NOT EXISTS EMR_DB;", connection);
          createDbCommand.ExecuteNonQuery();

        }
        this.connectionString = connectionString + ";Database=EMR_DB;";
        using (var connection = new MySqlConnection(this.connectionString))
        {
          connection.Open();
          // Init tables
          var command = new MySqlCommand(SqlSchema.TablesInitializationScript, connection);
          command.ExecuteNonQuery();
          // Load all normalized tables
          PatientsTable = PatientsHelper.GetAll(connection).Table;
          DoctorsTable = DoctorsHelper.GetAll(connection).Table;
          DiagnosesTable = DiagnosesHelper.GetAll(connection).Table;
          TreatmentsTable = TreatmentsHelper.GetAll(connection).Table;

          DiagnosesDoctorsTable = DiagnosesDoctorsHelper.GetAll(connection).Table;
          TreatmentDoctorsTable = TreatmentDoctorsHelper.GetAll(connection).Table;

          // Build UI view tables (in memory)
          BuildDiagnosesView();
          BuildTreatmentsView();
        }
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }

    private MySqlConnection OpenConnection()
    {
      MySqlConnection connection = new MySqlConnection(connectionString);
      connection.Open();
      return connection;
    }

    // Denormalize table for views
    private void BuildDiagnosesView()
    {
      // Initialize the view table and define columns if it doesn't exist yet.
      if (DiagnosesViewTable == null)
      {
        DiagnosesViewTable = new DataTable();
        DiagnosesViewTable.Columns.Add("DiagnosisID", typeof(int));
        DiagnosesViewTable.Columns.Add("PatientID", typeof(int));
        DiagnosesViewTable.Columns.Add("PatientName", typeof(string));
        DiagnosesViewTable.Columns.Add("Conditions", typeof(string));
        DiagnosesViewTable.Columns.Add("DateDiagnosed", typeof(DateTime));
        DiagnosesViewTable.Columns.Add("Notes", typeof(string));
        DiagnosesViewTable.Columns.Add("DoctorsList", typeof(string));
      }

      // Clear the existing data while preserving the table structure and reference.
      DiagnosesViewTable.Clear();

      foreach (DataRow diagnosisRow in DiagnosesTable.Rows)
      {
        int diagnosisId = (int)diagnosisRow["DiagnosisID"];
        int patientId = (int)diagnosisRow["PatientID"];

        DataRow patientRow = PatientsTable.AsEnumerable()
            .First(r => (int)r["PatientID"] == patientId);

        // Get linked doctors for this diagnosis
        var doctorIds = DiagnosesDoctorsTable.AsEnumerable()
            .Where(row => (int)row["DiagnosisID"] == diagnosisId)
            .Select(row => (int)row["DoctorID"])
            .ToList();

        // Build a string list of associated doctors
        string doctorList = string.Join(", ",
            doctorIds.Select(id =>
            {
              DataRow doc = DoctorsTable.AsEnumerable()
                                .First(r => (int)r["DoctorID"] == id);
              return $"{doc["DoctorName"]} (Id: {id})";
            })
        );

        // Add the denormalized row to the view table
        DiagnosesViewTable.Rows.Add(
            diagnosisId,
            patientId,
            patientRow["PatientName"],
            diagnosisRow["Conditions"],
            diagnosisRow["DateDiagnosed"],
            diagnosisRow["Notes"],
            doctorList
        );
      }
    }

    // Denormalize table for views
    private void BuildTreatmentsView()
    {
      // Initialize the view table and define columns if it doesn't exist yet.
      if (TreatmentsViewTable == null)
      {
        TreatmentsViewTable = new DataTable();
        TreatmentsViewTable.Columns.Add("TreatmentID", typeof(int));
        TreatmentsViewTable.Columns.Add("PatientID", typeof(int));
        TreatmentsViewTable.Columns.Add("PatientName", typeof(string));
        TreatmentsViewTable.Columns.Add("TreatmentDescription", typeof(string));
        TreatmentsViewTable.Columns.Add("StartDate", typeof(DateTime));
        TreatmentsViewTable.Columns.Add("Status", typeof(string));
        TreatmentsViewTable.Columns.Add("DoctorsList", typeof(string));
      }

      // Clear the existing data while preserving the table structure and reference.
      TreatmentsViewTable.Clear();

      foreach (DataRow treatmentRow in TreatmentsTable.Rows)
      {
        int treatmentId = (int)treatmentRow["TreatmentID"];
        int patientId = (int)treatmentRow["PatientID"];

        DataRow patientRow = PatientsTable.AsEnumerable()
            .First(r => (int)r["PatientID"] == patientId);

        // Get linked doctors for this treatment
        var doctorIds = TreatmentDoctorsTable.AsEnumerable()
            .Where(row => (int)row["TreatmentID"] == treatmentId)
            .Select(row => (int)row["DoctorID"])
            .ToList();

        // Build a string list of associated doctors
        string doctorList = string.Join(", ",
            doctorIds.Select(id =>
            {
              DataRow doc = DoctorsTable.AsEnumerable()
                                .First(r => (int)r["DoctorID"] == id);
              return $"{doc["DoctorName"]} (Id: {id})";
            })
        );

        // Add the denormalized row to the view table
        TreatmentsViewTable.Rows.Add(
            treatmentId,
            patientId,
            patientRow["PatientName"],
            treatmentRow["TreatmentDescription"],
            treatmentRow["StartDate"],
            treatmentRow["Status"],
            doctorList
        );
      }
    }

    // Patients CRUD
    public string? AddPatient(Patient patient)
    {
      try
      {
        using (var connection = OpenConnection())
        {
          var result = PatientsHelper.Add(connection, patient);
          if (result.Error != null) return result.Error;

          DataRow newRow = PatientsTable.NewRow();
          newRow["PatientID"] = result.NewId;
          newRow["PatientName"] = patient.PatientName;
          newRow["Birthdate"] = patient.Birthdate;
          newRow["Phone"] = patient.Phone;
          PatientsTable.Rows.Add(newRow);

          BuildDiagnosesView();
          BuildTreatmentsView();

          return null;
        }
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }

    public string? UpdatePatient(Patient patient)
    {
      try
      {
        using (var connection = OpenConnection())
        {
          var result = PatientsHelper.Update(connection, patient);
          if (result.Error != null) return result.Error;

          DataRow row = PatientsTable.AsEnumerable()
              .First(r => (int)r["PatientID"] == patient.PatientID);

          row["PatientName"] = patient.PatientName;
          row["Birthdate"] = patient.Birthdate;
          row["Phone"] = patient.Phone;

          BuildDiagnosesView();
          BuildTreatmentsView();

          return null;
        }
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }

    public string? DeletePatient(int patientId)
    {
      try
      {
        using (var connection = OpenConnection())
        {
          var result = PatientsHelper.Delete(connection, patientId);
          if (result.Error != null) return result.Error;

          DataRow row = PatientsTable.AsEnumerable()
              .First(r => (int)r["PatientID"] == patientId);
          PatientsTable.Rows.Remove(row);

          BuildDiagnosesView();
          BuildTreatmentsView();

          return null;
        }
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }

    // Doctors CRUD
    public string? AddDoctor(Doctor doctor)
    {
      try
      {
        using (var connection = OpenConnection())
        {
          var result = DoctorsHelper.Add(connection, doctor);
          if (result.Error != null) return result.Error;

          DataRow newRow = DoctorsTable.NewRow();
          newRow["DoctorID"] = result.NewId;
          newRow["DoctorName"] = doctor.DoctorName;
          newRow["Specialization"] = doctor.Specialization;
          DoctorsTable.Rows.Add(newRow);

          BuildDiagnosesView();
          BuildTreatmentsView();

          return null;
        }
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }

    public string? UpdateDoctor(Doctor doctor)
    {
      try
      {
        using (var connection = OpenConnection())
        {
          var result = DoctorsHelper.Update(connection, doctor);
          if (result.Error != null) return result.Error;

          DataRow row = DoctorsTable.AsEnumerable()
              .First(r => (int)r["DoctorID"] == doctor.DoctorID);

          row["DoctorName"] = doctor.DoctorName;
          row["Specialization"] = doctor.Specialization;

          BuildDiagnosesView();
          BuildTreatmentsView();

          return null;
        }
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }

    public string? DeleteDoctor(int doctorId)
    {
      try
      {
        using (var connection = OpenConnection())
        {
          var result = DoctorsHelper.Delete(connection, doctorId);
          if (result.Error != null) return result.Error;

          DataRow row = DoctorsTable.AsEnumerable()
              .First(r => (int)r["DoctorID"] == doctorId);
          DoctorsTable.Rows.Remove(row);

          BuildDiagnosesView();
          BuildTreatmentsView();

          return null;
        }
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }

    // Diagnoses CRUD
    public string? AddDiagnosis(Diagnosis diagnosis)
    {
      try
      {
        using (var connection = OpenConnection())
        {
          var result = DiagnosesHelper.Add(connection, diagnosis);
          if (result.Error != null) return result.Error;

          DataRow newRow = DiagnosesTable.NewRow();
          newRow["DiagnosisID"] = result.NewId;
          newRow["PatientID"] = diagnosis.PatientID;
          newRow["Conditions"] = diagnosis.Conditions;
          newRow["DateDiagnosed"] = diagnosis.DateDiagnosed;
          newRow["Notes"] = diagnosis.Notes;
          DiagnosesTable.Rows.Add(newRow);

          BuildDiagnosesView();
          return null;
        }
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }

    public string? UpdateDiagnosis(Diagnosis diagnosis)
    {
      try
      {
        using (var connection = OpenConnection())
        {
          var result = DiagnosesHelper.Update(connection, diagnosis);
          if (result.Error != null) return result.Error;

          DataRow row = DiagnosesTable.AsEnumerable()
              .First(r => (int)r["DiagnosisID"] == diagnosis.DiagnosisID);

          row["PatientID"] = diagnosis.PatientID;
          row["Conditions"] = diagnosis.Conditions;
          row["DateDiagnosed"] = diagnosis.DateDiagnosed;
          row["Notes"] = diagnosis.Notes;

          BuildDiagnosesView();
          return null;
        }
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }

    public string? DeleteDiagnosis(int diagnosisId)
    {
      try
      {
        using (var connection = OpenConnection())
        {
          var result = DiagnosesHelper.Delete(connection, diagnosisId);
          if (result.Error != null) return result.Error;

          DataRow row = DiagnosesTable.AsEnumerable()
              .First(r => (int)r["DiagnosisID"] == diagnosisId);
          DiagnosesTable.Rows.Remove(row);

          foreach (var link in DiagnosesDoctorsTable.AsEnumerable()
                   .Where(r => (int)r["DiagnosisID"] == diagnosisId).ToList())
            DiagnosesDoctorsTable.Rows.Remove(link);

          BuildDiagnosesView();
          return null;
        }
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }

    // Treatments CRUD
    public string? AddTreatment(Treatment treatment)
    {
      try
      {
        using (var connection = OpenConnection())
        {
          var result = TreatmentsHelper.Add(connection, treatment);
          if (result.Error != null) return result.Error;

          DataRow row = TreatmentsTable.NewRow();
          row["TreatmentID"] = result.NewId;
          row["PatientID"] = treatment.PatientID;
          row["TreatmentDescription"] = treatment.TreatmentDescription;
          row["StartDate"] = treatment.StartDate;
          row["Status"] = treatment.Status;
          TreatmentsTable.Rows.Add(row);

          BuildTreatmentsView();
          return null;
        }
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }

    public string? UpdateTreatment(Treatment treatment)
    {
      try
      {
        using (var connection = OpenConnection())
        {
          var result = TreatmentsHelper.Update(connection, treatment);
          if (result.Error != null) return result.Error;

          DataRow row = TreatmentsTable.AsEnumerable()
              .First(r => (int)r["TreatmentID"] == treatment.TreatmentID);

          row["PatientID"] = treatment.PatientID;
          row["TreatmentDescription"] = treatment.TreatmentDescription;
          row["StartDate"] = treatment.StartDate;
          row["Status"] = treatment.Status;

          BuildTreatmentsView();
          return null;
        }
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }

    public string? DeleteTreatment(int treatmentId)
    {
      try
      {
        using (var connection = OpenConnection())
        {
          var result = TreatmentsHelper.Delete(connection, treatmentId);
          if (result.Error != null) return result.Error;

          DataRow row = TreatmentsTable.AsEnumerable()
              .First(r => (int)r["TreatmentID"] == treatmentId);
          TreatmentsTable.Rows.Remove(row);

          foreach (var link in TreatmentDoctorsTable.AsEnumerable()
                   .Where(r => (int)r["TreatmentID"] == treatmentId).ToList())
            TreatmentDoctorsTable.Rows.Remove(link);

          BuildTreatmentsView();
          return null;
        }
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }

    // DiagnosesDoctors CRUD
    public string? AddDoctorToDiagnosis(int diagnosisId, int doctorId)
    {
      try
      {
        using (var connection = OpenConnection())
        {
          var result = DiagnosesDoctorsHelper.Add(connection, diagnosisId, doctorId);
          if (result.Error != null) return result.Error;

          DataRow row = DiagnosesDoctorsTable.NewRow();
          row["DiagnosisID"] = diagnosisId;
          row["DoctorID"] = doctorId;
          DiagnosesDoctorsTable.Rows.Add(row);

          BuildDiagnosesView();
          return null;
        }
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }

    public string? RemoveDoctorFromDiagnosis(int diagnosisId, int doctorId)
    {
      try
      {
        using (var connection = OpenConnection())
        {
          var result = DiagnosesDoctorsHelper.Delete(connection, diagnosisId, doctorId);
          if (result.Error != null) return result.Error;

          DataRow row = DiagnosesDoctorsTable.AsEnumerable()
              .First(r => (int)r["DiagnosisID"] == diagnosisId &&
                          (int)r["DoctorID"] == doctorId);
          DiagnosesDoctorsTable.Rows.Remove(row);

          BuildDiagnosesView();
          return null;
        }
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }

    // TreatmentDoctors CRUD
    public string? AddDoctorToTreatment(int treatmentId, int doctorId)
    {
      try
      {
        using (var connection = OpenConnection())
        {
          var result = TreatmentDoctorsHelper.Add(connection, treatmentId, doctorId);
          if (result.Error != null) return result.Error;

          DataRow row = TreatmentDoctorsTable.NewRow();
          row["TreatmentID"] = treatmentId;
          row["DoctorID"] = doctorId;
          TreatmentDoctorsTable.Rows.Add(row);

          BuildTreatmentsView();
          return null;
        }
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }

    public string? RemoveDoctorFromTreatment(int treatmentId, int doctorId)
    {
      try
      {
        using (var connection = OpenConnection())
        {
          var result = TreatmentDoctorsHelper.Delete(connection, treatmentId, doctorId);
          if (result.Error != null) return result.Error;

          DataRow row = TreatmentDoctorsTable.AsEnumerable()
              .First(r => (int)r["TreatmentID"] == treatmentId &&
                          (int)r["DoctorID"] == doctorId);
          TreatmentDoctorsTable.Rows.Remove(row);

          BuildTreatmentsView();
          return null;
        }
      }
      catch (Exception ex)
      {
        return ex.Message;
      }
    }
  }
}
