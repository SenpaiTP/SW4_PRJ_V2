import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Homepage from './Pages/Homepage';
//import Login from './Pages/Login/Login';
import Header from './Components/Header/Header';
import Indtægter from './Pages/Indtægter/Indtægter';
// Login pages
import Login from './Pages/Login/Login';
import Register from './Pages/Login/Register';
import ForgotPassword from './Pages/Login/ForgotPassword';
import ResetPassword from './Pages/Login/ResetPassword';

function App() {
  return (
    <Router>
      <div className="App">
        <Header />
        <Routes>
          <Route path="/" element={<Homepage />} />
          <Route path="/indtægter" element={<Indtægter />} />
          <Route path="/Login" element={<Login />} />
          <Route path="/Register" element={<Register />} />
          <Route path="/ForgotPassword" element={<ForgotPassword />} />
          <Route path="/ResetPassword" element={<ResetPassword />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
