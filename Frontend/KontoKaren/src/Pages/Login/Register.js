import React, { useState } from 'react';
import { TextField, Button, Typography, Box, FormControlLabel, Checkbox } from '@mui/material';
import { Link, useNavigate } from 'react-router-dom'; // Importer useNavigate
import { getBoxStyles } from '../../Assets/Styles/boxStyles';
import { getTextFieldStyles } from '../../Assets/Styles/textFieldStyles'; // Importer textFieldStyles

function Register() {
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false); // State for adgangskodesynlighed
  const [errors, setErrors] = useState({
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    confirmPassword: '',
  });
  const [loading, setLoading] = useState(false); // Loading state for knap
  const navigate = useNavigate(); // Hook til at redirecte brugeren

  const handleSubmit = async (event) => {
    event.preventDefault();

    let formErrors = { ...errors };
    let formValid = true;

    // Nulstil fejl
    Object.keys(formErrors).forEach((key) => (formErrors[key] = ''));

    // Tjek om alle felter er udfyldt
    if (!firstName || !lastName || !email || !password || !confirmPassword) {
      formErrors.firstName = 'Fornavn er påkrævet';
      formErrors.lastName = 'Efternavn er påkrævet';
      formErrors.email = 'Email er påkrævet';
      formErrors.password = 'Adgangskode er påkrævet';
      formErrors.confirmPassword = 'Bekræft adgangskode er påkrævet';
      formValid = false;
    }

    // Tjek om email er gyldig
    const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    if (email && !emailRegex.test(email)) {
      formErrors.email = 'Indtast en gyldig emailadresse';
      formValid = false;
    }

    // Tjek om adgangskoderne matcher
    if (password && confirmPassword && password !== confirmPassword) {
      formErrors.confirmPassword = 'Adgangskoderne matcher ikke';
      formValid = false;
    }

    // Tjek om adgangskoden opfylder kravene
    const passwordRegex = /^(?=.*[A-Z]).{8,}$/;
    if (password && !passwordRegex.test(password)) {
      formErrors.password = 'Adgangskode skal være mindst 8 tegn lang og indeholde mindst ét stort bogstav';
      formValid = false;
    }

    setErrors(formErrors);

    if (!formValid) return; // Hvis der er fejl, forhindrer vi formularen i at blive sendt

    // Hvis alle valideringer er bestået
    setLoading(true); // Start loading
    try {
      const response = await fetch('http://localhost:5168/Account/Register', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          Fornavn: firstName,
          Efternavn: lastName,
          Email: email,
          Password: password,
        }),
      });

      console.log('API-svar:', response.status);

      const data = await response.json();

      if (response.ok) {
        console.log('Registrering lykkedes:', data);
        navigate('/login'); // Redirect til login-siden
      } else {
        console.error('Registrering mislykkedes:', data);
        setErrors({ ...errors, email: data.message || 'Registrering mislykkedes' });
      }
    } catch (error) {
      console.error('Registrering mislykkedes:', error);
      setErrors({ ...errors, email: 'Der opstod en fejl. Prøv igen.' });
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
        Tilmeld
      </Typography>

      {/* Fornavn */}
      <TextField
        label="Fornavn"
        value={firstName}
        onChange={(e) => setFirstName(e.target.value)}
        {...getTextFieldStyles()}
        error={!!errors.firstName}
        helperText={errors.firstName}
      />

      {/* Efternavn */}
      <TextField
        label="Efternavn"
        value={lastName}
        onChange={(e) => setLastName(e.target.value)}
        {...getTextFieldStyles()}
        error={!!errors.lastName}
        helperText={errors.lastName}
      />

      {/* Email */}
      <TextField
        label="Email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        {...getTextFieldStyles()}
        error={!!errors.email}
        helperText={errors.email}
      />

      {/* Adgangskode */}
      <TextField
        label="Adgangskode"
        type={showPassword ? 'text' : 'password'}
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        {...getTextFieldStyles()}
        error={!!errors.password}
        helperText={
          errors.password ||
          'Adgangskode skal være mindst 8 tegn lang og indeholde mindst ét stort bogstav og et special tegn (f.eks. !@#$%^&*)'
        }
      />

      {/* Bekræft adgangskode */}
      <TextField
        label="Bekræft adgangskode"
        type={showPassword ? 'text' : 'password'}
        value={confirmPassword}
        onChange={(e) => setConfirmPassword(e.target.value)}
        {...getTextFieldStyles()}
        error={!!errors.confirmPassword}
        helperText={errors.confirmPassword}
      />

      {/* Vis adgangskode checkbox */}
      <FormControlLabel
        control={
          <Checkbox
            checked={showPassword}
            onChange={(e) => setShowPassword(e.target.checked)}
            color="primary"
          />
        }
        label="Vis adgangskode"
      />

      <Button variant="contained" color="primary" type="submit" fullWidth sx={{ mt: 2 }} disabled={loading}>
        {loading ? 'Registrerer...' : 'Tilmeld nu'}
      </Button>

      <Typography variant="body2" sx={{ mt: 2 }}>
        Har du allerede en konto? <Link to="/login">Log ind her</Link>
      </Typography>
    </Box>
  );
}

export default Register;
