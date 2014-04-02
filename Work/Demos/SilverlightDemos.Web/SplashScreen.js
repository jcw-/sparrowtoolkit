function onSourceDownloadProgressChanged(sender, eventArgs) {
    sender.findName("Progress").Text = (Math.round(eventArgs.progress * 100)).toString();
    sender.findName("progressbar").ScaleX = 1 - (Math.round(eventArgs.progress * 100)) / 100;
}

