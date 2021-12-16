function LoadPita(resources, name) {


    loadResourceFromSpriteSheet(
        resources.sprites.spritesheet,
        name + "_idle", ["units", name, "idle"],
        terrainWidth * 3,
        terrainHeight * 3,
        [
            640,
            80,
            640,
            80,
        ],
        null,
        null,
        { x: 1 / 3, y: 1 / 3 },
        true,
        false,
        [0,1,2,1],
        new PIXI.Rectangle(0, 0, 16, 16)
    );

    var data1 = ["move_down",   "move_up",      "move_right",   "move_right",   "attack_down",  "attack_up",    "attack_right",     "attack_right"];
    var data2 = ["moveSouth",   "moveNorth",    "moveEast",     "moveWest",     "attackSouth",  "attackNorth",  "attackEast",       "attackWest"];
    var dataA = [false,         false,          false,          true,           false,          false,          false,              true];

    for (let i = 0; i < data1.length; i++) {
        loadResourceFromSpriteSheet(
            resources.sprites.spritesheet,
            name + "_" + data1[i], ["units", name, data2[i]],
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
            true,
            dataA[i]
        );
    }


    var data3 = ["damaged_up", "damaged_right", "damaged_down", "damaged_right"];
    var data4 = ["damagedFromNorth", "damagedFromEast", "damagedFromSouth", "damagedFromWest"];
    var dataB = [false, false, false, true];

    // Damaged
    for (let i = 0; i < data3.length; i++) {
        loadResourceFromSpriteSheet(
            resources.sprites.spritesheet,
            name + "_" + data3[i], ["units", name, data4[i]],
            terrainWidth * 3,
            terrainHeight * 3,
            [
                120,
                80,
                80,
                80,
                80,
                80
            ],
            null,
            null,
            { x: 1 / 3, y: 1 / 3 },
            true,
            dataB[i],
            [
                0,1,2,1,2,0
            ]
        );
    }

    var data5 = ["damaged_up", "damaged_right", "damaged_down", "damaged_right"];
    var data6 = ["shortDamagedFromNorth", "shortDamagedFromEast", "shortDamagedFromSouth", "shortDamagedFromWest"];
    var dataC = [false, false, false, true];

    for (let i = 0; i < data5.length; i++) {
        loadResourceFromSpriteSheet(
            resources.sprites.spritesheet,
            name + "_" + data5[i], ["units", name, data6[i]],
            terrainWidth * 3,
            terrainHeight * 3,
            [
                120,
                80,
                80,
                80,
            ],
            null,
            null,
            { x: 1 / 3, y: 1 / 3 },
            true,
            dataC[i],
            [
                0, 1, 2, 0
            ]
        );
    }
}