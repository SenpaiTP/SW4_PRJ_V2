//set theme:
import { useEffect, useState } from 'react';
import { Container, Box, Grid, Card, CardContent, Typography, FormControlLabel, Switch } from '@mui/material';
import { useContext } from 'react';
import { ThemeContext } from './ThemeProvider';

function SetTheme (){

    const { theme, toggleTheme } = useContext(ThemeContext); // Hent temaet

    const [checked, setChecked] = useState(() => {
        const savedChecked = localStorage.getItem('checked');
        return savedChecked ? JSON.parse(savedChecked) : false; //default er false
      });

    const handleChange = (event) => {
        setChecked(event.target.checked);
        toggleTheme(); //Skift tema
    };

    useEffect(() => {
        localStorage.setItem('checked', checked); 
      }, [checked]); 

return (
    <Box sx={{ padding: 4 }}>
    <FormControlLabel
                checked={checked}
                control={<Switch onChange={handleChange}/>}  
                label="Skift til mørk tema"
                labelPlacement="end" />
    </Box>
)        
};
export default SetTheme;