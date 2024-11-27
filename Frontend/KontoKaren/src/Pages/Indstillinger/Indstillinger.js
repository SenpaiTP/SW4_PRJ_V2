//Indstillinger
import React from 'react';
import { Typography, Box, Grid, Card, CardContent } from '@mui/material';
import ChangePassword from './ChangePassword';

function Indstillinger() {
  return (
    <Box sx={{ padding: 4 }}>
      <Typography variant="h4" gutterBottom>
        Indstillinger
      </Typography>
      <Grid container spacing={4}>
        {/* Other settings sections can go here */}
        <Grid item xs={12} md={6}>
          <Card variant="outlined">
            <CardContent>
              <Typography variant="h6" gutterBottom>
                Change Password
              </Typography>
              <ChangePassword />
            </CardContent>
          </Card>
        </Grid>
        {/* Add additional settings sections here */}
      </Grid>
    </Box>
  );
}

export default Indstillinger;
