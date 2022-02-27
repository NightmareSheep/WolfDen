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

namespace LupusBlazor.Pixi.LupusPixi
{
    public class PixiUnit
    {
        public IJSRuntime JSRuntime { get; }
        public Application Application { get; }
        public AudioPlayer AudioPlayer { get; }
        public Actors Actor { get; }
        public ActionQueue ActionQueue { get; }
        public DotNetObjectReference<Clickable> Clickable { get; }
        public Container Container { get; private set; }
        public Container AnimationContainer { get; private set; }
        public Container TextContainer { get; private set; }
        public Dictionary<AnimationAndDirection, Animation> UnitAnimations { get; set; } = new();

        public AnimationAndDirection BaseAnimation { get; set; } = new (Animations.Idle, Direction.None);

        private Animation CurrentAnimation { get; set; }

        public PixiUnit(IJSRuntime jSRuntime, Application application, AudioPlayer audioPlayer, Actors actor, ActionQueue actionQueue, DotNetObjectReference<Clickable> clickable = null)
        {
            JSRuntime = jSRuntime;
            Application = application;
            AudioPlayer = audioPlayer;
            Actor = actor;
            ActionQueue = actionQueue;
            Clickable = clickable;
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

            if (!id.Equals(this.BaseAnimation) && name != Animations.Move)
                animation.OnCompleteEvent += PlayBaseAnimation;

            animation.OnQueueCompleteEvent += this.ActionQueue.ContinueQueue;
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


            var action = async () =>
            {
                // Action executed
                await this.PlayAnimation(name, direction);
                if (this.BaseAnimation.Equals(new AnimationAndDirection(name, direction)))
                    await this.ActionQueue.ContinueQueue();
            };

            await this.ActionQueue.AddAction(action);
        }


        public async Task PlayAnimation(Animations animation)
        {
            await PlayAnimation(animation, Direction.None);
        }

        public async Task PlayAnimation(Animations name, Direction direction)
        {
            var id = new AnimationAndDirection(name, direction);
            this.UnitAnimations.TryGetValue(id, out var instance);

            if (instance == null)
            {
                await ActionQueue.ContinueQueue();
                return;
            }

            if (this.CurrentAnimation != null && this.CurrentAnimation != instance)
            {
                await this.CurrentAnimation.End();
            }

            

            this.CurrentAnimation = instance;
            await instance.Play();
        }

        public async Task PlayBaseAnimation() => await this.PlayAnimation(this.BaseAnimation.Animation, this.BaseAnimation.Direction);

        public async Task Dispose()
        {
            await this.ActionQueue.AddAction(DisposeAction);
            
        }

        private async Task DisposeAction()
        {
            foreach (var animation in this.UnitAnimations.Values)
                await animation.Dispose();

            Clickable.Dispose();
            await this.Container.Dispose();
            await ActionQueue.ContinueQueue();
        }
    }
}
