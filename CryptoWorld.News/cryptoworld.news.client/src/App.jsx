import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import './App.css';
import Navbar from './Components/Navbar.jsx'; 
import Home from './Pages/Home';
import Login from './Pages/Login';
import Register from './Pages/Register';
import Profile from './Pages/ProfileDropdown.jsx';
import MyNews from './Pages/MyNews';
import EditProfile from './Pages/EditProfile';
import ForgotPassword from './Pages/ForgotPassword';
import ResetPassword from './Pages/ResetPassword';

function App() {
    return (
        <div>
            <Router>
                <Navbar />
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/Login" element={<Login />} />
                    <Route path="/Register" element={<Register />} />
                    <Route path="/Profile" element={<Profile />} />
                    <Route path="/EditProfile" element={<Profile />} />
                    <Route path="/ChangeEmail" element={<EditProfile formType="email" />} />
                    <Route path="/ChangePassword" element={<EditProfile formType="password" />} />
                    <Route path="/MyNews" element={<MyNews />} />
                    <Route path="/ForgotPassword" element={<ForgotPassword />} />
                    <Route path="/ResetPassword/:id/:email" element={<ResetPassword />} />
                </Routes>
                
            </Router>
        </div>
    ); 
}

export default App;