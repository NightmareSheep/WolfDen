function toggleFullScreen() {
    try {
        if (!document.fullscreenElement) {
            document.documentElement.requestFullscreen();
        } else {
            if (document.exitFullscreen) {
                document.exitFullscreen();
            }
        }
    }
    catch {
        alert("Fullscreen failed");
    }
}