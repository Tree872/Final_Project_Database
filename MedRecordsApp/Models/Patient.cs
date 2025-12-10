namespace MedRecordsApp.Models;

public class Patient
{
  public int PatientID { get; set; }
  public required string PatientName { get; set; }
  public DateTime Birthdate { get; set; }
  public required string Phone { get; set; }
}