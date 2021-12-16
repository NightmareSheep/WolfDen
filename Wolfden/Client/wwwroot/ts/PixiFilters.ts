var filters: any = {};

function setFilters(serializedColorFilters: string) {

    var basicColors = [0xd9d9d9, 0x9b9b9b, 0x828282, 0x747474, 0x676767, 0x151515];

    var colorFilters = JSON.parse(serializedColorFilters);
    for (var i = 0; i < colorFilters.length; i++) {
        var colorFilter = colorFilters[i];
        var colorValues = [];
        var subColors = colorFilter.Colors.split(',');
        for (var j = 0; j < subColors.length; j++) {
            colorValues.push([basicColors[j], parseInt(subColors[j], 16)]);
        }

        filters[colorFilter.Name + "Team"] = new PIXI.filters.MultiColorReplaceFilter(
            colorValues,
            0.001
        );
    }

    filters.Neutral = new PIXI.filters.MultiColorReplaceFilter(
        [
            [0xf8b878, 0xf8f898],
            [0xf89868, 0xdedede],
            [0xf85800, 0xf8c000],
            [0xf00008, 0xa5a5a5],
            [0xc00000, 0xb88000]
        ],
        0.001
    );

    filters.Inactive = new PIXI.filters.AdjustmentFilter();
    filters.Inactive.saturation = 0.1;
    filters.GlowFilter = new PIXI.filters.GlowFilter({ outerStrength: 1 });
    filters.Desaturate = new PIXI.filters.ColorMatrixFilter();
    filters.Desaturate.desaturate();

}

