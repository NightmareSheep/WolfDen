using Lupus.Behaviours.Defend;
using Lupus.Units;
using LupusBlazor.Animation;
using LupusBlazor.Extensions;
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

        public BlazorHealth(BlazorGame blazorGame, IJSRuntime jSRuntime, Unit unit, int health) : base(unit, health)
        {
            BlazorGame = blazorGame;
            JSRuntime = jSRuntime;
            HealthDisplay = health;
        }

        public virtual async Task Draw()
        {
            await PixiHelper.SetTextSprite(JSRuntime, Unit.Id + " Health damage",   Unit.Tile.XCoord() - 4, Unit.Tile.YCoord() - 6, DamageDisplay != 0 ? HealthDisplay.ToString() + " - " + DamageDisplay.ToString() : HealthDisplay.ToString(), true, null, UI.TextStyle.Damage);
            await PixiHelper.SetTextSprite(JSRuntime, Unit.Id + " Health",          Unit.Tile.XCoord() - 4, Unit.Tile.YCoord() - 6, HealthDisplay.ToString(), true);
        }

        public override async Task Damage(int damage)
        {
            DelayedDisplayId = Guid.NewGuid();
            await base.Damage(damage);
            DamageDisplay += damage;


            if (CurrentHealth > 0)
                await Draw();
            else
            {
                await PixiHelper.DestroySprite(JSRuntime, Unit.Id + " Health damage");
                await PixiHelper.DestroySprite(JSRuntime, Unit.Id + " Health");

                var makeInvisible = new ChangeVisibilityAnimation(JSRuntime, this.Unit.Id + " Idle", false);
                await BlazorGame.AnimationPlayer.QueueAnimation(makeInvisible);
            }

            DelayedHealthDisplay(DelayedDisplayId);
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

        
    }
}
