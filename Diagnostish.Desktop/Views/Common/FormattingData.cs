namespace Diagnostish.Desktop.Views.Common;

public static class FormattingData
{
    public static string FormatDate(DateTime date) => date == DateTime.MinValue ? "Unknown" : date.ToString("dd.MM.yyyy");
}