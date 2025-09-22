
using Cronos;
using JobTracker.SharedKernel.Messaging;
using JobTracker.UserService.Application.Repositories;
using JobTracker.UserService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JobTracker.UserService.Infrastructure.Services;

public class QuoteFetchingService : BackgroundService
{
    private readonly HttpClient _httpClient;
    private readonly CronExpression _cronExpression;
    private readonly IKafkaProducer _kafkaProducer;
    private readonly IUserRepository _userRepository;
    private const int LIMIT = 10;
    private readonly string _apiUrl;
    private readonly string _apiKey;

    public QuoteFetchingService(
        IServiceProvider serviceProvider,
        HttpClient httpClient,
        IConfiguration configuration,
        IKafkaProducer kafkaProducer

        )
    {
        var quoteApi = configuration.GetSection("QuoteAPI");
        _apiUrl = Environment.GetEnvironmentVariable("QUOTE_ENDPOINT")
                        ?? quoteApi["EndPoint"]
                        ?? throw new ArgumentException("QuoteAPI setting 'EndPoint' is missing or empty.");

        _apiKey = Environment.GetEnvironmentVariable("QUOTE_API_KEY")
                        ?? quoteApi["ApiKey"]
                        ?? throw new ArgumentException("QuoteAPI setting 'ApiKey' is missing or empty.");


        var cronSchedule = Environment.GetEnvironmentVariable("CRON_SCHEDULE")
                        ?? configuration.GetValue<string>("Cron:Schedule")
                        ?? throw new ArgumentException("Cron setting 'Schedule' is missing or empty.");

        _cronExpression = CronExpression.Parse(cronSchedule, CronFormat.IncludeSeconds);
        _httpClient = httpClient;
        _kafkaProducer = kafkaProducer;
        var scope = serviceProvider.CreateScope();
        _userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await WaitUntilNextSchedule(stoppingToken);

                var quote = await FetchQuoteAsync(stoppingToken);
                if (quote != null)
                {
                    var total = await _userRepository.GetUserCountAsync(string.Empty, string.Empty);
                    var totalPages = (int) Math.Ceiling((double)total / LIMIT);
                    var currentPage = 1;
                    while (currentPage <= totalPages) {
                        var users = await _userRepository.GetAllAsync(string.Empty, string.Empty, currentPage++, LIMIT);
                        foreach (var user in users)
                        {
                            var notificationPayload = new
                            {
                                Type = "Quote",
                                user.FirstName,
                                user.Email,
                                quote.Quote,
                                quote.Author,
                                quote.Category,
                                
                            };
                            await _kafkaProducer.Produce("job-tracker-topic", Guid.NewGuid().ToString(), notificationPayload);
                        }
                    }
                }
            }
            catch (TaskCanceledException)
            {
                break;
            }
            catch (Exception )
            {
                throw;
            }
        }
    }

    private async Task WaitUntilNextSchedule(CancellationToken stoppingToken)
    {
        var now = DateTimeOffset.Now;
        var next = _cronExpression.GetNextOccurrence(now, TimeZoneInfo.Local);

        if (next == null)
        {
            throw new InvalidOperationException("Invalid cron schedule.");
        }

        var delay = next.Value - now;

        await Task.Delay(0, stoppingToken);
    }

    private async Task<QuoteApiResponse?> FetchQuoteAsync(CancellationToken stoppingToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, _apiUrl);
        request.Headers.Add("X-Api-Key", _apiKey);

        var response = await _httpClient.SendAsync(request, stoppingToken);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var responseContent = await response.Content.ReadAsStringAsync(stoppingToken);
        var quoteData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<QuoteApiResponse>>(responseContent);

        if (quoteData != null && quoteData.Any())
        {
            return quoteData[0];
        }
        return null;
    }

}