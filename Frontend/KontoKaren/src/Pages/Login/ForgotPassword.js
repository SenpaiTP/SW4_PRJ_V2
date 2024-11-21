// ForgotPassword.js
import React, { useState } from 'react';
import { TextField, Button, Typography, Box } from '@mui/material';
import { Link, useNavigate } from 'react-router-dom'; // Import useNavigate here
import { getBoxStyles } from '../../Assets/Styles/boxStyles';
import { getTextFieldStyles } from '../../Assets/Styles/textFieldStyles'; // Import the textFieldStyles

function ForgotPassword() {
  const [email, setEmail] = useState('');
  const navigate = useNavigate(); // Initialize the useNavigate hook

  const handleSubmit = (event) => {
    event.preventDefault();
    console.log('Email for password reset:', email);

    // Navigate to the ResetPassword page after submitting
    navigate('/ResetPassword'); // Ensure this matches the route path in App.js
  };

  return (
    <Box
      component="form"
      onSubmit={handleSubmit}
      sx={{
        ...getBoxStyles('medium'), // Dynamically get the medium box styles
      }}
    >
      <Typography variant="h5" gutterBottom>
        Forgot Password
      </Typography>

      {/* Email Input with shared TextField styles */}
      <TextField
        label="Enter your email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        {...getTextFieldStyles()} // Apply the shared styles
      />

      <Button variant="contained" color="primary" type="submit" fullWidth sx={{ mt: 2 }}>
        Reset Password
      </Button>

      <Typography variant="body2" sx={{ mt: 2 }}>
        Remembered your password? <Link to="/login">Login Now</Link>
      </Typography>
    </Box>
  );
}

export default ForgotPassword;
