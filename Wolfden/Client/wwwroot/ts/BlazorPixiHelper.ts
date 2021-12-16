

class BlazorPixiHelper {
    Initialize() {
        Sprites = {};
        CsObjects = {};
    }

    CreateSprite(resourcePath: string[], id: string, x: number, y: number, clickCsObjRef: any, hoverCsObjRef: any, visible: boolean = true, tint = null) {

        let completeResourcePath = "";       

        let resource = gameResources;
        for (let path of resourcePath) {
            resource = resource[path];
            completeResourcePath += path;
        }

        console.log("Draw: " + completeResourcePath + " with id: " + id + " at(X: " + x + ", Y: " + y + ")");

        let sprite: any = resource();
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

    SetTextSprite(id: string, x: number, y: number, text: string = null, visible: boolean = true, containerId: string = null, textStyle: number = 0) {
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

    DestroySprite(id: string) {
        console.log("Destroy sprite with id: " + id);
        try {
            let sprite: any = Sprites[id];

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

    SetPositionOfSprite(id: string, x: number, y: number) {
        console.log("Changing position of " + id + "To (X: " + x + ",Y " + y + ")");

        let sprite: any = Sprites[id];

        if (!sprite)
            return;

        sprite.x = x;
        sprite.y = y;
    }

    SetVisibleOfSprite(id: string, visible: boolean) {
        let sprite: any = Sprites[id];
        if (sprite)
            sprite.visible = visible;
    }

    SetFilter(id: string, filterName: string, apply: boolean) {
        let sprite: any = Sprites[id];
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

