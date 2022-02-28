using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi.LupusPixi.Jobs
{
    public class PlayAnimationJob : JobBase
    {
        public PlayAnimationJob(PixiUnit pixiUnit, Animation animation)
        {
            PixiUnit = pixiUnit;
            Animation = animation;
        }

        public PixiUnit PixiUnit { get; }
        public Animation Animation { get; }

        public override async Task Run()
        {
            await RaiseOnStartEvent();
            Animation.OnCompleteEvent += OnAnimationComplete;
            await PixiUnit.PlayAnimation(Animation);
        }

        public async Task OnAnimationComplete()
        {
            Animation.OnCompleteEvent -= OnAnimationComplete;
            await RaiseOnCompleteEvent();
        }

        
    }
}
