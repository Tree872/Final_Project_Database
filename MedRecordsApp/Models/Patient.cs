namespace MedRecordsApp.Models;

public class Patient
{
  public int PatientID { get; set; }
  public string PatientName { get; set; }
  public DateTime Birthdate { get; set; }
  public string Phone { get; set; }

  // Display property for ComboBox
  public string DisplayText => $"{PatientID} - {PatientName}";
}