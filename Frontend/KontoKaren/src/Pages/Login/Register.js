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
  const [errors, setErrors] = useState({
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    confirmPassword: '',
  });

  const handleSubmit = (event) => {
    event.preventDefault();

    let formErrors = { ...errors };
    let formValid = true;

    // Reset errors
    Object.keys(formErrors).forEach((key) => formErrors[key] = '');

    // Check if all fields are filled
    if (!firstName || !lastName || !email || !password || !confirmPassword) {
      formErrors.firstName = 'First name is required';
      formErrors.lastName = 'Last name is required';
      formErrors.email = 'Email is required';
      formErrors.password = 'Password is required';
      formErrors.confirmPassword = 'Confirm password is required';
      formValid = false;
    }

    // Check if email is valid
    const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    if (email && !emailRegex.test(email)) {
      formErrors.email = 'Please enter a valid email address';
      formValid = false;
    }

    // Check if passwords match
    if (password && confirmPassword && password !== confirmPassword) {
      formErrors.confirmPassword = 'Passwords do not match';
      formValid = false;
    }

    // Check if password is at least 8 characters long and contains at least one uppercase letter
    const passwordRegex = /^(?=.*[A-Z]).{8,}$/;
    if (password && !passwordRegex.test(password)) {
      formErrors.password = 'Password must be at least 8 characters long and contain at least one uppercase letter';
      formValid = false;
    }

    setErrors(formErrors);

    // If there are errors, prevent form submission
    if (!formValid) return;

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

      {/* First Name */}
      <TextField
        label="First Name"
        value={firstName}
        onChange={(e) => setFirstName(e.target.value)}
        {...getTextFieldStyles()} // Apply the shared styles here
        error={!!errors.firstName} // Show error if there's an error for this field
        helperText={errors.firstName} // Display error message
      />
      
      {/* Last Name */}
      <TextField
        label="Last Name"
        value={lastName}
        onChange={(e) => setLastName(e.target.value)}
        {...getTextFieldStyles()} // Apply the shared styles here
        error={!!errors.lastName}
        helperText={errors.lastName}
      />

      {/* Email */}
      <TextField
        label="Email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        {...getTextFieldStyles()} // Apply the shared styles here
        error={!!errors.email}
        helperText={errors.email}
      />

      {/* Password */}
      <TextField
        label="Password"
        type="password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        {...getTextFieldStyles()} // Apply the shared styles here
        error={!!errors.password}
        helperText={errors.password || "Password must be at least 8 characters long and contain at least one uppercase letter"} // Display default message if no error
        FormHelperTextProps={{
          style: {
            color: errors.password ? 'red' : 'black', // Change color to red if error exists
          },
        }}
      />

      {/* Confirm Password */}
      <TextField
        label="Confirm Password"
        type="password"
        value={confirmPassword}
        onChange={(e) => setConfirmPassword(e.target.value)}
        {...getTextFieldStyles()} // Apply the shared styles here
        error={!!errors.confirmPassword}
        helperText={errors.confirmPassword}
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
