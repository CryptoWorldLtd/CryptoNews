import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import './App.css';
import Navbar from './Components/Navbar'; 
import Home from './Pages/Home';
import Login from './Pages/Login';
import Register from './Pages/Register';
import Profile from './Pages/Profile';
import ChangePassword from './Pages/ChangePassword.jsx';
import ChangeEmail from './Pages/ChangeEmail';
import MyNews from './Pages/MyNews';
import Logout from './Pages/Logout';

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
                    <Route path="/ChangePassword" element={<ChangePassword />} />
                    <Route path="/ChangeEmail" element={<ChangeEmail />} />
                    <Route path="/MyNews" element={<MyNews />} />
                    <Route path="/Logout" element={<Logout />} />
                </Routes>
            </Router>
        </div>
    ); 
}

export default App;