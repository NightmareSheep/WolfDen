function loadChest(resources) {

    // Idle
    loadResource(resources.spritesheet.texture,
        ["objects", "chest", "idle"],
        terrainWidth,
        terrainHeight,
        [
            new PIXI.Rectangle(224, 256, 16, 16),
        ],
    );

    // Opening
    loadResourceFromSpriteSheet(resources.sprites.spritesheet,
        "chest_opening",
        ["objects", "chest", "opening"],
        terrainWidth * 3,
        terrainHeight * 3,
        [
            2, 80, 80, 640
        ],
        null,
        null,
        { x: 1 / 3, y: 1 / 3 },
        false
    );
}