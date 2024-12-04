import React, { useState, useEffect } from 'react';
import { Container, Typography, Box, Button, Grid, Card, CardContent } from '@mui/material'; // Box and Button from Material-UI
import { getBoxStyles } from '../Assets/Styles/boxStyles'; // Adjust path if needed
import { getButtonStyles } from '../Assets/Styles/buttonStyles'; // Import button styles
import PieChart from '../Components/IndtægterComponents/PieChart/PieChart';

function Homepage() {

  const [showPieChart, setShowPieChart] = useState(false);
  const [showBudget, setShowBudget] = useState(false);
  const [showIndtægter, setShowIndtægter] = useState(false);
  const [showUdgifter, setShowUdgifter] = useState(false);
  const [showSøjlediagram, setShowSøjlediagram] = useState(false);


  useEffect(() => {
    const savedPieChart = localStorage.getItem('showPieChart') === 'true';
    const savedBudget = localStorage.getItem('showBudget') === 'true';
    const savedIndtægter = localStorage.getItem('showIndtægter') === 'true';
    const savedUdgifter = localStorage.getItem('showUdgifter') === 'true';
    const savedSøjlediagram = localStorage.getItem('showSøjlediagram') === 'true';

    setShowPieChart(savedPieChart); //default er at den er true
    setShowBudget(savedBudget); 
    setShowIndtægter(savedIndtægter);
    setShowUdgifter(savedUdgifter);
    setShowSøjlediagram(savedSøjlediagram);
}, []);

  return (
    <Container>
      {/* Heading */}
      <Typography variant="h2" component="h2" gutterBottom color="text.primary">
        Velkommen til din forside
      </Typography>

      {/* Vises kun hvis piechart er tilvalgt i indstillinger */}
      {showPieChart && (
          <Grid item xs={12} md={6}>
            <Card variant="outlined">
              <CardContent>
                <Typography variant="h6" gutterBottom color="text.primary">
                  Jeg er et Pie Chart
                </Typography>
                <PieChart/>
              </CardContent>
            </Card>
          </Grid>
        )}

      {showBudget && (
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

      {showIndtægter && (
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

        {showUdgifter && (
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

        {showSøjlediagram&& (
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
