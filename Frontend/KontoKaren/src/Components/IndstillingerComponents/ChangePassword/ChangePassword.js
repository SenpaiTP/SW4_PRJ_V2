//Change Password
import { Typography, TextField, Button, Box } from "@mui/material";
//import React from "react";
import React, { useState } from 'react';

/*const API_URL = 'http://localhost:5168/api';

const getAuthToken = () => localStorage.getItem('authToken');*/

function ChangePassword()
 {
  const [email, setEmail] = useState('');
  const [currentPassword, setCurrentPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [confirmNewPassword, setConfirmNewPassword] = useState('');
  const [errors, setErrors] = useState({
    email: '',
    currentPassword: '',
    newPassword: '',
    confirmNewPassword: '',
  });
  const [loading, setLoading] = useState(false); // Loading state for button
  const [successMessage, setSuccessMessage] = useState('');

  const handleSubmit = async (event) => {
    event.preventDefault();

    let formErrors = { ...errors };
    let formValid = true;

    // Reset errors
    Object.keys(formErrors).forEach((key) => (formErrors[key] = ''));

    // Validate email
    const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    if (!email) {
      formErrors.email = 'Email is required';
      formValid = false;
    } else if (!emailRegex.test(email)) {
      formErrors.email = 'Please enter a valid email address';
      formValid = false;
    }

    // Validate current password
    if (!currentPassword) {
      formErrors.currentPassword = 'Current password is required';
      formValid = false;
    }

    // Validate new password
    if (!newPassword) {
      formErrors.newPassword = 'New password is required';
      formValid = false;
    } else if (newPassword.length < 8) {
      formErrors.newPassword = 'Password must be at least 8 characters';
      formValid = false;
    }

    // Validate confirm new password
    if (!confirmNewPassword) {
      formErrors.confirmNewPassword = 'Please confirm your new password';
      formValid = false;
    } else if (newPassword !== confirmNewPassword) {
      formErrors.confirmNewPassword = 'Passwords do not match';
      formValid = false;
    }

    setErrors(formErrors);

    // If validation fails, do not proceed
    if (!formValid) return;

    // Proceed with password change
    setLoading(true); // Start loading
    setSuccessMessage(''); // Reset success message
    try {
      const response = await fetch('https://localhist:5168/change-password', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${localStorage.getItem('authToken')}`, // Assuming a token is stored in localStorage
        },
        body: JSON.stringify({ email, currentPassword, newPassword }),
      });

      const data = await response.json();

      if (response.ok) {
        setSuccessMessage('Password changed successfully!');
        setEmail('');
        setCurrentPassword('');
        setNewPassword('');
        setConfirmNewPassword('');
      } else {
        setErrors({ ...formErrors, email: data.message || 'Failed to change password' });
      }
    } catch (error) {
      console.error('Password change failed:', error);
      setErrors({ ...formErrors, email: 'An error occurred. Please try again.' });
    } finally {
      setLoading(false); // Stop loading
    }
  };

  return (
    <Box sx={{ maxWidth: 400, margin: '0 auto', padding: 2 }}>
      <Typography variant="h5" gutterBottom>
        Ændre kodeord
      </Typography>
      {successMessage && (
        <Typography color="success.main" gutterBottom>
          {successMessage}
        </Typography>
      )}
      <form onSubmit={handleSubmit}>
        <TextField
          fullWidth
          margin="normal"
          label="Email"
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          error={!!errors.email}
          helperText={errors.email}
        />
        <TextField
          fullWidth
          margin="normal"
          label="Nuværende kodeord"
          type="password"
          value={currentPassword}
          onChange={(e) => setCurrentPassword(e.target.value)}
          error={!!errors.currentPassword}
          helperText={errors.currentPassword}
        />
        <TextField
          fullWidth
          margin="normal"
          label="Nyt kodeord"
          type="password"
          value={newPassword}
          onChange={(e) => setNewPassword(e.target.value)}
          error={!!errors.newPassword}
          helperText={errors.newPassword}
        />
        <TextField
          fullWidth
          margin="normal"
          label="Gentag nyt kodeord"
          type="password"
          value={confirmNewPassword}
          onChange={(e) => setConfirmNewPassword(e.target.value)}
          error={!!errors.confirmNewPassword}
          helperText={errors.confirmNewPassword}
        />
        <Button
          fullWidth
          variant="contained"
          color="primary"
          type="submit"
          disabled={loading}
          sx={{ mt: 2 }}
        >
          {loading ? 'Ændrer...' : 'Gem nyt kodeord'}
        </Button>
      </form>
    </Box>
  );
}


 
export default ChangePassword;