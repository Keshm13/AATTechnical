function downloadFile(filename, content, contentType) {
    const blob = new Blob([content], { type: contentType });
    const link = document.createElement("a");
    link.href = URL.createObjectURL(blob);
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}
