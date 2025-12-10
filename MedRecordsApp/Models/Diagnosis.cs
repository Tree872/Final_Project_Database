namespace MedRecordsApp.Models;

public class Diagnosis
{
  public int DiagnosisID { get; set; }
  public int PatientID { get; set; }
  public string Conditions { get; set; }
  public DateTime DateDiagnosed { get; set; }
  public string Notes { get; set; }

}