using JobTracker.JobService.Domain.Enums;

namespace JobTracker.JobService.Domain.Entities;

public class SalaryRange
{
    public int MinSalary { get; private set; }
    public int MaxSalary { get; private set; }
    public Currency Currency { get; private set; }
    
    public SalaryRange () { }

    private SalaryRange(int minSalary, int maxSalary, Currency currency)
    {
        MinSalary = minSalary;
        MaxSalary = maxSalary;
        Currency = currency;
    }

    public static SalaryRange Create(int minSalary, int maxSalary, Currency currency)
    {
        return new SalaryRange(minSalary, maxSalary, currency);
    }

    public void Update(int minSalary, int maxSalary, Currency currency)
    {
        MinSalary = minSalary;
        MaxSalary = maxSalary;
        Currency = currency;
    }
}