public interface INewsApiService
{ 
    Task SyncTopNewsToDatabase();
    Task SendDailyDigestToSubscribers();
    Task SendWeeklyDigestToSubscribers();
}