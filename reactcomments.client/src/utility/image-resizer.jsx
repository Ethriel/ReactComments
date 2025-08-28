export const imageResizer = (file, maxWidth = 320, maxHeight = 240) => {
    return new Promise((resolve, reject) => {
        const img = new Image();
        const reader = new FileReader();

        reader.onload = (e) => {
            img.src = e.target.result;
        };

        img.onload = () => {
            let { width, height } = img;

            if (width > maxWidth || height > maxHeight) {
                const scale = Math.min(maxWidth / width, maxHeight / height);
                width = width * scale;
                height = height * scale;
            }

            const canvas = document.createElement("canvas");
            canvas.width = width;
            canvas.height = height;
            const ctx = canvas.getContext("2d");
            ctx.drawImage(img, 0, 0, width, height);

            canvas.toBlob(
                (blob) => {
                    if (!blob) return reject(new Error("Canvas is empty"));
                    const newFile = new File([blob], file.name, { type: file.type });
                    resolve(newFile);
                },
                file.type,
                0.95
            );
        };

        reader.onerror = (err) => reject(err);
        reader.readAsDataURL(file);
    });
};