namespace WhatsappAudioTranscriber;

using System.Diagnostics;
using Whisper.net.Ggml;
using Whisper.net.LibraryLoader;
using Whisper.net;

public partial class Form1 : Form, IDisposable
{
    private readonly WhisperProcessor processor;
    private readonly WhisperFactory whisperFactory;

    public Form1()
    {
        var ggmlType = GgmlType.LargeV3;
        var modelFileName = "ggml-large-v3.bin";

        RuntimeOptions.RuntimeLibraryOrder = [
            RuntimeLibrary.Cuda
        ];

        // This section detects whether the "ggml-base.bin" file exists in our project disk. If it doesn't, it downloads it from the internet
        if (!File.Exists(modelFileName))
        {
            DownloadModel(modelFileName, ggmlType).GetAwaiter().GetResult();
        }

        // This section creates the whisperFactory object which is used to create the processor object.
        this.whisperFactory = WhisperFactory.FromPath(modelFileName);

        // This section creates the processor object which is used to process the audio file, it uses language `auto` to detect the language of the audio file.
        this.processor = whisperFactory.CreateBuilder()
            .WithLanguage("auto")
            .Build();

        this.InitializeComponent();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.processor.Dispose();
            this.whisperFactory.Dispose();
        }

        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private async void button1_Click(object sender, EventArgs e)
    {
        await this.ManualImplementation();
    }

    private async Task ManualImplementation()
    {
        // I need to grab a file reference from the clipboard and past it in a temp folder
        var wavFileName = Path.Combine(Directory.GetCurrentDirectory(), "output.wav");

        // Get the file from the clipboard
        var data = Clipboard.GetDataObject();

        if (data is null 
            || !data.GetDataPresent(DataFormats.FileDrop) 
            || data.GetData(DataFormats.FileDrop) is not string[] files 
            || files is not { Length: > 0 } )
        {
            MessageBox.Show("No file found in clipboard");
            return;
        }

        var file = files[0];

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
        ffmpegProcess.StartInfo.ArgumentList.Add(file); // file path from the clipboard
        ffmpegProcess.StartInfo.ArgumentList.Add("-ar");
        ffmpegProcess.StartInfo.ArgumentList.Add("16000");
        ffmpegProcess.StartInfo.ArgumentList.Add(wavFileName);

        ffmpegProcess.Start();
        await ffmpegProcess.WaitForExitAsync();

        this.textBox1.Text = ffmpegProcess.StandardOutput.ReadToEnd();

        this.textBox1.Text += ffmpegProcess.StandardError.ReadToEnd();

        if (File.Exists(wavFileName))
        {
            // This section creates the whisperFactory object which is used to create the processor object.
            using var fileStream = File.OpenRead(wavFileName);

            textBox1.Text = "";

            // This section processes the audio file and prints the results (start time, end time and text) to the console.
            await foreach (var result in processor.ProcessAsync(fileStream))
            {
                textBox1.Text += $" {result.Text}";
            }
        }
    }

    private static async Task DownloadModel(string fileName, GgmlType ggmlType)
    {
        Console.WriteLine($"Downloading Model {fileName}");
        using var modelStream = await WhisperGgmlDownloader.GetGgmlModelAsync(ggmlType);
        using var fileWriter = File.OpenWrite(fileName);
        await modelStream.CopyToAsync(fileWriter);
    }
}
