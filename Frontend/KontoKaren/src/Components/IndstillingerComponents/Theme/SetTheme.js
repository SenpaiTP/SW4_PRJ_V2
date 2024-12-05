import { useEffect, useState } from 'react';
import { FormControlLabel, Switch, Box } from '@mui/material';
import { useContext } from 'react';
import { ThemeContext } from './ThemeProvider';

const API_URL = 'http://localhost:5168/api/Indstillinger';

const getAuthToken = () => localStorage.getItem('authToken'); // Make sure you have an auth token saved in localStorage

const SetTheme = () => {
    const { theme, toggleTheme } = useContext(ThemeContext); 
    const [checked, setChecked] = useState(() => {
        const savedChecked = localStorage.getItem('checked');
        try {
            // Try to parse the saved value, defaulting to false if parsing fails
            return savedChecked ? JSON.parse(savedChecked) : false;
        } catch (e) {
            // In case JSON parsing fails (e.g., value is not valid JSON)
            return false; // Default to false
        }
    });

    const handleChange = async (event) => {
        const newChecked = event.target.checked;
        setChecked(newChecked); // Update local state
        toggleTheme(); // Toggle the theme
        console.log('newChecked: ', newChecked);

        // Update theme in the API
        const token = getAuthToken();
        if (token) {
            const response = await fetch(`${API_URL}/UpdateTheme`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify({ theme: newChecked }) // Sending the theme as a bool
            });
            console.log('body: ', response);
            if (!response.ok) {
                console.error('Failed to update theme');
            }
        }
    };

    useEffect(() => {
        // Save to localStorage when checked changes
        localStorage.setItem('checked', checked); 
    }, [checked]);

    return (
        <Box sx={{ padding: 4 }}>
            <FormControlLabel
                checked={checked}
                control={<Switch onChange={handleChange} />}
                label="Skift til mørk tema"
                labelPlacement="end"
            />
        </Box>
    );
};

export default SetTheme;
