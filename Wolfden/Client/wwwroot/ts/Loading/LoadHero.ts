function LoadHero(resources) {

    // Idle
    loadResource(resources.hero.texture,
        ["units", "hero", "idle"],
        terrainWidth,
        terrainHeight,
        [
            new PIXI.Rectangle(16, 16, 16, 16),
            new PIXI.Rectangle(16 * 4, 16, 16, 16),
            new PIXI.Rectangle(16 * 7, 16, 16, 16),
            new PIXI.Rectangle(16 * 4, 16, 16, 16),
        ],
        [
            640,
            80,
            640,
            80,
        ]
    );

    // Walking
    loadResource(resources.hero.texture,
        ["units", "hero", "moveSouth"],
        terrainWidth * 3,
        terrainHeight * 3,
        [
            new PIXI.Rectangle(48 * 0, 48 * 2, 48, 48),
            new PIXI.Rectangle(48 * 1, 48 * 2, 48, 48),
            new PIXI.Rectangle(48 * 2, 48 * 2, 48, 48),
            new PIXI.Rectangle(48 * 3, 48 * 2, 48, 48),
        ],
        [
            100, 100, 100, 100
        ],
        null,
        null,
        { x: 1 / 3, y: 1 / 3 }
    );

    // Right
    loadResource(resources.hero.texture,
        ["units", "hero", "moveEast"],
        terrainWidth * 3,
        terrainHeight * 3,
        [
            new PIXI.Rectangle(48 * 0, 48 * 3, 48, 48),
            new PIXI.Rectangle(48 * 1, 48 * 3, 48, 48),
            new PIXI.Rectangle(48 * 2, 48 * 3, 48, 48),
            new PIXI.Rectangle(48 * 3, 48 * 3, 48, 48),
        ],
        [
            100, 100, 100, 100
        ],
        null,
        null,
        { x: 1 / 3, y: 1 / 3 }
    );


    // Left
    loadResource(resources.hero.texture,
        ["units", "hero", "moveWest"],
        terrainWidth * 3,
        terrainHeight * 3,
        [
            new PIXI.Rectangle(48 * 0, 48 * 3, 48, 48),
            new PIXI.Rectangle(48 * 1, 48 * 3, 48, 48),
            new PIXI.Rectangle(48 * 2, 48 * 3, 48, 48),
            new PIXI.Rectangle(48 * 3, 48 * 3, 48, 48),
        ],
        [
            100, 100, 100, 100
        ],
        null,
        null,
        { x: 1 / 3, y: 1 / 3 },
        true,
        true
    );

    // Up
    loadResource(resources.hero.texture,
        ["units", "hero", "moveNorth"],
        terrainWidth * 3,
        terrainHeight * 3,
        [
            new PIXI.Rectangle(48 * 0, 48 * 4, 48, 48),
            new PIXI.Rectangle(48 * 1, 48 * 4, 48, 48),
            new PIXI.Rectangle(48 * 2, 48 * 4, 48, 48),
            new PIXI.Rectangle(48 * 3, 48 * 4, 48, 48),
        ],
        [
            100, 100, 100, 100
        ],
        null,
        null,
        { x: 1 / 3, y: 1 / 3 }
    );

    // Attacking

    // Down
    loadResource(resources.hero.texture,
        ["units", "hero", "attackSouth"],
        terrainWidth * 3,
        terrainHeight * 3,
        [
            new PIXI.Rectangle(48 * 0, 48 * 5, 48, 48),
            new PIXI.Rectangle(48 * 1, 48 * 5, 48, 48),
            new PIXI.Rectangle(48 * 2, 48 * 5, 48, 48),
            new PIXI.Rectangle(48 * 3, 48 * 5, 48, 48),
        ],
        [
            100, 100, 100, 100
        ],
        null,
        null,
        { x: 1 / 3, y: 1 / 3 }
    );

    // right
    loadResource(resources.hero.texture,
        ["units", "hero", "attackEast"],
        terrainWidth * 3,
        terrainHeight * 3,
        [
            new PIXI.Rectangle(48 * 0, 48 * 6, 48, 48),
            new PIXI.Rectangle(48 * 1, 48 * 6, 48, 48),
            new PIXI.Rectangle(48 * 2, 48 * 6, 48, 48),
            new PIXI.Rectangle(48 * 3, 48 * 6, 48, 48),
        ],
        [
            100, 100, 100, 100
        ],
        null,
        null,
        { x: 1 / 3, y: 1 / 3 }
    );

    // Left
    loadResource(resources.hero.texture,
        ["units", "hero", "attackWest"],
        terrainWidth * 3,
        terrainHeight * 3,
        [
            new PIXI.Rectangle(48 * 0, 48 * 6, 48, 48),
            new PIXI.Rectangle(48 * 1, 48 * 6, 48, 48),
            new PIXI.Rectangle(48 * 2, 48 * 6, 48, 48),
            new PIXI.Rectangle(48 * 3, 48 * 6, 48, 48),
        ],
        [
            100, 100, 100, 100
        ],
        null,
        null,
        { x: 1 / 3, y: 1 / 3 },
        true,
        true
    );

    // Up
    loadResource(resources.hero.texture,
        ["units", "hero", "attackNorth"],
        terrainWidth * 3,
        terrainHeight * 3,
        [
            new PIXI.Rectangle(48 * 0, 48 * 7, 48, 48),
            new PIXI.Rectangle(48 * 1, 48 * 7, 48, 48),
            new PIXI.Rectangle(48 * 2, 48 * 7, 48, 48),
            new PIXI.Rectangle(48 * 3, 48 * 7, 48, 48),
        ],
        [
            100, 100, 100, 100
        ],
        null,
        null,
        { x: 1 / 3, y: 1 / 3 }
    );

    // Damaged

    loadResource(resources.hero.texture,
        ["units", "hero", "damagedFromSouth"],
        terrainWidth * 3,
        terrainHeight * 3,
        [
            new PIXI.Rectangle(48 * 0, 48 * 8, 48, 48),
            new PIXI.Rectangle(48 * 1, 48 * 8, 48, 48),
            new PIXI.Rectangle(48 * 2, 48 * 8, 48, 48),
            new PIXI.Rectangle(48 * 1, 48 * 8, 48, 48),
            new PIXI.Rectangle(48 * 2, 48 * 8, 48, 48),
            new PIXI.Rectangle(48 * 0, 48 * 8, 48, 48),
        ],
        [
            120, 80, 80, 80, 80, 80
        ],
        null,
        null,
        { x: 1 / 3, y: 1 / 3 }
    );

    loadResource(resources.hero.texture,
        ["units", "hero", "damagedFromEast"],
        terrainWidth * 3,
        terrainHeight * 3,
        [
            new PIXI.Rectangle(48 * 0, 48 * 9, 48, 48),
            new PIXI.Rectangle(48 * 1, 48 * 9, 48, 48),
            new PIXI.Rectangle(48 * 2, 48 * 9, 48, 48),
            new PIXI.Rectangle(48 * 1, 48 * 9, 48, 48),
            new PIXI.Rectangle(48 * 2, 48 * 9, 48, 48),
            new PIXI.Rectangle(48 * 0, 48 * 9, 48, 48),
        ],
        [
            120, 80, 80, 80, 80, 80
        ],
        null,
        null,
        { x: 1 / 3, y: 1 / 3 }
    );

    loadResource(resources.hero.texture,
        ["units", "hero", "damagedFromWest"],
        terrainWidth * 3,
        terrainHeight * 3,
        [
            new PIXI.Rectangle(48 * 0, 48 * 9, 48, 48),
            new PIXI.Rectangle(48 * 1, 48 * 9, 48, 48),
            new PIXI.Rectangle(48 * 2, 48 * 9, 48, 48),
            new PIXI.Rectangle(48 * 1, 48 * 9, 48, 48),
            new PIXI.Rectangle(48 * 2, 48 * 9, 48, 48),
            new PIXI.Rectangle(48 * 0, 48 * 9, 48, 48),
        ],
        [
            120, 80, 80, 80, 80, 80
        ],
        null,
        null,
        { x: 1 / 3, y: 1 / 3 },
        false,
        true
    );

    loadResource(resources.hero.texture,
        ["units", "hero", "damagedFromNorth"],
        terrainWidth * 3,
        terrainHeight * 3,
        [
            new PIXI.Rectangle(48 * 0, 48 * 10, 48, 48),
            new PIXI.Rectangle(48 * 1, 48 * 10, 48, 48),
            new PIXI.Rectangle(48 * 2, 48 * 10, 48, 48),
            new PIXI.Rectangle(48 * 1, 48 * 10, 48, 48),
            new PIXI.Rectangle(48 * 2, 48 * 10, 48, 48),
            new PIXI.Rectangle(48 * 0, 48 * 10, 48, 48),
        ],
        [
            120, 80, 80, 80, 80, 80
        ],
        null,
        null,
        { x: 1 / 3, y: 1 / 3 },
        false,
        true
    );

    // Damaged short
    loadResource(resources.hero.texture,
        ["units", "hero", "shortDamagedFromSouth"],
        terrainWidth * 3,
        terrainHeight * 3,
        [
            new PIXI.Rectangle(48 * 0, 48 * 8, 48, 48),
            new PIXI.Rectangle(48 * 1, 48 * 8, 48, 48),
            new PIXI.Rectangle(48 * 2, 48 * 8, 48, 48),
            new PIXI.Rectangle(48 * 0, 48 * 8, 48, 48),
        ],
        [
            120, 80, 80, 80
        ],
        null,
        null,
        { x: 1 / 3, y: 1 / 3 }
    );

    loadResource(resources.hero.texture,
        ["units", "hero", "shortDamagedFromEast"],
        terrainWidth * 3,
        terrainHeight * 3,
        [
            new PIXI.Rectangle(48 * 0, 48 * 9, 48, 48),
            new PIXI.Rectangle(48 * 1, 48 * 9, 48, 48),
            new PIXI.Rectangle(48 * 2, 48 * 9, 48, 48),
            new PIXI.Rectangle(48 * 0, 48 * 9, 48, 48)
        ],
        [
            120, 80, 80, 80
        ],
        null,
        null,
        { x: 1 / 3, y: 1 / 3 }
    );

    loadResource(resources.hero.texture,
        ["units", "hero", "shortDamagedFromWest"],
        terrainWidth * 3,
        terrainHeight * 3,
        [
            new PIXI.Rectangle(48 * 0, 48 * 9, 48, 48),
            new PIXI.Rectangle(48 * 1, 48 * 9, 48, 48),
            new PIXI.Rectangle(48 * 2, 48 * 9, 48, 48),
            new PIXI.Rectangle(48 * 0, 48 * 9, 48, 48),
        ],
        [
            120, 80, 80, 80
        ],
        null,
        null,
        { x: 1 / 3, y: 1 / 3 },
        false,
        true
    );

    loadResource(resources.hero.texture,
        ["units", "hero", "shortDamagedFromNorth"],
        terrainWidth * 3,
        terrainHeight * 3,
        [
            new PIXI.Rectangle(48 * 0, 48 * 10, 48, 48),
            new PIXI.Rectangle(48 * 1, 48 * 10, 48, 48),
            new PIXI.Rectangle(48 * 2, 48 * 10, 48, 48),
            new PIXI.Rectangle(48 * 0, 48 * 10, 48, 48),
        ],
        [
            120, 80, 80, 80
        ],
        null,
        null,
        { x: 1 / 3, y: 1 / 3 },
        false,
        true
    );
}