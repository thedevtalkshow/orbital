namespace orbital.test.shared;

/// <summary>
/// Provides test data builders for domain models.
/// </summary>
public static class TestDataBuilders
{
    /// <summary>
    /// Creates a default valid DateTime for testing.
    /// </summary>
    public static DateTime GetTestDateTime(int hoursFromNow = 0)
    {
        return DateTime.UtcNow.Date.AddHours(9 + hoursFromNow); // 9 AM + offset
    }

    /// <summary>
    /// Creates a random string for testing purposes.
    /// </summary>
    public static string GetRandomString(int length = 10)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Range(0, length).Select(_ => chars[random.Next(chars.Length)]).ToArray());
    }
}
