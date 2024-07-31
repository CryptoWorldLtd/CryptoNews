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
import News from './Pages/News';
import ParticlesComponent from './Components/Particles';
import CreateNews from './Pages/CreateNews.jsx';
import { Toaster } from 'react-hot-toast';
import RssFeed from './Pages/RssFeed.jsx';

function App() {
    return (
        <div className='App'>
            <ParticlesComponent id='particles' />
            <Router>
                <Navbar />
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/login" element={<Login />} />
                    <Route path="/register" element={<Register />} />
                    <Route path="/profile" element={<Profile />} />
                    <Route path="/editProfile" element={<Profile />} />
                    <Route path="/changeEmail" element={<EditProfile formType="email" />} />
                    <Route path="/changePassword" element={<EditProfile formType="password" />} />
                    <Route path="/myNews" element={<MyNews />} />
                    <Route path="/forgotPassword" element={<ForgotPassword />} />
                    <Route path="/resetPassword/:id/:email" element={<ResetPassword />} />
                    <Route path="/news" element={<News />} />
                    <Route path="/createNews" element={<CreateNews />} />
                    <Route path="/RssFeed" element={<RssFeed />} />
                </Routes>
            </Router>
            <Toaster />
        </div>
    );
}

export default App;