var sounds = {};
window["sounds"] = sounds;
function addSound(name, src, loop = true, duration = 0, intro = 0, outro = 0) {
    let sprite = undefined;
    if (duration)
        sprite = {
            intro: [0, intro],
            loop: [intro, outro - intro, 1],
            outro: [outro, duration - outro]
        };
    let sound = new Howl({
        src: src,
        loop: loop,
        volume: 0.2,
        sprite: sprite
    });
    sound.name = name;
    sounds[name] = sound;
    sound.on("fade", function () {
        sound.stop();
        console.log("sound has stopped: " + name);
    });
    console.log("Added track: " + name);
}
function setMasterVolume(volume) {
    Howler.volume(volume / 100);
}
function playSound(name) {
    sounds[name].play();
}
function playMusic(name, volume) {
    let sound = sounds[name];
    sound.volume(volume / 100);
    if (!sound.playing()) {
        if (sound._sprite.intro[1] == 0) {
            sound.play("loop");
            return;
        }
        var introId = sound.play("intro");
        sound.once("end", function (endId) {
            if (endId == introId) {
                sound.play("loop");
            }
        });
    }
}
function stopSound(name, volume) {
    var sound = sounds[name];
    sound.volume(volume / 100);
    if (sound.playing()) {
        sound.fade(0.2, 0, 500);
        console.log("stop sound: " + name);
    }
}
class BlazorPixiHelper {
    Initialize() {
        Sprites = {};
        CsObjects = {};
    }
    CreateSprite(resourcePath, id, x, y, clickCsObjRef, hoverCsObjRef, visible = true, tint = null) {
        let completeResourcePath = "";
        let resource = gameResources;
        for (let path of resourcePath) {
            resource = resource[path];
            completeResourcePath += path;
        }
        console.log("Draw: " + completeResourcePath + " with id: " + id + " at(X: " + x + ", Y: " + y + ")");
        let sprite = resource();
        sprite.x = x;
        sprite.y = y;
        sprite.visible = visible;
        Sprites[id] = sprite;
        if (tint) {
            sprite.tint = parseInt(tint.substring(1), 16);
        }
        if (clickCsObjRef) {
            CsObjects[id + " click"] = clickCsObjRef;
            sprite.interactive = true;
            sprite.on("pointertap", function (e) {
                // Do not trigger on right click
                if (e.data && e.data.button == 2)
                    return;
                clickCsObjRef.invokeMethodAsync("RaisClickEvent");
            });
        }
        if (hoverCsObjRef) {
            CsObjects[id + " hover"] = clickCsObjRef;
            sprite.interactive = true;
            sprite.on("pointerover", function (e) {
                clickCsObjRef.invokeMethodAsync("RaisPointerOverEvent");
            });
            sprite.on("pointerout", function (e) {
                clickCsObjRef.invokeMethodAsync("RaisPointerOutEvent");
            });
        }
        viewport.addChild(sprite);
    }
    SetTextSprite(id, x, y, text = null, visible = true, containerId = null, textStyle = 0) {
        console.log("Draw text sprite with text:" + text + " at x: " + x + " and y: " + y);
        if (!pixiApp)
            return;
        if (!Sprites[id]) {
            Sprites[id] = new PIXI.Text(text, TextStyles[textStyle]);
            let container = containerId ? Sprites[containerId] : viewport;
            container.addChild(Sprites[id]);
        }
        let textSprite = Sprites[id];
        textSprite.x = x;
        textSprite.y = y;
        textSprite.text = text ? text : textSprite.text;
        textSprite.visible = visible;
    }
    DestroySprite(id) {
        console.log("Destroy sprite with id: " + id);
        try {
            let sprite = Sprites[id];
            if (!sprite)
                return;
            delete Sprites[id];
            pixiApp.stage.removeChild(sprite);
            sprite.destroy();
            if (CsObjects[id + " click"])
                CsObjects[id + " click"].dispose();
            if (CsObjects[id + " hover"])
                CsObjects[id + " hover"].dispose();
        }
        catch {
            console.log("Destroy failed for sprite with id: " + id);
        }
    }
    SetPositionOfSprite(id, x, y) {
        console.log("Changing position of " + id + "To (X: " + x + ",Y " + y + ")");
        let sprite = Sprites[id];
        if (!sprite)
            return;
        sprite.x = x;
        sprite.y = y;
    }
    SetVisibleOfSprite(id, visible) {
        let sprite = Sprites[id];
        if (sprite)
            sprite.visible = visible;
    }
    SetFilter(id, filterName, apply) {
        let sprite = Sprites[id];
        let filter = filters[filterName];
        if (!filter || !sprite)
            return;
        filter["name"] = filterName;
        if (!sprite.filters)
            sprite.filters = [];
        let alreadyAddedFilter = sprite.filters.find(f => f["name"] == filterName);
        let index = alreadyAddedFilter ? sprite.filters.indexOf(alreadyAddedFilter) : -1;
        if (apply && index == -1)
            sprite.filters.push(filter);
        else if (!apply && index != -1)
            sprite.filters.splice(index, 1);
    }
}
let Sprites = {};
let CsObjects = {};
window["PixiHelper"] = new BlazorPixiHelper();
function DrawGrass(x, y) {
    console.log("Drawing grass tile.");
    let sprite = gameResources.terrain.Plain();
    sprite.x = x;
    sprite.y = y;
    pixiApp.stage.addChild(sprite);
}
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
    let height = $(window).height();
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
    });
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
            }, resources, mapName);
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
function getImageAsync(src) {
    return new Promise((resolve, reject) => {
        let img = new Image();
        img.onload = () => resolve(img);
        img.onerror = reject;
        img.src = src;
    });
}
var filters = {};
function setFilters(serializedColorFilters) {
    var basicColors = [0xd9d9d9, 0x9b9b9b, 0x828282, 0x747474, 0x676767, 0x151515];
    var colorFilters = JSON.parse(serializedColorFilters);
    for (var i = 0; i < colorFilters.length; i++) {
        var colorFilter = colorFilters[i];
        var colorValues = [];
        var subColors = colorFilter.Colors.split(',');
        for (var j = 0; j < subColors.length; j++) {
            colorValues.push([basicColors[j], parseInt(subColors[j], 16)]);
        }
        filters[colorFilter.Name + "Team"] = new PIXI.filters.MultiColorReplaceFilter(colorValues, 0.001);
    }
    filters.Neutral = new PIXI.filters.MultiColorReplaceFilter([
        [0xf8b878, 0xf8f898],
        [0xf89868, 0xdedede],
        [0xf85800, 0xf8c000],
        [0xf00008, 0xa5a5a5],
        [0xc00000, 0xb88000]
    ], 0.001);
    filters.Inactive = new PIXI.filters.AdjustmentFilter();
    filters.Inactive.saturation = 0.1;
    filters.GlowFilter = new PIXI.filters.GlowFilter({ outerStrength: 1 });
    filters.Desaturate = new PIXI.filters.ColorMatrixFilter();
    filters.Desaturate.desaturate();
}
var TextStyles = [
    { fontFamily: 'Helvetica', fontSize: 12, fill: 'white', align: 'left', stroke: 'black', strokeThickness: 6 },
    { fontFamily: 'Helvetica', fontSize: 12, fill: 'red', align: 'left', stroke: 'black', strokeThickness: 6 },
    { fontFamily: 'Helvetica', fontSize: 12, fill: 'yellow', align: 'left', stroke: 'black', strokeThickness: 6 }
];
function toggleFullScreen() {
    if (!document.fullscreenElement) {
        document.documentElement.requestFullscreen();
    }
    else {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        }
    }
}
function PlayAnimation(callbackObject, id, queueDuration, duration, x, y, resetAnimation = false, ticker = null) {
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
                console.log("Duration Callback");
                durationCallbackDone = true;
                callbackObject.invokeMethodAsync('DurationCallBack');
            }
        }
        if (callbackObject && currentTime >= queueDuration && queueDurationCallbackDone == false) {
            console.log("QueueDurationCallback");
            queueDurationCallbackDone = true;
            callbackObject.invokeMethodAsync('QueueDurationCallBack');
        }
    };
    tickerFunction(0);
    pixiApp.ticker.add(tickerFunction);
}
function MoveSprite(callbackObject, id, queueDuration, duration, startX, startY, destinationX, destinationY, resetAnimation = false) {
    PlayAnimation(callbackObject, id, queueDuration, duration, startX, startY, resetAnimation, (sprite, currentTime) => {
        sprite.x = (((duration - currentTime) / duration) * startX) + ((currentTime / duration) * destinationX);
        sprite.y = (((duration - currentTime) / duration) * startY) + ((currentTime / duration) * destinationY);
    });
}
function loadChest(resources) {
    // Idle
    loadResource(resources.spritesheet.texture, ["objects", "chest", "idle"], terrainWidth, terrainHeight, [
        new PIXI.Rectangle(224, 256, 16, 16),
    ]);
    // Opening
    loadResourceFromSpriteSheet(resources.sprites.spritesheet, "chest_opening", ["objects", "chest", "opening"], terrainWidth * 3, terrainHeight * 3, [
        2, 80, 80, 640
    ], null, null, { x: 1 / 3, y: 1 / 3 }, false);
}
function LoadHero(resources) {
    // Idle
    loadResource(resources.hero.texture, ["units", "hero", "idle"], terrainWidth, terrainHeight, [
        new PIXI.Rectangle(16, 16, 16, 16),
        new PIXI.Rectangle(16 * 4, 16, 16, 16),
        new PIXI.Rectangle(16 * 7, 16, 16, 16),
        new PIXI.Rectangle(16 * 4, 16, 16, 16),
    ], [
        640,
        80,
        640,
        80,
    ]);
    // Walking
    loadResource(resources.hero.texture, ["units", "hero", "moveSouth"], terrainWidth * 3, terrainHeight * 3, [
        new PIXI.Rectangle(48 * 0, 48 * 2, 48, 48),
        new PIXI.Rectangle(48 * 1, 48 * 2, 48, 48),
        new PIXI.Rectangle(48 * 2, 48 * 2, 48, 48),
        new PIXI.Rectangle(48 * 3, 48 * 2, 48, 48),
    ], [
        100, 100, 100, 100
    ], null, null, { x: 1 / 3, y: 1 / 3 });
    // Right
    loadResource(resources.hero.texture, ["units", "hero", "moveEast"], terrainWidth * 3, terrainHeight * 3, [
        new PIXI.Rectangle(48 * 0, 48 * 3, 48, 48),
        new PIXI.Rectangle(48 * 1, 48 * 3, 48, 48),
        new PIXI.Rectangle(48 * 2, 48 * 3, 48, 48),
        new PIXI.Rectangle(48 * 3, 48 * 3, 48, 48),
    ], [
        100, 100, 100, 100
    ], null, null, { x: 1 / 3, y: 1 / 3 });
    // Left
    loadResource(resources.hero.texture, ["units", "hero", "moveWest"], terrainWidth * 3, terrainHeight * 3, [
        new PIXI.Rectangle(48 * 0, 48 * 3, 48, 48),
        new PIXI.Rectangle(48 * 1, 48 * 3, 48, 48),
        new PIXI.Rectangle(48 * 2, 48 * 3, 48, 48),
        new PIXI.Rectangle(48 * 3, 48 * 3, 48, 48),
    ], [
        100, 100, 100, 100
    ], null, null, { x: 1 / 3, y: 1 / 3 }, true, true);
    // Up
    loadResource(resources.hero.texture, ["units", "hero", "moveNorth"], terrainWidth * 3, terrainHeight * 3, [
        new PIXI.Rectangle(48 * 0, 48 * 4, 48, 48),
        new PIXI.Rectangle(48 * 1, 48 * 4, 48, 48),
        new PIXI.Rectangle(48 * 2, 48 * 4, 48, 48),
        new PIXI.Rectangle(48 * 3, 48 * 4, 48, 48),
    ], [
        100, 100, 100, 100
    ], null, null, { x: 1 / 3, y: 1 / 3 });
    // Attacking
    // Down
    loadResource(resources.hero.texture, ["units", "hero", "attackSouth"], terrainWidth * 3, terrainHeight * 3, [
        new PIXI.Rectangle(48 * 0, 48 * 5, 48, 48),
        new PIXI.Rectangle(48 * 1, 48 * 5, 48, 48),
        new PIXI.Rectangle(48 * 2, 48 * 5, 48, 48),
        new PIXI.Rectangle(48 * 3, 48 * 5, 48, 48),
    ], [
        100, 100, 100, 100
    ], null, null, { x: 1 / 3, y: 1 / 3 });
    // right
    loadResource(resources.hero.texture, ["units", "hero", "attackEast"], terrainWidth * 3, terrainHeight * 3, [
        new PIXI.Rectangle(48 * 0, 48 * 6, 48, 48),
        new PIXI.Rectangle(48 * 1, 48 * 6, 48, 48),
        new PIXI.Rectangle(48 * 2, 48 * 6, 48, 48),
        new PIXI.Rectangle(48 * 3, 48 * 6, 48, 48),
    ], [
        100, 100, 100, 100
    ], null, null, { x: 1 / 3, y: 1 / 3 });
    // Left
    loadResource(resources.hero.texture, ["units", "hero", "attackWest"], terrainWidth * 3, terrainHeight * 3, [
        new PIXI.Rectangle(48 * 0, 48 * 6, 48, 48),
        new PIXI.Rectangle(48 * 1, 48 * 6, 48, 48),
        new PIXI.Rectangle(48 * 2, 48 * 6, 48, 48),
        new PIXI.Rectangle(48 * 3, 48 * 6, 48, 48),
    ], [
        100, 100, 100, 100
    ], null, null, { x: 1 / 3, y: 1 / 3 }, true, true);
    // Up
    loadResource(resources.hero.texture, ["units", "hero", "attackNorth"], terrainWidth * 3, terrainHeight * 3, [
        new PIXI.Rectangle(48 * 0, 48 * 7, 48, 48),
        new PIXI.Rectangle(48 * 1, 48 * 7, 48, 48),
        new PIXI.Rectangle(48 * 2, 48 * 7, 48, 48),
        new PIXI.Rectangle(48 * 3, 48 * 7, 48, 48),
    ], [
        100, 100, 100, 100
    ], null, null, { x: 1 / 3, y: 1 / 3 });
    // Damaged
    loadResource(resources.hero.texture, ["units", "hero", "damagedFromSouth"], terrainWidth * 3, terrainHeight * 3, [
        new PIXI.Rectangle(48 * 0, 48 * 8, 48, 48),
        new PIXI.Rectangle(48 * 1, 48 * 8, 48, 48),
        new PIXI.Rectangle(48 * 2, 48 * 8, 48, 48),
        new PIXI.Rectangle(48 * 1, 48 * 8, 48, 48),
        new PIXI.Rectangle(48 * 2, 48 * 8, 48, 48),
        new PIXI.Rectangle(48 * 0, 48 * 8, 48, 48),
    ], [
        120, 80, 80, 80, 80, 80
    ], null, null, { x: 1 / 3, y: 1 / 3 });
    loadResource(resources.hero.texture, ["units", "hero", "damagedFromEast"], terrainWidth * 3, terrainHeight * 3, [
        new PIXI.Rectangle(48 * 0, 48 * 9, 48, 48),
        new PIXI.Rectangle(48 * 1, 48 * 9, 48, 48),
        new PIXI.Rectangle(48 * 2, 48 * 9, 48, 48),
        new PIXI.Rectangle(48 * 1, 48 * 9, 48, 48),
        new PIXI.Rectangle(48 * 2, 48 * 9, 48, 48),
        new PIXI.Rectangle(48 * 0, 48 * 9, 48, 48),
    ], [
        120, 80, 80, 80, 80, 80
    ], null, null, { x: 1 / 3, y: 1 / 3 });
    loadResource(resources.hero.texture, ["units", "hero", "damagedFromWest"], terrainWidth * 3, terrainHeight * 3, [
        new PIXI.Rectangle(48 * 0, 48 * 9, 48, 48),
        new PIXI.Rectangle(48 * 1, 48 * 9, 48, 48),
        new PIXI.Rectangle(48 * 2, 48 * 9, 48, 48),
        new PIXI.Rectangle(48 * 1, 48 * 9, 48, 48),
        new PIXI.Rectangle(48 * 2, 48 * 9, 48, 48),
        new PIXI.Rectangle(48 * 0, 48 * 9, 48, 48),
    ], [
        120, 80, 80, 80, 80, 80
    ], null, null, { x: 1 / 3, y: 1 / 3 }, false, true);
    loadResource(resources.hero.texture, ["units", "hero", "damagedFromNorth"], terrainWidth * 3, terrainHeight * 3, [
        new PIXI.Rectangle(48 * 0, 48 * 10, 48, 48),
        new PIXI.Rectangle(48 * 1, 48 * 10, 48, 48),
        new PIXI.Rectangle(48 * 2, 48 * 10, 48, 48),
        new PIXI.Rectangle(48 * 1, 48 * 10, 48, 48),
        new PIXI.Rectangle(48 * 2, 48 * 10, 48, 48),
        new PIXI.Rectangle(48 * 0, 48 * 10, 48, 48),
    ], [
        120, 80, 80, 80, 80, 80
    ], null, null, { x: 1 / 3, y: 1 / 3 }, false, true);
    // Damaged short
    loadResource(resources.hero.texture, ["units", "hero", "shortDamagedFromSouth"], terrainWidth * 3, terrainHeight * 3, [
        new PIXI.Rectangle(48 * 0, 48 * 8, 48, 48),
        new PIXI.Rectangle(48 * 1, 48 * 8, 48, 48),
        new PIXI.Rectangle(48 * 2, 48 * 8, 48, 48),
        new PIXI.Rectangle(48 * 0, 48 * 8, 48, 48),
    ], [
        120, 80, 80, 80
    ], null, null, { x: 1 / 3, y: 1 / 3 });
    loadResource(resources.hero.texture, ["units", "hero", "shortDamagedFromEast"], terrainWidth * 3, terrainHeight * 3, [
        new PIXI.Rectangle(48 * 0, 48 * 9, 48, 48),
        new PIXI.Rectangle(48 * 1, 48 * 9, 48, 48),
        new PIXI.Rectangle(48 * 2, 48 * 9, 48, 48),
        new PIXI.Rectangle(48 * 0, 48 * 9, 48, 48)
    ], [
        120, 80, 80, 80
    ], null, null, { x: 1 / 3, y: 1 / 3 });
    loadResource(resources.hero.texture, ["units", "hero", "shortDamagedFromWest"], terrainWidth * 3, terrainHeight * 3, [
        new PIXI.Rectangle(48 * 0, 48 * 9, 48, 48),
        new PIXI.Rectangle(48 * 1, 48 * 9, 48, 48),
        new PIXI.Rectangle(48 * 2, 48 * 9, 48, 48),
        new PIXI.Rectangle(48 * 0, 48 * 9, 48, 48),
    ], [
        120, 80, 80, 80
    ], null, null, { x: 1 / 3, y: 1 / 3 }, false, true);
    loadResource(resources.hero.texture, ["units", "hero", "shortDamagedFromNorth"], terrainWidth * 3, terrainHeight * 3, [
        new PIXI.Rectangle(48 * 0, 48 * 10, 48, 48),
        new PIXI.Rectangle(48 * 1, 48 * 10, 48, 48),
        new PIXI.Rectangle(48 * 2, 48 * 10, 48, 48),
        new PIXI.Rectangle(48 * 0, 48 * 10, 48, 48),
    ], [
        120, 80, 80, 80
    ], null, null, { x: 1 / 3, y: 1 / 3 }, false, true);
}
function LoadMarkings(resources) {
    for (let x = 0; x < 5; x++) {
        for (let y = 0; y < 3; y++) {
            loadResource(resources.markings.texture, ["other", "zones", "zone" + (x + y * 10)], terrainWidth, terrainHeight, [
                new PIXI.Rectangle(x * 16, y * 16, 16, 16),
            ], null, 0.5);
        }
    }
}
function LoadPita(resources, name) {
    loadResourceFromSpriteSheet(resources.sprites.spritesheet, name + "_idle", ["units", name, "idle"], terrainWidth * 3, terrainHeight * 3, [
        640,
        80,
        640,
        80,
    ], null, null, { x: 1 / 3, y: 1 / 3 }, true, false, [0, 1, 2, 1], new PIXI.Rectangle(0, 0, 16, 16));
    var data1 = ["move_down", "move_up", "move_right", "move_right", "attack_down", "attack_up", "attack_right", "attack_right"];
    var data2 = ["moveSouth", "moveNorth", "moveEast", "moveWest", "attackSouth", "attackNorth", "attackEast", "attackWest"];
    var dataA = [false, false, false, true, false, false, false, true];
    for (let i = 0; i < data1.length; i++) {
        loadResourceFromSpriteSheet(resources.sprites.spritesheet, name + "_" + data1[i], ["units", name, data2[i]], terrainWidth * 3, terrainHeight * 3, [
            100,
            100,
            100,
            100,
        ], null, null, { x: 1 / 3, y: 1 / 3 }, true, dataA[i]);
    }
    var data3 = ["damaged_up", "damaged_right", "damaged_down", "damaged_right"];
    var data4 = ["damagedFromNorth", "damagedFromEast", "damagedFromSouth", "damagedFromWest"];
    var dataB = [false, false, false, true];
    // Damaged
    for (let i = 0; i < data3.length; i++) {
        loadResourceFromSpriteSheet(resources.sprites.spritesheet, name + "_" + data3[i], ["units", name, data4[i]], terrainWidth * 3, terrainHeight * 3, [
            120,
            80,
            80,
            80,
            80,
            80
        ], null, null, { x: 1 / 3, y: 1 / 3 }, true, dataB[i], [
            0, 1, 2, 1, 2, 0
        ]);
    }
    var data5 = ["damaged_up", "damaged_right", "damaged_down", "damaged_right"];
    var data6 = ["shortDamagedFromNorth", "shortDamagedFromEast", "shortDamagedFromSouth", "shortDamagedFromWest"];
    var dataC = [false, false, false, true];
    for (let i = 0; i < data5.length; i++) {
        loadResourceFromSpriteSheet(resources.sprites.spritesheet, name + "_" + data5[i], ["units", name, data6[i]], terrainWidth * 3, terrainHeight * 3, [
            120,
            80,
            80,
            80,
        ], null, null, { x: 1 / 3, y: 1 / 3 }, true, dataC[i], [
            0, 1, 2, 0
        ]);
    }
}
var gameResources = {};
gameResources.Dungeon = {};
gameResources.Hero = {};
gameResources.Maps = {};
var terrainWidth = 48;
var terrainHeight = 48;
function getResourceDestination(path) {
    let resourceFinalPath = gameResources;
    for (var i = 0; i < path.length - 1; i++) {
        var subPath = path[i];
        if (!resourceFinalPath[subPath])
            resourceFinalPath[subPath] = {};
        resourceFinalPath = resourceFinalPath[subPath];
    }
    return resourceFinalPath;
}
function getSprite(sheetTexture, rectangles, times) {
    let sprite;
    if (rectangles.length == 1) {
        var spriteTexture = new PIXI.Texture(sheetTexture, rectangles[0]);
        sprite = new PIXI.Sprite.from(spriteTexture);
    }
    else {
        var frames = [];
        for (var i = 0; i < rectangles.length; i++) {
            var spriteTexture = new PIXI.Texture(sheetTexture, rectangles[i]);
            frames.push({ texture: spriteTexture, time: times[i] });
        }
        sprite = new PIXI.AnimatedSprite(frames);
        sprite.play();
    }
    return sprite;
}
function getSpriteFromSpriteSheet(spriteSheet, name, times, frameNumbers = null) {
    if (frameNumbers == null)
        frameNumbers = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
    var maxFrameNumber = getMaxFrameNumber(spriteSheet, name);
    let sprite;
    if (times.length == 1) {
        sprite = new PIXI.Sprite.from(spriteSheet[name = ".png"]);
    }
    else {
        var frames = [];
        for (var i = 0; i < times.length; i++) {
            var spriteTexture = spriteSheet.textures[name + "_" + frameNumbers[i] + ".png"];
            frames.push({ texture: spriteTexture, time: times[i] });
        }
        sprite = new PIXI.AnimatedSprite(frames);
        sprite.play();
    }
    return sprite;
}
function loadResource(sheetTexture, path, width, height, rectangles, times = null, alpha = null, tint = null, anchor = null, loop = true, mirror = false) {
    var getSpriteFunction = () => { return getSprite(sheetTexture, rectangles, times); };
    loadResource2(getSpriteFunction, path, width, height, alpha, tint, anchor, loop, mirror);
}
function loadResourceFromSpriteSheet(spriteSheet, name, path, width, height, times = null, alpha = null, tint = null, anchor = null, loop = true, mirror = false, frameNumbers = null, hitArea = null) {
    var getSpriteFunction = () => { return getSpriteFromSpriteSheet(spriteSheet, name, times, frameNumbers); };
    loadResource2(getSpriteFunction, path, width, height, alpha, tint, anchor, loop, mirror, hitArea);
}
function loadResource2(getSprite, path, width, height, alpha = null, tint = null, anchor = null, loop = true, mirror = false, hitArea = null) {
    let destination = getResourceDestination(path);
    destination[path[path.length - 1]] = function () {
        var sprite = getSprite();
        sprite.loop = loop;
        sprite.width = width;
        sprite.height = height;
        if (anchor) {
            sprite.anchor.set(anchor.x, anchor.y);
        }
        if (mirror) {
            sprite.anchor.set(1 - sprite.anchor.x, anchor.y);
            sprite.scale.x = -sprite.scale.x;
        }
        if (tint)
            sprite.tint = tint;
        if (alpha)
            sprite.alpha = alpha;
        if (hitArea)
            sprite.hitArea = hitArea;
        return sprite;
    };
}
function getMaxFrameNumber(spritesheet, name) {
    var number = 0;
    while (spritesheet.textures[name + "_" + number + ".png"])
        number++;
    return number - 1;
}
function loadMap(resources, mapName) {
    gameResources.Maps[mapName] = function () {
        var sprite = new PIXI.Sprite(resources["maps/" + mapName].texture);
        sprite.width = sprite.width / 16 * terrainWidth;
        sprite.height = sprite.height / 16 * terrainHeight;
        return sprite;
    };
}
function setup(callback, resources, mapName) {
    loadResource(PIXI.Texture.WHITE, ["indicators", "green"], terrainWidth, terrainHeight, [new PIXI.Rectangle(0, 0, 16, 16)], null, 0.5, 0x34eb5e);
    loadResource(PIXI.Texture.WHITE, ["indicators", "red"], terrainWidth, terrainHeight, [new PIXI.Rectangle(0, 0, 16, 16)], null, 0.5, 0xfc2803);
    loadChest(resources);
    LoadPita(resources, "slime");
    LoadPita(resources, "grunt");
    LoadPita(resources, "hero");
    LoadPita(resources, "goblin");
    LoadMarkings(resources);
    // Load slime pull
    var data1 = ["attack_down", "attack_up", "attack_right", "attack_right"];
    var data2 = ["pullSouth", "pullNorth", "pullEast", "pullWest"];
    for (let i = 0; i < data1.length; i++) {
        loadResourceFromSpriteSheet(resources.sprites.spritesheet, "slime" + "_" + data1[i], ["units", "slime", data2[i]], terrainWidth * 3, terrainHeight * 3, [
            100,
            100,
            100,
            100,
        ], null, null, { x: 1 / 3, y: 1 / 3 }, false, false, [3, 2, 1, 0]);
    }
    callback();
}
//# sourceMappingURL=bundle.js.map