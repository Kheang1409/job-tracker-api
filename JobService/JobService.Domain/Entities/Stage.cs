using JobTracker.JobService.Domain.Enums;

namespace JobTracker.JobService.Domain.Entities;

public class Stage
{
    public string Name { get; private set; } = string.Empty;
    public DateTime AppointmentDate { get; private set; }
    public StageStatus Status { get; private set; } = StageStatus.Processing;
    public string Remarked { get; private set; } = string.Empty;

    public Stage() { }

    private Stage(string name, DateTime appointmentDate)
    {
        Name = name;
        AppointmentDate = appointmentDate;
    }

    public static Stage Create(string name, DateTime? appointmentDate = null)
    {
        return new Stage(name, appointmentDate ?? default(DateTime));
    }

    public void Cleared()
    {
        Status = StageStatus.Cleared;
    }
    public void Rejected()
    {
        Status = StageStatus.Rejected;
    }
}