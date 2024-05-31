import React from "react";
import { useNavigate } from "react-router-dom"
import './Login.css'

import user_icon from '../assets/user.png'
import password_icon from '../assets/password.png'

const Login = () => {
    return (
        <div className="container">
            <div className="header">
                <div className="text">Welcome Back</div>
                <div className="underline"></div>
            </div>
            <div className="inputs">
                <div className="input">
                    <img src={user_icon} alt="" />
                    <input type="username"placeholder="Name"/>
                </div>
            </div>
            <div className="inputs">
                <div className="input">
                    <img src={password_icon} alt="" />
                    <input type="password" placeholder="Password"/>
                </div>
            </div>
            <div className="forgot-password">Forgot Password? <span>Click Here!</span></div>
            <div className="submit-container">
                <div className="submit">Sign Up</div>
                <div className="submit">Login</div>
            </div>
        </div>
    )
};
export default Login