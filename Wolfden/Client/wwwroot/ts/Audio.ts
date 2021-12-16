declare var Howl: any;
declare var Howler: any;

var sounds = {};
window["sounds"] = sounds;

function addSound(name, src, loop = true, duration: number = 0, intro: number = 0, outro: number = 0) {
    let sprite = undefined;
    if (duration)
        sprite = {
            intro: [0, intro],
            loop: [intro, outro - intro, 1],
            outro: [outro, duration - outro]
        }

    let sound = new Howl({
        src: src,
        loop: loop,
        volume: 0.2,
        sprite: sprite
    });

    sound.name = name;
    sounds[name] = sound;
    sound.on("fade", function () {
        sound.stop();
        console.log("sound has stopped: " + name);
    });
    console.log("Added track: " + name);
}

function setMasterVolume(volume: number) {
    Howler.volume(volume / 100);
}

function playSound(name) {
    sounds[name].play();
}

function playMusic(name: string, volume: number) {
    let sound = sounds[name];
    sound.volume(volume / 100);

    if (!sound.playing()) {

        if (sound._sprite.intro[1] == 0) {
            sound.play("loop");
            return;
        }

        var introId = sound.play("intro");
        sound.once("end", function (endId) {
            if (endId == introId) {
                sound.play("loop");
            }
        });
    }
}

function stopSound(name, volume: number) {
    var sound = sounds[name];
    sound.volume(volume / 100);

    if (sound.playing()) {
        sound.fade(0.2, 0, 500);
        console.log("stop sound: " + name);
    }
}