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
using PIXI;

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

        public virtual void Draw()
        {
            this.text = new Text(this.JSRuntime, this.CurrentHealth.ToString());
            text.ScaleX = 0.2f;
            text.ScaleY = 0.2f;
            text.X = -7;
            text.Y = -7;
            text.StrokeThickNess = 5f;
             text.SetAnchor(0.5f);
            text.Color = this.Unit.Owner.Color;
             this.BlazorUnit.PixiUnit.Container.AddChild(text);
        }

        public override void Damage(int damage)
        {
             base.Damage(damage);
            DelayedDisplayId = Guid.NewGuid();
            if (text != null)
                this.text.SpriteText = CurrentHealth.ToString();
            
        }

        public void DelayedHealthDisplay(Guid delayedDisplayId)
        {
             Task.Delay(3000);

            if (this.CurrentHealth <= 0 || this.DelayedDisplayId != delayedDisplayId)
                return;

            DamageDisplay = 0;
            HealthDisplay = this.CurrentHealth;
             this.Draw();
        }

        protected override void Death()
        {
             BlazorUnit?.PixiUnit?.QueueAnimation(Animations.Death);
             base.Death();
        }

        public void Dispose()
        {
            if (text != null)
                 this?.text?.Dispose();
        }

        
    }
}
