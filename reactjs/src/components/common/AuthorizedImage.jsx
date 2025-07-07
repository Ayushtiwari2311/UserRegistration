// components/common/AuthorizedImage.jsx
import React, { useEffect, useState } from 'react';

const AuthorizedImage = ({ src, alt, onClick, className }) => {
    const [imageUrl, setImageUrl] = useState(null);

    useEffect(() => {
        const fetchImage = async () => {
            try {
                const response = await fetch(src);
                if (!response.ok) throw new Error('Image fetch failed');

                const blob = await response.blob();
                const url = URL.createObjectURL(blob);
                setImageUrl(url);
            } catch (error) {
                console.error('Image error:', error);
            }
        };

        fetchImage();

        return () => {
            if (imageUrl) URL.revokeObjectURL(imageUrl);
        };
    }, [src]);

    if (!imageUrl) {
        return <div className="w-10 h-10 bg-gray-200 rounded-full animate-pulse" />;
    }

    return <img src={imageUrl} alt={alt} onClick={onClick} className={className} />;
};

export default AuthorizedImage;
