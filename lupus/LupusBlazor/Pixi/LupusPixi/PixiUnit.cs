using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Lupus.Other;
using LupusBlazor.Units;
using LupusBlazor.Audio;
using LupusBlazor.Pixi.LupusPixi.Jobs;
using PIXI;

namespace LupusBlazor.Pixi.LupusPixi
{
    public class PixiUnit
    {
        public IJSRuntime JSRuntime { get; }
        public Application Application { get; }
        public AudioPlayer AudioPlayer { get; }
        public Actors Actor { get; }
        public Container Container { get; private set; }
        public Container AnimationContainer { get; private set; }
        public Container TextContainer { get; private set; }
        private Dictionary<AnimationAndDirection, Animation> UnitAnimations { get; set; } = new();
        public JobQueue JobQueue { get; } = new();
        public event EventHandler ClickEvent;

        public AnimationAndDirection BaseAnimation { get; set; } = new (Animations.Idle, Direction.None);

        private Animation CurrentAnimation { get; set; }

        public PixiUnit(IJSRuntime jSRuntime, Application application, AudioPlayer audioPlayer, Actors actor)
        {
            JSRuntime = jSRuntime;
            Application = application;
            AudioPlayer = audioPlayer;
            Actor = actor;
            JobQueue.QueueEmptyEvent += PlayBaseAnimation;
        }        

        public void Initialize()
        {
            this.Container = new Container(this.JSRuntime);
            this.AnimationContainer = new Container(this.JSRuntime);
            this.TextContainer = new Container(this.JSRuntime);
            this.Container.AddChild(AnimationContainer);
            this.Container.AddChild(TextContainer);

            var animation =  GetAnimation(BaseAnimation);
            if (animation != null)
            {
                animation.Sprite.Interactive = true;
                animation.Sprite.ClickEvent += RaiseClickEvent;
            }
        }

        public void RaiseClickEvent(object sender, EventArgs e)
        {
            ClickEvent?.Invoke(this, e);
        }

        public void QueueAnimation(Animations animation)
        {
             QueueAnimation(animation, Direction.None);
        }

        public void QueueAnimation(AnimationAndDirection animationAndDirection)
        {
             QueueAnimation(animationAndDirection.Animation, animationAndDirection.Direction);
        }

        public Animation GetAnimation(AnimationAndDirection animationAndDirection)
        {
            Animation animation;

            if (UnitAnimations.TryGetValue(animationAndDirection, out animation))
                return animation;

            var animationFactory =  AnimationFactory.GetInstance(this.JSRuntime, this.Application, this.AudioPlayer);
            animation =  animationFactory.GetAnimation(this.Actor, animationAndDirection.Animation, animationAndDirection.Direction);
            animation ??=  animationFactory.GetMovingAnimation(this.Actor, animationAndDirection.Animation, animationAndDirection.Direction);
            if (animation is MovingAnimation movingAnimation)
                movingAnimation.ContainerToMove = this.Container;

            UnitAnimations.Add(animationAndDirection, animation);
            return animation;
        }

        public void QueueAnimation(Animations name, Direction direction)
        {
            var id = new AnimationAndDirection(name, direction);
            var instance =  GetAnimation(id);

            if (instance == null)
                return;


            var playAnimationJob = new PlayAnimationJob(this, instance);
            JobQueue.EnqueueJob(playAnimationJob);
        }

        public void QueueInteraction(Animations name, Direction direction, IEnumerable<PixiUnit> targets)
        {
            var id = new AnimationAndDirection(name, direction);
            var instance =  GetAnimation(id);

            if (instance == null)
                return;

            var playAnimationJob = new PlayAnimationJob(this, instance);

            foreach (var target in targets)
            {
                var triggerJob = new TriggerJob();
                var waitJob = new WaitJob(triggerJob);
                var waitForQueueJob = new WaitForQueueJob(playAnimationJob);
                 JobQueue.EnqueueJob(waitJob);
                 target.JobQueue.EnqueueJob(triggerJob);
                 target.JobQueue.EnqueueJob(waitForQueueJob);
            }
            
             JobQueue.EnqueueJob(playAnimationJob);
        }

        public void PlayAnimation(Animations animation)
        {
             PlayAnimation(animation, Direction.None);
        }        

        public void PlayAnimation(Animations name, Direction direction)
        {
            var id = new AnimationAndDirection(name, direction);
            var instance =  GetAnimation(id);

            if (instance != null)
                 PlayAnimation(instance);
        }

        public void PlayAnimation(Animation animation)
        {
            if (this.CurrentAnimation != null && this.CurrentAnimation != animation)
            {
                 this.CurrentAnimation.End();
                 this.AnimationContainer.RemoveChild(CurrentAnimation.Sprite);
            }

            this.CurrentAnimation = animation;
            animation.Play();
            AnimationContainer.AddChild(animation.Sprite);
        }

        public void PlayBaseAnimation(object sender, EventArgs e) =>  this.PlayAnimation(this.BaseAnimation.Animation, this.BaseAnimation.Direction);

        public void Dispose()
        {
            var actionJob = new ActionJob(DisposeAction);
             JobQueue?.EnqueueJob(actionJob);

        }

        private void DisposeAction()
        {
            var baseAnimation =  GetAnimation(BaseAnimation);
            if (baseAnimation != null)
                baseAnimation.Sprite.ClickEvent -= RaiseClickEvent;

            foreach (var animation in this.UnitAnimations.Values)
                 animation.Dispose();

             this.Container.Dispose();
            JobQueue.QueueEmptyEvent -= PlayBaseAnimation;
        }
    }
}
