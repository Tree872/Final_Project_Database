using System.Windows;
using System.Windows.Controls;

namespace MedRecordsApp.Views
{
  /// <summary>
  /// Interaction logic for DoctorsView.xaml
  /// </summary>
  public partial class DoctorsView : UserControl
  {
    public DoctorsView()
    {
      InitializeComponent();
      LoadDoctors();
    }

    private void AddSaveButton_Click(object sender, RoutedEventArgs e)
    {
      // TODO: Validate input fields
      // TODO: Create new Doctor object
      // TODO: Insert into database
      // TODO: Refresh DataGrid
      // TODO: Clear form fields
    }

    private void UpdateSaveButton_Click(object sender, RoutedEventArgs e)
    {
      // TODO: Validate input fields
      // TODO: Create Doctor object with updated values
      // TODO: Update database record
      // TODO: Refresh DataGrid
      // TODO: Clear form fields
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
      // TODO: Validate Doctor ID
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

    private void DoctorsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // TODO: Get selected doctor
      // TODO: Populate Update tab fields with selected doctor data
    }

    private void LoadDoctors()
    {
      // TODO: Query database for all doctors
      // TODO: Bind results to DataGrid
    }
  }
}