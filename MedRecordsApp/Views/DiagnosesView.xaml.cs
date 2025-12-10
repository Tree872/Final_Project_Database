using MedRecordsApp.DAL;
using MedRecordsApp.Models;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MedRecordsApp.Views
{
  public partial class DiagnosesView : UserControl
  {
    private readonly DataManager _dataManager;

    public DiagnosesView(DataManager dm)
    {
      InitializeComponent();
      _dataManager = dm;

      DiagnosisDoctorsFrame.Content = new DiagnosisDoctorsView(dm);
      DiagnosesDataGrid.ItemsSource = _dataManager.DiagnosesViewTable.DefaultView;
      LoadComboBoxes();
    }

    private void LoadComboBoxes()
    {
      AddPatientComboBox.ItemsSource = _dataManager.PatientsTable.DefaultView;
      UpdatePatientComboBox.ItemsSource = _dataManager.PatientsTable.DefaultView;
      UpdateDiagnosisIDComboBox.ItemsSource = _dataManager.DiagnosesTable.DefaultView;
      DeleteDiagnosisIDComboBox.ItemsSource = _dataManager.DiagnosesTable.DefaultView;
    }

    private void AddSaveButton_Click(object sender, RoutedEventArgs e)
    {
      if (AddPatientComboBox.SelectedValue == null)
      {
        MessageBox.Show("Please select a patient.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (!AddDateDiagnosedPicker.SelectedDate.HasValue)
      {
        MessageBox.Show("Date diagnosed is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (string.IsNullOrWhiteSpace(AddConditionsTextBox.Text))
      {
        MessageBox.Show("Conditions are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      var diagnosis = new Diagnosis
      {
        PatientID = (int)AddPatientComboBox.SelectedValue,
        Conditions = AddConditionsTextBox.Text.Trim(),
        DateDiagnosed = AddDateDiagnosedPicker.SelectedDate.Value,
        Notes = AddNotesTextBox.Text.Trim()
      };

      string error = _dataManager.AddDiagnosis(diagnosis);
      if (error != null)
      {
        MessageBox.Show($"Error adding diagnosis: {error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      ClearAddFields();
      LoadComboBoxes();
 
    }

    private void UpdateSaveButton_Click(object sender, RoutedEventArgs e)
    {
      if (UpdateDiagnosisIDComboBox.SelectedValue == null)
      {
        MessageBox.Show("Please select a diagnosis ID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (UpdatePatientComboBox.SelectedValue == null)
      {
        MessageBox.Show("Please select a patient.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (!UpdateDateDiagnosedPicker.SelectedDate.HasValue)
      {
        MessageBox.Show("Date diagnosed is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (string.IsNullOrWhiteSpace(UpdateConditionsTextBox.Text))
      {
        MessageBox.Show("Conditions are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      var diagnosis = new Diagnosis
      {
        DiagnosisID = (int)UpdateDiagnosisIDComboBox.SelectedValue,
        PatientID = (int)UpdatePatientComboBox.SelectedValue,
        Conditions = UpdateConditionsTextBox.Text.Trim(),
        DateDiagnosed = UpdateDateDiagnosedPicker.SelectedDate.Value,
        Notes = UpdateNotesTextBox.Text.Trim()
      };

      string error = _dataManager.UpdateDiagnosis(diagnosis);
      if (error != null)
      {
        MessageBox.Show($"Error updating diagnosis: {error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      ClearUpdateFields();
      LoadComboBoxes();
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
      if (DeleteDiagnosisIDComboBox.SelectedValue == null)
      {
        MessageBox.Show("Please select a diagnosis ID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      var result = MessageBox.Show("Are you sure you want to delete this diagnosis?",
          "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

      if (result != MessageBoxResult.Yes)
        return;

      int diagnosisId = (int)DeleteDiagnosisIDComboBox.SelectedValue;
      string error = _dataManager.DeleteDiagnosis(diagnosisId);
      if (error != null)
      {
        MessageBox.Show($"Error deleting diagnosis: {error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      DeleteDiagnosisIDComboBox.SelectedIndex = -1;
      LoadComboBoxes();
    }

    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
      string searchTerm = SearchTextBox.Text.Trim().ToLower();

      if (string.IsNullOrWhiteSpace(searchTerm))
      {
        DiagnosesDataGrid.ItemsSource = _dataManager.DiagnosesViewTable.DefaultView;
        return;
      }

      var filteredRows = _dataManager.DiagnosesViewTable.AsEnumerable()
          .Where(row =>
              row["PatientName"].ToString().ToLower().Contains(searchTerm) ||
              row["Conditions"].ToString().ToLower().Contains(searchTerm) ||
              row["Notes"].ToString().ToLower().Contains(searchTerm))
          .ToList();

      if (filteredRows.Any())
      {
        var filteredView = filteredRows.CopyToDataTable();
        DiagnosesDataGrid.ItemsSource = filteredView.DefaultView;
      }
      else
      {
        DiagnosesDataGrid.ItemsSource = null;
      }

    }

    private void DiagnosesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (DiagnosesDataGrid.SelectedItem is DataRowView selectedRow)
      {
        int diagnosisId = Convert.ToInt32(selectedRow["DiagnosisID"]);

        DataRow diagnosisRow = _dataManager.DiagnosesTable.AsEnumerable()
            .FirstOrDefault(r => (int)r["DiagnosisID"] == diagnosisId);

        if (diagnosisRow != null)
        {
          UpdateDiagnosisIDComboBox.SelectedValue = diagnosisId;
          UpdatePatientComboBox.SelectedValue = diagnosisRow["PatientID"];
          UpdateConditionsTextBox.Text = diagnosisRow["Conditions"].ToString();
          UpdateDateDiagnosedPicker.SelectedDate = Convert.ToDateTime(diagnosisRow["DateDiagnosed"]);
          UpdateNotesTextBox.Text = diagnosisRow["Notes"].ToString();
        }
      }
    }

    private void ClearAddFields()
    {
      AddPatientComboBox.SelectedIndex = -1;
      AddConditionsTextBox.Clear();
      AddDateDiagnosedPicker.SelectedDate = null;
      AddNotesTextBox.Clear();
    }

    private void ClearUpdateFields()
    {
      UpdateDiagnosisIDComboBox.SelectedIndex = -1;
      UpdatePatientComboBox.SelectedIndex = -1;
      UpdateConditionsTextBox.Clear();
      UpdateDateDiagnosedPicker.SelectedDate = null;
      UpdateNotesTextBox.Clear();
    }

  }
}