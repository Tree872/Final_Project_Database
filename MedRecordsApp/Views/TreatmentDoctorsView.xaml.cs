using MedRecordsApp.DAL;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MedRecordsApp.Views
{
  public partial class TreatmentDoctorsView : UserControl
  {
    private readonly DataManager _dataManager;

    public TreatmentDoctorsView(DataManager dm)
    {
      InitializeComponent();
      _dataManager = dm;

      LoadTreatmentsComboBox();
    }

    private void LoadTreatmentsComboBox()
    {
      TreatmentComboBox.ItemsSource = _dataManager.TreatmentsTable.DefaultView;
    }

    private void TreatmentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (TreatmentComboBox.SelectedValue == null)
      {
        TreatmentDetailsTextBlock.Text = "Select a treatment to manage its doctors";
        AvailableDoctorsListBox.ItemsSource = null;
        AssignedDoctorsListBox.ItemsSource = null;
        return;
      }

      int treatmentId = (int)TreatmentComboBox.SelectedValue;

      // Get treatment details
      DataRow treatmentRow = _dataManager.TreatmentsTable.AsEnumerable()
          .FirstOrDefault(r => (int)r["TreatmentID"] == treatmentId);

      if (treatmentRow != null)
      {
        int patientId = (int)treatmentRow["PatientID"];
        DataRow patientRow = _dataManager.PatientsTable.AsEnumerable()
            .FirstOrDefault(r => (int)r["PatientID"] == patientId);

        string patientName = patientRow != null ? patientRow["PatientName"].ToString() : "Unknown";
        DateTime startDate = Convert.ToDateTime(treatmentRow["StartDate"]);
        string status = treatmentRow["Status"].ToString();

        TreatmentDetailsTextBlock.Text = $"Patient: {patientName} | Date: {startDate:yyyy-MM-dd} | Status: {status}";
      }

      LoadDoctorLists(treatmentId);
    }

    private void LoadDoctorLists(int treatmentId)
    {
      // Get assigned doctor IDs
      var assignedDoctorIds = _dataManager.TreatmentDoctorsTable.AsEnumerable()
          .Where(row => (int)row["TreatmentID"] == treatmentId)
          .Select(row => (int)row["DoctorID"])
          .ToList();

      // Get assigned doctors
      var assignedRows = _dataManager.DoctorsTable.AsEnumerable()
          .Where(row => assignedDoctorIds.Contains((int)row["DoctorID"]))
          .ToList();

      if (assignedRows.Any())
      {
        var assignedDoctors = assignedRows.CopyToDataTable();
        AssignedDoctorsListBox.ItemsSource = assignedDoctors.DefaultView;
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
        var availableDoctors = availableRows.CopyToDataTable();
        AvailableDoctorsListBox.ItemsSource = availableDoctors.DefaultView;
      }
      else
      {
        AvailableDoctorsListBox.ItemsSource = null;
      }
    }

    private void AddDoctorButton_Click(object sender, RoutedEventArgs e)
    {
      if (TreatmentComboBox.SelectedValue == null)
      {
        MessageBox.Show("Please select a treatment first.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (AvailableDoctorsListBox.SelectedItems.Count == 0)
      {
        MessageBox.Show("Please select at least one doctor to add.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      int treatmentId = (int)TreatmentComboBox.SelectedValue;

      foreach (DataRowView item in AvailableDoctorsListBox.SelectedItems)
      {
        int doctorId = (int)item["DoctorID"];
        string error = _dataManager.AddDoctorToTreatment(treatmentId, doctorId);

        if (error != null)
        {
          MessageBox.Show($"Error adding doctor: {error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          return;
        }
      }

      LoadDoctorLists(treatmentId);
    }

    private void RemoveDoctorButton_Click(object sender, RoutedEventArgs e)
    {
      if (TreatmentComboBox.SelectedValue == null)
      {
        MessageBox.Show("Please select a treatment first.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (AssignedDoctorsListBox.SelectedItems.Count == 0)
      {
        MessageBox.Show("Please select at least one doctor to remove.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      int treatmentId = (int)TreatmentComboBox.SelectedValue;

      foreach (DataRowView item in AssignedDoctorsListBox.SelectedItems)
      {
        int doctorId = (int)item["DoctorID"];
        string error = _dataManager.RemoveDoctorFromTreatment(treatmentId, doctorId);

        if (error != null)
        {
          MessageBox.Show($"Error removing doctor: {error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          return;
        }
      }

      LoadDoctorLists(treatmentId);
    }
  }
}