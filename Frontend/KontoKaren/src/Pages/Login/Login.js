import React, { useState } from 'react';
import { TextField, Button, Typography, Box, Checkbox, FormControlLabel } from '@mui/material';
import { Link, useNavigate } from 'react-router-dom'; // Importer useNavigate
import { getBoxStyles } from '../../Assets/Styles/boxStyles'; // Box styling
import { getTextFieldStyles } from '../../Assets/Styles/textFieldStyles'; // TextField styling

function Login({ setUserFullName }) {
  const [email, setEmail] = useState('');
  const [showPassword, setShowPassword] = useState(false); // State for password visibility
  const [password, setPassword] = useState('');
  const [rememberMe, setRememberMe] = useState(false);
  const [errors, setErrors] = useState({
    email: '',
    password: '',
  });
  const [loading, setLoading] = useState(false); // Loading state for button
  const navigate = useNavigate(); // Hook til at redirecte brugeren

  const handleSubmit = async (event) => {
    event.preventDefault();

    let formErrors = { ...errors };
    let formValid = true;

    // Nulstil fejl
    Object.keys(formErrors).forEach((key) => (formErrors[key] = ''));

    // Tjek om email er gyldig
    const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    if (!email) {
      formErrors.email = 'Email er påkrævet';
      formValid = false;
    } else if (!emailRegex.test(email)) {
      formErrors.email = 'Indtast en gyldig emailadresse';
      formValid = false;
    }

    // Tjek om adgangskode er indtastet
    if (!password) {
      formErrors.password = 'Adgangskode er påkrævet';
      formValid = false;
    }

    setErrors(formErrors);

    // Hvis der er fejl, forhindrer vi formularen i at blive sendt
    if (!formValid) return;

    // Hvis der ikke er valideringsfejl, fortsæt med login API-kald
    setLoading(true); // Start loading
    try {
      const response = await fetch('http://localhost:5168/Account/Login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          Username: email,
          Password: password,
        }),
      });

      const data = await response.json();

      if (response.ok) {
<<<<<<< HEAD
        // If login is successful, store token in localStorage or cookies
        localStorage.setItem('authToken', data.token); // Assuming token is returned
        // console.log(data.token);
        // console.log("Stored Token:", localStorage.getItem('authToken'));

=======
        localStorage.setItem('authToken', data.token); // Antag, at token returneres
>>>>>>> main

        const nameResponse = await fetch('http://localhost:5168/Account/WhoAmI', {
          headers: {
            Authorization: `Bearer ${data.token}`,
          },
        });

        if (nameResponse.ok) {
          const user = await nameResponse.json();
          setUserFullName(user.fullName); // Sæt brugerens fulde navn
        }
        console.log("Stored Token:", localStorage.getItem('authToken'));

        navigate('/user-dashboard');
      } else {
        setErrors({ ...errors, password: data.message || 'Ugyldige loginoplysninger' });
      }
    } catch (error) {
      console.error('Login mislykkedes:', error);
      setErrors({ ...errors, password: 'Der opstod en fejl. Prøv igen.' });
    } finally {
      setLoading(false); // Stop loading
    }
  };

  return (
    <Box
      component="form"
      onSubmit={handleSubmit}
      sx={{
        ...getBoxStyles('medium'), // Dynamisk styling til medium box
      }}
    >
      <Typography variant="h5" gutterBottom>
        Log ind
      </Typography>

      {/* Email Input */}
      <TextField
        label="Indtast din email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        {...getTextFieldStyles()} // Brug de delte styles
        error={!!errors.email}
        helperText={errors.email}
      />

      {/* Password Input */}
      <TextField
        label="Indtast din adgangskode"
        type={showPassword ? 'text' : 'password'}
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        {...getTextFieldStyles()}
        error={!!errors.password}
        helperText={errors.password}
      />

      {/* Checkboxes */}
      <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'flex-start' }}>
        <FormControlLabel
          control={
            <Checkbox
              checked={rememberMe}
              onChange={(e) => setRememberMe(e.target.checked)}
              color="primary"
            />
          }
          label="Husk mig"
          labelPlacement="end"
        />
        <FormControlLabel
          control={
            <Checkbox
              checked={showPassword}
              onChange={(e) => setShowPassword(e.target.checked)}
              color="primary"
            />
          }
          label="Vis adgangskode"
          labelPlacement="end"
        />
      </Box>

      {/* Login Button */}
      <Button 
        variant="contained" 
        color="primary" 
        type="submit" 
        fullWidth 
        sx={{ mt: 2 }} 
        disabled={loading}
      >
        {loading ? 'Logger ind...' : 'LOG IND'}
      </Button>

      {/* Links */}
      <Typography variant="body2" sx={{ mt: 2 }}>
        Ikke medlem? <Link to="/Register">Tilmeld dig nu</Link>
      </Typography>
      <Typography variant="body2">
        <Link to="/ForgotPassword">Glemt adgangskode?</Link>
      </Typography>
    </Box>
  );
}

export default Login;
