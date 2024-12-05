import React, { useState } from 'react';
import { AppBar, Toolbar, Typography, Button, Popover, MenuItem } from '@mui/material';
import { Link, useNavigate } from 'react-router-dom';

function Header({ userFullName }) {
  const [anchorEl, setAnchorEl] = useState(null);
  const navigate = useNavigate();
  const token = localStorage.getItem('authToken');

  const handleClick = (event) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  const handleLogout = async () => {
    try{
      const response = await fetch(('http://localhost:5168/Account/Logout'), {
        headers: {
          'Authorization': `Bearer ${token}`
        },  
        method: 'POST',
      });

      console.log({token});
      console.log("Stored Token:", localStorage.getItem('authToken'));
      

    if(response.ok){
      localStorage.removeItem('authToken'); // Remove token on logout
      window.location.reload();
      navigate('/'); // Redirect to login page after logout
      
    }
    
    }catch(error){
      console.error('Logout failed:', error);
    }

    localStorage.removeItem('authToken'); // Remove token on logout
    navigate('/login'); // Redirect to login page after logout
    handleClose();
  };

  const open = Boolean(anchorEl);
  const id = open ? 'simple-popover' : undefined;

  return (
    <AppBar position="static">
      <Toolbar>
        <Typography variant="h6" style={{ flexGrow: 1 }}>
          KontoKaren
        </Typography>
        <Button color="inherit" component={Link} to="/">
          Home
        </Button>
        <Button color="inherit" component={Link} to="/indtægter">
          Indtægter
        </Button>
        <Button color="inherit" component={Link} to="/Budget">
          Budget
        </Button>

        <Button color="inherit" component={Link} to="/Indstillinger">
          Indstillinger
        </Button>
        {userFullName ? (
          <>
            <Typography
              variant="h6"
              style={{ marginRight: '16px', cursor: 'pointer' }}
              onClick={handleClick}
            >
              {userFullName}
            </Typography>
            <Popover
              id={id}
              open={open}
              anchorEl={anchorEl}
              onClose={handleClose}
              anchorOrigin={{
                vertical: 'bottom',
                horizontal: 'center',
              }}
              transformOrigin={{
                vertical: 'top',
                horizontal: 'center',
              }}
            >
              <MenuItem onClick={handleLogout}>Logout</MenuItem>
            </Popover>
          </>
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