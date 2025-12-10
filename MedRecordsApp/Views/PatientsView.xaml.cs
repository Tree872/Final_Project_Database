using System.Windows;
using System.Windows.Controls;

namespace MedRecordsApp.Views
{
  /// <summary>
  /// Interaction logic for PatientsView.xaml
  /// </summary>
  public partial class PatientsView : UserControl
  {
    public PatientsView()
    {
      InitializeComponent();
      LoadPatients();
    }

    private void AddSaveButton_Click(object sender, RoutedEventArgs e)
    {
      // TODO: Validate input fields
      // TODO: Create new Patient object
      // TODO: Insert into database
      // TODO: Refresh DataGrid
      // TODO: Clear form fields
    }

    private void UpdateSaveButton_Click(object sender, RoutedEventArgs e)
    {
      // TODO: Validate input fields
      // TODO: Create Patient object with updated values
      // TODO: Update database record
      // TODO: Refresh DataGrid
      // TODO: Clear form fields
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
      // TODO: Validate Patient ID
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

    private void PatientsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // TODO: Get selected patient
      // TODO: Populate Update tab fields with selected patient data
    }

    private void LoadPatients()
    {
      // TODO: Query database for all patients
      // TODO: Bind results to DataGrid
    }
  }
}