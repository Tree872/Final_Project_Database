using System.Windows;
using System.Windows.Controls;

namespace MedRecordsApp.Views
{
  /// <summary>
  /// Interaction logic for TreatmentsView.xaml
  /// </summary>
  public partial class TreatmentsView : UserControl
  {
    public TreatmentsView()
    {
      InitializeComponent();
      LoadTreatments();
      LoadDiagnoses();
    }

    private void AddSaveButton_Click(object sender, RoutedEventArgs e)
    {
      // TODO: Validate input fields
      // TODO: Create new Treatment object
      // TODO: Insert into database
      // TODO: Refresh DataGrid
      // TODO: Clear form fields
    }

    private void UpdateSaveButton_Click(object sender, RoutedEventArgs e)
    {
      // TODO: Validate input fields
      // TODO: Create Treatment object with updated values
      // TODO: Update database record
      // TODO: Refresh DataGrid
      // TODO: Clear form fields
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
      // TODO: Validate Treatment ID
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

    private void TreatmentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // TODO: Get selected treatment
      // TODO: Populate Update tab fields with selected treatment data
    }

    private void LoadTreatments()
    {
      // TODO: Query database for all treatments
      // TODO: Bind results to DataGrid
    }

    private void LoadDiagnoses()
    {
      // TODO: Query database for all diagnoses
      // TODO: Bind results to Diagnosis ComboBoxes
    }
  }
}