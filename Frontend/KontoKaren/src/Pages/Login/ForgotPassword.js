// ForgotPassword.js
import React, { useState } from 'react';
import { TextField, Button, Typography, Box } from '@mui/material';
import { Link, useNavigate } from 'react-router-dom'; // Importer useNavigate her
import { getBoxStyles } from '../../Assets/Styles/boxStyles';
import { getTextFieldStyles } from '../../Assets/Styles/textFieldStyles'; // Importer textFieldStyles

function ForgotPassword() {
  const [email, setEmail] = useState('');
  const navigate = useNavigate(); // Initialiser useNavigate hook

  const handleSubmit = (event) => {
    event.preventDefault();
    console.log('Email til nulstilling af adgangskode:', email);

    // Naviger til ResetPassword-siden efter indsendelse
    navigate('/ResetPassword'); // Sørg for, at denne matcher stien i App.js
  };

  return (
    <Box
      component="form"
      onSubmit={handleSubmit}
      sx={{
        ...getBoxStyles('medium'), // Hent de dynamiske medium box-styles
      }}
    >
      <Typography variant="h5" gutterBottom>
        Glemt Adgangskode?
      </Typography>

      {/* Email Input med delte TextField-styles */}
      <TextField
        label="Indtast din email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        {...getTextFieldStyles()} // Anvend de delte styles
      />

      <Button variant="contained" color="primary" type="submit" fullWidth sx={{ mt: 2 }}>
        Nulstil Adgangskode
      </Button>

      <Typography variant="body2" sx={{ mt: 2 }}>
        Har du husket din adgangskode? <Link to="/login">Log ind her</Link>
      </Typography>
    </Box>
  );
}

export default ForgotPassword;
