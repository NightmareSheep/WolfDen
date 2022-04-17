using LupusBlazor.Animation;
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
        public event Func<Task> ClickEvent;

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

        public async Task Initialize()
        {
            this.Container = new Container(this.JSRuntime);
            this.AnimationContainer = new Container(this.JSRuntime);
            this.TextContainer = new Container(this.JSRuntime);
            
            await this.Container.Initialize();
            await this.AnimationContainer.Initialize();
            await this.TextContainer.Initialize();

            await this.Container.AddChild(AnimationContainer);
            await this.Container.AddChild(TextContainer);

            var animation = await GetAnimation(BaseAnimation);
            if (animation != null)
            {
                animation.Sprite.Interactive = true;
                animation.Sprite.ClickEvent += RaiseClickEvent;
            }
        }

        public async Task RaiseClickEvent()
        {
            if (ClickEvent != null)
            {
                var invocationList = ClickEvent.GetInvocationList().Cast<Func<Task>>();
                foreach (var subscriber in invocationList)
                    await subscriber();
            }
        }

        public async Task QueueAnimation(Animations animation)
        {
            await QueueAnimation(animation, Direction.None);
        }

        public async Task QueueAnimation(AnimationAndDirection animationAndDirection)
        {
            await QueueAnimation(animationAndDirection.Animation, animationAndDirection.Direction);
        }

        public async Task<Animation> GetAnimation(AnimationAndDirection animationAndDirection)
        {
            Animation animation;

            if (UnitAnimations.TryGetValue(animationAndDirection, out animation))
                return animation;

            var animationFactory = await AnimationFactory.GetInstance(this.JSRuntime, this.Application, this.AudioPlayer);
            animation = await animationFactory.GetAnimation(this.Actor, animationAndDirection.Animation, animationAndDirection.Direction);
            animation ??= await animationFactory.GetMovingAnimation(this.Actor, animationAndDirection.Animation, animationAndDirection.Direction);
            if (animation is MovingAnimation movingAnimation)
                movingAnimation.ContainerToMove = this.Container;

            UnitAnimations.Add(animationAndDirection, animation);
            return animation;
        }

        public async Task QueueAnimation(Animations name, Direction direction)
        {
            var id = new AnimationAndDirection(name, direction);
            var instance = await GetAnimation(id);

            if (instance == null)
                return;


            var playAnimationJob = new PlayAnimationJob(this, instance);
            await JobQueue.EnqueueJob(playAnimationJob);
        }

        public async Task QueueInteraction(Animations name, Direction direction, IEnumerable<PixiUnit> targets)
        {
            var id = new AnimationAndDirection(name, direction);
            var instance = await GetAnimation(id);

            if (instance == null)
                return;

            var playAnimationJob = new PlayAnimationJob(this, instance);

            foreach (var target in targets)
            {
                var triggerJob = new TriggerJob();
                var waitJob = new WaitJob(triggerJob);
                var waitForQueueJob = new WaitForQueueJob(playAnimationJob);
                await JobQueue.EnqueueJob(waitJob);
                await target.JobQueue.EnqueueJob(triggerJob);
                await target.JobQueue.EnqueueJob(waitForQueueJob);
            }
            
            await JobQueue.EnqueueJob(playAnimationJob);
        }

        public async Task PlayAnimation(Animations animation)
        {
            await PlayAnimation(animation, Direction.None);
        }        

        public async Task PlayAnimation(Animations name, Direction direction)
        {
            var id = new AnimationAndDirection(name, direction);
            var instance = await GetAnimation(id);

            if (instance != null)
                await PlayAnimation(instance);
        }

        public async Task PlayAnimation(Animation animation)
        {
            if (this.CurrentAnimation != null && this.CurrentAnimation != animation)
            {
                await this.CurrentAnimation.End();
                await this.AnimationContainer.RemoveChild(CurrentAnimation.Sprite);
            }

            this.CurrentAnimation = animation;
            await animation.Play();
            await AnimationContainer.AddChild(animation.Sprite);
        }

        public async Task PlayBaseAnimation() => await this.PlayAnimation(this.BaseAnimation.Animation, this.BaseAnimation.Direction);

        public async Task Dispose()
        {
            var actionJob = new ActionJob(DisposeAction);
            await JobQueue?.EnqueueJob(actionJob);

        }

        private async Task DisposeAction()
        {
            var baseAnimation = await GetAnimation(BaseAnimation);
            if (baseAnimation != null)
                baseAnimation.Sprite.ClickEvent -= RaiseClickEvent;

            foreach (var animation in this.UnitAnimations.Values)
                await animation.Dispose();

            await this.Container.Dispose();
            JobQueue.QueueEmptyEvent -= PlayBaseAnimation;
        }
    }
}
