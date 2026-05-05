// Triggered by Blazor via JS.InvokeVoidAsync("downloadFile", fileName, contentType, base64)
window.downloadFile = function (fileName, contentType, base64) {
	const bytes = atob(base64);
	const buf = new Uint8Array(bytes.length);
	for (let i = 0; i < bytes.length; i++) buf[i] = bytes.charCodeAt(i);
	const url = URL.createObjectURL(new Blob([buf], { type: contentType }));
	Object.assign(document.createElement("a"), {
		href: url,
		download: fileName,
	}).click();
	URL.revokeObjectURL(url);
};
