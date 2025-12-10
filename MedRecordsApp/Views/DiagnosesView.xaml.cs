using System.Windows;
using System.Windows.Controls;

namespace MedRecordsApp.Views
{
  /// <summary>
  /// Interaction logic for DiagnosesView.xaml
  /// </summary>
  public partial class DiagnosesView : UserControl
  {
    public DiagnosesView()
    {
      InitializeComponent();
      LoadDiagnoses();
      LoadAppointments();
    }

    private void AddSaveButton_Click(object sender, RoutedEventArgs e)
    {
      // TODO: Validate input fields
      // TODO: Create new Diagnosis object
      // TODO: Insert into database
      // TODO: Refresh DataGrid
      // TODO: Clear form fields
    }

    private void UpdateSaveButton_Click(object sender, RoutedEventArgs e)
    {
      // TODO: Validate input fields
      // TODO: Create Diagnosis object with updated values
      // TODO: Update database record
      // TODO: Refresh DataGrid
      // TODO: Clear form fields
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
      // TODO: Validate Diagnosis ID
      // TODO: Confirm deletion with user
      // TODO: Delete from database
      // TODO: Refresh DataGrid
      // TODO: Clear form fields
    }

    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
      // TODO: Get search term
      // TODO: Query database with search criteria
      // TODO: Update DataGrid with filtered results
    }

    private void DiagnosesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // TODO: Get selected diagnosis
      // TODO: Populate Update tab fields with selected diagnosis data
    }

    private void LoadDiagnoses()
    {
      // TODO: Query database for all diagnoses
      // TODO: Bind results to DataGrid
    }

    private void LoadAppointments()
    {
      // TODO: Query database for all appointments
      // TODO: Bind results to Appointment ComboBoxes
    }
  }
}