using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi.LupusPixi
{
    public class MovingAnimation : Animation
    {
        public Container ContainerToMove { get; set; }
        private DotNetObjectReference<MovingAnimation> ObjRef { get; set; }
        public Application Application { get; }
        public int XDistance { get; }
        public int YDistance { get; }
        public int Duration { get; }
        public int QueueDuration { get; }
        private bool queueDurationExpired; 

        private int startingX;
        private int startingY;
        private int endingX;
        private int endingY;
        private long startingTime;
        private long queueEndingTime;

        public MovingAnimation(Application application, AnimatedSprite sprite, int xDistance, int yDistance, int duration, int queueDuration = -1, Container containerToMove = null) : base(sprite)
        {
            QueueFrame = -1;

            if (containerToMove == null)
                this.ContainerToMove = this.Sprite;
            else
                this.ContainerToMove = containerToMove;

            this.ObjRef = DotNetObjectReference.Create(this);
            Application = application;
            XDistance = xDistance;
            YDistance = yDistance;
            Duration = duration;
            if (queueDuration == -1)
                QueueDuration = duration;
            else
                QueueDuration = Math.Min(duration, queueDuration);
        }

        public override async Task Play()
        {
            Console.WriteLine("Play animation");

            await base.Play();
            startingX = this.ContainerToMove.X;
            startingY = this.ContainerToMove.Y;
            endingX = this.ContainerToMove.X + XDistance;
            endingY = this.ContainerToMove.Y + YDistance;
            startingTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            queueEndingTime = startingTime + (long)QueueDuration;
            queueDurationExpired = false;
            Application.TickEvent += Tick;
        }

        public override async Task End()
        {
            await base.End();
            this.ContainerToMove.X = endingX;
            this.ContainerToMove.Y = endingY;
            Application.TickEvent -= Tick;
        }

        public async Task Tick()
        {
            var currentTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            var elapsedTime = currentTime - startingTime;
            var elapsed = Math.Min(1, (float)elapsedTime / (float)Duration);

            this.ContainerToMove.X = (int)((float)startingX * (1 - elapsed) + (float)endingX * elapsed);
            this.ContainerToMove.Y = (int)((float)startingY * (1 - elapsed) + (float)endingY * elapsed);

            

            if (elapsed == 1)
            {
                await this.RaiseOnCompleteEvent();
                await this.End();
            }

            if (!queueDurationExpired && (currentTime >= queueEndingTime || elapsed == 1))
            {
                Console.WriteLine("queue complete");
                queueDurationExpired = true;
                await RaiseOnQueueCompleteEvent(this.QueueFrame);
            }
        }
    }
}
