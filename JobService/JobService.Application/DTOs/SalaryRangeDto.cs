using JobTracker.JobService.Domain.Entities;

namespace JobTracker.JobService.Application.DTOs;

public class SalaryRangeDto
{
    public int MinSalary { get; private set; }
    public int MaxSalary { get; private set; }
    public string Currency { get; private set; } = string.Empty;

    public static explicit operator SalaryRangeDto(SalaryRange salaryRange)
    {
        return new SalaryRangeDto
        {
            MinSalary = salaryRange.MaxSalary,
            MaxSalary = salaryRange.MaxSalary,
            Currency = salaryRange.Currency.ToString()
        };
    }
}