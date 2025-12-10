using MedRecordsApp.DAL;
using MedRecordsApp.Models;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MedRecordsApp.Views
{
  public partial class DoctorsView : UserControl
  {
    private readonly DataManager _dataManager;

    public DoctorsView(DataManager dm)
    {
      InitializeComponent();
      _dataManager = dm;

      DoctorsDataGrid.ItemsSource = _dataManager.DoctorsTable.DefaultView;
      LoadComboBoxes();
    }

    private void LoadComboBoxes()
    {
      UpdateDoctorIDComboBox.ItemsSource = _dataManager.DoctorsTable.DefaultView;
      DeleteDoctorIDComboBox.ItemsSource = _dataManager.DoctorsTable.DefaultView;
    }

    private void AddSaveButton_Click(object sender, RoutedEventArgs e)
    {
      if (string.IsNullOrWhiteSpace(AddNameTextBox.Text))
      {
        MessageBox.Show("Doctor name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (string.IsNullOrWhiteSpace(AddSpecializationTextBox.Text))
      {
        MessageBox.Show("Specialization is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      var doctor = new Doctor
      {
        DoctorName = AddNameTextBox.Text.Trim(),
        Specialization = AddSpecializationTextBox.Text.Trim()
      };

      string error = _dataManager.AddDoctor(doctor);
      if (error != null)
      {
        MessageBox.Show($"Error adding doctor: {error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      ClearAddFields();
      LoadComboBoxes();
    }

    private void UpdateSaveButton_Click(object sender, RoutedEventArgs e)
    {
      if (UpdateDoctorIDComboBox.SelectedValue == null)
      {
        MessageBox.Show("Please select a doctor ID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (string.IsNullOrWhiteSpace(UpdateNameTextBox.Text))
      {
        MessageBox.Show("Doctor name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (string.IsNullOrWhiteSpace(UpdateSpecializationTextBox.Text))
      {
        MessageBox.Show("Specialization is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      var doctor = new Doctor
      {
        DoctorID = (int)UpdateDoctorIDComboBox.SelectedValue,
        DoctorName = UpdateNameTextBox.Text.Trim(),
        Specialization = UpdateSpecializationTextBox.Text.Trim()
      };

      string error = _dataManager.UpdateDoctor(doctor);
      if (error != null)
      {
        MessageBox.Show($"Error updating doctor: {error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      ClearUpdateFields();
      LoadComboBoxes();
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
      if (DeleteDoctorIDComboBox.SelectedValue == null)
      {
        MessageBox.Show("Please select a doctor ID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      var result = MessageBox.Show("Are you sure you want to delete this doctor? This will remove them from all diagnoses and treatments.",
          "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

      if (result != MessageBoxResult.Yes)
        return;

      int doctorId = (int)DeleteDoctorIDComboBox.SelectedValue;
      string error = _dataManager.DeleteDoctor(doctorId);
      if (error != null)
      {
        MessageBox.Show($"Error deleting doctor: {error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      DeleteDoctorIDComboBox.SelectedIndex = -1;
      LoadComboBoxes();
    }

    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
      string searchTerm = SearchTextBox.Text.Trim().ToLower();

      if (string.IsNullOrWhiteSpace(searchTerm))
      {
        DoctorsDataGrid.ItemsSource = _dataManager.DoctorsTable.DefaultView;
        return;
      }

      var filteredRows = _dataManager.DoctorsTable.AsEnumerable()
          .Where(row =>
              row["DoctorName"].ToString().ToLower().Contains(searchTerm) ||
              row["Specialization"].ToString().ToLower().Contains(searchTerm))
          .ToList();

      if (filteredRows.Any())
      {
        var filteredView = filteredRows.CopyToDataTable();
        DoctorsDataGrid.ItemsSource = filteredView.DefaultView;
      }
      else
      {
        DoctorsDataGrid.ItemsSource = null;
      }
    }

    private void DoctorsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (DoctorsDataGrid.SelectedItem is DataRowView selectedRow)
      {
        UpdateDoctorIDComboBox.SelectedValue = selectedRow["DoctorID"];
        UpdateNameTextBox.Text = selectedRow["DoctorName"].ToString();
        UpdateSpecializationTextBox.Text = selectedRow["Specialization"].ToString();
      }
    }

    private void ClearAddFields()
    {
      AddNameTextBox.Clear();
      AddSpecializationTextBox.Clear();
    }

    private void ClearUpdateFields()
    {
      UpdateDoctorIDComboBox.SelectedIndex = -1;
      UpdateNameTextBox.Clear();
      UpdateSpecializationTextBox.Clear();
    }
  }
}