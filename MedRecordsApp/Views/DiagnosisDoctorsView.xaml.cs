using MedRecordsApp.DAL;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MedRecordsApp.Views
{
  public partial class DiagnosisDoctorsView : UserControl
  {
    private readonly DataManager _dataManager;

    public DiagnosisDoctorsView(DataManager dm)
    {
      InitializeComponent();
      _dataManager = dm;

      LoadDiagnosesComboBox();
    }

    private void LoadDiagnosesComboBox()
    {
      DiagnosisComboBox.ItemsSource = _dataManager.DiagnosesTable.DefaultView;
    }

    private void DiagnosisComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (DiagnosisComboBox.SelectedValue == null)
      {
        DiagnosisDetailsTextBlock.Text = "Select a diagnosis to manage its doctors";
        AvailableDoctorsListBox.ItemsSource = null;
        AssignedDoctorsListBox.ItemsSource = null;
        return;
      }

      int diagnosisId = (int)DiagnosisComboBox.SelectedValue;

      // Get diagnosis details
      DataRow diagnosisRow = _dataManager.DiagnosesTable.AsEnumerable()
          .FirstOrDefault(r => (int)r["DiagnosisID"] == diagnosisId);

      if (diagnosisRow != null)
      {
        int patientId = (int)diagnosisRow["PatientID"];
        DataRow patientRow = _dataManager.PatientsTable.AsEnumerable()
            .FirstOrDefault(r => (int)r["PatientID"] == patientId);

        string patientName = patientRow != null ? patientRow["PatientName"].ToString() : "Unknown";
        DateTime dateDiagnosed = Convert.ToDateTime(diagnosisRow["DateDiagnosed"]);
        string conditions = diagnosisRow["Conditions"].ToString();

        DiagnosisDetailsTextBlock.Text = $"Patient: {patientName} | Date: {dateDiagnosed:yyyy-MM-dd} | Conditions: {conditions}";
      }

      LoadDoctorLists(diagnosisId);
    }

    private void LoadDoctorLists(int diagnosisId)
    {
      // Get assigned doctor IDs
      var assignedDoctorIds = _dataManager.DiagnosesDoctorsTable.AsEnumerable()
          .Where(row => (int)row["DiagnosisID"] == diagnosisId)
          .Select(row => (int)row["DoctorID"])
          .ToList();

      // Get assigned doctors rows
      var assignedRows = _dataManager.DoctorsTable.AsEnumerable()
          .Where(row => assignedDoctorIds.Contains((int)row["DoctorID"]))
          .ToList(); // Materialize to a List first to check .Any()

      // Only call CopyToDataTable if rows exist
      if (assignedRows.Any())
      {
        AssignedDoctorsListBox.ItemsSource = assignedRows.CopyToDataTable().DefaultView;
      }
      else
      {
        AssignedDoctorsListBox.ItemsSource = null;
      }

      // Get available doctors 
      var availableRows = _dataManager.DoctorsTable.AsEnumerable()
          .Where(row => !assignedDoctorIds.Contains((int)row["DoctorID"]))
          .ToList();


      if (availableRows.Any())
      {
        AvailableDoctorsListBox.ItemsSource = availableRows.CopyToDataTable().DefaultView;
      }
      else
      {
        AvailableDoctorsListBox.ItemsSource = null;
      }
    }

    private void AddDoctorButton_Click(object sender, RoutedEventArgs e)
    {
      if (DiagnosisComboBox.SelectedValue == null)
      {
        MessageBox.Show("Please select a diagnosis first.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (AvailableDoctorsListBox.SelectedItems.Count == 0)
      {
        MessageBox.Show("Please select at least one doctor to add.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      int diagnosisId = (int)DiagnosisComboBox.SelectedValue;

      foreach (DataRowView item in AvailableDoctorsListBox.SelectedItems)
      {
        int doctorId = (int)item["DoctorID"];
        string error = _dataManager.AddDoctorToDiagnosis(diagnosisId, doctorId);

        if (error != null)
        {
          MessageBox.Show($"Error adding doctor: {error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          return;
        }
      }

      LoadDoctorLists(diagnosisId);
    }

    private void RemoveDoctorButton_Click(object sender, RoutedEventArgs e)
    {
      if (DiagnosisComboBox.SelectedValue == null)
      {
        MessageBox.Show("Please select a diagnosis first.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (AssignedDoctorsListBox.SelectedItems.Count == 0)
      {
        MessageBox.Show("Please select at least one doctor to remove.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      int diagnosisId = (int)DiagnosisComboBox.SelectedValue;

      foreach (DataRowView item in AssignedDoctorsListBox.SelectedItems)
      {
        int doctorId = (int)item["DoctorID"];
        string error = _dataManager.RemoveDoctorFromDiagnosis(diagnosisId, doctorId);

        if (error != null)
        {
          MessageBox.Show($"Error removing doctor: {error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          return;
        }
      }

      LoadDoctorLists(diagnosisId);
    }
  }
}