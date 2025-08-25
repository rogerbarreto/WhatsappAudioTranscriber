namespace WhatsappAudioTranscriber;

using System.Diagnostics;
using Whisper.net.Ggml;
using Whisper.net.LibraryLoader;
using Whisper.net;

public partial class MainForm : Form, IDisposable
{
    private WhisperProcessor? processor;
    private WhisperFactory? whisperFactory;
    private bool isModelLoaded = false;

    public MainForm()
    {
        this.InitializeComponent();
        this.Load += this.MainForm_Load;
        this.KeyPreview = true; // Enable key preview to capture key events
    }

    private async void MainForm_Load(object? sender, EventArgs e)
    {
        if (!this.isModelLoaded)
        {
            // Start loading the model asynchronously after the form is loaded
            await this.LoadModelAsync();
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.processor?.Dispose();
            this.whisperFactory?.Dispose();
        }

        if (disposing && (this.components != null))
        {
            this.components.Dispose();
        }

        base.Dispose(disposing);
    }

    private async void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.V)
        {
            if (!this.isModelLoaded)
            {
                MessageBox.Show(this, "Model is still loading. Please wait for the model to finish loading before transcribing audio.", "Model Loading", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            await this.ManualImplementation();

            e.Handled = true; // Mark the event as handled
        }
    }

    private async void Button1_Click(object sender, EventArgs e)
    {
        if (!this.isModelLoaded)
        {
            MessageBox.Show(this, "Model is still loading. Please wait for the model to finish loading before transcribing audio.", "Model Loading", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

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
            || files is not { Length: > 0 })
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

            this.textBox1.Text = "";

            this.processor ??= this.whisperFactory?.CreateBuilder().WithLanguage("auto").Build();

            // This section processes the audio file and prints the results (start time, end time and text) to the console.
            if (this.processor != null)
            {
                await foreach (var result in this.processor.ProcessAsync(fileStream))
                {
                    this.textBox1.Text += $" {result.Text}";
                }
            }
        }
    }

    private async Task LoadModelAsync()
    {
        var progressDialog = new ProgressDialog();
        progressDialog.Show(this);
        progressDialog.SetIndeterminate(true);

        try
        {
            // Run the heavy work on a background thread
            await Task.Run(async () =>
            {
                var ggmlType = GgmlType.LargeV3;
                var modelFileName = "ggml-large-v3.bin";

                RuntimeOptions.RuntimeLibraryOrder = [
                    RuntimeLibrary.Cuda
                ];

                progressDialog.UpdateProgress(0, "Loading model...");

                if (!File.Exists(modelFileName))
                {
                    await DownloadModel(modelFileName, ggmlType);
                }

                progressDialog.UpdateProgress(50, "Loading model...");

                this.whisperFactory = WhisperFactory.FromPath(modelFileName);

                progressDialog.UpdateProgress(100, "Model loaded successfully!");
                this.isModelLoaded = true;
            });

            // Close progress dialog after a brief delay
            await Task.Delay(500);
        }
        catch (Exception ex)
        {
            var errorMessage = $"Failed to load model: {ex.Message}";
            if (ex.InnerException != null)
            {
                errorMessage += $"\n\nDetails: {ex.InnerException.Message}";
            }
            MessageBox.Show(this, errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            progressDialog?.Close();
            progressDialog?.Dispose();
        }
    }

    static async Task DownloadModel(string fileName, GgmlType ggmlType)
    {
        using var modelStream = await WhisperGgmlDownloader.GetGgmlModelAsync(ggmlType);
        using var fileWriter = File.OpenWrite(fileName);
        await modelStream.CopyToAsync(fileWriter);
    }
}
