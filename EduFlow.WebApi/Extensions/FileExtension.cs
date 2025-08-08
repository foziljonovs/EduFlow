namespace EduFlow.WebApi.Extensions;

public static class FileExtension
{
    public static string GetContentType(
        this string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        return extension switch
        {
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            _ => "application/octet-stream"
        };
    }
}
