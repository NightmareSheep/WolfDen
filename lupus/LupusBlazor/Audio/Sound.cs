﻿using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Audio
{
    public class Sound
    {
        public string Name { get; }
        public string AssetPath { get; set; }
        public IJSRuntime IJSRuntime { get; }
        public bool Loop { get; }
        public int Duration { get; }
        public int Intro { get; }
        public int Outro { get; }

        public Sound(string name, string assetPath, IJSRuntime iJSRuntime, bool loop = true, int duration = 0, int intro = 0, int outro = 0)
        {
            Name = name;
            AssetPath = assetPath;
            IJSRuntime = iJSRuntime;
            Loop = loop;
            Duration = duration;
            Intro = intro;
            Outro = outro;
        }

        public async Task Initialize()
        {
            await this.IJSRuntime.InvokeVoidAsync("addSound", this.Name, this.AssetPath, Loop, Duration, Intro, Outro);
        }

        public async Task Play(int volume)
        {
            await this.IJSRuntime.InvokeVoidAsync("playSound", this.Name, volume);
        }

        public async Task PlayMusic(int volume)
        {
            await this.IJSRuntime.InvokeVoidAsync("playMusic", this.Name, volume);
        }

        public async Task Stop()
        {
            await this.IJSRuntime.InvokeVoidAsync("stopSound", this.Name);
        }
    }
}
