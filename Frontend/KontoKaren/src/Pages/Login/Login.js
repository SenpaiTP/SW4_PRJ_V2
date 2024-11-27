import React, { useState } from 'react';
import { TextField, Button, Typography, Box, Checkbox, FormControlLabel } from '@mui/material';
import { Link, useNavigate } from 'react-router-dom'; // Import useNavigate
import { getBoxStyles } from '../../Assets/Styles/boxStyles'; // Box styling
import { getTextFieldStyles } from '../../Assets/Styles/textFieldStyles'; // TextField styling

function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [rememberMe, setRememberMe] = useState(false);
  const [errors, setErrors] = useState({
    email: '',
    password: '',
  });
  const [loading, setLoading] = useState(false); // Loading state for button
  const navigate = useNavigate(); // Hook to redirect the user

  const handleSubmit = async (event) => {
    event.preventDefault();

    let formErrors = { ...errors };
    let formValid = true;

    // Reset errors
    Object.keys(formErrors).forEach((key) => formErrors[key] = '');

    // Check if email is valid
    const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
    if (!email) {
      formErrors.email = 'Email is required';
      formValid = false;
    } else if (!emailRegex.test(email)) {
      formErrors.email = 'Please enter a valid email address';
      formValid = false;
    }

    // Check if password is entered
    if (!password) {
      formErrors.password = 'Password is required';
      formValid = false;
    }

    setErrors(formErrors);

    // If there are errors, prevent form submission
    if (!formValid) return;

    // If no validation errors, proceed with login API call
    setLoading(true); // Start loading
    try {
      // Make a POST request to your backend for login
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
      
      // Check if login is successful (you can adjust this check based on your backend response)
      if (response.ok) {
        // If login is successful, store token in localStorage or cookies
        localStorage.setItem('authToken', data.token); // Assuming token is returned
        // Redirect to user site/dashboard
        navigate('/user-dashboard');
      } else {
        // If login fails, show error message (you can adjust based on your backend response)
        setErrors({ ...errors, password: data.message || 'Invalid login credentials' });
      }
    } catch (error) {
      console.error('Login failed:', error);
      setErrors({ ...errors, password: 'An error occurred. Please try again.' });
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
        Login
      </Typography>

      {/* Email Input */}
      <TextField
        label="Enter your email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        {...getTextFieldStyles()} // Apply the shared styles
        error={!!errors.email} // Show error if there's an error for this field
        helperText={errors.email} // Display error message
      />

      {/* Password Input */}
      <TextField
        label="Enter your password"
        type="password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        {...getTextFieldStyles()} // Apply the shared styles
        error={!!errors.password} // Show error if there's an error for this field
        helperText={errors.password} // Display error message
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
      <Button 
        variant="contained" 
        color="primary" 
        type="submit" 
        fullWidth 
        sx={{ mt: 2 }} 
        disabled={loading} // Disable button while loading
      >
        {loading ? 'Logging in...' : 'LOGIN NOW'}
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


// import React, { useState } from 'react';
// import { TextField, Button, Typography, Box, FormControlLabel, Checkbox } from '@mui/material';
// import { Link, useNavigate } from 'react-router-dom';  // Import useNavigate for redirection
// import { getBoxStyles } from '../../Assets/Styles/boxStyles';
// import { getTextFieldStyles } from '../../Assets/Styles/textFieldStyles';

// function Login() {
//   const navigate = useNavigate();
  
//   const [email, setEmail] = useState('');
//   const [password, setPassword] = useState('');
//   const [rememberMe, setRememberMe] = useState(false);
//   const [error, setError] = useState(''); // To display error message if login fails

//   // Dummy credentials for testing
//   const dummyEmail = 'test@example.com';
//   const dummyPassword = 'Password123';

//   const handleSubmit = (event) => {
//     event.preventDefault();

//     // Simulate user login with dummy data
//     if (email === dummyEmail && password === dummyPassword) {
//       // Simulate setting a JWT token in localStorage after a successful login
//       localStorage.setItem('authToken', 'dummyAuthToken123');
//       navigate('/user-dashboard');  // Redirect to the logged-in page
//     } else {
//       setError('Invalid email or password');  // Display error if credentials do not match
//     }
//   };

//   return (
//     <Box
//       component="form"
//       onSubmit={handleSubmit}
//       sx={{
//         ...getBoxStyles('medium'),
//       }}
//     >
//       <Typography variant="h5" gutterBottom>
//         Login
//       </Typography>

//       {/* Email Input */}
//       <TextField
//         label="Enter your email"
//         value={email}
//         onChange={(e) => setEmail(e.target.value)}
//         {...getTextFieldStyles()} // Apply shared styles
//       />

//       {/* Password Input */}
//       <TextField
//         label="Enter your password"
//         type="password"
//         value={password}
//         onChange={(e) => setPassword(e.target.value)}
//         {...getTextFieldStyles()} // Apply shared styles
//       />

//       {/* Remember me checkbox */}
//       <FormControlLabel
//         control={
//           <Checkbox
//             checked={rememberMe}
//             onChange={(e) => setRememberMe(e.target.checked)}
//             color="primary"
//           />
//         }
//         label="Remember me"
//       />

//       {/* Error message */}
//       {error && <Typography color="error">{error}</Typography>}

//       {/* Login button */}
//       <Button variant="contained" color="primary" type="submit" fullWidth sx={{ mt: 2 }}>
//         LOGIN NOW
//       </Button>

//       {/* Register and forgot password links */}
//       <Typography variant="body2" sx={{ mt: 2 }}>
//         Not a member? <Link to="/register">Register Now</Link>
//       </Typography>
//       <Typography variant="body2">
//         <Link to="/forgot-password">Forgot password?</Link>
//       </Typography>
//     </Box>
//   );
// }

// export default Login;
