import React from 'react';
import { useNavigate } from 'react-router-dom';

function LoggedInPage() {
  const navigate = useNavigate();

  // Check if the user is logged in by verifying the presence of an auth token
  const token = localStorage.getItem('authToken');

  // If no token exists, redirect the user to the login page
  if (!token) {
    navigate('/login'); // Redirect to login page if not logged in
    return null; // Return nothing while redirecting
  }

  return (
    <div>
      <h1>Welcome to the User Dashboard!</h1>
      <p>You are logged in and can access this page.</p>
      <button onClick={() => {
        localStorage.removeItem('authToken'); // Remove token on logout
        navigate('/login'); // Redirect to login page after logout
      }}>
        Log Out
      </button>
    </div>
  );
}

export default LoggedInPage;
