export function InstantiateViewport(pixiApp, elementId, worldWidth, worldHeight) {
    var element = document.getElementById(elementId);

    var viewport = new PIXI.Viewport({
        screenWidth: element.clientWidth,
        screenHeight: element.clientHeight,
        worldWidth: worldWidth,
        worldHeight: worldHeight,
        interaction: pixiApp.renderer.plugins.interaction // the interaction module is important for wheel to work properly when renderer.view is placed or scaled
    })

    return viewport;
}