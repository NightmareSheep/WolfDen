//import { Viewport } from 'wwwroot/js/modules/viewport.min.js';

console.log("This is a test");

let pixiApp = null;
let viewport = null;
//let DotNet: any;

async function InitializePixiApp(dotnetHelper, mapName) {
    window["PixiHelper"].Initialize();

    PIXI.settings.SCALE_MODE = PIXI.SCALE_MODES.NEAREST;
    PIXI.settings.PRECISION_FRAGMENT = PIXI.PRECISION.HIGH;


    let width = $(window).width();
    let height = $(window).height()

    pixiApp = new PIXI.Application({ width: width, height: height, resizeTo: window });
    pixiApp.renderer.view.style.display = "block";
    pixiApp.renderer.options.backgroundColor = 0x495057;
    $("#game").append(pixiApp.view);

    let loader = PIXI.Loader.shared;
    var image = await getImageAsync("/game/maps/" + mapName + "/" + mapName + ".png");

    viewport = new PIXI.Viewport({
        screenWidth: window.innerWidth,
        screenHeight: window.innerHeight,
        worldWidth: image.width * 3,
        worldHeight: image.height * 3,

        interaction: pixiApp.renderer.plugins.interaction // the interaction module is important for wheel to work properly when renderer.view is placed or scaled
    })

    pixiApp.stage.addChild(viewport);

    viewport
        .drag()
        .pinch()
        .wheel();

    if (!loader.resources["maps/" + mapName]) {
        await new Promise((resolve, reject) => {
            loader.add("maps/" + mapName, "/game/maps/" + mapName + "/" + mapName + ".png").load((loader, resources) => {
                loadMap(resources, mapName);
                resolve(null);
            });
        });        
    }

    if (!loader.resources.spritesheet) {
        loader
            .add("sprites", "/Images/sprites.json")
            .add("spritesheet", "/images/tiles_dungeon.png")
            .add("markings", "/images/Markings.png")
            .add("hero", "/game/units/hero/chara_hero.png")
            .load((loader, resources) => {
                setup(() => {
                    resourcesLoadedEvent(dotnetHelper);
                    //DotNet.invokeMethodAsync('WolfDen.Client', 'InitializeGame')
                }, resources, mapName)
            });
    }
    else {
        dotnetHelper.invokeMethodAsync('Draw');
    }
}

function resourcesLoadedEvent(dotnetHelper) {
    console.log("Resources are loaded");
    dotnetHelper.invokeMethodAsync('Draw');
}

function getImageAsync(src): Promise<HTMLImageElement>
{
    return new Promise((resolve, reject) => {
        let img = new Image()
        img.onload = () => resolve(img)
        img.onerror = reject
        img.src = src
    })
}

