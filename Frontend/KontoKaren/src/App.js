import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Homepage from './Pages/Homepage';
import Header from './Components/HeaderComponents/Header';
import Indtægter from './Pages/Indtægter/Indtægter';
import Login from './Pages/Login/Login';
import Budget from './Pages/Budget/Budget';
import Register from './Pages/Login/Register';
import ForgotPassword from './Pages/Login/ForgotPassword';
import ResetPassword from './Pages/Login/ResetPassword';
import LoggedInPage from './Pages/LoggedInPage/test';
import Udgifter from './Pages/Udgifter/Udgifter';

function App() {
  const [userFullName, setUserFullName] = useState(null);

  useEffect(() => {
    const fetchUserFullName = async () => {
      const token = localStorage.getItem('authToken');
      if (token) {
        try {
          const response = await fetch('http://localhost:5168/Account/WhoAmI', {
            headers: {
              'Authorization': `Bearer ${token}`
            }
          });
          if (response.ok) {
            const user = await response.json();
            setUserFullName(user.fullName);
          }
        } catch (error) {
          console.error('Failed to fetch user full name:', error);
        }
      }
    };

    fetchUserFullName();
  }, []);

  return (
    <Router>
      <div className="App">
        <Header userFullName={userFullName} />
        <Routes>
          <Route path="/" element={<Homepage />} />
          <Route path="/Indtægter" element={<Indtægter />} />
          <Route path="/Udgifter" element={<Udgifter />} />
          <Route path="/Budget" element={<Budget />} />
          <Route path="/Login" element={<Login setUserFullName={setUserFullName} />} />
          <Route path="/Register" element={<Register />} />
          <Route path="/ForgotPassword" element={<ForgotPassword />} />
          <Route path="/ResetPassword" element={<ResetPassword />} />
          <Route path="/user-dashboard" element={<LoggedInPage />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;