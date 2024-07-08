import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import './App.css';
import Navbar from './Components/Navbar'; 
import Home from './Pages/Home';
import Login from './Pages/Login';
import Register from './Pages/Register';
import ParticlesComponent from './Components/Particles';



function App() {
    return (
        <div className='App'>
            <ParticlesComponent id='particles' />
            <Router>
                <Navbar />
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/Login" element={<Login />} />
                    <Route path="/Register" element={<Register />} />
                </Routes>
            </Router>
        </div>
    ); 
}

export default App;

