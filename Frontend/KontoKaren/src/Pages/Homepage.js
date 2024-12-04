import React, { useState, useEffect } from 'react';
import { Container, Typography, Box, Button, Grid, Card, CardContent } from '@mui/material'; // Box and Button from Material-UI
import { getBoxStyles } from '../Assets/Styles/boxStyles'; // Adjust path if needed
import { getButtonStyles } from '../Assets/Styles/buttonStyles'; // Import button styles
import PieChart from '../Components/IndtægterComponents/PieChart/PieChart';

function Homepage() {

  const [showPieChart, setShowPieChart] = useState(false);
  const [showBudget, setShowBudget] = useState(false);

  useEffect(() => {
    const savedPieChart = localStorage.getItem('showPieChart') === 'true';
    setShowPieChart(savedPieChart);

    const savedBudget = localStorage.getItem('showBudget') === 'true';
    setShowBudget(savedBudget);
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
      
    </Container>
  );
}

export default Homepage;
