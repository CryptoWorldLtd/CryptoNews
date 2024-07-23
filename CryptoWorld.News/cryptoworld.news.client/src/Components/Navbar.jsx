import React from "react";
import { Nav, NavLink, NavMenu } from "./NavbarElements";
import Profile from "../Pages/ProfileDropdown.jsx";

const Navbar = () => {
    return (
        <Nav>
            <NavMenu>
                <NavLink to="/" className={({ isActive }) => isActive ? "active" : ""}>
                    Home
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
            </NavMenu>
        </Nav>
    );
};

export default Navbar;
