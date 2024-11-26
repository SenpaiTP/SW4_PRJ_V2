import React, { useState } from 'react';
import { TextField, Button, Typography, Box, FormControlLabel, Checkbox } from '@mui/material';
import { Link, useNavigate } from 'react-router-dom'; // Import useNavigate
import { getBoxStyles } from '../../Assets/Styles/boxStyles';
import { getTextFieldStyles } from '../../Assets/Styles/textFieldStyles'; // Import textFieldStyles

function Register() {
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false); // State for password visibility
  const [errors, setErrors] = useState({
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    confirmPassword: '',
  });
  const [loading, setLoading] = useState(false); // Loading state for button
  const navigate = useNavigate(); // Hook to redirect the user

  const handleSubmit = async (event) => {
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
    setLoading(true); // Start loading
    try {
      // Make a POST request to your backend for registration
      const response = await fetch('http://localhost:5168/api/Bruger', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ firstName, lastName, email, password }),
      });

      const data = await response.json();

      // Check if registration is successful (you can adjust this check based on your backend response)
      if (response.ok) {
        // If registration is successful, redirect to login page
        navigate('/login');
      } else {
        // If registration fails, show error message (you can adjust based on your backend response)
        setErrors({ ...errors, email: data.message || 'Registration failed' });
      }
    } catch (error) {
      console.error('Registration failed:', error);
      setErrors({ ...errors, email: 'An error occurred. Please try again.' });
    } finally {
      setLoading(false); // Stop loading
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
        type={showPassword ? 'text' : 'password'} // Toggle between text and password
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
        type={showPassword ? 'text' : 'password'} // Toggle between text and password
        value={confirmPassword}
        onChange={(e) => setConfirmPassword(e.target.value)}
        {...getTextFieldStyles()} // Apply the shared styles here
        error={!!errors.confirmPassword}
        helperText={errors.confirmPassword}
      />

      {/* Show Password Checkbox */}
      <FormControlLabel
        control={
          <Checkbox
            checked={showPassword}
            onChange={(e) => setShowPassword(e.target.checked)}
            color="primary"
          />
        }
        label="Show Password"
      />

      <Button variant="contained" color="primary" type="submit" fullWidth sx={{ mt: 2 }} disabled={loading}>
        {loading ? 'Signing Up...' : 'Sign Up Now'}
      </Button>

      <Typography variant="body2" sx={{ mt: 2 }}>
        Already have an account? <Link to="/login">Login Here</Link>
      </Typography>
    </Box>
  );
}

export default Register;