import React from 'react';
import { Container, Typography, Box, Button } from '@mui/material'; // Box and Button from Material-UI
import { getBoxStyles } from '../Assets/Styles/boxStyles'; // Adjust path if needed
import { getButtonStyles } from '../Assets/Styles/buttonStyles'; // Import button styles

function Homepage() {
  return (
    <Container>
      {/* Heading */}
      <Typography variant="h1" component="h2" gutterBottom>
        Welcome to the Homepage
      </Typography>

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
