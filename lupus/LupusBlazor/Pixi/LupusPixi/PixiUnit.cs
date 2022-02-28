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
        public DotNetObjectReference<Clickable> Clickable { get; }
        public Container Container { get; private set; }
        public Container AnimationContainer { get; private set; }
        public Container TextContainer { get; private set; }
        public Dictionary<AnimationAndDirection, Animation> UnitAnimations { get; set; } = new();
        public JobQueue JobQueue { get; } = new();

        public AnimationAndDirection BaseAnimation { get; set; } = new (Animations.Idle, Direction.None);

        private Animation CurrentAnimation { get; set; }

        public PixiUnit(IJSRuntime jSRuntime, Application application, AudioPlayer audioPlayer, Actors actor, DotNetObjectReference<Clickable> clickable = null)
        {
            JSRuntime = jSRuntime;
            Application = application;
            AudioPlayer = audioPlayer;
            Actor = actor;
            Clickable = clickable;
            JobQueue.QueueEmptyEvent += PlayBaseAnimation;
        }

        public async Task AddAllAnimations()
        {
            var animationFactory = await new AnimationFactory(this.JSRuntime, this.Application, this.AudioPlayer).Initialize();


            foreach (var name in (Animations[])Enum.GetValues(typeof(Animations)))
            {
                foreach (var direction in (Direction[])Enum.GetValues(typeof(Direction)))
                {
                    var animation = await animationFactory.GetAnimation(this.Actor, name, direction);
                    if (animation != null)
                        await this.AddAnimation(name, direction, animation);

                    var movingAnimation = await animationFactory.GetMovingAnimation(this.Actor, name, direction);
                    if (movingAnimation != null)
                    {
                        movingAnimation.ContainerToMove = this.Container;
                        await this.AddAnimation(name, direction, movingAnimation);
                    }
                }
            }
                    
        }

        public async Task AddAnimation(Animations name, Animation animation)
        {
            await AddAnimation(name, Direction.None, animation);
        }

        public async Task AddAnimation(Animations name, Direction direction, Animation animation)
        {
            var id = new AnimationAndDirection(name, direction);
            UnitAnimations.Add(id, animation);
            await this.AnimationContainer.AddChild(animation.Sprite);
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

            await AddAllAnimations();

            if (Clickable != null && this.UnitAnimations.TryGetValue(BaseAnimation, out var animation))
            {
                animation.Sprite.Interactive = true;
                await animation.Sprite.OnClick(Clickable, "RaisClickEvent");
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

        public async Task QueueAnimation(Animations name, Direction direction)
        {
            var id = new AnimationAndDirection(name, direction);
            this.UnitAnimations.TryGetValue(id, out var instance);

            if (instance == null)
                return;


            var playAnimationJob = new PlayAnimationJob(this, instance);
            await JobQueue.EnqueueJob(playAnimationJob);
        }

        public async Task QueueInteraction(Animations name, Direction direction, IEnumerable<PixiUnit> targets)
        {
            var id = new AnimationAndDirection(name, direction);
            this.UnitAnimations.TryGetValue(id, out var instance);

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
            this.UnitAnimations.TryGetValue(id, out var instance);

            if (instance != null)
                await PlayAnimation(instance);
        }

        public async Task PlayAnimation(Animation animation)
        {
            if (this.CurrentAnimation != null && this.CurrentAnimation != animation)
                await this.CurrentAnimation.End();

            this.CurrentAnimation = animation;
            await animation.Play();
        }

        public async Task PlayBaseAnimation() => await this.PlayAnimation(this.BaseAnimation.Animation, this.BaseAnimation.Direction);

        public async Task Dispose()
        {
            var actionJob = new ActionJob(DisposeAction);
            await JobQueue?.EnqueueJob(actionJob);

        }

        private async Task DisposeAction()
        {
            foreach (var animation in this.UnitAnimations.Values)
                await animation.Dispose();

            Clickable.Dispose();
            await this.Container.Dispose();
            JobQueue.QueueEmptyEvent -= PlayBaseAnimation;
        }
    }
}
