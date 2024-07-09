import React, { useState } from 'react';
import Logo from '/logo.jpg'
import './Navbar.css';
import { Nav, NavLink, NavMenu } from "./NavbarElements";
import Profile from "../Pages/ProfileDropdown.jsx";


const Navbar = () => {
    const[menuOpen, setMenuOpen] = useState(false)
    return(
        <Nav>
            <NavLink to="/" className="logo">
                  <img alt='logo' src={Logo}></img>
            </NavLink>
            <div className="menu" onClick={() => {
                setMenuOpen(!menuOpen)
            }}>
                <span></span>
                <span></span>
                <span></span>
            </div>
            <ul className={menuOpen ? 'open' : ""}>
            <li>
                <NavLink to="/Login">Login</NavLink>
            </li>
            <li>
                <NavLink to="/Register">Register</NavLink>
            </li>
            <Profile/>
            </ul>
        </Nav>
    );
};

export default Navbar;
