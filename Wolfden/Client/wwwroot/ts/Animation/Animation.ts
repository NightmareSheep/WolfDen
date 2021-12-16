function PlayAnimation(callbackObject: any, id: string, queueDuration: number, duration: number, x: number, y: number, resetAnimation: boolean = false, ticker: (sprite: any, currentTime: number)=> void = null) {
    let currentTime = 0;
    let sprite = Sprites[id];
    let queueDurationCallbackDone = false;
    let durationCallbackDone = false;

    if (!sprite) {
        sprite = { x: 0, y: 0, visible: false };
        console.log("Animation not foud: " + id);
        currentTime = duration;
    }

    console.log("Performing animation: " + id);

    sprite.x = x;
    sprite.y = y;

    if (resetAnimation && sprite.gotoAndPlay)
        sprite.gotoAndPlay(0);

    sprite.visible = true;

    let tickerFunction = function (delta) {
        currentTime += pixiApp.ticker.elapsedMS;

        if (ticker)
            ticker(sprite, currentTime);

        if (currentTime >= duration) {
            pixiApp.ticker.remove(tickerFunction);
            sprite.visible = false;

            if (callbackObject && durationCallbackDone == false) {
                console.log("Duration Callback")
                durationCallbackDone = true;
                callbackObject.invokeMethodAsync('DurationCallBack');
            }
        }

        if (callbackObject && currentTime >= queueDuration && queueDurationCallbackDone == false) {
            console.log("QueueDurationCallback")
            queueDurationCallbackDone = true;
            callbackObject.invokeMethodAsync('QueueDurationCallBack');
        }
    }

    tickerFunction(0);
    pixiApp.ticker.add(tickerFunction);
}

