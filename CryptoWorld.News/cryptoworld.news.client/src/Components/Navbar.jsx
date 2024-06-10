import React from "react";
import { Nav, NavLink, NavMenu } from "./NavbarElements";

const Navbar = () => {
    return (
        <Nav>
            <NavMenu>
                <NavLink to="/" className={({ isActive }) => isActive ? "active" : ""}>
                    Home
                </NavLink>
                <NavLink to="/Contact" className={({ isActive }) => isActive ? "active" : ""}>
                    Contact Us
                </NavLink>
                <NavLink to="/Login" className={({ isActive }) => isActive ? "active" : ""}>
                    Login
                </NavLink>
                <NavLink to="/Register" className={({ isActive }) => isActive ? "active" : ""}>
                    Register
                </NavLink>
            </NavMenu>
        </Nav>
    );
};

export default Navbar;
