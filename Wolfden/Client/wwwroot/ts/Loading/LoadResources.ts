var gameResources: any = {};
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
    var getSpriteFunction = () => { return getSpriteFromSpriteSheet(spriteSheet, name, times, frameNumbers) };
    loadResource2(getSpriteFunction, path, width, height, alpha, tint, anchor, loop, mirror, hitArea);
}

function loadResource2(getSprite, path, width, height, alpha = null, tint = null, anchor = null, loop = true, mirror = false, hitArea = null){
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
            sprite.alpha = alpha

        if (hitArea)
            sprite.hitArea = hitArea;
        return sprite;
    }
}

function getMaxFrameNumber(spritesheet, name) {
    var number = 0;
    while (spritesheet.textures[name + "_" + number + ".png"])
        number++
    return number - 1;
}

function loadMap(resources, mapName: string) {
    gameResources.Maps[mapName] = function () {
        var sprite = new PIXI.Sprite(resources["maps/" + mapName].texture);
        sprite.width = sprite.width / 16 * terrainWidth;
        sprite.height = sprite.height / 16 * terrainHeight;
        return sprite;
    };
}

function setup(callback: () => void, resources, mapName: string) {

    

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
        loadResourceFromSpriteSheet(
            resources.sprites.spritesheet,
            "slime" + "_" + data1[i], ["units", "slime", data2[i]],
            terrainWidth * 3,
            terrainHeight * 3,
            [
                100,
                100,
                100,
                100,
            ],
            null,
            null,
            { x: 1 / 3, y: 1 / 3 },
            false,
            false,
            [3,2,1,0]
        );
    }

    callback();
}

