using System.Windows;
using System.Windows.Controls;

namespace MedRecordsApp.Views
{
  /// <summary>
  /// Interaction logic for TreatmentDoctorsView.xaml
  /// </summary>
  public partial class TreatmentDoctorsView : UserControl
  {
    public TreatmentDoctorsView()
    {
      InitializeComponent();
    }

    private void TreatmentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // TODO: Get selected treatment
      // TODO: Update TreatmentDetailsTextBlock with treatment info (Patient, Date, Status)
      // TODO: Load assigned doctors for this treatment
      // TODO: Refresh available doctors list (exclude already assigned)
    }

    private void AddDoctorButton_Click(object sender, RoutedEventArgs e)
    {
      // TODO: Get selected treatment ID
      // TODO: Get selected doctor(s) from AvailableDoctorsListBox
      // TODO: Insert into TreatmentDoctors junction table
      // TODO: Move doctor(s) from Available to Assigned list
      // TODO: Show success message
    }

    private void RemoveDoctorButton_Click(object sender, RoutedEventArgs e)
    {
      // TODO: Get selected treatment ID
      // TODO: Get selected doctor(s) from AssignedDoctorsListBox
      // TODO: Delete from TreatmentDoctors junction table
      // TODO: Move doctor(s) from Assigned to Available list
      // TODO: Show success message
    }
  }
}