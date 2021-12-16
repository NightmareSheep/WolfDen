function LoadMarkings(resources) {

    for (let x = 0; x < 5; x++) {
        for (let y = 0; y < 3; y++) {
            loadResource(resources.markings.texture,
                ["other", "zones", "zone" + (x + y * 10)],
                terrainWidth,
                terrainHeight,
                [
                    new PIXI.Rectangle(x * 16, y * 16, 16, 16),
                ],
                null,
                0.5
            );
        }
    }

    

}