export function InstantiateViewport(pixiApp, elementId, worldWidth, worldHeight) {
    var element = document.getElementById(elementId);

    var viewport = new PIXI.Viewport({
        screenWidth: element.clientWidth,
        screenHeight: element.clientHeight,
        worldWidth: worldWidth,
        worldHeight: worldHeight,
        interaction: pixiApp.renderer.plugins.interaction // the interaction module is important for wheel to work properly when renderer.view is placed or scaled
    });

    viewport.clamp({ left: -50, right: viewport.worldWidth + 50, top: -50, bottom: viewport.worldHeight + 50 }, "all", "none");

    window.onresize = () => {
        viewport.resize();
    }

    viewport.fit();

    viewport.on("drag-start", () => {
        viewport.interactiveChildren = false;
    });
    viewport.on("drag-end", () => {
        viewport.interactiveChildren = true;
    });


    return viewport;
}