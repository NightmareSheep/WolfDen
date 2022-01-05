export function InitializePixiApp(blazorApplication, elementId, worldWidth, worldHeight) {

    var pixiApp = new PIXI.Application({ resizeTo: window });
    pixiApp.renderer.options.scaleMode = PIXI.SCALE_MODES.NEAREST;
    var element = document.getElementById(elementId);
    element.appendChild(pixiApp.view);

    pixiApp.loadResources = function () {
        pixiApp.loader.load((loader, resources) => {
            blazorApplication.invokeMethodAsync('RaiseResourcesLoadedEvent');
        });
    }

    return pixiApp;
}

export function ConstructAnimatedSprite(textures, times) {
    var frames = [];
    for (var i = 0; i < textures.length; i++)
        frames.push({ texture: textures[i], time: times[i] });

    var sprite = new PIXI.AnimatedSprite(frames);
    return sprite;
}