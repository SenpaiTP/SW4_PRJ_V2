import React, { useState } from 'react';
import { TextField, Button, Typography, Box } from '@mui/material';
import { useNavigate } from 'react-router-dom';

function ForgotPassword() {
  const [email, setEmail] = useState('');
  const navigate = useNavigate(); // Initialize the useNavigate hook

  const handleSubmit = (event) => {
    event.preventDefault();
    console.log('Email for password reset:', email);

    // Correct navigation to match the route
    navigate('/ResetPassword'); // Ensure this matches the route path in App.js
  };

  return (
    <Box
      component="form"
      onSubmit={handleSubmit}
      sx={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        padding: 2,
        maxWidth: 400,
        margin: 'auto',
        boxShadow: 3,
        borderRadius: 2,
        backgroundColor: 'white',
      }}
    >
      <Typography variant="h5" gutterBottom>
        Forgot Password
      </Typography>
      <TextField
        label="Enter your email"
        variant="outlined"
        fullWidth
        margin="normal"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
      />
      <Button variant="contained" color="primary" type="submit" fullWidth sx={{ mt: 2 }}>
        Reset Password
      </Button>
      <Typography variant="body2" sx={{ mt: 2 }}>
        Remembered your password? <a href="/login">Login Now</a>
      </Typography>
    </Box>
  );
}

export default ForgotPassword;
