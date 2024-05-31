import { useEffect, useState } from 'react';
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import './App.css';
import Navbar from './Components/Navbar'; 
import Login from './Pages/Login';

function App() {
    
    return (
        
        <div>
            <Router>
            <Navbar />
            <Routes>
                <Route
                    path="/Login"
                    element={<Login />}
                />
            </Routes>
        </Router>
            <h1 id="tabelLabel">Crypto World News</h1> 
        </div>
    ); 
}

export default App;