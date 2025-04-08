using System.Diagnostics;
using Whisper.net;
using Whisper.net.Ggml;
using Whisper.net.LibraryLoader;

var ggmlType = GgmlType.LargeV3;
var modelFileName = "ggml-large-v3.bin";

Console.Write("Checking model file ... ");

RuntimeOptions.RuntimeLibraryOrder = [
    RuntimeLibrary.Cuda
];

// This section detects whether the "ggml-base.bin" file exists in our project disk. If it doesn't, it downloads it from the internet
if (!File.Exists(modelFileName))
{
    Console.Write("not found\n");
    await DownloadModel(modelFileName, ggmlType);

}
Console.Write("\u001b[32mok\n\u001b[0m\n");

Console.Write("Loading the model ... ");
// This section creates the whisperFactory object which is used to create the processor object.
var whisperFactory = WhisperFactory.FromPath(modelFileName);

// This section creates the processor object which is used to process the audio file, it uses language `auto` to detect the language of the audio file.
var processor = whisperFactory.CreateBuilder()
    .WithLanguage("auto")
    .Build();

Console.Write("\u001b[32mok\n\u001b[0m\n");

while (true)
{
    Console.Write("Audio file path > ");

    string? filePath = Console.ReadLine();
    if (string.IsNullOrEmpty(filePath))
    {
        break;
    }
    await LoadTranscription(filePath);
}

Console.WriteLine("\nExiting ... ");

static async Task DownloadModel(string fileName, GgmlType ggmlType)
{
    Console.WriteLine($"Downloading model [{fileName}] ... ");
    using var modelStream = await WhisperGgmlDownloader.GetGgmlModelAsync(ggmlType);
    using var fileWriter = File.OpenWrite(fileName);
    await modelStream.CopyToAsync(fileWriter);
    Console.Write("\u001b[32mok\n\u001b[0m\n");
}

async Task LoadTranscription(string filePath)
{
    Console.Write($"Reading file ... ");
    // I need to grab a file reference from the clipboard and past it in a temp folder
    var wavFileName = Path.Combine(Directory.GetCurrentDirectory(), "output.wav");

    using var ffmpegProcess = new Process
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = "ffmpeg",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        }
    };

    ffmpegProcess.StartInfo.ArgumentList.Add("-y");
    ffmpegProcess.StartInfo.ArgumentList.Add("-i");
    ffmpegProcess.StartInfo.ArgumentList.Add(filePath); // file path from the clipboard
    ffmpegProcess.StartInfo.ArgumentList.Add("-ar");
    ffmpegProcess.StartInfo.ArgumentList.Add("16000");
    ffmpegProcess.StartInfo.ArgumentList.Add(wavFileName);

    ffmpegProcess.Start();
    await ffmpegProcess.WaitForExitAsync();
    

    if (File.Exists(wavFileName))
    {
        Console.Write("\u001b[32mok\n\u001b[0m\n");
        var consoleOutput = ffmpegProcess.StandardOutput.ReadToEnd();
        if (!string.IsNullOrEmpty(consoleOutput))
        {
            Console.WriteLine(consoleOutput);
        }
        Console.WriteLine("Transcription:");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("-----------------");

        // This section creates the whisperFactory object which is used to create the processor object.
        using var fileStream = File.OpenRead(wavFileName);

        // This section processes the audio file and prints the results (start time, end time and text) to the console.
        await foreach (var result in processor.ProcessAsync(fileStream))
        {
            Console.Write($" {result.Text}");
        }
        Console.WriteLine("\n-----------------\n");
        Console.ResetColor();
    }
    else
    {
        var errorMessage = ffmpegProcess.StandardError.ReadToEnd();
        if (!string.IsNullOrEmpty(errorMessage))
        {
            Console.Write("failed\n");

            throw new ApplicationException($"Error: {errorMessage}");
        }

        Console.Out.WriteLine("Transcription was not generated");
    }
}