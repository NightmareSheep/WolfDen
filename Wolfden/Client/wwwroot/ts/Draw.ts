function DrawGrass(x: number, y: number) {
    console.log("Drawing grass tile.");
    let sprite: any = gameResources.terrain.Plain();
    sprite.x = x;
    sprite.y = y;
    pixiApp.stage.addChild(sprite);
}