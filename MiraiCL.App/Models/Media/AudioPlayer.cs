#define WINDOWS

using System;
using System.IO;

namespace MiraiCL.App.Models.Media;

#if WINDOWS
using NAudio.Wave;
public class AudioPlayer:IDisposable{
    private WaveOutEvent _playEvent = new();
    private AudioFileReader? _reader;
    public bool IsPlaying {
        get => _playEvent.PlaybackState == PlaybackState.Playing;
        set {
            if(value && _playEvent.PlaybackState == PlaybackState.Paused) Play();
            else if (!value && _playEvent.PlaybackState == PlaybackState.Playing) Pause();
        }
    }
    public void LoadAudioFile(string audioFile){
        Console.WriteLine(File.Exists(audioFile));
        if(!File.Exists(audioFile)) throw new FileNotFoundException($"{audioFile} doesn't exist");
        _reader = new AudioFileReader(audioFile);
        _playEvent.Init(_reader);
    }
    public void Play(){
        _playEvent.Play();
    }
    public void Pause(){
        _playEvent.Pause();
    }

    public void SetVolume(float volume){
        _playEvent.Volume = volume;
    }

    public void Stop(){
        if(_playEvent.PlaybackState != PlaybackState.Stopped) _playEvent.Stop();
    }

    public void Dispose(){
        _playEvent.Dispose();
        _reader?.Dispose();
    }
}
#elif LINUX

#else

#endif