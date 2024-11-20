import React from 'react';
import { Box, Typography } from '@mui/material';

function ResetPassword() {
  return (
    <Box sx={{ padding: 2, maxWidth: 400, margin: 'auto' }}>
      <Typography variant="h5" gutterBottom>
        Reset Password
      </Typography>
      <Typography variant="body1">
        Instructions to reset your password have been sent to your email.
      </Typography>
    </Box>
  );
}

export default ResetPassword;
