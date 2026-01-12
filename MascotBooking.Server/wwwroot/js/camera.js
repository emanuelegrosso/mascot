// Camera capture functionality for Blazor
window.captureFromCamera = async function () {
    try {
        // Check if getUserMedia is available
        if (!navigator.mediaDevices || !navigator.mediaDevices.getUserMedia) {
            throw new Error('Camera not available');
        }

        // Create video element
        const video = document.createElement('video');
        video.style.position = 'fixed';
        video.style.top = '50%';
        video.style.left = '50%';
        video.style.transform = 'translate(-50%, -50%)';
        video.style.zIndex = '10000';
        video.style.width = '640px';
        video.style.maxWidth = '90vw';
        video.style.border = '3px solid #1e3a8a';
        video.style.borderRadius = '8px';
        video.autoplay = true;
        video.playsInline = true;

        // Create overlay
        const overlay = document.createElement('div');
        overlay.style.position = 'fixed';
        overlay.style.top = '0';
        overlay.style.left = '0';
        overlay.style.width = '100%';
        overlay.style.height = '100%';
        overlay.style.backgroundColor = 'rgba(0, 0, 0, 0.8)';
        overlay.style.zIndex = '9999';
        overlay.style.display = 'flex';
        overlay.style.flexDirection = 'column';
        overlay.style.alignItems = 'center';
        overlay.style.justifyContent = 'center';
        overlay.style.gap = '20px';

        // Create buttons container
        const buttonsContainer = document.createElement('div');
        buttonsContainer.style.display = 'flex';
        buttonsContainer.style.gap = '10px';

        // Capture button
        const captureBtn = document.createElement('button');
        captureBtn.textContent = '📷 Cattura';
        captureBtn.className = 'btn btn-primary';
        captureBtn.style.padding = '12px 24px';
        captureBtn.style.fontSize = '16px';

        // Cancel button
        const cancelBtn = document.createElement('button');
        cancelBtn.textContent = '❌ Annulla';
        cancelBtn.className = 'btn btn-secondary';
        cancelBtn.style.padding = '12px 24px';
        cancelBtn.style.fontSize = '16px';

        buttonsContainer.appendChild(captureBtn);
        buttonsContainer.appendChild(cancelBtn);
        overlay.appendChild(video);
        overlay.appendChild(buttonsContainer);
        document.body.appendChild(overlay);

        // Get user media
        const stream = await navigator.mediaDevices.getUserMedia({ 
            video: { 
                facingMode: 'environment', // Back camera
                width: { ideal: 1280 },
                height: { ideal: 720 }
            } 
        });
        video.srcObject = stream;

        return new Promise((resolve, reject) => {
            captureBtn.onclick = () => {
                // Create canvas
                const canvas = document.createElement('canvas');
                canvas.width = video.videoWidth;
                canvas.height = video.videoHeight;
                const ctx = canvas.getContext('2d');
                ctx.drawImage(video, 0, 0);

                // Convert to base64
                const imageData = canvas.toDataURL('image/jpeg', 0.8);

                // Stop stream
                stream.getTracks().forEach(track => track.stop());
                overlay.remove();

                resolve(imageData);
            };

            cancelBtn.onclick = () => {
                stream.getTracks().forEach(track => track.stop());
                overlay.remove();
                reject(new Error('Cancelled by user'));
            };

            // Close on escape
            const escapeHandler = (e) => {
                if (e.key === 'Escape') {
                    stream.getTracks().forEach(track => track.stop());
                    overlay.remove();
                    document.removeEventListener('keydown', escapeHandler);
                    reject(new Error('Cancelled by user'));
                }
            };
            document.addEventListener('keydown', escapeHandler);
        });
    } catch (error) {
        console.error('Camera error:', error);
        throw error;
    }
};
