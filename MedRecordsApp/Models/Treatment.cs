namespace MedRecordsApp.Models;

public class Treatment
{
  public int TreatmentID { get; set; }
  public int PatientID { get; set; }
  public string TreatmentDescription { get; set; }
  public DateTime StartDate { get; set; }
  public string Status { get; set; }

}