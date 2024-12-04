import { createTheme } from '@mui/material/styles';

export const lightTheme = createTheme({
  palette: {
    mode: 'light',
    primary: {
      main: '#1976d2',
    },
    background: {
      default: '#ffffff',
      paper: '#f5f5f5',
    },
    text: {
        primary: '#000000', // Sort til lyst tema
        secondary: '#666666',
      },
  },
});

export const darkTheme = createTheme({
  palette: {
    mode: 'dark',
    primary: {
      main: '#90caf9',
    },
    background: {
      default: '#000000',
      paper: '#333333',
    },
    text: {
        primary: '#ffffff', // Hvid til mørkt tema
        secondary: '#aaaaaa',
      },
  },
});
