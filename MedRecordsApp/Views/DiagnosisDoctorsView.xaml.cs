using System.Windows;
using System.Windows.Controls;

namespace MedRecordsApp.Views
{
  /// <summary>
  /// Interaction logic for DiagnosisDoctorsView.xaml
  /// </summary>
  public partial class DiagnosisDoctorsView : UserControl
  {
    public DiagnosisDoctorsView()
    {
      InitializeComponent();
    }

    private void DiagnosisComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      // TODO: Get selected diagnosis
      // TODO: Update DiagnosisDetailsTextBlock with diagnosis info (Patient, Date, Conditions)
      // TODO: Load assigned doctors for this diagnosis
      // TODO: Refresh available doctors list (exclude already assigned)
    }

    private void AddDoctorButton_Click(object sender, RoutedEventArgs e)
    {
      // TODO: Get selected diagnosis ID
      // TODO: Get selected doctor(s) from AvailableDoctorsListBox
      // TODO: Insert into DiagnosesDoctors junction table
      // TODO: Move doctor(s) from Available to Assigned list
      // TODO: Show success message
    }

    private void RemoveDoctorButton_Click(object sender, RoutedEventArgs e)
    {
      // TODO: Get selected diagnosis ID
      // TODO: Get selected doctor(s) from AssignedDoctorsListBox
      // TODO: Delete from DiagnosesDoctors junction table
      // TODO: Move doctor(s) from Assigned to Available list
      // TODO: Show success message
    }

  }
}