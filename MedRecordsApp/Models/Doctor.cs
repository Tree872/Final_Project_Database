namespace MedRecordsApp.Models;

public class Doctor
{
  public int DoctorID { get; set; }
  public string DoctorName { get; set; }
  public string Specialization { get; set; }

  // Display property for ComboBox
  public string DisplayText => $"{DoctorID} - {DoctorName} ({Specialization})";
}