using MedRecordsApp.DAL;
using MedRecordsApp.Models;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MedRecordsApp.Views
{
  public partial class TreatmentsView : UserControl
  {
    private readonly DataManager _dataManager;

    public TreatmentsView(DataManager dm)
    {
      InitializeComponent();
      _dataManager = dm;

      TreatmentDoctorsFrame.Content = new TreatmentDoctorsView(dm);
      TreatmentsDataGrid.ItemsSource = _dataManager.TreatmentsViewTable.DefaultView;
      LoadComboBoxes();
    }

    private void LoadComboBoxes()
    {
      AddPatientComboBox.ItemsSource = _dataManager.PatientsTable.DefaultView;
      UpdatePatientComboBox.ItemsSource = _dataManager.PatientsTable.DefaultView;
      UpdateTreatmentIDComboBox.ItemsSource = _dataManager.TreatmentsTable.DefaultView;
      DeleteTreatmentIDComboBox.ItemsSource = _dataManager.TreatmentsTable.DefaultView;
    }

    private void AddSaveButton_Click(object sender, RoutedEventArgs e)
    {
      if (AddPatientComboBox.SelectedValue == null)
      {
        MessageBox.Show("Please select a patient.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (!AddStartDatePicker.SelectedDate.HasValue)
      {
        MessageBox.Show("Start date is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (string.IsNullOrWhiteSpace(AddDescriptionTextBox.Text))
      {
        MessageBox.Show("Treatment description is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (AddStatusComboBox.SelectedItem == null)
      {
        MessageBox.Show("Please select a status.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      var treatment = new Treatment
      {
        PatientID = (int)AddPatientComboBox.SelectedValue,
        TreatmentDescription = AddDescriptionTextBox.Text.Trim(),
        StartDate = AddStartDatePicker.SelectedDate.Value,
        Status = ((ComboBoxItem)AddStatusComboBox.SelectedItem).Content.ToString()
      };

      string error = _dataManager.AddTreatment(treatment);
      if (error != null)
      {
        MessageBox.Show($"Error adding treatment: {error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      ClearAddFields();
      LoadComboBoxes();
    }

    private void UpdateSaveButton_Click(object sender, RoutedEventArgs e)
    {
      if (UpdateTreatmentIDComboBox.SelectedValue == null)
      {
        MessageBox.Show("Please select a treatment ID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (UpdatePatientComboBox.SelectedValue == null)
      {
        MessageBox.Show("Please select a patient.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (!UpdateStartDatePicker.SelectedDate.HasValue)
      {
        MessageBox.Show("Start date is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (string.IsNullOrWhiteSpace(UpdateDescriptionTextBox.Text))
      {
        MessageBox.Show("Treatment description is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (UpdateStatusComboBox.SelectedItem == null)
      {
        MessageBox.Show("Please select a status.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      var treatment = new Treatment
      {
        TreatmentID = (int)UpdateTreatmentIDComboBox.SelectedValue,
        PatientID = (int)UpdatePatientComboBox.SelectedValue,
        TreatmentDescription = UpdateDescriptionTextBox.Text.Trim(),
        StartDate = UpdateStartDatePicker.SelectedDate.Value,
        Status = ((ComboBoxItem)UpdateStatusComboBox.SelectedItem).Content.ToString()
      };

      string error = _dataManager.UpdateTreatment(treatment);
      if (error != null)
      {
        MessageBox.Show($"Error updating treatment: {error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      ClearUpdateFields();
      LoadComboBoxes();
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
      if (DeleteTreatmentIDComboBox.SelectedValue == null)
      {
        MessageBox.Show("Please select a treatment ID.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      var result = MessageBox.Show("Are you sure you want to delete this treatment?",
          "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

      if (result != MessageBoxResult.Yes)
        return;

      int treatmentId = (int)DeleteTreatmentIDComboBox.SelectedValue;
      string error = _dataManager.DeleteTreatment(treatmentId);
      if (error != null)
      {
        MessageBox.Show($"Error deleting treatment: {error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      DeleteTreatmentIDComboBox.SelectedIndex = -1;
      LoadComboBoxes();
    }

    private void TreatmentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (TreatmentsDataGrid.SelectedItem is DataRowView selectedRow)
      {
        int treatmentId = Convert.ToInt32(selectedRow["TreatmentID"]);

        DataRow treatmentRow = _dataManager.TreatmentsTable.AsEnumerable()
            .FirstOrDefault(r => (int)r["TreatmentID"] == treatmentId);

        if (treatmentRow != null)
        {
          UpdateTreatmentIDComboBox.SelectedValue = treatmentId;
          UpdatePatientComboBox.SelectedValue = treatmentRow["PatientID"];
          UpdateDescriptionTextBox.Text = treatmentRow["TreatmentDescription"].ToString();
          UpdateStartDatePicker.SelectedDate = Convert.ToDateTime(treatmentRow["StartDate"]);

          string status = treatmentRow["Status"].ToString();
          foreach (ComboBoxItem item in UpdateStatusComboBox.Items)
          {
            if (item.Content.ToString() == status)
            {
              UpdateStatusComboBox.SelectedItem = item;
              break;
            }
          }
        }
      }
    }

    private void ClearAddFields()
    {
      AddPatientComboBox.SelectedIndex = -1;
      AddDescriptionTextBox.Clear();
      AddStartDatePicker.SelectedDate = null;
      AddStatusComboBox.SelectedIndex = 0;
    }

    private void ClearUpdateFields()
    {
      UpdateTreatmentIDComboBox.SelectedIndex = -1;
      UpdatePatientComboBox.SelectedIndex = -1;
      UpdateDescriptionTextBox.Clear();
      UpdateStartDatePicker.SelectedDate = null;
      UpdateStatusComboBox.SelectedIndex = -1;
    }
  }
}