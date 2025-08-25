namespace WhatsappAudioTranscriber;

public partial class ProgressDialog : Form
{
    public ProgressDialog()
    {
        this.InitializeComponent();
    }

    public void UpdateProgress(int percentage, string statusText)
    {
        if (this.InvokeRequired)
        {
            this.Invoke(new Action<int, string>(this.UpdateProgress), percentage, statusText);
            return;
        }

        this.progressBar.Value = Math.Max(0, Math.Min(100, percentage));
        this.lblStatus.Text = statusText;
        Application.DoEvents();
    }

    public void SetIndeterminate(bool indeterminate)
    {
        if (this.InvokeRequired)
        {
            this.Invoke(new Action<bool>(this.SetIndeterminate), indeterminate);
            return;
        }

        this.progressBar.Style = indeterminate ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous;
    }
}
