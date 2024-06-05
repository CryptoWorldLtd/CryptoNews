import React from "react";
import { useNavigate } from "react-router-dom"
import './Login.css'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUser, faEnvelope } from '@fortawesome/free-regular-svg-icons';
import { faLock } from '@fortawesome/free-solid-svg-icons';

const Login = () => {
    return (
        <div className="container">
            <div className="header">
                <div className="text">Welcome Back</div>
                <div className="underline"></div>
            </div>
            <div className="inputs">
                <div className="input">
                <i>
                <FontAwesomeIcon icon={faUser} />
              </i>
                    <input type="username"placeholder="Name"/>
                </div>
            </div>
            <div className="inputs">
                <div className="input">
                <i>
            <FontAwesomeIcon icon={faLock} />
          </i>
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