using MedRecordsApp.DAL;
using MedRecordsApp.Models;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MedRecordsApp.Views
{
  public partial class PatientsView : UserControl
  {
    private readonly DataManager _dataManager;

    public PatientsView(DataManager dm)
    {
      InitializeComponent();
      _dataManager = dm;

      PatientsDataGrid.ItemsSource = _dataManager.PatientsTable.DefaultView;
      LoadComboBoxes();
    }

    private void LoadComboBoxes()
    {
      UpdatePatientIDComboBox.ItemsSource = _dataManager.PatientsTable.DefaultView;
      DeletePatientIDComboBox.ItemsSource = _dataManager.PatientsTable.DefaultView;
    }

    private void AddSaveButton_Click(object sender, RoutedEventArgs e)
    {
      if (string.IsNullOrWhiteSpace(AddNameTextBox.Text))
      {
        MessageBox.Show("Patient name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (!AddBirthdatePicker.SelectedDate.HasValue)
      {
        MessageBox.Show("Birthdate is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (string.IsNullOrWhiteSpace(AddPhoneTextBox.Text))
      {
        MessageBox.Show("Phone is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      var patient = new Patient
      {
        PatientName = AddNameTextBox.Text.Trim(),
        Birthdate = AddBirthdatePicker.SelectedDate.Value,
        Phone = AddPhoneTextBox.Text.Trim()
      };

      string error = _dataManager.AddPatient(patient);
      if (error != null)
      {
        MessageBox.Show($"Error adding patient: {error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      ClearAddFields();
      LoadComboBoxes();
    }

    private void UpdateSaveButton_Click(object sender, RoutedEventArgs e)
    {
      if (UpdatePatientIDComboBox.SelectedValue == null)
      {
        MessageBox.Show("Please select a patient ID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (string.IsNullOrWhiteSpace(UpdateNameTextBox.Text))
      {
        MessageBox.Show("Patient name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (!UpdateBirthdatePicker.SelectedDate.HasValue)
      {
        MessageBox.Show("Birthdate is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (string.IsNullOrWhiteSpace(UpdatePhoneTextBox.Text))
      {
        MessageBox.Show("Phone is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      var patient = new Patient
      {
        PatientID = (int)UpdatePatientIDComboBox.SelectedValue,
        PatientName = UpdateNameTextBox.Text.Trim(),
        Birthdate = UpdateBirthdatePicker.SelectedDate.Value,
        Phone = UpdatePhoneTextBox.Text.Trim()
      };

      string error = _dataManager.UpdatePatient(patient);
      if (error != null)
      {
        MessageBox.Show($"Error updating patient: {error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      ClearUpdateFields();
      LoadComboBoxes();
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
      if (DeletePatientIDComboBox.SelectedValue == null)
      {
        MessageBox.Show("Please select a patient ID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      var result = MessageBox.Show("Are you sure you want to delete this patient? This will also delete all associated diagnoses and treatments.",
          "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

      if (result != MessageBoxResult.Yes)
        return;

      int patientId = (int)DeletePatientIDComboBox.SelectedValue;
      string error = _dataManager.DeletePatient(patientId);
      if (error != null)
      {
        MessageBox.Show($"Error deleting patient: {error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      DeletePatientIDComboBox.SelectedIndex = -1;
      LoadComboBoxes();
    }

    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
      string searchTerm = SearchTextBox.Text.Trim().ToLower();

      if (string.IsNullOrWhiteSpace(searchTerm))
      {
        PatientsDataGrid.ItemsSource = _dataManager.PatientsTable.DefaultView;
        return;
      }

      var filteredView = _dataManager.PatientsTable.AsEnumerable()
          .Where(row =>
              row["PatientName"].ToString().ToLower().Contains(searchTerm) ||
              row["Phone"].ToString().ToLower().Contains(searchTerm))
          .CopyToDataTable();

      PatientsDataGrid.ItemsSource = filteredView.DefaultView;
    }

    private void PatientsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (PatientsDataGrid.SelectedItem is DataRowView selectedRow)
      {
        UpdatePatientIDComboBox.SelectedValue = selectedRow["PatientID"];
        UpdateNameTextBox.Text = selectedRow["PatientName"].ToString();
        UpdateBirthdatePicker.SelectedDate = Convert.ToDateTime(selectedRow["Birthdate"]);
        UpdatePhoneTextBox.Text = selectedRow["Phone"].ToString();
      }
    }

    private void ClearAddFields()
    {
      AddNameTextBox.Clear();
      AddBirthdatePicker.SelectedDate = null;
      AddPhoneTextBox.Clear();
    }

    private void ClearUpdateFields()
    {
      UpdatePatientIDComboBox.SelectedIndex = -1;
      UpdateNameTextBox.Clear();
      UpdateBirthdatePicker.SelectedDate = null;
      UpdatePhoneTextBox.Clear();
    }
  }
}