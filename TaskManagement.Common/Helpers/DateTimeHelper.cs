namespace TaskManagement.Common.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime GetCurrentDateTime()
        {
            return DateTime.UtcNow;
        }

        public static string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
} 