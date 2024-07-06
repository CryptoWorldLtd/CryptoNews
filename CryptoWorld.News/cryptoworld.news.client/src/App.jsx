import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import './App.css';
import Navbar from './Components/Navbar'; 
import Home from './Pages/Home';
import Login from './Pages/Login';
import Register from './Pages/Register';
import ForgotPassword from './Pages/ForgotPassword';


function App() {
    return (
        <div>
            <Router>
                <Navbar />
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/Login" element={<Login />} />
                    <Route path="/Register" element={<Register />} />
                    <Route path="/ForgotPassword" element={<ForgotPassword />} />
                </Routes>
                
            </Router>
        </div>
    ); 
}

export default App;