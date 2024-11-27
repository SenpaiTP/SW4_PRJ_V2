import React from 'react';
import { Container, Box, Grid, Card, CardContent, Typography } from '@mui/material';
import ChangePassword from './ChangePassword'; 
import DesignFrontPage from './DesignFrontPage';

function Indstillinger() {
  return (
    <Container>
    <Box sx={{ padding: 4 }}>
      {/* Page Title */}
      <Typography variant="h4" gutterBottom>
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

        {/* Design Forside Section */}
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
            </CardContent>
          </Card>
        </Grid>
      </Grid>
    </Box>


</Container>
  );
}

export default Indstillinger;
