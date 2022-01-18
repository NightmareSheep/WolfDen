export function InitializePixiApp(blazorApplication, elementId) {

    var pixiApp = new PIXI.Application({ resizeTo: window });
    PIXI.settings.SCALE_MODE = PIXI.SCALE_MODES.NEAREST;
    var element = document.getElementById(elementId);
    element.appendChild(pixiApp.view);

    pixiApp.ticker.add(() => { blazorApplication.invokeMethodAsync("Tick"); });

    pixiApp.loadResources = function () {
        pixiApp.loader.load((loader, resources) => {
            blazorApplication.invokeMethodAsync('RaiseResourcesLoadedEvent');
        });
    }

    return pixiApp;
}

export async function LoadResources() {
    var promise = new Promise(resolve => {
        PIXI.Loader.shared.load((loader, resources) => {
            resolve();
        });
    });

    await promise;
}

export function ConstructAnimatedSprite(textures, times) {
    var frames = [];
    for (var i = 0; i < textures.length; i++)
        frames.push({ texture: textures[i], time: times[i] });

    var sprite = new PIXI.AnimatedSprite(frames);
    return sprite;
}

export function ConstructTextSprite(text, textStyle) {
    var textStyles = [
        { fontFamily: 'Helvetica', fontSize: 12, fill: 'white', align: 'left', stroke: 'black', strokeThickness: 6 },
        { fontFamily: 'Helvetica', fontSize: 12, fill: 'red', align: 'left', stroke: 'black', strokeThickness: 6 },
        { fontFamily: 'Helvetica', fontSize: 12, fill: 'yellow', align: 'left', stroke: 'black', strokeThickness: 6 }
    ]

    var sprite = new PIXI.Sprite(text, textStyles[textStyle]);
    return sprite;
}

export function AddFilter(obj, filter) {
    if (!obj.filters)
        obj.filters = [];

    var filters = obj.filters;

    if (filters.indexOf(filter) == -1)
        filters.push(filter);
    obj.filters = filters;
}

export function RemoveFilter(obj, filter) {
    if (!obj.filters)
        obj.filters = [];

    var filters = obj.filters;

    var pos = filters.indexOf(filter);
    if (pos != -1) {
        filters.splice(pos, 1);
        obj.filters = filters;
    }    
}

export function On(displayObject, event, csObject, functionName) {
    displayObject.on(event, (e) => {
        csObject.invokeMethodAsync(functionName);
    });
}


export function SetOnClick(displayObject, csObject, functionName) {
    displayObject.on("pointertap", function (e) {

        // Do not trigger on right click
        if (e.data && e.data.button == 2)
            return;

        csObject.invokeMethodAsync(functionName);
    });
}