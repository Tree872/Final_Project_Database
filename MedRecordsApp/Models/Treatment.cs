namespace MedRecordsApp.Models;

public class Treatment
{
  public int TreatmentID { get; set; }
  public int DiagnosisID { get; set; }
  public required string TreatmentDescription { get; set; }
  public DateTime StartDate { get; set; }
}