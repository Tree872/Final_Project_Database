using System.Windows;
using System.Windows.Controls;

namespace MedRecordsApp.Views
{
  /// <summary>
  /// Interaction logic for AppointmentsView.xaml
  /// </summary>
  public partial class AppointmentsView : UserControl
  {
    public AppointmentsView()
    {
      InitializeComponent();
      LoadAppointments();
      LoadPatients();
      LoadDoctors();
    }

    private void AddSaveButton_Click(object sender, RoutedEventArgs e)
    {
      // TODO: Validate input fields
      // TODO: Create new Appointment object
      // TODO: Insert into database
      // TODO: Refresh DataGrid
      // TODO: Clear form fields
    }

    private void UpdateSaveButton_Click(object sender, RoutedEventArgs e)
    {
      // TODO: Validate input fields
      // TODO: Create Appointment object with updated values
      // TODO: Update database record
      // TODO: Refresh DataGrid
      // TODO: Clear form fields
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
      // TODO: Validate Appointment ID
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

    private void AppointmentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // TODO: Get selected appointment
      // TODO: Populate Update tab fields with selected appointment data
    }

    private void LoadAppointments()
    {
      // TODO: Query database for all appointments
      // TODO: Bind results to DataGrid
    }

    private void LoadPatients()
    {
      // TODO: Query database for all patients
      // TODO: Bind results to Patient ComboBoxes
    }

    private void LoadDoctors()
    {
      // TODO: Query database for all doctors
      // TODO: Bind results to Doctor ComboBoxes
    }
  }
}