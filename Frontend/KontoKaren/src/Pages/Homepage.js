import React, { useState, useEffect } from 'react';
import { Container, Typography, Box, Button, Grid, Card, CardContent } from '@mui/material'; // Box and Button from Material-UI
import { getBoxStyles } from '../Assets/Styles/boxStyles'; // Adjust path if needed
import { getButtonStyles } from '../Assets/Styles/buttonStyles'; // Import button styles
import PieChart from '../Pages/Indtægter/PieChart';

function Homepage() {

  const [showPieChart, setShowPieChart] = useState(false);

  useEffect(() => {
    const savedPieChart = localStorage.getItem('showPieChart') === 'true';
    setShowPieChart(savedPieChart);
}, []);

  return (
    <Container>
      {/* Heading */}
      <Typography variant="h1" component="h2" gutterBottom color="text.primary">
        Welcome to the Homepage
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

      {/* Small Box */}
      <Box sx={getBoxStyles('small')} mb={2}>
        <Typography variant="body1">This is a small box.</Typography>
      </Box>

      {/* Medium Box */}
      <Box sx={getBoxStyles('medium')} mb={2}>
        <Typography variant="body1">This is a medium box.</Typography>
      </Box>

      {/* Large Box */}
      <Box sx={getBoxStyles('large')} mb={2}>
        <Typography variant="body1">This is a large box.</Typography>
      </Box>

      {/* Small Button */}
      <Button
        variant="contained"
        color="primary"
        sx={{ ...getButtonStyles('small'), mt: 2 }} // Apply small button styles
      >
        Small Button
      </Button>

      {/* Medium Button */}
      <Button
        variant="contained"
        color="primary"
        sx={{ ...getButtonStyles('medium'), mt: 2 }} // Apply medium button styles
      >
        Medium Button
      </Button>

      {/* Large Button */}
      <Button
        variant="contained"
        color="primary"
        sx={{ ...getButtonStyles('large'), mt: 2 }} // Apply large button styles
      >
        Large Button
      </Button>
    </Container>
  );
}

export default Homepage;
