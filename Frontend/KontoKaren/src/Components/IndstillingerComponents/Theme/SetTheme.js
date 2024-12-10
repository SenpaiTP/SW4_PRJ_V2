import { useContext } from 'react';
import { FormControlLabel, Switch, Box } from '@mui/material';
import { ThemeContext } from './ThemeProvider';

const SetTheme = () => {
  const { theme, toggleTheme } = useContext(ThemeContext);

  const handleChange = () => {
    toggleTheme();
  };

  return (
    <Box sx={{ padding: 4 }}>
      <FormControlLabel
        control={<Switch checked={theme === 'dark'} onChange={handleChange} />}
        label="Skift til mørk tema"
        labelPlacement="end"
      />
    </Box>
  );
};

export default SetTheme;
