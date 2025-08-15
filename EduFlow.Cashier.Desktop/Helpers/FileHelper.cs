using System.Drawing;
using System.IO;

namespace EduFlow.Cashier.Desktop.Helpers;

public class FileHelper
{
    public static async Task<string> SaveLogoAsync(byte[] fileBytes, string fileName)
    {
        string logosHelper = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets");

        if (!Directory.Exists(logosHelper))
            Directory.CreateDirectory(logosHelper);

        string path = Path.Combine(logosHelper, fileName);

        await File.WriteAllBytesAsync(path, fileBytes);

        return path;
    }
}
