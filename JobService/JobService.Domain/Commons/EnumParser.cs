using JobTracker.JobService.Domain.Enums;

namespace JobTracker.JobService.Domain.Commons;

public class EnumParser
{

    public static Currency Currency(string input)
    {
        try
        {
            return (Currency)Enum.Parse(typeof(Currency), input, true);
        }
        catch (ArgumentException)
        {
            throw new InvalidOperationException("Invalid currency code");
        }
    }

    public static JobPostStatus Status(string input)
    {
        try
        {
            return (JobPostStatus)Enum.Parse(typeof(JobPostStatus), input, true);
        }
        catch (ArgumentException)
        {
            throw new InvalidOperationException("Invalid status code");
        }
    }

    public static WorkMode WorkMode(string input)
    {
        try
        {
            return (WorkMode)Enum.Parse(typeof(WorkMode), input, true);
        }
        catch (ArgumentException)
        {
            throw new InvalidOperationException("Invalid work mode code");
        }
    }

    public static EmploymentType EmploymentType(string input)
    {
        try
        {
            return (EmploymentType)Enum.Parse(typeof(EmploymentType), input, true);
        }
        catch (ArgumentException)
        {
            throw new InvalidOperationException("Invalid employment type code");
        }
    }
}