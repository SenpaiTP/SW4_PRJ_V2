import { useEffect, useState } from 'react';
import { createContext } from 'react';
import { ThemeProvider as MuiThemeProvider } from '@mui/material/styles';
import { lightTheme, darkTheme } from './theme';

// Context for theme management
export const ThemeContext = createContext();

const API_URL = 'http://localhost:5168/api/Indstillinger';

const ThemeProvider = ({ children }) => {
  const [theme, setTheme] = useState(() => {
    // Default to 'light' if nothing is saved in localStorage
    return localStorage.getItem('theme') || 'light';
  });

  useEffect(() => {
    // Fetch theme from localStorage, or backend if necessary
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme) {
      setTheme(savedTheme);
    } else {
      // Fetch theme from backend if not found in localStorage
      fetchThemeFromBackend();
    }
  }, []); // Only run once on mount

  const fetchThemeFromBackend = async () => {
    try {
      const token = localStorage.getItem('authToken');
      if (token) {
        const response = await fetch(`${API_URL}`, {
          method: 'GET',
          headers: {
            'Authorization': `Bearer ${token}`,
          },
        });
          console.log('response: ', response);
        if (response.ok) {
          const themeData = await response.json();
          const themeFromBackend = themeData.theme ? 'dark' : 'light'; // 'theme' is boolean in the backend
          setTheme(themeFromBackend);
          localStorage.setItem('theme', themeFromBackend); // Save theme to localStorage
        }
      }
    } catch (error) {
      console.error('Error fetching theme from backend:', error);
    }
  };

  useEffect(() => {
    // Persist theme in localStorage when it changes
    localStorage.setItem('theme', theme);
    document.body.style.backgroundColor = theme === 'light' ? lightTheme.palette.background.default : darkTheme.palette.background.default;
  }, [theme]);

  const toggleTheme = () => {
    const newTheme = theme === 'light' ? 'dark' : 'light';
    setTheme(newTheme);
  };

  const currentTheme = theme === 'light' ? lightTheme : darkTheme;

  return (
    <MuiThemeProvider theme={currentTheme}>
      <ThemeContext.Provider value={{ theme, toggleTheme }}>
        {children}
      </ThemeContext.Provider>
    </MuiThemeProvider>
  );
};

export default ThemeProvider;
