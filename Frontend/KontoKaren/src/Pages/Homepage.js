import React, { useState, useEffect } from 'react';
import { Container, Typography, Grid, Card, CardContent } from '@mui/material'; 
import PieChart from '../Components/IndtægterComponents/PieChart/PieChart'; // Assuming you have PieChart component

function Homepage() {
  const [settings, setSettings] = useState({
    showPieChart: false,
    showBudget: false,
    showIndtægter: false,
    showUdgifter: false,
    showSøjlediagram: false,
  });

  useEffect(() => {
    const savedSettings = localStorage.getItem('designSettings');
    if (savedSettings) {
      setSettings(JSON.parse(savedSettings)); // Load settings from localStorage
    }
  }, []);

  return (
    <Container>
      {/* Heading */}
      <Typography variant="h2" component="h2" gutterBottom color="text.primary">
        Velkommen til din forside
      </Typography>

      {/* Conditionally render sections based on the settings from localStorage */}
      {settings.showPieChart && (
        <Grid item xs={12} md={6}>
          <Card variant="outlined">
            <CardContent>
              <Typography variant="h6" gutterBottom color="text.primary">
                Jeg er et Pie Chart
              </Typography>
              <PieChart />
            </CardContent>
          </Card>
        </Grid>
      )}

      {settings.showBudget && (
        <Grid item xs={12} md={6}>
          <Card variant="outlined">
            <CardContent>
              <Typography variant="h6" gutterBottom color="text.primary">
                Jeg er et Budget
              </Typography>
            </CardContent>
          </Card>
        </Grid>
      )}

      {settings.showIndtægter && (
        <Grid item xs={12} md={6}>
          <Card variant="outlined">
            <CardContent>
              <Typography variant="h6" gutterBottom color="text.primary">
                Jeg er Indtægter
              </Typography>
            </CardContent>
          </Card>
        </Grid>
      )}

      {settings.showUdgifter && (
        <Grid item xs={12} md={6}>
          <Card variant="outlined">
            <CardContent>
              <Typography variant="h6" gutterBottom color="text.primary">
                Jeg er Udgifter
              </Typography>
            </CardContent>
          </Card>
        </Grid>
      )}

      {settings.showSøjlediagram && (
        <Grid item xs={12} md={6}>
          <Card variant="outlined">
            <CardContent>
              <Typography variant="h6" gutterBottom color="text.primary">
                Jeg er et Søjlediagram
              </Typography>
            </CardContent>
          </Card>
        </Grid>
      )}
    </Container>
  );
}

export default Homepage;
