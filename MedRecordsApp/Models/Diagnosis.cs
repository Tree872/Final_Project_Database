namespace MedRecordsApp.Models;

public class Diagnosis
{
  public int DiagnosisID { get; set; }
  public int AppointmentID { get; set; }
  public required string Conditions { get; set; }
  public string? Notes { get; set; }
}