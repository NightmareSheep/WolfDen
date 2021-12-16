using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LupusBlazor.Audio
{
    public class AudioPlayer
    {
        public IJSRuntime JSRuntime { get; }
        public bool SoundEnabled { get; set; } = true;
        private bool MusicEnabled { get; set; }
        public int MasterVolume { get; private set; }
        public int MusicVolume { get; private set; }
        public int EffectsVolume { get; private set; }

        public event Func<int, Task> ChangeMasterVolumeEvent;
        public event Func<int, Task> ChangeMusicVolumeEvent;
        public event Func<int, Task> ChangeEffectsVolumeEvent;

        Dictionary<Tracks, Sound> MusicLibrary = new();
        Dictionary<Effects, Sound> EffectsLibrary = new();
        Sound CurrentTrack { get; set; }

        public AudioPlayer(IJSRuntime jSRuntime, int masterVolume, int musicVolume, int effectsVolume)
        {
            JSRuntime = jSRuntime;
            MasterVolume = masterVolume;
            MusicVolume = musicVolume;
            EffectsVolume = effectsVolume;
        }

        public async Task Initialize()
        {
            // Music
            var TitleTheme = new Sound(Tracks.TitleTheme.ToString(), "game/audio/music/xDeviruchi - 8-bit Fantasy  & Adventure Music (2021)/xDeviruchi - Title Theme.wav", this.JSRuntime, false, 120600, 0, 120600);
            await TitleTheme.Initialize();
            MusicLibrary.Add(Tracks.TitleTheme, TitleTheme);

            var ExploringTheUnkownTheme = new Sound(Tracks.ExploringTheUnkown.ToString(), "game/audio/music/xDeviruchi - 8-bit Fantasy  & Adventure Music (2021)/xDeviruchi - Exploring The Unknown.wav", this.JSRuntime, false, 128000, 8733, 120767);
            await ExploringTheUnkownTheme.Initialize();
            MusicLibrary.Add(Tracks.ExploringTheUnkown, ExploringTheUnkownTheme);

            foreach (Effects effect in Enum.GetValues(typeof(Effects)))
            {
                if (effect == Effects.none)
                    continue;

                var sound = new Sound(effect.ToString(), "game/audio/effects/" + effect.ToString() + ".wav", this.JSRuntime, false);
                await sound.Initialize();
                EffectsLibrary.Add(effect, sound);
            }
        }

        public async Task ChangeMasterVolume(int value)
        {
            if (ChangeMasterVolumeEvent != null)
            {
                var invocationList = ChangeMasterVolumeEvent.GetInvocationList().Cast<Func<int, Task>>();
                foreach (var subscriber in invocationList)
                    await subscriber(value);
            }

            this.MasterVolume = value;
            await this.JSRuntime.InvokeVoidAsync("setMasterVolume", value);
        }

        public async Task ChangeMusicVolume(int value)
        {
            if (ChangeMusicVolumeEvent != null)
            {
                var invocationList = ChangeMusicVolumeEvent.GetInvocationList().Cast<Func<int, Task>>();
                foreach (var subscriber in invocationList)
                    await subscriber(value);
            }

            this.MusicVolume = value;
            if (CurrentTrack != null)
                await CurrentTrack.PlayMusic(this.MusicVolume);
        }

        public async Task ChangeEffectsVolume(int value)
        {
            if (ChangeEffectsVolumeEvent != null)
            {
                var invocationList = ChangeEffectsVolumeEvent.GetInvocationList().Cast<Func<int, Task>>();
                foreach (var subscriber in invocationList)
                    await subscriber(value);
            }

            this.EffectsVolume = value;
        }

        public async Task PlayMusic(Tracks track)
        {
            if (track == Tracks.Empty)
                return;

            if (CurrentTrack != null && CurrentTrack.Name != track.ToString())
                await CurrentTrack.Stop();

            if (this.MusicLibrary.TryGetValue(track, out var music))
            {
                CurrentTrack = music;

                if (this.MusicEnabled)
                    await music.PlayMusic(this.MusicVolume);
            }
        }

        public async Task StopMusic()
        {
            if (CurrentTrack != null)
                await CurrentTrack.Stop();
        }

        public async Task EnableMusic()
        {
            this.MusicEnabled = true;
            if (this.CurrentTrack != null)
                await CurrentTrack.PlayMusic(this.MusicVolume);
        }

        public async Task DisableMusic()
        {
            this.MusicEnabled = false;
            await this.StopMusic();
        }

        public async Task PlaySound(Effects effect)
        {
            if (!SoundEnabled)
                return;

            if (this.EffectsLibrary.TryGetValue(effect, out var sound))
            {
                await sound.Play(this.EffectsVolume);
            }
        }


    }
}
