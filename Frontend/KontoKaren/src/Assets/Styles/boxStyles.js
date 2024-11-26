
const boxSizes = {
    small: {
        padding: 1,
        maxWidth: 200,
    },
    
    medium: {
        padding: 2,
        maxWidth: 400,
    },
    large: {
        padding: 3,
        maxWidth: 600,
    },
};

export function getBoxStyles(size) {
    return {
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        margin: 'auto',
        boxShadow: 3,
        borderRadius: 2,
        backgroundColor: 'white',
        ...boxSizes[size],
    };
}
