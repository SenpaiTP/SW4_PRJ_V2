import React from 'react';
import { AppBar, Toolbar, Typography, Button } from '@mui/material';
import { Link } from 'react-router-dom';

function Header({ userFullName }) {
  return (
    <AppBar position="static">
      <Toolbar>
        <Typography variant="h6" style={{ flexGrow: 1 }}>
          KontoKaren
        </Typography>
        <Button color="inherit" component={Link} to="/">
          Home
        </Button>
        <Button color="inherit" component={Link} to="/Indtægter">
          Indtægter
        </Button>
        <Button color="inherit" component={Link} to="/Udgifter">
          Udgifter
        </Button>
        <Button color="inherit" component={Link} to="/Budget">
          Budget
        </Button>
        {userFullName ? (
          <Typography variant="h6" style={{ marginRight: '16px' }}>
            {userFullName}
          </Typography>
        ) : (
          <Button color="inherit" component={Link} to="/login">
            Sign In
          </Button>
        )}
      </Toolbar>
    </AppBar>
  );
}

export default Header;