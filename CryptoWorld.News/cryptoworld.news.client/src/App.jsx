import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import './App.css';
import Navbar from './Components/Navbar'; 
import Home from './Pages/Home';
import Login from './Pages/Login';

function App() {
    
    return (
        
        <div>
            <Router>
            <Navbar />
            <Routes>
                <Route
                    path="/"
                    element={<Home />}
                />
                <Route
                    path="/Login"
                    element={<Login />}
                />
            </Routes>
        </Router>
        </div>
    ); 
}

export default App;