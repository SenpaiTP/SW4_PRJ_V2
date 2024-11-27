const buttonSizes = {
  small: {
    padding: '6px 12px',
    fontSize: '0.875rem',
    width: 'auto',
  },
  medium: {
    padding: '8px 16px', // Original size
    fontSize: '1rem',
    width: 'auto', // Keep width auto
  },
  large: {
    padding: '12px 24px',
    fontSize: '1.125rem',
    width: '100%',
  },
};

export function getButtonStyles(size) {
  return {
    ...buttonSizes[size],
  };
}
