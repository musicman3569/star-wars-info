interface SectionIconProps {
    src: string; // URL or path to the icon
    alt: string; // Alternative text for the icon
}

function SectionIcon({ src, alt }: SectionIconProps) {
    return (
        <img
            src={src}
            alt={alt}
            style={{ height: '1em', marginRight: '0.5em', verticalAlign: 'middle' }}
        />
    );
}

export default SectionIcon;