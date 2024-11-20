// LoginForm.js
import React, { useState } from 'react';
import { TextField, Button, Typography, Box, Checkbox, FormControlLabel } from '@mui/material';
import { Link } from 'react-router-dom';
import { getBoxStyles } from '../../Assets/Styles/boxStyles'; // Box styling
import { getTextFieldStyles } from '../../Assets/Styles/textFieldStyles'; // TextField styling

function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [rememberMe, setRememberMe] = useState(false);

  const handleSubmit = (event) => {
    event.preventDefault();
    // Handle the form submission here (e.g., send data to your backend)
    console.log('Email:', email);
    console.log('Password:', password);
    console.log('Remember Me:', rememberMe);

    if (!email || !password) {
      alert('Please fill in all fields');
      return;
    }
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
        Login
      </Typography>

      {/* Email Input */}
      <TextField
        label="Enter your email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        {...getTextFieldStyles()} // Apply the shared styles
      />

      {/* Password Input */}
      <TextField
        label="Enter your password"
        type="password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        {...getTextFieldStyles()} // Apply the shared styles
      />

      {/* Remember me checkbox */}
      <FormControlLabel
        control={
          <Checkbox
            checked={rememberMe}
            onChange={(e) => setRememberMe(e.target.checked)}
            color="primary"
          />
        }
        label="Remember me"
      />

      {/* Login button */}
      <Button variant="contained" color="primary" type="submit" fullWidth sx={{ mt: 2 }}>
        LOGIN NOW
      </Button>

      {/* Register and forgot password links */}
      <Typography variant="body2" sx={{ mt: 2 }}>
        Not a member? <Link to="/Register">Register Now</Link>
      </Typography>
      <Typography variant="body2">
        <Link to="/ForgotPassword">Forgot password?</Link>
      </Typography>
    </Box>
  );
}

export default Login;
