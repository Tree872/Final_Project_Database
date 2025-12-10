// Filename : MainWindow.xaml.cs
using MedRecordsApp.DAL;
using MedRecordsApp.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MedRecordsApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
  private DataManager _dm;

  public MainWindow()
  {
    InitializeComponent();

    _dm = new DataManager();

    // Inject DataManager into each view
    DoctorsTab.Content = new DoctorsView(_dm);
    PatientsTab.Content = new PatientsView(_dm);
    DiagnosesTab.Content = new DiagnosesView(_dm);
    TreatmentsTab.Content = new TreatmentsView(_dm);
  }
}