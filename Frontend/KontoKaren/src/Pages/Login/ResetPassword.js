import React from 'react';
import { Box, Typography } from '@mui/material';

function ResetPassword() {
  return (
    <Box sx={{ padding: 2, maxWidth: 400, margin: 'auto' }}>
      <Typography variant="h5" gutterBottom>
        Nulstil adgangskode
      </Typography>
      <Typography variant="body1">
        Instruktioner til at nulstille din adgangskode er sendt til din email.
      </Typography>
    </Box>
  );
}

export default ResetPassword;
