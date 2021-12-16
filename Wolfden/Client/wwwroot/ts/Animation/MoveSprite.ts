function MoveSprite(callbackObject: any, id: string, queueDuration: number, duration: number, startX: number, startY: number, destinationX: number, destinationY: number, resetAnimation: boolean = false) {
    PlayAnimation(callbackObject, id, queueDuration, duration, startX, startY, resetAnimation,
        (sprite: any, currentTime: number) => {

            sprite.x = (((duration - currentTime) / duration) * startX) + ((currentTime / duration) * destinationX);
            sprite.y = (((duration - currentTime) / duration) * startY) + ((currentTime / duration) * destinationY);

        }
    );
}

