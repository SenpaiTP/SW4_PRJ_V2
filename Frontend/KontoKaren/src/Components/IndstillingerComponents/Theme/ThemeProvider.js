import { useEffect, useState } from 'react';
import { createContext } from 'react';
import { ThemeProvider as MuiThemeProvider } from '@mui/material/styles';
import { lightTheme, darkTheme } from './theme';

// Context for theme management
export const ThemeContext = createContext();

const API_URL = 'http://localhost:5168/api/Indstillinger';
const getAuthToken = () => localStorage.getItem('authToken'); // Make sure you have an auth token saved in localStorage


const ThemeProvider = ({ children }) => {
  const [theme, setTheme] = useState(() => localStorage.getItem('theme') || 'light');

  useEffect(() => {
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme) {
      setTheme(savedTheme);
    } else {
      fetchThemeFromBackend();
    }
  }, []);

  const fetchThemeFromBackend = async () => {
    try {
      const token = getAuthToken();
      if (token) {
        const response = await fetch(`${API_URL}/GetTheme`, {
          method: 'GET',
          headers: {
            'Authorization': `Bearer ${token}`,
          },
        });

        if (response.ok) {
          const themeData = await response.json();
          const themeFromBackend = themeData.theme ? 'dark' : 'light'; // Assuming backend sends a boolean
          setTheme(themeFromBackend);
          localStorage.setItem('theme', themeFromBackend);
        }
      }
    } catch (error) {
      console.error('Error fetching theme from backend:', error);
      postDefaultThemeToBackend();
    }
  };

  const saveThemeToBackend = async (newTheme) => {
    try {
      const token = getAuthToken();
      const themeBoolean = newTheme === 'dark'; // Convert theme to boolean for backend
      const id = 1; // Assume your backend requires an ID
      
      if (token) {
        const response = await fetch(`${API_URL}/UpdateTheme/${id}`, {
          method: 'PUT',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`,
          },
          body: JSON.stringify({ SetTheme: themeBoolean }),
        });
          console.log('Update theme:', response);
        if (!response.ok) {
          console.error('Error saving theme to backend:', await response.text());
        }
      }
    } catch (error) {
      console.error('Error saving theme to backend:', error);
    }
  };

  const postDefaultThemeToBackend = async () => {
    try {
      const token = getAuthToken();
      if (token) {
        const response = await fetch(`${API_URL}/AddTheme`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`,
          },
          body: JSON.stringify({ SetTheme: false }), // Default theme is light (false)
        });

        if (!response.ok) {
          console.error('Error posting default theme to backend:', await response.text());
        }
      }
    } catch (error) {
      console.error('Error posting default theme to backend:', error);
    }
  };

  useEffect(() => {
    // Update the background color when the theme changes
    document.body.style.backgroundColor =
      theme === 'light'
        ? lightTheme.palette.background.default
        : darkTheme.palette.background.default;
    localStorage.setItem('theme', theme);
  }, [theme]);


  const toggleTheme = () => {
    const newTheme = theme === 'light' ? 'dark' : 'light';
    setTheme(newTheme);
    saveThemeToBackend(newTheme); // Sync with backend
    localStorage.setItem('theme', newTheme); // Sync with localStorage
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
