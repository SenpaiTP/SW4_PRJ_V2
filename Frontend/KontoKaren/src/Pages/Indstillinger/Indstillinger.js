import React from 'react';
import { Container, Box, Grid, Card, CardContent, Typography, Button } from '@mui/material';
import ChangePassword from './ChangePassword'; 
import DesignFrontPage from './DesignFrontPage';
//import ThemeProvider from './ThemeProvider';
import { useContext } from 'react';
import { ThemeContext } from './ThemeProvider';

const API_URL = 'http://localhost:5168/api';

const getAuthToken = () => localStorage.getItem('authToken');

function Indstillinger() {

  const { theme, toggleTheme } = useContext(ThemeContext); // Hent temaet

  return (
    <Container>
    <Box sx={{ padding: 4 }}>
      {/* Page Title 
        color="text.primary" skal tilføjes til overskrifter for at de også skifter farve.
      */}
      <Typography variant="h4" gutterBottom color="text.primary">
        Indstillinger
      </Typography>

      {/* Grid Layout for Settings */}
      <Grid container spacing={4}>
        {/* Change Password Section */}
        <Grid item xs={12} md={6}>
          <Card variant="outlined">
            <CardContent>
              <ChangePassword />
            </CardContent>
          </Card>
        </Grid>

        {/* 
            Design Forside Section 
            <DesignFrontPage/> er link til DesignFrontpage.js
        */}
        <Grid item xs={12} md={6}>
          <Card variant="outlined">
            <CardContent>
              <Typography variant="h6" gutterBottom>
                Design Forside
              </Typography>
              <DesignFrontPage />
            </CardContent>
          </Card>
        </Grid>

        {/* Light/Dark Mode Section */}
        <Grid item xs={12} md={6}>
          <Card variant="outlined">
            <CardContent>
              <Typography variant="h6" gutterBottom>
                Light/Dark Mode
              </Typography>
              <Typography>
                Placeholder for Light/Dark Mode functionality.
              </Typography>
              <Button onClick={toggleTheme}>
               Skift til {theme === 'light' ? 'Dark' : 'Light'} Theme
               </Button>
            </CardContent>
          </Card>
        </Grid>
      </Grid>
    </Box>


</Container>
  );
}

export default Indstillinger;
