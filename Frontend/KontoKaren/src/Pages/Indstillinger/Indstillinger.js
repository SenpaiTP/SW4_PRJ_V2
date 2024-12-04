import React from 'react';
import { Container, Box, Grid, Card, CardContent, Typography, FormControlLabel, Switch } from '@mui/material';
import ChangePassword from '../../Components/IndstillingerComponents/ChangePassword/ChangePassword'; 
import DesignFrontPage from '../../Components/IndstillingerComponents/DesignFrontpage/DesignFrontPage';
import SetTheme from '../../Components/IndstillingerComponents/Theme/SetTheme';
import ThemeProvider from '../../Components/IndstillingerComponents/Theme/ThemeProvider';
import { useContext } from 'react';
import { ThemeContext } from '../../Components/IndstillingerComponents/Theme/ThemeProvider';

/*const API_URL = 'http://localhost:5168/api';

const getAuthToken = () => localStorage.getItem('authToken');*/

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
                Design din forside
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
                Mørk tilstand
              </Typography>
              <SetTheme />
            </CardContent>
          </Card>
        </Grid>
      </Grid>
    </Box>


</Container>
  );
}

export default Indstillinger;
