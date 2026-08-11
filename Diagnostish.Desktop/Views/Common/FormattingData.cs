namespace Diagnostish.Desktop.Views.Common;

public static class FormattingData
{
    public static string FormatDate(this DateTime date)
    {
        return date == DateTime.MinValue
            ? "Неизвестно"
            : date.ToString("dd.MM.yyyy");
    }
}