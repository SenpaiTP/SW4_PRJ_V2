import { useEffect, useState } from 'react';
import { createContext } from 'react';
import { ThemeProvider as MuiThemeProvider } from '@mui/material/styles';
import { lightTheme, darkTheme } from './theme';


export const ThemeContext = createContext();

const ThemeProvider = ({ children }) => {
  
  const [theme, setTheme] = useState(() => {
    // Hent tema fra localStorage, eller brug 'light' som standard
    return localStorage.getItem('theme') || 'light';
  });

  // Effekt til at hente tema fra localStorage
  useEffect(() => {
    const savedTheme = localStorage.getItem('theme');
    setTheme(savedTheme); // Sæt temaet til den gemte værdi
    
  }, []); //Når [] er tom, kørers effekten kun én gang

  useEffect(() => {
    localStorage.setItem('theme', theme); // Gem temaet i localStorage
  }, [theme]); // Denne effekt kører, når 'theme' ændrer sig

  useEffect(() => {
    // Opdater body baggrundsfarve baseret på temaets baggrund.default
    const currentTheme = theme === 'light' ? lightTheme : darkTheme;
    document.body.style.backgroundColor = currentTheme.palette.background.default;
  }, [theme]);

  const toggleTheme = () => {
    setTheme((prevTheme) => (prevTheme === 'light' ? 'dark' : 'light'));
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
