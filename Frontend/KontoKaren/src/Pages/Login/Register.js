// SignupForm.js (or Register.js)
import React, { useState } from 'react';
import { TextField, Button, Typography, Box } from '@mui/material';
import { Link } from 'react-router-dom';
import { getBoxStyles } from '../../Assets/Styles/boxStyles';
import { getTextFieldStyles } from '../../Assets/Styles/textFieldStyles'; // Import textFieldStyles

function Register() {
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');

  const handleSubmit = (event) => {
    event.preventDefault();

    // Check if all fields are filled
    if (!firstName || !lastName || !email || !password || !confirmPassword) {
      alert("Please fill in all fields");
      return;
    }

    // Check if passwords match
    if (password !== confirmPassword) {
      alert("Passwords do not match");
      return;
    }

    // If all validations pass, handle form submission
    console.log('First Name:', firstName);
    console.log('Last Name:', lastName);
    console.log('Email:', email);
    console.log('Password:', password);

    // Here you would send the form data to your backend
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
        Sign Up
      </Typography>

      {/* Apply shared styles to the TextFields */}
      <TextField
        label="First Name"
        value={firstName}
        onChange={(e) => setFirstName(e.target.value)}
        {...getTextFieldStyles()} // Apply the shared styles here
      />
      <TextField
        label="Last Name"
        value={lastName}
        onChange={(e) => setLastName(e.target.value)}
        {...getTextFieldStyles()} // Apply the shared styles here
      />
      <TextField
        label="Email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        {...getTextFieldStyles()} // Apply the shared styles here
      />
      <TextField
        label="Password"
        type="password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        {...getTextFieldStyles()} // Apply the shared styles here
      />
      <TextField
        label="Confirm Password"
        type="password"
        value={confirmPassword}
        onChange={(e) => setConfirmPassword(e.target.value)}
        {...getTextFieldStyles()} // Apply the shared styles here
      />

      <Button variant="contained" color="primary" type="submit" fullWidth sx={{ mt: 2 }}>
        Sign Up Now
      </Button>

      <Typography variant="body2" sx={{ mt: 2 }}>
        Already have an account? <Link to="/login">Login Here</Link>
      </Typography>
    </Box>
  );
}

export default Register;
