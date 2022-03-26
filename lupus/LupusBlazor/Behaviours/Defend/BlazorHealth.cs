using Lupus.Behaviours.Defend;
using Lupus.Units;
using LupusBlazor.Animation;
using LupusBlazor.Extensions;
using LupusBlazor.Pixi;
using LupusBlazor.Units;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Behaviours.Defend
{
    public class BlazorHealth : Health
    {
        public int HealthDisplay { get; set; }
        public int DamageDisplay { get; set; }
        public Guid DelayedDisplayId { get; set; }
        public BlazorGame BlazorGame { get; }
        public IJSRuntime JSRuntime { get; }
        public BlazorUnit BlazorUnit { get; }
        public Text text { get; set; }

        public BlazorHealth(BlazorGame blazorGame, IJSRuntime jSRuntime, BlazorUnit unit, int health) : base(unit, health)
        {
            BlazorGame = blazorGame;
            JSRuntime = jSRuntime;
            BlazorUnit = unit;
            HealthDisplay = health;
        }

        public virtual async Task Draw()
        {
            this.text = new Text(this.JSRuntime, this.CurrentHealth.ToString());
            await this.text.Initialize();
            text.ScaleX = 0.2f;
            text.ScaleY = 0.2f;
            text.X = -7;
            text.Y = -7;
            text.StrokeThickNess = 5f;
            await text.SetAnchor(0.5f);
            text.Color = this.Unit.Owner.Color;
            await this.BlazorUnit.PixiUnit.Container.AddChild(text);
        }

        public override async Task Damage(int damage)
        {
            await base.Damage(damage);
            DelayedDisplayId = Guid.NewGuid();
            if (text != null)
                this.text.SpriteText = CurrentHealth.ToString();
            
        }

        public async Task DelayedHealthDisplay(Guid delayedDisplayId)
        {
            await Task.Delay(3000);

            if (this.CurrentHealth <= 0 || this.DelayedDisplayId != delayedDisplayId)
                return;

            DamageDisplay = 0;
            HealthDisplay = this.CurrentHealth;
            await this.Draw();
        }

        protected override async Task Death()
        {
            await (BlazorUnit?.PixiUnit?.QueueAnimation(Animations.Death) ?? Task.CompletedTask);
            await base.Death();
        }

        public async Task Dispose()
        {
            if (text != null)
                await this?.text?.Dispose();
        }

        
    }
}
