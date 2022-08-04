using ImageMagick;
using MagickNETExceptionExample.App;
using MetadataExtractor;
using System.Diagnostics;

while (true)
{
    Console.WriteLine();
    Console.WriteLine();

    Console.Write("Enter file path: ");
    var filePath = Console.ReadLine();

    if (filePath is null)
    {
        Console.WriteLine("Invalid filePath!");
        continue;
    }

    Console.WriteLine($"Memory usage before: {CurrentMemoryUsageMb()}Mb");

    try
    {
        var stopwatch = Stopwatch.StartNew();

        using (var magickDebugStream = new DebugStream(new FileStream(filePath, FileMode.Open, FileAccess.Read)))
        {
            using var image = new MagickImage();
            image.Ping(magickDebugStream);
            Console.WriteLine($"Done pinging image with magick in {stopwatch.Elapsed}. Stream length: {magickDebugStream.Length}, bytes read: {magickDebugStream.ReadBytes}, seek count: {magickDebugStream.SeekCount}, memory usage: {CurrentMemoryUsageMb()}Mb");
        }

        Console.WriteLine($"Memory usage in between: {CurrentMemoryUsageMb()}Mb");

        stopwatch.Restart();

        using (var readerDebugStream = new DebugStream(new FileStream(filePath, FileMode.Open, FileAccess.Read)))
        {
            var metadata = ImageMetadataReader.ReadMetadata(readerDebugStream);
            Console.WriteLine($"Done reading metadata in {stopwatch.Elapsed}. Stream length: {readerDebugStream.Length}, bytes read: {readerDebugStream.ReadBytes}, seek count: {readerDebugStream.SeekCount}, memory usage: {CurrentMemoryUsageMb()}Mb");
        }

        Console.WriteLine($"Memory usage after: {CurrentMemoryUsageMb()}Mb");

        GC.Collect();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error reading file: {ex.Message}");
    }
}

static double CurrentMemoryUsageMb()
{
    using var process = Process.GetCurrentProcess();
    return process.PrivateMemorySize64 / (1024 * 1024);
}