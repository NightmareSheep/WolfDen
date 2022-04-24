using LupusBlazor.Audio.Json;
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

        public event EventHandler<int> ChangeMasterVolumeEvent;
        public event EventHandler<int> ChangeMusicVolumeEvent;
        public event EventHandler<int> ChangeEffectsVolumeEvent;

        Dictionary<Tracks, Sound> MusicLibrary = new();
        Dictionary<Effects, Sound> EffectsLibrary = new();
        public List<string> SoundEffects = new();
        Sound CurrentTrack { get; set; }
        public AudioJson AudioJson { get; }

        public AudioPlayer(IJSRuntime jSRuntime, int masterVolume, int musicVolume, int effectsVolume)
        {
            

            JSRuntime = jSRuntime;
            MasterVolume = masterVolume;
            MusicVolume = musicVolume;
            EffectsVolume = effectsVolume;
        }

        public AudioPlayer(IJSRuntime jSRuntime, int masterVolume, int musicVolume, int effectsVolume, AudioJson audioJson) : this(jSRuntime, masterVolume, musicVolume, effectsVolume)
        {
            AudioJson = audioJson;
        }

        public void Initialize()
        {
            // Music
            var TitleTheme = new Sound(Tracks.TitleTheme.ToString(), "game/audio/music/xDeviruchi - 8-bit Fantasy  & Adventure Music (2021)/xDeviruchi - Title Theme.wav", this.JSRuntime, false, 120600, 0, 120600);
            MusicLibrary.Add(Tracks.TitleTheme, TitleTheme);

            var ExploringTheUnkownTheme = new Sound(Tracks.ExploringTheUnkown.ToString(), "game/audio/music/xDeviruchi - 8-bit Fantasy  & Adventure Music (2021)/xDeviruchi - Exploring The Unknown.wav", this.JSRuntime, false, 128000, 8733, 120767);
            MusicLibrary.Add(Tracks.ExploringTheUnkown, ExploringTheUnkownTheme);

            foreach (Effects effect in Enum.GetValues(typeof(Effects)))
            {
                if (effect == Effects.none)
                    continue;

                var sound = new Sound(effect.ToString(), "game/audio/effects/" + effect.ToString() + ".wav", this.JSRuntime, false);
                EffectsLibrary.Add(effect, sound);
            }

            if (AudioJson != null)
            {
                foreach (var effect in AudioJson.Sprite.Keys)
                    this.SoundEffects.Add(effect);
            }
        }

        public void ChangeMasterVolume(int value)
        {
            ChangeMasterVolumeEvent?.Invoke(this, value);
            this.MasterVolume = value;
            this.JSRuntime.InvokeVoidAsync("setMasterVolume", value);
        }

        public void ChangeMusicVolume(int value)
        {
            ChangeMusicVolumeEvent?.Invoke(this, value);

            this.MusicVolume = value;
            if (CurrentTrack != null)
                 CurrentTrack.PlayMusic(this.MusicVolume);
        }

        public void ChangeEffectsVolume(int value)
        {
            ChangeEffectsVolumeEvent?.Invoke(this, value);
            this.EffectsVolume = value;
        }

        public void PlayMusic(Tracks track)
        {
            if (track == Tracks.Empty)
                return;

            if (CurrentTrack != null && CurrentTrack.Name != track.ToString())
                 CurrentTrack.Stop();

            if (this.MusicLibrary.TryGetValue(track, out var music))
            {
                CurrentTrack = music;

                if (this.MusicEnabled)
                     music.PlayMusic(this.MusicVolume);
            }
        }

        public void StopMusic()
        {
            if (CurrentTrack != null)
                 CurrentTrack.Stop();
        }

        public void EnableMusic()
        {
            this.MusicEnabled = true;
            if (this.CurrentTrack != null)
                 CurrentTrack.PlayMusic(this.MusicVolume);
        }

        public void DisableMusic()
        {
            this.MusicEnabled = false;
             this.StopMusic();
        }

        public void PlaySound(Effects effect)
        {
            if (!SoundEnabled)
                return;

            if (this.EffectsLibrary.TryGetValue(effect, out var sound))
            {
                sound.Play(this.EffectsVolume);
            }
        }

        public void PlaySoundEffect(string name)
        {
            if (!this.SoundEffects.Contains(name))
                return;

             this.JSRuntime.InvokeVoidAsync("playSoundEffect", name, this.EffectsVolume);
        }

    }
}
