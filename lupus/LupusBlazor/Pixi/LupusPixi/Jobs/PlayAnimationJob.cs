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

        public override void Run()
        {
             RaiseOnStartEvent();
            Animation.OnCompleteEvent += OnAnimationComplete;
             PixiUnit.PlayAnimation(Animation);
        }

        public void OnAnimationComplete(object sender, EventArgs e)
        {
            Animation.OnCompleteEvent -= OnAnimationComplete;
             RaiseOnCompleteEvent();
        }

        
    }
}
