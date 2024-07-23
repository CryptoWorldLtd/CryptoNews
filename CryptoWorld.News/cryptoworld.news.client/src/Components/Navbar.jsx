import React, { useState } from 'react';
import Logo from '/logo.jpg'
import './Navbar.css';
import { Nav, NavLink, NavMenu } from "./NavbarElements";
import Profile from "../Pages/ProfileDropdown.jsx";


const Navbar = () => {
    return (
        <Nav>
                <NavLink to="/" className="logo">
                    <img alt='logo' src={Logo}></img>
                </NavLink>
                <NavLink to="/News" className={({ isActive }) => isActive ? "active" : ""}>
                    News
                </NavLink>
                <NavLink to="/Login" className={({ isActive }) => isActive ? "active" : ""}>
                    Login
                </NavLink>
                <NavLink to="/Register" className={({ isActive }) => isActive ? "active" : ""}>
                    Register
                </NavLink>
                <Profile/>
        </Nav>
    );
};

export default Navbar;
