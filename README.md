# WhatsApp Audio Transcriber

A Windows application that transcribes WhatsApp audio messages to text using Whisper.net.

## Features

- Transcribes audio files to text
- Supports automatic language detection
- Uses Whisper.net for high-quality speech recognition
- Simple and intuitive user interface

## How It Works

1. Copy an audio file to your clipboard (Ctrl+C on the file in Windows Explorer)
2. Click the "Transcribe Audio" button in the application
3. The application will convert the audio to the required format using FFmpeg
4. The transcription will appear in the text box

## Requirements

- Windows operating system
- .NET 8.0 or higher
- FFmpeg installed and available in your PATH
- CUDA-compatible GPU (optional, for faster processing)

## Setup Instructions

1. Clone this repository
2. Open the solution in Visual Studio
3. Build and run the application
4. On first run, the application will automatically download the required Whisper model

## Dependencies

- [Whisper.net](https://github.com/sandrohanea/whisper.net) - .NET wrapper for OpenAI's Whisper model
- [FFmpeg](https://ffmpeg.org/) - For audio conversion
- NVidia GPU with CUDA support
- [CUDA Toolkit (>= 12.1)](https://developer.nvidia.com/cuda-downloads)

## Technical Details

The application uses:
- Whisper.net for speech recognition
- FFmpeg for audio conversion
- Windows Forms for the user interface
- CUDA acceleration (if available)

The first time you run the application, it will download the large Whisper model (approximately 3GB). This may take some time depending on your internet connection.

## License

This project is open source and available under the MIT License.
